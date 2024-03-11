// <copyright file="BingoValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests.Validation;
using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public class BingoValidator<TRequest> : Validator<TRequest>
    where TRequest : IBaseRequest
{
    // TODO: JR - decide how to word this.
    protected const string UserIsAlreadyOnATeamResponse = "The user '{0}' is already on a team.";
    protected const string UserIsAlreadyOnATeamUserPerspectiveResponse = "You are already on a team.";
    protected const string UserIsNotOnATeamResponse = "That user is not on a team.";
    protected const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";
    protected const string NoTilesError = "The team has no tiles to submit evidence for.";
    protected const string CompetitionStartedError = "This cannot be interacted with once the competition has started.";

    // CSV
    protected const string CsvMediaType = "text/csv";
    protected const string NonCsvFileError = "The uploaded file must be a csv.";

    // Image
    // TODO: JR - use ImageSharp for image validation.
    private readonly string[] mediaTypes = new string[] { "image/png", "image/jpeg" };
    private const string NotAValidImage = "Attachment must be of type: png or jpeg";

    public IDataWorker DataWorker = DataFactory.CreateDataWorker();

    public override IEnumerable<SemaphoreSlim> GetSemaphores(TRequest request) =>
        GetSemaphores(request, (RequestSemaphores)General.DI.GetService(typeof(RequestSemaphores))!);

    protected virtual IEnumerable<SemaphoreSlim> GetSemaphores(TRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>();

    public void UserNotNull(Func<TRequest, User?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void DiscordTeamNotNull(Func<TRequest, RSBingoBot.Discord.DiscordTeam?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ObjectIsNull.FormatConst("DiscordTeam"));
    }

    public void TeamExists(Func<TRequest, string> func)
    {
        RuleFor(r => func(r))
            .Must(t => DataWorker.Teams.DoesTeamExist(t))
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));
    }

    // TODO: JR - change this to take a DiscordTeam and format with the name not the id.
    public void TeamExists(Func<TRequest, int> func)
    {
        RuleFor(r => func(r))
            .Must(t => DataWorker.Teams.GetTeamByID(t) is not null)
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));
    }

    public void NewTeamName(Func<TRequest, string> func)
    {
        RuleFor(r => func(r))
            .SetValidator(new NewTeamNameValidator<TRequest>(DataWorker));
    }

    /// <summary>
    /// Checks the user from their Discord id.
    /// </summary>
    public void UserOnATeam(Func<TRequest, ulong> func)
    {
        RuleFor(r => func(r))
            .Must(id => DataWorker.Users.Where(u => u.DiscordUserId == id).Any())
            .WithMessage(r => UserIsNotOnATeamResponse);
    }

    public void UserNotOnATeam(Func<TRequest, DiscordUser> func, bool userPerspective)
    {
        RuleFor(r => func(r))
            .Must(u => u.IsOnATeam(DataWorker) is false)
            .WithMessage(r => GetUserOnTeamError(func(r), userPerspective));
    }

    public void UserOnTeam(Func<TRequest, (DiscordUser, string)> func, bool isUserPerspective)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<TRequest>(DataWorker, func, isUserPerspective));
    }

    public void UserOnTeam(Func<TRequest, (DiscordUser, int)> func, bool isUserPerspective)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<TRequest>(DataWorker, func, isUserPerspective));
    }

    public void UserOnTeam(Func<TRequest, (DiscordUser, Team)> func, bool isUserPerspective)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<TRequest>(DataWorker, func, isUserPerspective));
    }

    public void TeamHasTiles(Func<TRequest, int> func)
    {
        TeamExists(r => func(r));
        RuleFor(r => GetTeam(func(r)).Tiles.Any())
                    .Equal(true)
                    .WithMessage(NoTilesError);
    }

    private Team GetTeam(int id) =>
        DataWorker.Teams.FirstOrDefault(t => t.RowId == id)!;

    public void IsCSVFile(Func<TRequest, DiscordAttachment> func)
    {
        RuleFor(r => func(r))
            .Must(a => a.MediaType.StartsWith(CsvMediaType))
            .WithMessage(r => NonCsvFileError);
    }

    public void IsImage(Func<TRequest, DiscordAttachment> func)
    {
        RuleFor(r => func(r).MediaType)
            .Must(t => mediaTypes.Contains(t))
            .WithMessage(NotAValidImage);
    }

    public void CompetitionNotStarted()
    {
        RuleFor(r => General.HasCompetitionStarted)
            .Equal(false)
            .WithMessage(CompetitionStartedError);
    }

    private string GetUserOnTeamError(DiscordUser user, bool userPerspective) =>
        userPerspective ?
            UserIsAlreadyOnATeamUserPerspectiveResponse :
            UserIsAlreadyOnATeamResponse.FormatConst(user.Username);
}
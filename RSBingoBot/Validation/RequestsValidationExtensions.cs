// <copyright file="RequestsValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using Requests;
using RSBingo_Framework.Interfaces;
using RSBingoBot.DiscordExtensions;

internal static class RequestsValidationUtilities
{
    private const string UserIsNull = "User cannot be null";
    private const string UserIsAlreadyOnATeamResponse = "This user '{0}' is already on a team.";
    private const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";

    // CSV
    private const string CsvMediaType = "text/csv";
    private const string NonCsvFileError = "The uploaded file must be a csv.";

    public static void ValidateDiscordUserNotNull<T>(this AbstractValidator<T> validator)
        where T : IRequestWithDiscordUser
    {
        validator.RuleFor(r => r.DiscordUser)
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public static void ValidateUserNotNull<T>(this AbstractValidator<T> validator)
        where T : IRequestWithUser
    {
        validator.RuleFor(r => r.User)
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public static void ValidateDiscordUserOnATeam<T>(this AbstractValidator<T> validator, IDataWorker dataWorker)
        where T : IRequestWithDiscordUser
    {
        validator.RuleFor(r => r.DiscordUser)
            .Must(u => u.IsOnATeam(dataWorker))
            .WithMessage(r => UserIsAlreadyOnATeamResponse.FormatConst(r.DiscordUser.Username));
    }

    public static void ValidateDiscordUserOnTeam<T>(this AbstractValidator<T> validator, IDataWorker dataWorker)
        where T : IRequestWithDiscordUser, IRequestWithTeamName
    {
        validator.RuleFor(r => r.DiscordUser)
            .SetValidator(new UserOnTeamValidator<T>(dataWorker));
    }

    public static void ValidateTeamExists<T>(this AbstractValidator<T> validator, IDataWorker dataWorker)
        where T : IRequestWithTeamName
    {
        validator.RuleFor(r => r.TeamName)
            .Must(n => dataWorker.Teams.DoesTeamExist(n))
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(r.TeamName));
    }

    public static void ValidateIsCSVFile<T>(this AbstractValidator<T> validator)
        where T : IRequestWithDiscordAttachment
    {
        validator.RuleFor(r => r.Attachment)
            .Must(a => a.MediaType.StartsWith(CsvMediaType))
            .WithMessage(r => NonCsvFileError);
    }
}
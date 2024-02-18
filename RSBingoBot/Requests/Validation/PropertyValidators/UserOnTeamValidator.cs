// <copyright file="UserOnTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DSharpPlus.Entities;
using FluentValidation;
using FluentValidation.Validators;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public class UserOnTeamValidator<T> : IPropertyValidator<T, DiscordUser>
{
    private const string UserIsNotOnTheTeamError = "The user '{0}' must be on the team '{1}'.";
    private const string UserIsNotOnTheTeamUserPerspectiveError = "You must be on the team '{0}'.";
    private const string TeamDoesNotExistError = "The team does not exist.";

    private readonly IDataWorker dataWorker;
    private readonly Func<T, (DiscordUser, Team)>? userAndTeam;
    private readonly Func<T, (DiscordUser, string)>? userAndTeamName;
    private readonly Func<T, (DiscordUser, int)>? userAndTeamId;
    private readonly bool isUserPerspective;

    private string error = string.Empty;

    public string Name => nameof(UserOnTeamValidator<T>);

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, Team)> func, bool isUserPerspective)
    {
        this.dataWorker = dataWorker;
        this.userAndTeam = func;
        this.isUserPerspective = isUserPerspective;
    }

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, string)> func, bool isUserPerspective)
    {
        this.dataWorker = dataWorker;
        this.userAndTeamName = func;
        this.isUserPerspective = isUserPerspective;
    }

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, int)> func, bool isUserPerspective)
    {
        this.dataWorker = dataWorker;
        this.userAndTeamId = func;
        this.isUserPerspective = isUserPerspective;
    }

    public string GetDefaultMessageTemplate(string errorCode) =>
        error;

    public bool IsValid(ValidationContext<T> context, DiscordUser user)
    {
        Team? team;
        if (userAndTeam is not null)
        {
            team = userAndTeam(context.InstanceToValidate).Item2;
        }
        else if (userAndTeamName is not null)
        {
            string name = userAndTeamName(context.InstanceToValidate).Item2;
            team = dataWorker.Teams.GetByName(name);
        }
        else
        {
            int id = userAndTeamId!(context.InstanceToValidate).Item2;
            team = dataWorker.Teams.GetTeamByID(id);

        }

        if (team is null)
        {
            SetTeamErrorMessage();
            return false;
        }

        if (user.IsOnTeam(dataWorker, team) is false)
        {
            SetUserErrorMessage(user, team);
            return false;
        }

        return true;
    }


    private void SetTeamErrorMessage()
    {
        error = TeamDoesNotExistError;
    }

    private void SetUserErrorMessage(DiscordUser user, Team team)
    {
        error = isUserPerspective ?
            UserIsNotOnTheTeamUserPerspectiveError.FormatConst(team.Name) :
            UserIsNotOnTheTeamError.FormatConst(user.Username, team.Name);
    }
}
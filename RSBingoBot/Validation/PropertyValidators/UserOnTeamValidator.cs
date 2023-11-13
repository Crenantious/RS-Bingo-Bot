// <copyright file="UserOnTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DSharpPlus.Entities;
using FluentValidation;
using FluentValidation.Validators;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.DiscordExtensions;

public class UserOnTeamValidator<T> : IPropertyValidator<T, DiscordUser>
{
    private const string UserIsNotOnTheTeam = "The user '{0}' is not on that team.";

    private readonly IDataWorker dataWorker;
    private readonly Func<T, (DiscordUser, Team)>? userAndTeam;
    private readonly Func<T, (DiscordUser, string)>? userAndTeamName;

    public string Name => "UserOnTeamValidator";

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, Team)> func)
    {
        this.dataWorker = dataWorker;
        this.userAndTeam = func;
    }

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, string)> func)
    {
        this.dataWorker = dataWorker;
        this.userAndTeamName = func;
    }

    public string GetDefaultMessageTemplate(string errorCode)
    {
        throw new NotImplementedException();
    }

    public bool IsValid(ValidationContext<T> context, DiscordUser user)
    {
        Team team;
        if (userAndTeam is not null)
        {
            team = userAndTeam(context.InstanceToValidate).Item2;
        }
        else
        {
            string name = userAndTeamName(context.InstanceToValidate).Item2;
            team = dataWorker.Teams.GetByName(name);
        }
        return user.IsOnTeam(dataWorker, team);
    }
}
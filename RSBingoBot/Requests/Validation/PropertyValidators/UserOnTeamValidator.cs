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
    private const string UserIsNotOnTheTeam = "The user '{0}' is not on that team.";

    private readonly IDataWorker dataWorker;
    private readonly Func<T, (DiscordUser, Team)>? userAndTeam;
    private readonly Func<T, (DiscordUser, string)>? userAndTeamName;
    private readonly Func<T, (DiscordUser, int)>? userAndTeamId;

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

    public UserOnTeamValidator(IDataWorker dataWorker, Func<T, (DiscordUser, int)> func)
    {
        this.dataWorker = dataWorker;
        this.userAndTeamId = func;
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
        return user.IsOnTeam(dataWorker, team);
    }
}
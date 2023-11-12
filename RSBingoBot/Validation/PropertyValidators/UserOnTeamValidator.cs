// <copyright file="UserOnTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DSharpPlus.Entities;
using FluentValidation;
using FluentValidation.Validators;
using RSBingo_Framework.Interfaces;
using RSBingoBot.DiscordExtensions;

public class UserOnTeamValidator<T> : IPropertyValidator<T, DiscordUser>
    where T : IRequestWithDiscordUser, IRequestWithTeamName
{
    private const string UserIsNotOnTheTeam = "The user '{0}' is not on that team.";

    private readonly IDataWorker dataWorker;

    public string Name => "UserOnTeamValidator";

    public UserOnTeamValidator(IDataWorker dataWorker) =>
        this.dataWorker = dataWorker;

    public string GetDefaultMessageTemplate(string errorCode)
    {
        throw new NotImplementedException();
    }

    public bool IsValid(ValidationContext<T> context, DiscordUser user) =>
        user.IsOnTeam(dataWorker, context.InstanceToValidate.TeamName);
}
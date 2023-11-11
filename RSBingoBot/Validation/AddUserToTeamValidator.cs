// <copyright file="AddUserToTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class AddUserToTeamValidator : AbstractValidator<AddUserToTeamRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public AddUserToTeamValidator()
    {
        this.DiscordUserNotNull();
        this.DiscordUserOnATeam(dataWorker);
        this.TeamExists(dataWorker);
    }
}
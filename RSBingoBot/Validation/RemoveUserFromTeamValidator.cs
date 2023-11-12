// <copyright file="RemoveUserFromTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class RemoveUserFromTeamValidator : AbstractValidator<RemoveUserFromTeamRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public RemoveUserFromTeamValidator()
    {
        this.ValidateTeamExists(dataWorker);
        this.ValidateDiscordUserOnTeam(dataWorker);
    }
}
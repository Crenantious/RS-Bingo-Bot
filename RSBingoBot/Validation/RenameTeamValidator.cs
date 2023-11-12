// <copyright file="RenameTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class RenameTeamValidator : AbstractValidator<RenameTeamRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public RenameTeamValidator()
    {
        this.ValidateTeamExists(dataWorker);
        this.ValidateNewTeamName(dataWorker);
    }
}
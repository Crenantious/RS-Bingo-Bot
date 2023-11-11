// <copyright file="DeleteTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class DeleteTeamValidator : AbstractValidator<DeleteTeamRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public DeleteTeamValidator()
    {
        this.ValidateTeamExists(dataWorker);
    }
}
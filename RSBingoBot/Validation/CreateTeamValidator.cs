﻿// <copyright file="CreateTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class CreateTeamValidator : AbstractValidator<CreateTeamRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public CreateTeamValidator()
    {
        this.ValidateNewTeamName(dataWorker);
    }
}
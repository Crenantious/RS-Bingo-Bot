// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : AbstractValidator<ViewEvidenceButtonRequest>
{
    IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public ViewEvidenceButtonValidator()
    {
        this.ValidateDiscordUserNotOnATeam(dataWorker, r => r.Interaction.User);
    }
}
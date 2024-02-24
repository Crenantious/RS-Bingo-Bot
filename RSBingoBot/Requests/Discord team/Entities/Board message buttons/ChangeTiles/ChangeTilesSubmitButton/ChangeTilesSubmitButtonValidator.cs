// <copyright file="ChangeTilesSubmitButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingoBot.Requests.Validation;

internal class ChangeTilesSubmitButtonValidator : BingoValidator<ChangeTilesSubmitButtonRequest>
{
    private const string ErrorMessage = "Must select a tile to change {0}.";

    public ChangeTilesSubmitButtonValidator()
    {
        UserInteraction(r => r.User);
        TeamExists(r => r.TeamId);

        RuleFor(r => r.DTO.ChangeFromTileBoardIndex)
            .NotNull()
            .WithMessage(ErrorMessage.FormatConst("from"));

        RuleFor(r => r.DTO.ChangeToTask)
            .NotNull()
            .WithMessage(ErrorMessage.FormatConst("to"));
    }
}
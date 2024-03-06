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

        RuleFor(r => r.DTO.TileBoardIndex)
            .NotNull()
            .WithMessage(ErrorMessage.FormatConst("from"));

        RuleFor(r => r.DTO.Task)
            .NotNull()
            .WithMessage(ErrorMessage.FormatConst("to"));
    }
}
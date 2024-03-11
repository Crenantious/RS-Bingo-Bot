// <copyright file="UpdateTeamBoardMessageButtonsValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests.Validation;
using FluentValidation;

internal class UpdateTeamBoardMessageButtonsValidator : Validator<UpdateTeamBoardMessageButtonsRequest>
{
    private const int RowIndex = 0;
    private const int ComponentIndex = 1;

    private const string NotEnoughComponentRows = "There must be at least {0} component row{1}.";
    private const string NotEnoughComponentsInRows = "There must be at least {0} components in row with index {1}.";

    public UpdateTeamBoardMessageButtonsValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => GetComponentRows(r))
            .Must(rows => rows.Count > RowIndex)
            .WithMessage(NotEnoughComponentRows.FormatConst(RowIndex + 1));

        RuleFor(r => GetComponentRows(r)[0])
           .Must(row => row.Count > ComponentIndex)
           .WithMessage(NotEnoughComponentsInRows.FormatConst(ComponentIndex + 1));
    }

    private static List<List<IComponent>> GetComponentRows(UpdateTeamBoardMessageButtonsRequest r) =>
        r.Message.Components.GetRows();
}
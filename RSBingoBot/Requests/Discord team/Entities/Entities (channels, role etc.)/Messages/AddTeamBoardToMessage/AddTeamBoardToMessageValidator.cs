// <copyright file="AddTeamBoardToMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class AddTeamBoardToMessageValidator : BingoValidator<AddTeamBoardToMessageRequest>
{
    public AddTeamBoardToMessageValidator()
    {
        TeamExists(r => r.Team.RowId);
    }
}
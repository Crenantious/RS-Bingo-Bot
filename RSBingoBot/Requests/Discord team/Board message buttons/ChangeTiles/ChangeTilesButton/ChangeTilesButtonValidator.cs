// <copyright file="ChangeTilesButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class ChangeTilesButtonValidator : BingoValidator<ChangeTilesButtonRequest>
{
    public ChangeTilesButtonValidator()
    {
        UserOnTeam(r => (r.InteractionArgs.User, r.TeamId));
    }
}
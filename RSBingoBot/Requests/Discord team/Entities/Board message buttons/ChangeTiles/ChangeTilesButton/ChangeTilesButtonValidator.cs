// <copyright file="ChangeTilesButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class ChangeTilesButtonValidator : BingoValidator<ChangeTilesButtonRequest>
{
    public ChangeTilesButtonValidator()
    {
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.TeamId));
    }
}
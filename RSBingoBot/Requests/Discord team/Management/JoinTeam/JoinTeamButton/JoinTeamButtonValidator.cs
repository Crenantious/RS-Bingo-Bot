// <copyright file="JoinTeamButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class JoinTeamButtonValidator : BingoValidator<JoinTeamButtonRequest>
{
    public JoinTeamButtonValidator()
    {
        UserNotOnATeam(r => r.GetDiscordInteraction().User, true);
    }
}
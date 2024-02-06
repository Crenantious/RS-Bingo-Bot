// <copyright file="CreateTeamButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class CreateTeamButtonValidator : BingoValidator<CreateTeamButtonRequest>
{
    public CreateTeamButtonValidator()
    {
        UserNotOnATeam(r => r.GetDiscordInteraction().User, true);
    }
}
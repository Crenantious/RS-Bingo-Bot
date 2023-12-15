// <copyright file="CreateTeamCategoryChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamCategoryChannelValidator : BingoValidator<CreateTeamCategoryChannelRequest>
{
    public CreateTeamCategoryChannelValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Team);
        RoleNotNull(r => r.DiscordTeam.Role);
    }
}
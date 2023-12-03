// <copyright file="CreateTeamGeneralChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamGeneralChannelValidator : Validator<CreateTeamGeneralChannelRequest>
{
    public CreateTeamGeneralChannelValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Team);
        RoleNotNull(r => r.DiscordTeam.Role);
        ChannelNotNull(r => r.DiscordTeam.CategoryChannel);
    }
}
// <copyright file="CreateTeamVoiceChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamVoiceChannelValidator : BingoValidator<CreateTeamVoiceChannelRequest>
{
    public CreateTeamVoiceChannelValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Team);
        RoleNotNull(r => r.DiscordTeam.Role);
        ChannelNotNull(r => r.DiscordTeam.CategoryChannel);
    }
}
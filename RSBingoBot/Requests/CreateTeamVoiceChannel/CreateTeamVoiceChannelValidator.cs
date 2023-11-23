// <copyright file="CreateTeamVoiceChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamVoiceChannelValidator : Validator<CreateTeamVoiceChannelRequest>
{
    public CreateTeamVoiceChannelValidator()
    {
        TeamExists(r => r.Team);
        ChannelNotNull(r => r.Category);
    }
}
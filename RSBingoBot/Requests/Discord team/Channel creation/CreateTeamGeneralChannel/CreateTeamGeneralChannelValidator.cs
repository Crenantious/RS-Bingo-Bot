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
        TeamExists(r => r.Team);
        ChannelNotNull(r => r.Category);
    }
}
// <copyright file="CreateTeamBoardChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamBoardChannelValidator : Validator<CreateTeamBoardChannelRequest>
{
    public CreateTeamBoardChannelValidator()
    {
        TeamExists(r => r.Team);
        ChannelNotNull(r => r.Category);
    }
}
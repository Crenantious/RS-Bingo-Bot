// <copyright file="InitialiseTeamBoardChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class InitialiseTeamBoardChannelValidator : Validator<InitialiseTeamBoardChannelRequest>
{
    public InitialiseTeamBoardChannelValidator()
    {
        ChannelNotNull(r => r.BoardChannel);
    }
}
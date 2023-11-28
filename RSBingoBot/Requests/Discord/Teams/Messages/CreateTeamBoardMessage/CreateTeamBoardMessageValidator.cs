// <copyright file="CreateTeamBoardMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamBoardMessageValidator : Validator<CreateTeamBoardMessageRequest>
{
    public CreateTeamBoardMessageValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Team);
        ChannelNotNull(r => r.DiscordTeam.BoardChannel);
    }
}
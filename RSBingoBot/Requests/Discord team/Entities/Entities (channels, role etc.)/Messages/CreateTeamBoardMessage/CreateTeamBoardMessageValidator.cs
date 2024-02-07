// <copyright file="CreateTeamBoardMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class CreateTeamBoardMessageValidator : BingoValidator<CreateTeamBoardMessageRequest>
{
    public CreateTeamBoardMessageValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Id);
        ChannelNotNull(r => r.DiscordTeam.BoardChannel);
    }
}
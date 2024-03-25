// <copyright file="UpdateTeamBoardMessageImageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class UpdateTeamBoardMessageImageValidator : Validator<UpdateTeamBoardMessageImageRequest>
{
    public UpdateTeamBoardMessageImageValidator()
    {
        DiscordMessageExists(r => r.DiscordTeam.BoardMessage);
    }
}
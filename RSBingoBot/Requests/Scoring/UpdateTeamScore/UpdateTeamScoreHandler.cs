// <copyright file="UpdateTeamScoreHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

internal class UpdateTeamScoreHandler : RequestHandler<UpdateTeamScoreRequest>
{
    protected override async Task Process(UpdateTeamScoreRequest request, CancellationToken cancellationToken)
    {
        request.DiscordTeam.Score.Calculate(request.Team.Tiles);
    }
}
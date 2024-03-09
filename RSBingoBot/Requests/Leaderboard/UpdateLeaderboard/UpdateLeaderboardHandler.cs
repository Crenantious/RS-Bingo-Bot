// <copyright file="UpdateLeaderboardHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using Imaging.Leaderboard;
using RSBingoBot.Discord;

internal class UpdateLeaderboardHandler : RequestHandler<UpdateLeaderboardRequest>
{
    protected override async Task Process(UpdateLeaderboardRequest request, CancellationToken cancellationToken)
    {
        var teams = DiscordTeam.ExistingTeams.Select(t => (t.Value.Name, t.Value.Score.Score));

        Image image = LeaderboardImage.Create(teams);
        request.Message!.Files.ElementAt(0)
            .SetContent(image, ".png");

        var messageServices = GetRequestService<IDiscordMessageServices>();
        await messageServices.Update(request.Message);
    }
}
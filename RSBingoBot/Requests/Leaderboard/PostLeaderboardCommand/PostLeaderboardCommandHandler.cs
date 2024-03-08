// <copyright file="PostLeaderboardCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using Imaging.Leaderboard;
using RSBingoBot.Discord;

internal class PostLeaderboardCommandHandler : CommandHandler<PostLeaderboardCommandRequest>
{
    private readonly MessageFactory messageFactory;

    public PostLeaderboardCommandHandler(MessageFactory messageFactory)
    {
        this.messageFactory = messageFactory;
    }

    protected override async Task Process(PostLeaderboardCommandRequest request, CancellationToken cancellationToken)
    {
        Message message = messageFactory.Create(request.Channel);

        var teams = DiscordTeam.ExistingTeams.Select(t => (t.Value.Name, t.Value.Score.Score));

        MessageFile leaderboard = new("Leaderboard");
        leaderboard.SetContent(LeaderboardImage.Create(teams), ".png");
        message.AddFile(leaderboard);

        var messageServices = GetRequestService<IDiscordMessageServices>();
        await messageServices.Send(message);
    }
}
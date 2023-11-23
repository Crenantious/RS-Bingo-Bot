// <copyright file="InitialiseTeamBoardChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingoBot.Requests;

internal class InitialiseTeamBoardChannelHandler : RequestHandler<InitialiseTeamBoardChannelRequest, Result<Message>>
{
    private readonly ButtonFactory buttonFactory;

    public InitialiseTeamBoardChannelHandler(ButtonFactory buttonFactory)
    {
        this.buttonFactory = buttonFactory;
    }

    protected override async Task<Message> Process(InitialiseTeamBoardChannelRequest request, CancellationToken cancellationToken)
    {
        // TODO: send a request to get the board image.
        Button changeTile = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Change tile"));
        Button submitEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Submit evidence"));
        Button submitDrop = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Submit drop"));
        Button viewEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "View evidence"));
        Button clearEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Clear evidence"));
        Button completeNextTileEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Complete next tile"));

        Message message = new();
        message.AddComponents(changeTile, submitEvidence, submitDrop, viewEvidence);

#if DEBUG
        message.AddComponents(clearEvidence, completeNextTileEvidence);
#endif

        message.Send(request.BoardChannel);
        return message;
    }
}
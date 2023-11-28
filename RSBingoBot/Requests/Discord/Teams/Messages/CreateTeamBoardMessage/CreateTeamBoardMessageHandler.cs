// <copyright file="CreateTeamBoardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using RSBingoBot.Requests;

internal class CreateTeamBoardMessageHandler : RequestHandler<CreateTeamBoardMessageRequest, Message>
{
    private readonly ButtonFactory buttonFactory;

    public CreateTeamBoardMessageHandler(ButtonFactory buttonFactory)
    {
        this.buttonFactory = buttonFactory;
    }

    protected override async Task<Message> Process(CreateTeamBoardMessageRequest request, CancellationToken cancellationToken)
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

        AddSuccess(new CreateTeamBoardMessageSuccess());
        return message;
    }
}
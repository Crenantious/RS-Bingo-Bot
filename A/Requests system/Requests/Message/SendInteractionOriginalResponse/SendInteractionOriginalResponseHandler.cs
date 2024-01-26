// <copyright file="SendInteractionOriginalResponseHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class SendInteractionOriginalResponseHandler : DiscordHandler<SendInteractionOriginalResponseRequest>
{
    private readonly InteractionResponseTracker responseTracker;

    public SendInteractionOriginalResponseHandler(InteractionResponseTracker responseTracker) =>
        this.responseTracker = responseTracker;

    protected override async Task Process(SendInteractionOriginalResponseRequest request, CancellationToken cancellationToken)
    {
        bool interactionAlreadyRespondedTo = await TrySendResponse(request);

        if (interactionAlreadyRespondedTo)
        {
            AddError(new SendInteractionOriginalResponseAlreadyRespondedError(request.Message.Interaction));
        }
        else
        {
            await HandleResponseSuccess(request.Message);
        }
    }

    private async Task<bool> TrySendResponse(SendInteractionOriginalResponseRequest request) =>
        !(await BadRequestCheck(InteractionRespondedToCode, () => SendResponse(request)));

    private static Task SendResponse(SendInteractionOriginalResponseRequest request) =>
        request.Message.Interaction.CreateResponseAsync(request.ResponseType, request.Message.GetInteractionResponseBuilder());

    private async Task HandleResponseSuccess(InteractionMessage message)
    {
        DiscordMessage? discordMessage = await GetOriginalResponse(message);
        if (discordMessage is null)
        {
            AddSuccess(new SendInteractionOriginalResponseSuccess(message));
            return;
        }

        message.DiscordMessage = discordMessage;
        responseTracker.Register(message.Interaction, message);
        AddSuccess(new SendInteractionOriginalResponseSuccess(message));
    }

    private static async Task<DiscordMessage?> GetOriginalResponse(InteractionMessage message)
    {
        try
        {
            return await message.Interaction.GetOriginalResponseAsync();
        }
        catch
        {
            return null;
        }
    }
}
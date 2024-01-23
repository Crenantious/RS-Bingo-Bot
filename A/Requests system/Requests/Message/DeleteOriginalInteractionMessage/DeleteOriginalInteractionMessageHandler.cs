// <copyright file="DeleteOriginalInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace DiscordLibrary.Requests;

internal class DeleteOriginalInteractionMessageHandler : DiscordHandler<DeleteOriginalInteractionMessageRequest, InteractionMessage>
{
    protected override async Task<InteractionMessage> Process(DeleteOriginalInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DiscordMessage message = await request.Interaction.GetOriginalResponseAsync();
            var interactionMessage = new InteractionMessage(request.Interaction, message);
            await message.DeleteAsync();
            AddSuccess(new DeleteOriginalInteractionMessageSuccess(request.Interaction));
            return interactionMessage;
        }
        catch (NotFoundException)
        {
            AddError(new DeleteOriginalInteractionMessageError(request.Interaction));
            return null!;
        }
    }
}
// <copyright file="DeleteOriginalInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace DiscordLibrary.Requests;

internal class DeleteOriginalInteractionMessageHandler : DiscordHandler<DeleteOriginalInteractionMessageRequest, InteractionMessage>
{
    protected override async Task<InteractionMessage> Process(DeleteOriginalInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        try
        {
            DiscordMessage message = await request.Interaction.GetOriginalResponseAsync();
            await messageServices.Delete(message);

            AddSuccess(new DeleteOriginalInteractionMessageSuccess(request.Interaction));

            // TODO: JR - fix the return (probably remove the entire class since there's no reason to treat this differently
            // from deleting a normal Message).
            return null!;
        }
        catch (NotFoundException)
        {
            AddError(new DeleteOriginalInteractionMessageError(request.Interaction));
            return null!;
        }
    }
}
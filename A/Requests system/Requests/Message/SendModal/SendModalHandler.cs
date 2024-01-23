// <copyright file="SendModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

internal class SendModalHandler : DiscordHandler<SendModalRequest>
{
    protected override async Task Process(SendModalRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await request.Modal.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.Modal,
                request.Modal.GetInteractionResponseBuilder());

            DiscordMessage message = await request.Modal.Interaction.GetOriginalResponseAsync();
            request.Modal.DiscordMessage = message;

            AddSuccess(new SendModalSuccess(request.Modal));
        }
        catch (BadRequestException e)
        {
            if (e.Code != InteractionRespondedToCode)
            {
                throw;
            }
            AddError(new SendModalError(request.Modal));
            AddError(new InternalError());
        }
    }
}
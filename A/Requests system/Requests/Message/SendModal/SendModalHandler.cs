// <copyright file="SendModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordExtensions;

internal class SendModalHandler : DiscordHandler<SendModalRequest>
{
    protected override async Task Process(SendModalRequest request, CancellationToken cancellationToken)
    {
        if (await request.Modal.Interaction.HasResponse())
        {
            AddError(new SendModalError(request.Modal));
        }
        else
        {
            await request.Modal.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.Modal,
                request.Modal.GetInteractionResponseBuilder());
            AddSuccess(new SendModalSuccess(request.Modal));
        }
    }
}
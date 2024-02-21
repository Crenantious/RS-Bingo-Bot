// <copyright file="SendInteractionFollowUpHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

internal class SendInteractionFollowUpHandler : DiscordHandler<SendInteractionFollowUpRequest>
{
    protected override async Task Process(SendInteractionFollowUpRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage? message = await TrySendResponse(request);

        if (message is null)
        {
            AddError(new SendInteractionFollowUpError(request.Message.Interaction));
            AddError(new InternalError());
        }
        else
        {
            request.Message.DiscordMessage = message;
            AddSuccess(new SendInteractionFollowUpSuccess(request.Message));
        }
    }

    private async Task<DiscordMessage?> TrySendResponse(SendInteractionFollowUpRequest request)
    {
        try
        {
            DiscordFollowupMessageBuilder builder = request.Message.GetFollowupMessageBuilder();
            return await request.Message.Interaction.CreateFollowupMessageAsync(builder);
        }
        catch (BadRequestException)
        {
            return null;
        }
    }
}
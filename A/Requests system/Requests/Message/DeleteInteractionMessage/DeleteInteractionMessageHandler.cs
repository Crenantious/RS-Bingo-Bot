// <copyright file="DeleteInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Exceptions;

internal class DeleteInteractionMessageHandler : DiscordHandler<DeleteInteractionMessageRequest>
{
    protected override async Task Process(DeleteInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await request.Message.DiscordMessage.DeleteAsync();
            AddSuccess(new DeleteInteractionMessageSuccess(request.Message));
        }
        catch (NotFoundException)
        {
            AddError(new DeleteInteractionMessageDoesntExistError(request.Message));
        }
    }
}
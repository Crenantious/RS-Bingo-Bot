// <copyright file="CloseButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.InteractionHandlers;

using RSBingoBot.Requests;

internal class CloseButtonHandler : InteractionHandler<ConclueInteractionrequest>
{
    protected override async Task Process(ConclueInteractionrequest request, CancellationToken cancellationToken)
    {
        await base.Handle(request, cancellationToken);

        await request.ParentHandler.Conclude();
    }
}
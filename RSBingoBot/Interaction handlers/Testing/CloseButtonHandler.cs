// <copyright file="CloseButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.InteractionHandlers;

using RSBingoBot.Requests;

internal class CloseButtonHandler : InteractionHandler<CloseButtonRequest>
{
    protected override async Task Process(CloseButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Handle(request, cancellationToken);

        await request.ParentHandler.Conclude();
    }
}
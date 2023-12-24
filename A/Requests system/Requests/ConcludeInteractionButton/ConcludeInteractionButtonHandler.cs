// <copyright file="ConcludeInteractionButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class ConcludeInteractionButtonHandler : ButtonHandler<ConcludeInteractionButtonRequest>
{
    protected override async Task Process(ConcludeInteractionButtonRequest request, CancellationToken cancellationToken)
    {
        await request.handler.Conclude();
        AddSuccess(new ConcludeInteractionButtonSuccess(request.handler));
    }
}
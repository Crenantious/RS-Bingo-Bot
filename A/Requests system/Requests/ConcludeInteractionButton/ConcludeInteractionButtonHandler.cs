// <copyright file="ConcludeInteractionButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;

internal class ConcludeInteractionButtonHandler : ButtonHandler<ConcludeInteractionButtonRequest>
{
    protected override bool SendKeepAliveMessage => false;

    protected override async Task Process(ConcludeInteractionButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        await TryDeleteMessages(request, messageServices);
        await request.Tracker.ConcludeInteraction();

        AddSuccess(new ConcludeInteractionButtonSuccess(request.Tracker));
    }

    private static async Task TryDeleteMessages(ConcludeInteractionButtonRequest request, IDiscordMessageServices messageServices)
    {
        if (request.MessagesToDelete is not null)
        {
            foreach (var message in request.MessagesToDelete)
            {
                await messageServices.Delete(message);
            }
        }
    }
}
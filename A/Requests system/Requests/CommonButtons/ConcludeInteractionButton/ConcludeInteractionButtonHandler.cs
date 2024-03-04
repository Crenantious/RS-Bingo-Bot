// <copyright file="ConcludeInteractionButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;

public class ConcludeInteractionButtonHandler<TRequest> : ButtonHandler<TRequest>
    where TRequest : ConcludeInteractionButtonRequest
{
    protected override bool SendKeepAliveMessage => false;

    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        await TryDeleteMessages(request, messageServices);
        await request.Tracker.ConcludeInteraction();
        AddSuccess(new ConcludeInteractionButtonSuccess(request.Tracker));
    }

    private static async Task TryDeleteMessages(TRequest request, IDiscordMessageServices messageServices)
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

public class ConcludeInteractionButtonHandler : ConcludeInteractionButtonHandler<ConcludeInteractionButtonRequest>
{

}
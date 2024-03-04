// <copyright file="SubmitEvidenceCloseButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal class SubmitEvidenceCloseButtonHandler : ConcludeInteractionButtonHandler<SubmitEvidenceCloseButtonRequest>
{
    protected override async Task Process(SubmitEvidenceCloseButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Process(request, cancellationToken);

        var messageServices = GetRequestService<IDiscordMessageServices>();
        messageServices.UnregisterMessageCreatedHandler(request.MessageCreatedDEHSubscriptionId);
    }
}
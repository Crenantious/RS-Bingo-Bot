// <copyright file="SendModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;

internal class SendModalHandler : DiscordHandler<SendModalRequest>
{
    private readonly InteractionResponseTracker responseTracker;

    public SendModalHandler(InteractionResponseTracker responseTracker) =>
        this.responseTracker = responseTracker;

    protected override async Task Process(SendModalRequest request, CancellationToken cancellationToken)
    {
        var messageService = GetRequestService<IDiscordInteractionMessagingServices>();
        var result = await messageService.SendOriginalResponse(DSharpPlus.InteractionResponseType.Modal, request.Modal);

        if (result.IsSuccess)
        {
            AddSuccess(new SendModalSuccess(request.Modal));
        }
        else if (result.HasError<SendInteractionOriginalResponseAlreadyRespondedError>())
        {
            AddError(new SendModalAlreadyRespondedError(request.Modal));
            AddError(new InternalError());
        }
    }
}
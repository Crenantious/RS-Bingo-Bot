// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;
using RSBingo_Common;

public abstract class InteractionHandler<TRequest> : RequestHandler<TRequest>, IInteractionHandler
    where TRequest : IInteractionRequest
{
    private readonly InteractionsTracker interactionTrackers;
    private readonly InteractionMessageFactory interactionMessageFactory;

    private bool isConcluded = false;

    protected InteractionTracker<TRequest> InteractionTracker = null!;

    /// <summary>
    /// If true, automatically responds to the interaction with an empty keep alive message. The user sees a "thinking" state.
    /// </summary>
    protected virtual bool SendKeepAliveMessage => true;

    /// <summary>
    /// The next response sent to the interaction will use this value, regardless of what the builder says (how Discord workss).
    /// </summary>
    protected virtual bool SendKeepAliveMessageIsEphemeral => true;

    protected DiscordInteraction Interaction { get; set; } = null!;

    public InteractionHandler()
    {
        this.interactionTrackers = General.DI.Get<InteractionsTracker>();
        this.interactionMessageFactory = General.DI.Get<InteractionMessageFactory>();
    }

    private protected override async Task PreProcess(TRequest request, CancellationToken cancellationToken)
    {
        Interaction = request.GetDiscordInteraction();
        InteractionTracker = new InteractionTracker<TRequest>(request, Interaction);
        interactionTrackers.Add(InteractionTracker);

        if (SendKeepAliveMessage)
        {
            var service = GetRequestService<IDiscordInteractionMessagingServices>();
            await service.SendKeepAlive(Interaction, SendKeepAliveMessageIsEphemeral);
        }
    }
}
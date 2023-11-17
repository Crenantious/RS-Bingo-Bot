// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using FluentResults;

// TODO: JR - track instances against DiscordUsers to be able to limit how many they have open and potentially time them out.
public abstract class InteractionHandler<TRequest, TComponent> : RequestHandler<TRequest, Result>, IInteractionHandler
    where TRequest : IInteractionRequest
    where TComponent : IDiscordComponent
{
    // 100 is arbitrary. Could remove the need all together but other handlers should use one so it's kept to ensure that.
    private static SemaphoreSlim semaphore = new(100);

    private IInteractionHandler? parent;
    private List<Message> messagesForCleanup = new();
    private bool isClosed = false;
    private bool isConcluded = false;

    protected TComponent Component { get; private set; }
    protected ComponentInteractionCreateEventArgs InteractionArgs { get; private set; }
    
    public static event Func<IInteractionHandler, Task> Closed;
    public static event Func<IInteractionHandler, Task> Concluded;

    protected InteractionHandler() : base(semaphore)
    {

    }

    protected override Task Process(TRequest request, CancellationToken cancellationToken)
    {
        parent = request.ParentHandler;
        Component = MetaData.Get<TComponent>();
        InteractionArgs = MetaData.Get<ComponentInteractionCreateEventArgs>();
        Closed += OnClose;
        Concluded += OnConclude;
        return Task.CompletedTask;
    }

    public async Task Close()
    {
        if (isClosed) { return; }

        // Delete all messages in messagesForCleanup.
        Closed?.Invoke(this);
        parent?.Close();

        throw new NotImplementedException();
    }

    public async Task Conclude()
    {
        if (isConcluded) { return; }

        // Remove self from active interaction handlers.
        Concluded?.Invoke(this);
        parent?.Conclude();

        throw new NotImplementedException();
    }

    private async Task OnClose(IInteractionHandler hander)
    {
        if (hander == parent) { await Close(); }
    }

    private async Task OnConclude(IInteractionHandler hander)
    {
        if (hander == parent) { await Conclude(); }
    }

    protected void AddMessageForCleanup(Message message)
    {
        messagesForCleanup.Add(message);
    }

    private void CleanupMessages()
    {
        throw new NotImplementedException();
    }
}
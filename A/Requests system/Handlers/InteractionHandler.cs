// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;

// TODO: JR - track instances against DiscordUsers to be able to limit how many they have open and potentially time them out.
// TODO: JR - add a CascadeMessageDelete request that utilises ICasecadeDeleteMessages (in RSBingoBot).
public abstract class InteractionHandler<TRequest, TComponent> : RequestHandler<TRequest, Result>, IInteractionHandler
    where TRequest : IInteractionRequest
    where TComponent : IInteractable
{
    // 100 is arbitrary. Could remove the need all together but other handlers should use one so it's kept to ensure that.
    private static SemaphoreSlim semaphore = new(100);

    private bool isConcluded = false;

    protected DiscordUser DiscordUser { get; private set; } = null!;
    protected User User { get; private set; } = null!;

    internal IInteractionHandler ParentHandler { get; set; } = null!;

    protected InteractionHandler() : base(semaphore)
    {

    }

    protected override Task Process(TRequest request, CancellationToken cancellationToken)
    {
        DiscordUser = request.InteractionArgs.Interaction.User; ;
        User = DiscordUser.GetDBUser(DataWorker)!;
        return Task.CompletedTask;
    }

    public async Task Conclude()
    {
        if (isConcluded) { return; }

        // Remove self from active interaction handlers.

        throw new NotImplementedException();
    }

    #region Add result responses

    /// <inheritdoc cref="RequestHandler{TRequest, TResult}.AddSuccess(ISuccess)"/>
    protected Message AddSuccess(IInteractionSuccess success)
    {
        base.AddSuccess(success);
        return success.DiscordMessage;
    }

    /// <inheritdoc cref="RequestHandler{TRequest, TResult}.AddWarning(IWarning)"/>
    protected Message AddWarning(IInteractionWarning warning)
    {
        base.AddWarning(warning);
        return warning.DiscordMessage;
    }

    /// <inheritdoc cref="RequestHandler{TRequest, TResult}.AddError(IError)"/>
    protected Message AddError(IInteractionError error)
    {
        base.AddError(error);
        return error.DiscordMessage;
    }

    #endregion
}
// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using System.Text;

// TODO: JR - track instances against DiscordUsers to be able to limit how many they have open and potentially time them out.
// TODO: JR - add a CascadeMessageDelete request that utilises ICasecadeDeleteMessages (in RSBingoBot).
public abstract class InteractionHandler<TRequest, TArgs> : RequestHandler<TRequest>, IInteractionHandler
    where TRequest : IInteractionRequest<TArgs>
    where TArgs : InteractionCreateEventArgs
{
    // 100 is arbitrary. Could remove the need all together but other handlers should use one so it's kept to ensure that.
    private static SemaphoreSlim semaphore = new(100);

    private bool isConcluded = false;

    protected List<InteractionMessage> ResponseMessages { get; set; } = new();
    protected TArgs InteractionArgs { get; set; } = null!;

    internal IInteractionHandler ParentHandler { get; set; } = null!;

    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();

    protected InteractionHandler() : base(semaphore)
    {

    }
    internal protected override Task PreProcess(TRequest request, CancellationToken cancellationToken)
    {
        InteractionArgs = request.InteractionArgs;
        return Task.CompletedTask;
    }

    internal protected override Task PostProcess(TRequest request, CancellationToken cancellationToken)
    {
        ResponseMessages.ForEach(async m => await m.Send());
        return Task.CompletedTask;
    }

    protected User? GetUser() =>
        InteractionArgs.Interaction.User.GetDBUser(DataWorker);

    public virtual async Task Conclude()
    {
        if (isConcluded) { return; }

        // Remove self from active interaction handlers.

        throw new NotImplementedException();
    }

    protected void DeleteResponses()
    {
        ResponseMessages.ForEach(async m => await m.Delete());
    }

    #region Add result responses

    public InteractionMessage AddResponses(ResultBase result, bool addToLastResponse = true) =>
        AddSuccesses(result.Successes.GetDiscordResponses(), addToLastResponse) +
        AddErrors(result.Errors.GetDiscordResponses(), addToLastResponse);

    /// <summary>
    /// Adds the <paramref name="success"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddSuccess(ISuccess)"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddSuccess(ISuccess success, bool addToLastResponse = true)
    {
        base.AddSuccess(success);
        return AddResponseCommon(success.Message, addToLastResponse);
    }

    /// <summary>
    /// Adds the <paramref name="successes"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddSuccesses(IEnumerable{ISuccess})"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddSuccesses(IEnumerable<ISuccess> successes, bool addToLastResponse = true)
    {
        base.AddSuccesses(successes);
        return AddResponseCommon(successes, addToLastResponse);
    }

    /// <summary>
    /// Adds the <paramref name="warning"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddWarning(IWarning)"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddWarning(IWarning warning, bool addToLastResponse = true)
    {
        base.AddWarning(warning);
        return AddResponseCommon(warning.Message, addToLastResponse);
    }

    /// <summary>
    /// Adds the <paramref name="warnings"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddWarnings(IEnumerable{IWarning})"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddWarnings(IEnumerable<IWarning> warnings, bool addToLastResponse = true)
    {
        base.AddWarnings(warnings);
        return AddResponseCommon(warnings, addToLastResponse);
    }

    /// <summary>
    /// Adds the <paramref name="error"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddError(IError)"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddError(IError error, bool addToLastResponse = true)
    {
        base.AddError(error);
        return AddResponseCommon(error.Message, addToLastResponse);
    }

    /// <summary>
    /// Adds the <paramref name="errors"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddErrors(IEnumerable{IError})"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddErrors(IEnumerable<IError> errors, bool addToLastResponse = true)
    {
        base.AddErrors(errors);
        return AddResponseCommon(errors, addToLastResponse);
    }

    private InteractionMessage AddResponseCommon(string content, bool addToLastResponse)
    {
        var message = new InteractionMessage(InteractionArgs.Interaction).WithContent(content);

        if (addToLastResponse)
        {
            AppendResponse(message);
        }
        return message;
    }

    private InteractionMessage AddResponseCommon(IEnumerable<IReason> reasons, bool addToLastResponse)
    {
        StringBuilder sb = new();

        foreach (IReason reason in reasons)
        {
            sb.AppendLine(reason.Message);
        }

        return AddResponseCommon(sb.ToString(), addToLastResponse);
    }

    private void AppendResponse(InteractionMessage message)
    {
        if (ResponseMessages.Any())
        {
            ResponseMessages[^1] += message;
        }
        else
        {
            ResponseMessages.Add(message);
        }
    }

    #endregion
}
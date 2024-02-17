// <copyright file="InteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using System.Text;

public abstract class InteractionHandler<TRequest> : RequestHandler<TRequest>, IInteractionHandler
    where TRequest : IInteractionRequest
{
    private readonly InteractionsTracker interactionTrackers;

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

    // TODO: JR - remove this as responses should be sent directly via a request service or a pipeline behaviour.
    protected List<InteractionMessage> ResponseMessages { get; set; } = new();
    protected DiscordInteraction Interaction { get; set; } = null!;

    // TODO: JR - remove this as not all handlers need it thus it should not be made.
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();

    public InteractionHandler()
    {
        this.interactionTrackers = (InteractionsTracker)General.DI.GetService(typeof(InteractionsTracker))!;
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

    protected User? GetUser() =>
        Interaction.User.GetDBUser(DataWorker);

    // TODO: JR - remove.
    protected void DeleteResponses()
    {
        ResponseMessages.ForEach(async m => await m.Delete());
    }

    // TODO: JR - cleanup region and remove old systems.
    #region Add result responses

    public InteractionMessage AddResponses(ResultBase result, bool addToLastResponse = true) =>
        AddSuccessResponses(result.Successes.GetDiscordResponses(), addToLastResponse) +
        AddErrorResponses(result.Errors.GetDiscordResponses(), addToLastResponse);

    /// <summary>
    /// Adds the <paramref name="success"/> according to <see cref="RequestHandlerBase{TRequest, TResult}.AddSuccess(ISuccess)"/>,
    /// converts it to an <see cref="InteractionMessage"/> and potentially appends that to <see cref="ResponseMessages"/>.
    /// </summary>
    /// <param name="addToLastResponse"><see langword="true"/>: either appends the <see cref="InteractionMessage"/>
    /// to the last message in <see cref="ResponseMessages"/> or adds it if there are no responses yet.<br/>
    /// <see langword="false"/>: nothing.</param>
    protected InteractionMessage AddSuccessResponse<T>(T success, bool addToLastResponse = true)
        where T : class, ISuccess, IDiscordResponse
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
    protected InteractionMessage AddSuccessResponses<T>(IEnumerable<T> successes, bool addToLastResponse = true)
        where T : class, ISuccess, IDiscordResponse
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
    protected InteractionMessage AddWarningResponse<T>(T warning, bool addToLastResponse = true)
        where T : class, IWarning, IDiscordResponse
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
    protected InteractionMessage AddWarningResponses<T>(IEnumerable<T> warnings, bool addToLastResponse = true)
        where T : class, IWarning, IDiscordResponse
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
    protected InteractionMessage AddErrorResponse<T>(T error, bool addToLastResponse = true)
        where T : class, IError, IDiscordResponse
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
    protected InteractionMessage AddErrorResponses<T>(IEnumerable<T> errors, bool addToLastResponse = true)
        where T : class, IError, IDiscordResponse
    {
        base.AddErrors(errors);
        return AddResponseCommon(errors, addToLastResponse);
    }

    private InteractionMessage AddSuccessResponses(IEnumerable<ISuccess> successes, bool addToLastResponse = true)
    {
        base.AddSuccesses(successes);
        return AddResponseCommon(successes, addToLastResponse);
    }

    private InteractionMessage AddErrorResponses(IEnumerable<IError> errors, bool addToLastResponse = true)
    {
        base.AddErrors(errors);
        return AddResponseCommon(errors, addToLastResponse);
    }

    private InteractionMessage AddResponseCommon(string content, bool addToLastResponse)
    {
        var message = new InteractionMessage(Interaction).WithContent(content);

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
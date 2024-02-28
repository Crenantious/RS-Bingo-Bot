// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;

public class Validator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IBaseRequest
{
    private const string TooManyInteractionInstances = "You can only interact with {0} instance{1} of '{2}' at a time.";

    private readonly RequestsTracker requestsTracker;
    private readonly InteractionsTracker interactionTrackers;

    // TODO: JR - decide how to word this.
    protected const string ObjectIsNull = "{0} cannot be null.";
    protected const string UserIsNull = "User cannot be null.";
    protected const string MemberIsNull = "Member cannot be null.";
    protected const string ChannelDoesNotExist = "The channel does not exist.";
    protected const string RoleDoesNotExist = "The role does not exist.";
    protected const string DiscordMessageDoesNotExist = "The Discord message does not exist.";
    protected const string IncorrectUserInteraction = "Only the user {0} can interact with this.";

    public Validator()
    {
        requestsTracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
        interactionTrackers = (InteractionsTracker)General.DI.GetService(typeof(InteractionsTracker))!;
    }

    public virtual IEnumerable<SemaphoreSlim> GetSemaphores(TRequest request)
    {
        return new List<SemaphoreSlim>();
    }

    protected void NotNull(Func<TRequest, object?> func, string message)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(message);
    }

    protected void UserNotNull(Func<TRequest, DiscordUser?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    protected void MemberNotNull(Func<TRequest, DiscordMember?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(MemberIsNull);
    }

    protected void ChannelNotNull(Func<TRequest, DiscordChannel?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ChannelDoesNotExist);
    }

    protected void RoleNotNull(Func<TRequest, DiscordRole?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(RoleDoesNotExist);
    }

    protected void DiscordMessageExists(Func<TRequest, Message> func)
    {
        RuleFor(r => func(r).DiscordMessage)
            .NotNull()
            .WithMessage(DiscordMessageDoesNotExist);
    }

    // TODO: JR - find a better way to do this.
    /// <summary>
    /// Ensures the <see cref="DiscordUser"/> responsible for the interaction is that return from <paramref name="func"/>.<br/>
    /// Throws if <typeparamref name="TRequest"/> is not an <see cref="IInteractionRequest"/>.
    /// </summary>
    /// <param name="func">If the <see cref="DiscordUser"/> is <see langword="null"/> the case is ignored.</param>
    /// <exception cref="InvalidCastException"/>
    protected void UserInteraction(Func<TRequest, DiscordUser?> func)
    {
        RuleFor(r => ((IInteractionRequest)r).GetDiscordInteraction().User)
            .Must((r, u) => func(r) is null || func(r)! == u)
            .WithMessage((r, u) => IncorrectUserInteraction.FormatConst(u.Username));
    }

    ///<summary>
    /// Verifies that the amount of active interactions that satisfies <paramref name="constraints"/> is at most <paramref name="max"/>.<br/>
    ///</summary>
    /// <param name="message">The message to send if this is invalid.</param>
    /// <param name="max">The maximum amount that can be active at once for this to be valid.</param>
    protected void ActiveInteractions<TCompareRequest>(Func<TRequest, InteractionTracker<TCompareRequest>, bool> constraints, string message, int max)
        where TCompareRequest : IInteractionRequest
    {
        RuleFor<Func<InteractionTracker<TCompareRequest>, bool>>(r => (compareRequest) => constraints(r, compareRequest))
            .Must(f => interactionTrackers.ActiveCount<TCompareRequest>(f) < max)
            .WithMessage(message);
    }

    /// <param name="maxInstances">The maximum amount of instances allowed to be active under the conditions at a time.</param>
    /// <param name="name">The name of the interaction e.g. "Next page button".</param>
    public string GetTooManyInteractionInstancesError(int max, string name) =>
        TooManyInteractionInstances.FormatConst(max, max == 1 ? "" : "s", name);
}
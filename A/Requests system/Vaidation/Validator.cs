// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;

public class Validator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IBaseRequest
{
    private readonly RequestsTracker requestsTracker;

    // TODO: JR - decide how to word this.
    protected const string ObjectIsNull = "{0} cannot be null.";
    protected const string UserIsNull = "User cannot be null.";
    protected const string MemberIsNull = "Member cannot be null.";
    protected const string ChannelDoesNotExist = "The channel does not exist.";
    protected const string RoleDoesNotExist = "The role does not exist.";
    protected const string DiscordMessageDoesNotExist = "The Discord message does not exist.";

    public Validator()
    {
        requestsTracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    public virtual IEnumerable<SemaphoreSlim> GetSemaphores(TRequest request)
    {
        return new List<SemaphoreSlim>();
    }

    protected void NotNull(Func<TRequest, object?> func, string name)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ObjectIsNull.FormatConst(name));
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

    ///<summary>
    /// Verifies that the amount of active requests that satisfies <paramref name="constraints"/> is at most <paramref name="max"/>.<br/>
    ///</summary>
    /// <param name="message">The message to send if this is invalid.</param>
    /// <param name="max">The maximum amount that can be active at once for this to be valid.</param>
    protected void ActiveRequestInstances<TCompareRequest>(Func<TRequest, TCompareRequest, bool> constraints, string message, int max)
        where TCompareRequest : IBaseRequest
    {
        // TODO : JR - this doesn't work for interaction instances since the request completes
        // but the message for the user to interact with still exists, thus won't count as an active instance.
        RuleFor<Func<TCompareRequest, bool>>(r => (compareRequest) => constraints(r, compareRequest))
            .Must(f => requestsTracker.ActiveCount(f) <= max)
            .WithMessage(message);
    }
}
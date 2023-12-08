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
    private readonly InteractionHandlersTracker handlersTracker;

    // TODO: JR - decide how to word this.
    protected const string ObjectIsNull = "{0} cannot be null.";
    protected const string UserIsNull = "User cannot be null.";
    protected const string ChannelDoesNotExist = "The channel does not exist.";
    protected const string RoleDoesNotExist = "The role does not exist.";
    protected const string DiscordMessageDoesNotExist = "The Discord message does not exist.";

    internal IReadOnlyList<SemaphoreSlim> Semaphores { get; private set; } = new List<SemaphoreSlim>().AsReadOnly();

    public Validator()
    {
        handlersTracker = (InteractionHandlersTracker)General.DI.GetService(typeof(InteractionHandlersTracker))!;
    }

    protected void SetSemaphores(params SemaphoreSlim[] semaphores)
    {
        Semaphores = semaphores.AsReadOnly();
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

    /// <inheritdoc cref="InteractionHandlersTracker.IsActive{TRequest}(Func{TRequest, bool})"/>
    protected void RequestHandlerInstanceExists<TCompareRequest>(Func<TRequest, TCompareRequest, bool> constraints, string message)
    {
        RuleFor<Func<TCompareRequest, bool>>(r => (compareRequest) => constraints(r, compareRequest))
            .Must(f => handlersTracker.IsActive(f))
            .WithMessage(message);
    }
}
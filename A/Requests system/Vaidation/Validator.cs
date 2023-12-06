// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

public class Validator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IBaseRequest
{
    // TODO: JR - decide how to word this.
    internal protected const string ObjectIsNull = "{0} cannot be null.";
    internal protected const string UserIsNull = "User cannot be null.";
    internal protected const string ChannelDoesNotExist = "The channel does not exist.";
    internal protected const string RoleDoesNotExist = "The role does not exist.";

    public void NotNull(Func<TRequest, object?> func, string name)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ObjectIsNull.FormatConst(name));
    }

    public void UserNotNull(Func<TRequest, DiscordUser?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void ChannelNotNull(Func<TRequest, DiscordChannel?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ChannelDoesNotExist);
    }

    public void RoleNotNull(Func<TRequest, DiscordRole?> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(RoleDoesNotExist);
    }
}
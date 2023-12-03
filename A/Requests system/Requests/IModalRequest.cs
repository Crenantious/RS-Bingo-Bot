// <copyright file="IModalRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;

public interface IModalRequest : IRequest<Result>
{
    /// <summary>
    /// Value will be set by the framework.
    /// </summary>
    public ModalSubmitEventArgs InteractionArgs { get; set; }
}
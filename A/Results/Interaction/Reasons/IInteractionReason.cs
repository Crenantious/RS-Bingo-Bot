// <copyright file="IInteractionReason.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using FluentResults;

public interface IInteractionReason : IReason
{
    public Message DiscordMessage { get; }
}
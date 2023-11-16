// <copyright file="IInteractionReason.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal interface IInteractionReason : IReason
{
    public const string DiscordMessageMetaDataKey = "DiscordMessage";
}
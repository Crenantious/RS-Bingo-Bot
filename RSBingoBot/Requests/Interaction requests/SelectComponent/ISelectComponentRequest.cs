// <copyright file="ISelectComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DiscordComponents;

internal interface ISelectComponentRequest : IInteractionRequest
{
    /// <summary>
    /// Value will be set when the request is being processed.
    /// </summary>
    // TODO: JR - put the base requests, handlers and validators in their own assembly and make this internal.
    public SelectComponent SelectComponent { get; set; }
}
// <copyright file="ICommandRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public interface ICommandRequest : IInteractionRequest
{
    internal const string InteractionContextMetaDataKey = "InteractionContext";
}
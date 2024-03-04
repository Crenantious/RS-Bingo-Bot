// <copyright file="MessageReactedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus.EventArgs;

public class MessageReactedDEH : DiscordEventHandlerBase<MessageReactionAddEventArgs>
{
    public MessageReactedDEH() =>
        Client.MessageReactionAdded += OnEvent;
}
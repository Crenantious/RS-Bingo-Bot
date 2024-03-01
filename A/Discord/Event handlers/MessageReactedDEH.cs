// <copyright file="MessageReactedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

/// <summary>
/// Handles which subscribers to call when the <see cref="DiscordClient.MessageReactionAdded"/> event is fired based off given constraints.
/// </summary>
public class MessageReactedDEH : DiscordEventHandlerBase<MessageReactionAddEventArgs, MessageReactedDEH.Constraints>
{
    public record Constraints(DiscordChannel? channel = null, DiscordUser? user = null, string? emojiName = null);

    public MessageReactedDEH() =>
        DiscordClient.MessageReactionAdded += OnEvent;

    /// <inheritdoc/>
    public override List<object> GetConstraintValues(Constraints constriants) =>
        new() { constriants.channel, constriants.user, constriants.emojiName };

    /// <inheritdoc/>
    public override List<object> GetArgValues(MessageReactionAddEventArgs args) =>
        new() { args.Channel, args.User, args.Emoji.Name };
}
// <copyright file="ModalSubmittedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public class ModalSubmittedDEH : DiscordEventHandlerBase<ModalSubmitEventArgs>
{
    public record Constraints(DiscordUser? user = null, string? customId = null);

    public ModalSubmittedDEH() =>
        Client.ModalSubmitted += OnEvent;
}
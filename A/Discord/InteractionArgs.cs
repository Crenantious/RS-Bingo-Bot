// <copyright file="InteractionArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary;

using DiscordLibrary.RequestHandlers;
using DSharpPlus.EventArgs;

public record InteractionArgs(InteractionCreateEventArgs DiscordArgs, IInteractionHandler? ParentHandler);
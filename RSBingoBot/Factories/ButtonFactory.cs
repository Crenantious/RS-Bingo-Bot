// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Interfaces;
using RSBingoBot.Requests;
using RSBingoBot.Services;

internal static class ButtonFactory
{
    public static DiscordButtonComponent GetClose(CloseButtonRequest request, DiscordUser? user = null)
    {
        string id = Guid.NewGuid().ToString();
        DiscordButtonComponent button = new(DSharpPlus.ButtonStyle.Primary, id, "Close");
        DiscordInteractionServices.RegisterInteractionHandler(request, new(customId: id, user: user));
        return button;
    }
}
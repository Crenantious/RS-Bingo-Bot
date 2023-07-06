// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DSharpPlus.Entities;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests;
using RSBingoBot.Services;

internal static class ButtonFactory
{
    public static DiscordButton GetClose(CloseButtonRequest request, DiscordUser? user = null)
    {
        string id = Guid.NewGuid().ToString();
        DiscordButton button = new(new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, id, "Close"));
        DiscordInteractionServices.RegisterInteractionHandler(request, new(customId: id, user: user));
        return button;
    }
}
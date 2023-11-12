// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DSharpPlus.Entities;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Interactions;
using RSBingoBot.Requests;

// TODO: JR - Put each factory (e.g. Close button) into its own class and have them injected.
internal static class ButtonFactory
{
    public static DiscordButton CreateClose(CloseButtonRequest request, DiscordUser? user = null)
    {
        string id = Guid.NewGuid().ToString();
        DiscordButton button = new(new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, id, "Close"));
        button.Register(request, user: user);
        return button;
    }
}
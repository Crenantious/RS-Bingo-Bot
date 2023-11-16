// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DSharpPlus.Entities;
using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;

// TODO: JR - Put each factory (e.g. Close button) into its own class and have them injected.
public static class ButtonFactory
{
    public static DiscordButton Create(IButtonRequest request, DiscordUser? user = null)
    {
        string id = Guid.NewGuid().ToString();
        DiscordButton button = new(new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, id, "Close"));
        button.Register(request, new(User: user));
        return button;
    }
}
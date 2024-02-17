// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

public class ButtonFactory : InteractableComponentFactory<ButtonInfo, Button, IButtonRequest>
{
    public ButtonInfo CloseButton => new(DSharpPlus.ButtonStyle.Primary, "Close");

    internal protected override Button Create(ButtonInfo buttonInfo) =>
        new Button(buttonInfo);

    public Button CreateConcludeInteraction(ConcludeInteractionButtonRequest request) =>
        Create(CloseButton, () => request);
}
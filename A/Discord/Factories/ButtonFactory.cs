// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using RSBingoBot.Requests;

public class ButtonFactory : InteractableComponentFactory<ButtonInfo, Button, IButtonRequest>
{
    public ButtonInfo CloseButton => new(DSharpPlus.ButtonStyle.Primary, "Close");
    public ButtonInfo BackButton => new(DSharpPlus.ButtonStyle.Primary, "Back");

    internal protected override Button Create(ButtonInfo buttonInfo) =>
        new Button(buttonInfo);

    public Button CreateConcludeInteraction(Func<ConcludeInteractionButtonRequest> request) =>
        Create(CloseButton, request);

    public Button CreateSelectComponentBackButton(Func<SelectComponentBackButtonRequest> request) =>
        Create(BackButton, request);
}
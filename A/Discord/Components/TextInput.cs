// <copyright file="TextInput.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class TextInput : Component, IInteractable
{
    public override DiscordComponent DiscordComponent { get; }
    public override string Name { get; protected set; }

    public TextInput(TextInputComponent textInput) : base(textInput.CustomId)
    {
        DiscordComponent = textInput;
        Name = $"({nameof(TextInput)}) {textInput.Label}";
    }
}
// <copyright file="TextInputFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.Entities;

// TODO: JR - check to see if this should inherit InteractableComponentFactory.
public class TextInputFactory
{
    public TextInput Create(TextInputComponent info) =>
        new TextInput(info);
}
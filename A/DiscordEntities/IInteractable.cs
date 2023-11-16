// <copyright file="IInteractable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

internal interface IInteractable
{
    // TODO: JR - figure out if this should go here. Perhaps not all interactables have an id.
    public string CustomId { get; }
}
// <copyright file="InteractableComponentAlreadySubscribed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

internal class InteractableComponentAlreadySubscribed : Exception
{
    private const string message = "The component is already subscribed to receive interactions.";

    public InteractableComponentAlreadySubscribed() : base()
    {

    }
}
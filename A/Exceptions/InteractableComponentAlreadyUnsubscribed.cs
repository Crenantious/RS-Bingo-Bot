// <copyright file="InteractableComponentAlreadyUnsubscribed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

internal class InteractableComponentAlreadyUnsubscribed : Exception
{
    private const string message = "The component has already been unsubscribed from receiving interactions.";

    public InteractableComponentAlreadyUnsubscribed() : base()
    {

    }
}
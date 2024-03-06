// <copyright file="InteractableComponentNotSubscribed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

internal class InteractableComponentNotSubscribed : Exception
{
    private const string message = "The component is not subscribed to receive interactions.";

    public InteractableComponentNotSubscribed() : base()
    {

    }
}
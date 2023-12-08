// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

public abstract class ButtonHandler<TRequest> : ComponentInteractionHandler<TRequest, Button>
    where TRequest : IButtonRequest
{

}
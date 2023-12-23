// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;

public abstract class ButtonHandler<TRequest> : ComponentInteractionHandler<TRequest, Button>
    where TRequest : IButtonRequest
{
    public override string GetLogInfo(TRequest request) =>
        $"Button with name {request.Component.Name} and id '{request.Component.CustomId}'.";
}
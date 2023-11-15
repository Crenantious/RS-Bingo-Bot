// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using RSBingoBot.InteractionHandlers;
using RSBingoBot.Requests;

internal abstract class ButtonHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : IButtonRequest
{

}
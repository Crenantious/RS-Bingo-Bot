// <copyright file="CommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interaction_handlers;

using RSBingoBot.DiscordComponents;
using MediatR;

internal abstract class CommandHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : IRequest<DiscordMessage>
{

}
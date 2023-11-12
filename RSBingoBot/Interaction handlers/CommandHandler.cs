// <copyright file="CommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.InteractionHandlers;

using RSBingoBot.DiscordEntities;
using MediatR;

internal abstract class CommandHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : IRequest<Message>
{

}
// <copyright file="MessageCreatedHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public abstract class MessageCreatedHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IMessageCreatedRequest
{

}
// <copyright file="MessageCreatedHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public abstract class MessageCreatedHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : IMessageCreatedRequest
{
    public override string GetLogInfo(TRequest request) =>
        $"Message created with id {request.Message.DiscordMessage.Id} and content '{request.Message.Content}'.";
}
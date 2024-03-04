// <copyright file="CommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public abstract class CommandHandler<TRequest> : RequestHandler<TRequest>
    where TRequest : ICommandRequest
{

}
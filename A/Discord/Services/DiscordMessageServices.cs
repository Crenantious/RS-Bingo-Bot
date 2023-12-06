// <copyright file="DiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FluentResults;

// TODO: JR - implement. Remember to set the Message.Id when sending or getting.
public class DiscordMessageServices : IDiscordMessageServices
{
    private readonly MessageCreatedDEH messageCreatedDEH;

    public DiscordMessageServices(MessageCreatedDEH messageCreatedDEH)
    {
        this.messageCreatedDEH = messageCreatedDEH;
    }

    public Task<bool> Send(Message message, DiscordChannel channel)
    {
        throw new NotImplementedException();
    }

    public Task<Message> Get(ulong id, DiscordChannel channel)
    {
        throw new NotImplementedException();
    }

    public void RegisterCreationHandler(IMessageCreatedRequest request, MessageCreatedDEH.Constraints constraints)
    {
        messageCreatedDEH.Subscribe(constraints, (client, args) => OnMessageCreated(request, args));
    }

    private static async Task OnMessageCreated(IMessageCreatedRequest request, MessageCreateEventArgs args)
    {
        request.MessageArgs = args;
        request.Message = new Message(args.Message);
        Result result = await RequestServices.Run(request);
    }
}
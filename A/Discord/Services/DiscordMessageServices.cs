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
using RSBingoBot.Requests;

public class DiscordMessageServices : IDiscordMessageServices
{
    private readonly MessageCreatedDEH messageCreatedDEH;

    public DiscordMessageServices(MessageCreatedDEH messageCreatedDEH)
    {
        this.messageCreatedDEH = messageCreatedDEH;
    }

    public async Task<Result> Send(Message message, DiscordChannel channel) =>
        await RequestServices.Run(new SendMessageRequest(message, channel));

    public async Task<Result<Message>> Get(ulong id, DiscordChannel channel) =>
        await RequestServices.Run<GetMessageRequest, Message>(new GetMessageRequest(id, channel));

    public async Task<Result> Delete(Message message) =>
        await RequestServices.Run(new DeleteMessageRequest(message.DiscordMessage));

    public async Task<Result> Delete(DiscordMessage message) =>
        await RequestServices.Run(new DeleteMessageRequest(message));

    public void RegisterMessageCreatedHandler(IMessageCreatedRequest request, MessageCreatedDEH.Constraints constraints)
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
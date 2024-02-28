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

public class DiscordMessageServices : RequestService, IDiscordMessageServices
{
    private readonly MessageCreatedDEH messageCreatedDEH;

    public DiscordMessageServices(MessageCreatedDEH messageCreatedDEH)
    {
        this.messageCreatedDEH = messageCreatedDEH;
    }

    public async Task<Result> Send(Message message) =>
        await RunRequest(new SendMessageRequest(message));

    public async Task<Result<Message>> Get(ulong id, DiscordChannel channel) =>
        await RunRequest<GetMessageRequest, Message>(new GetMessageRequest(id, channel));

    public async Task<Result> Update(IMessage message) =>
        await RunRequest(new UpdateMessageRequest(message));

    public async Task<Result> Delete(Message message) =>
        await RunRequest(new DeleteMessageRequest(message.DiscordMessage));

    public async Task<Result> Delete(DiscordMessage message) =>
        await RunRequest(new DeleteMessageRequest(message));

    public void RegisterMessageCreatedHandler(Func<IMessageCreatedRequest> getRequest, MessageCreatedDEH.Constraints constraints)
    {
        messageCreatedDEH.Subscribe(constraints, (client, args) => OnMessageCreated(getRequest, args));
    }

    private async Task OnMessageCreated(Func<IMessageCreatedRequest> getRequest, MessageCreateEventArgs args)
    {
        Result result = await RunRequest(getRequest(),
            (null, args),
            (null, new Message(args.Message)));
    }
}
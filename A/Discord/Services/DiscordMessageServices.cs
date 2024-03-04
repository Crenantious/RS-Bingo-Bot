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
using Microsoft.Extensions.Logging;
using RSBingo_Common;
using RSBingoBot.Requests;

public class DiscordMessageServices : RequestService, IDiscordMessageServices
{
    private readonly MessageCreatedDEH messageCreatedDEH;
    private readonly MessageReactedDEH messageReactedDEH;
    private readonly IDiscordMessageServices messageServices;

    public DiscordMessageServices(MessageCreatedDEH messageCreatedDEH, MessageReactedDEH messageReactedDEH)
    {
        this.messageCreatedDEH = messageCreatedDEH;
        this.messageReactedDEH = messageReactedDEH;
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

    public void RegisterMessageCreatedHandler(Func<IMessageCreatedRequest> getRequest, Func<MessageCreateEventArgs, bool> constraints)
    {
        messageCreatedDEH.Subscribe(constraints, args => OnMessageCreated(getRequest, args));
    }

    public void RegisterMessageReactedHandler(Func<IMessageReactedRequest> getRequest, Func<MessageReactionAddEventArgs, bool> constraints)
    {
        messageReactedDEH.Subscribe(constraints, args => OnMessageReacted(getRequest, args));
    }

    private async Task OnMessageCreated(Func<IMessageCreatedRequest> getRequest, MessageCreateEventArgs args)
    {
        Result result = await RunRequest(getRequest(),
            (null, args),
            (null, new Message(args.Message)));
    }

    private async Task OnMessageReacted(Func<IMessageReactedRequest> getRequest, MessageReactionAddEventArgs args)
    {
        // The message provided by args contains limited information (no content, attachments etc.) so the
        // message must currently be retrieved via the given id.
        var messageResult = await Get(args.Message.Id, args.Message.Channel);

        if (messageResult.IsFailed)
        {
            General.LoggingLog($"Failed to retrieve message with id {args.Message.Id} " +
                $"from a reaction so cannot run the request {getRequest().GetType()}.",
                LogLevel.Error);
            return;
        }

        Result result = await RunRequest(getRequest(),
            (null, args),
            (null, messageResult.Value),
            (null, args.Emoji));
    }
}
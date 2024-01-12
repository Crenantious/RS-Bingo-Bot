// <copyright file="InteractionMessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordServices;
using FluentResults;
using RSBingo_Common;

public static class InteractionMessageExtensions
{
    public static T AsEphemeral<T>(this T message, bool status)
        where T : InteractionMessage
    {
        message.IsEphemeral = status;
        return message;
    }

    public static async Task<Result> Send(this InteractionMessage message) =>
        await GetMessageService().Send(message);

    public static async Task<bool> Update(this InteractionMessage message) =>
        await GetMessageService().Update(message);

    public static async Task<bool> Delete(this InteractionMessage message) =>
        await GetMessageService().Delete(message);

    private static IDiscordInteractionMessagingServices GetMessageService() =>
        (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
}
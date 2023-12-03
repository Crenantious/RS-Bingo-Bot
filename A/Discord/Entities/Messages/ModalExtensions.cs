// <copyright file="ModalExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using RSBingo_Common;

public static class ModalExtensions
{
    public static async Task<bool> Send(this Modal modal, IModalRequest request)
    {
        if (await GetMessageService().Send(modal))
        {
            modal.Register(request);
            return true;
        }
        return false;
    }

    public static async Task<bool> Update(this InteractionMessage message) =>
        await GetMessageService().Update(message);

    public static async Task<bool> Delete(this InteractionMessage message) =>
        await GetMessageService().Delete(message);

    private static IDiscordInteractionMessagingServices GetMessageService() =>
        (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
}
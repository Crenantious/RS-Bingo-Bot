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

    private static IDiscordInteractionMessagingServices GetMessageService() =>
        (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
}
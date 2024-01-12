// <copyright file="ModalExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Common;

public static class ModalExtensions
{
    public static async Task<Result> Send(this Modal modal, IModalRequest request)
    {
        var result = await GetMessageService().Send(modal);
        if (result.IsSuccess)
        {
            modal.Register(request);
        }
        return result;
    }

    private static IDiscordInteractionMessagingServices GetMessageService() =>
        (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
}
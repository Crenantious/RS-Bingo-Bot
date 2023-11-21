// <copyright file="ViewEvidenceSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal class ViewEvidenceSuccess : InteractionSuccess
{
    private const string SuccessMessage = "{0} Select a tile to view its evidence.";

    public ViewEvidenceSuccess(DiscordUser user, SelectComponent selectComponent, Button close) :
        base(CreateMessage(user, selectComponent, close))
    {

    }

    private static Message CreateMessage(DiscordUser user, SelectComponent selectComponent, Button close) =>
        new Message()
            .WithContent(SuccessMessage.FormatConst(user.Mention))
            .AddComponents(selectComponent)
            .AddComponents(close);
}
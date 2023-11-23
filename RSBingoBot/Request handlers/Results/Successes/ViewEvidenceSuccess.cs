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

    public ViewEvidenceSuccess(DiscordInteraction interaction, DiscordUser user, SelectComponent selectComponent, Button close) :
        base(CreateMessage(interaction, user, selectComponent, close))
    {

    }

    private static InteractionMessage CreateMessage(DiscordInteraction interaction, DiscordUser user,
                                                    SelectComponent selectComponent, Button close) =>
        new InteractionMessage(interaction)
            .WithContent(SuccessMessage.FormatConst(user.Mention))
            .AddComponents(selectComponent)
            .AddComponents(close);
}
// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using DSharpPlus.Entities;

public interface IMessage
{
    public string Content { get; set; }
    public List<DiscordActionRowComponent> Components { get; set; }

    public DiscordMessageBuilder GetMessageBuilder();
    public DiscordWebhookBuilder GetWebhookBuilder();
}
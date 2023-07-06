﻿// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using DSharpPlus.Entities;
using RSBingoBot.DiscordComponents;

public interface IMessage
{
    public string Content { get; set; }
    public List<IEnumerable<IDiscordComponent>> Components { get; set; }

    public DiscordMessageBuilder GetMessageBuilder();
    public DiscordWebhookBuilder GetWebhookBuilder();
}
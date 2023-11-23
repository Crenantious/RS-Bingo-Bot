﻿// <copyright file="IMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;
using DiscordLibrary.DataStructures;
using DiscordLibrary.DiscordComponents;

public interface IMessage
{
    public string Content { get; set; }
    public DynamicGrid<IComponent> Components { get; set; }

    public DiscordMessageBuilder GetMessageBuilder();
    public DiscordWebhookBuilder GetWebhookBuilder();
}
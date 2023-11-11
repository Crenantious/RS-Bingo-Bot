// <copyright file="IRequestWithDiscordAttachment.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;

public interface IRequestWithDiscordAttachment
{
    public DiscordAttachment Attachment { get; }
}
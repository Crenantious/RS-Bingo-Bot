// <copyright file="IRequestWithDiscordUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;

public interface IRequestWithDiscordUser
{
    public DiscordUser DiscordUser { get; }
}
// <copyright file="DiscordUserExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordExtensions;

using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;

public static class DiscordUserExtensions
{
    public static bool IsOnATeam(this DiscordUser user, IDataWorker dataWorker) =>
        dataWorker.Users.FirstOrDefault(u => u.DiscordUserId == user.Id) is not null;
}
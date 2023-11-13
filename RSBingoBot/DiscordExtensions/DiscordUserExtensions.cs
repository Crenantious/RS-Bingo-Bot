// <copyright file="DiscordUserExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordExtensions;

using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public static class DiscordUserExtensions
{
    public static bool IsOnATeam(this DiscordUser discordUser, IDataWorker dataWorker) =>
        GetUser(discordUser, dataWorker) is not null;

    public static bool IsOnTeam(this DiscordUser discordUser, IDataWorker dataWorker, string teamName)
    {
        User? user = GetUser(discordUser, dataWorker);
        if (user is null) { return false; }
        return user.Team.Name == teamName;
    }

    public static bool IsOnTeam(this DiscordUser discordUser, IDataWorker dataWorker, Team team)
    {
        User? user = GetUser(discordUser, dataWorker);
        if (user is null) { return false; }
        return user.Team == team;
    }

    private static User? GetUser(DiscordUser discordUser, IDataWorker dataWorker) =>
        dataWorker.Users.FirstOrDefault(u => u.DiscordUserId == discordUser.Id);
}
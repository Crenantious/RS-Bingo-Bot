// <copyright file="DiscordUserExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordExtensions;

using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public static class DiscordUserExtensions
{
    public static bool IsOnATeam(this DiscordUser discordUser, IDataWorker dataWorker) =>
        GetDBUser(discordUser, dataWorker) is not null;

    public static bool IsOnTeam(this DiscordUser discordUser, IDataWorker dataWorker, string teamName)
    {
        User? user = GetDBUser(discordUser, dataWorker);
        if (user is null) { return false; }
        return user.Team.Name == teamName;
    }

    public static bool IsOnTeam(this DiscordUser discordUser, IDataWorker dataWorker, Team team)
    {
        User? user = GetDBUser(discordUser, dataWorker);
        if (user is null) { return false; }
        return user.Team == team;
    }

    public static User? GetDBUser(this DiscordUser discordUser, IDataWorker dataWorker) =>
        dataWorker.Users.FirstOrDefault(u => u.DiscordUserId == discordUser.Id);
}
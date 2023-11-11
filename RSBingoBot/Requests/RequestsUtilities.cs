// <copyright file="RequestsUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

internal static class RequestsUtilities
{
    public static DiscordRole GetTeamRole(IDataWorker dataWorker, string teamName) =>
        DataFactory.Guild.GetRole(dataWorker.Teams.GetByName(teamName)!.RoleId);

    public static DiscordRole GetRole(IDataWorker dataWorker, Team team) =>
        DataFactory.Guild.GetRole(team.RoleId);
}
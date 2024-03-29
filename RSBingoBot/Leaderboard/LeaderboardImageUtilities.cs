﻿// <copyright file="LeaderboardImageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingoBot.DTO;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;

public class LeaderboardImageUtilities
{
    /// <summary>
    /// Gets all the text values to be drawn onto the leaderboard.
    /// </summary>
    /// <param name="dataWorker"></param>
    /// <returns>The values. Dimension 0 is the columns and dimension 1 is the rows.</returns>
    public static Grid GetCellValues(IDataWorker dataWorker)
    {
        IEnumerable<Team> teams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);
        Grid cellValues = new(3, teams.Count() + 1);

        AddHeaders(cellValues);
        AddTeams(cellValues, teams);

        return cellValues;
    }

    private static void AddHeaders(Grid cellValues) =>
       cellValues.SetRow(0, new string[] { "Name", "Score", "Rank" });

    private static void AddTeams(Grid cellValues, IEnumerable<Team> teams)
    {
        for (int i = 0; i < teams.Count(); i++)
        {
            Team team = teams.ElementAt(i);
            cellValues.SetRow(i + 1, new string[] { team.Name, team.Score.ToString(), (i + 1).ToString() });
        }
    }
}
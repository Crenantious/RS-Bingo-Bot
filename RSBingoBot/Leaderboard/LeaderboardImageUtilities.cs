// <copyright file="LeaderboardImageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;

public class LeaderboardImageUtilities
{
    /// <summary>
    /// Gets all the text values to be drawn onto the leaderboard.
    /// </summary>
    /// <param name="dataWorker"></param>
    /// <returns>The values. Dimension 0 is the columns and dimension 1 is the rows.</returns>
    public static string[,] GetCellValues(IDataWorker dataWorker)
    {
        IEnumerable<Team> teams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);
        string[,] cellValues = new string[3, teams.Count() + 1];

        AddHeaders(cellValues);
        AddTeams(cellValues, teams);

        return cellValues;
    }

    private static void AddHeaders(string[,] cellValues) =>
       AddRow(cellValues, 0, new string[] { "Name", "Score", "Rank" });

    private static void AddTeams(string[,] cellValues, IEnumerable<Team> teams)
    {
        for (int i = 0; i < teams.Count(); i++)
        {
            Team team = teams.ElementAt(i);
            AddRow(cellValues, i + 1, new string[] { team.Name, team.Score.ToString(), (i + 1).ToString() });
        }
    }

    private static void AddRow(string[,] cellValues, int rowIndex, string[] rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            cellValues[i, rowIndex] = rowValues.ElementAt(i);
        }
    }
}
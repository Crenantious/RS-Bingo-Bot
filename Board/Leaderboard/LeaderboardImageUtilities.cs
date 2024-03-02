// <copyright file="LeaderboardImageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Leaderboard;

using RSBingo_Common.DataStructures;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

internal class LeaderboardImageUtilities
{
    /// <summary>
    /// Gets all the text values to be drawn onto the leaderboard.
    /// </summary>
    /// <param name="dataWorker"></param>
    /// <returns>The values. Dimension 0 is the columns and dimension 1 is the rows.</returns>
    public static Grid<string> GetCellValues(IDataWorker dataWorker)
    {
        IEnumerable<Team> teams = dataWorker.Teams.GetAll().OrderByDescending(t => t.Score);
        Grid<string> cellValues = new(3, teams.Count() + 1);

        AddHeaders(cellValues);
        AddTeams(cellValues, teams);

        return cellValues;
    }

    private static void AddHeaders(Grid<string> cellValues) =>
       cellValues.SetRow(0, new string[] { "Name", "Score", "Rank" });

    private static void AddTeams(Grid<string> cellValues, IEnumerable<Team> teams)
    {
        for (int i = 0; i < teams.Count(); i++)
        {
            Team team = teams.ElementAt(i);
            cellValues.SetRow(i + 1, new string[] { team.Name, team.Score.ToString(), (i + 1).ToString() });
        }
    }
}
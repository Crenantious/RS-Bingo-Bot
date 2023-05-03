// <copyright file="LeaderboardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public class Grid
{
    public string[,] Cells { get; }

    public Grid(int columnCount, int rowCount) =>
        Cells = new string[columnCount, rowCount];

    public void SetRow(int rowIndex, string[] rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            Cells[i, rowIndex] = rowValues.ElementAt(i);
        }
    }

    public void SetColumn(int columnIndex, string[] columnValues)
    {
        for (int i = 0; i < columnValues.Count(); i++)
        {
            Cells[columnIndex, i] = columnValues.ElementAt(i);
        }
    }
}
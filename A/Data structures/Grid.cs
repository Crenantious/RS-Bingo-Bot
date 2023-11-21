// <copyright file="Grid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DataStructures;

public class Grid<T>
{
    public T[,] Cells { get; }

    public Grid(int columnCount, int rowCount) =>
        Cells = new T[columnCount, rowCount];

    public void SetRow(int rowIndex, T[] rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            Cells[i, rowIndex] = rowValues[i];
        }
    }

    public void SetColumn(int columnIndex, T[] columnValues)
    {
        for (int i = 0; i < columnValues.Count(); i++)
        {
            Cells[columnIndex, i] = columnValues[i];
        }
    }
}
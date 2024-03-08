// <copyright file="Grid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common.DataStructures;

// TODO: JR - convert to using only rows and columnc; no cells.
public class Grid<T>
{
    public T[][] Rows;
    public T[][] Columns;
    public T[,] Cells { get; }

    public Grid(int columnCount, int rowCount)
    {
        Cells = new T[columnCount, rowCount];
        Rows = new T[rowCount][];
        Columns = new T[columnCount][];

        for (int i = 0; i < rowCount; i++)
        {
            Rows[i] = new T[columnCount];
        }

        for (int i = 0; i < columnCount; i++)
        {
            Columns[i] = new T[rowCount];
        }
    }

    public void SetRow(int rowIndex, T[] rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            Cells[i, rowIndex] = rowValues[i];
        }
        Rows[rowIndex] = rowValues;

        for (int i = 0; i < rowValues.Length; i++)
        {
            Columns[i][rowIndex] = rowValues[i];
        }
    }
}
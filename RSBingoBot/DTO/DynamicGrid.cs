﻿// <copyright file="DynamicGrid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public class DynamicGrid<T>
{
    private int enumerableRowIndex = 0;
    private int enumerableColumnIndex = 0;

    public List<List<T>> Cells { get; }

    public DynamicGrid(int initialColumnCount = 0, int iniitialRowCount = 0) =>
        Cells = new(initialColumnCount) { new(iniitialRowCount) };

    public void SetRow(int rowIndex, T[] rowValues)
    {
        for (int i = 0; i < rowValues.Count(); i++)
        {
            Cells[i][rowIndex] = rowValues[i];
        }
    }

    public void SetColumn(int columnIndex, T[] columnValues)
    {
        for (int i = 0; i < columnValues.Count(); i++)
        {
            Cells[columnIndex][i] = columnValues[i];
        }
    }

    public List<List<T>> GetRows() =>
        Cells;
}
// <copyright file="LineGraphDataPlotInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record LineGraphDataPlotInfo<TXData, TYData>(
    IEnumerable<(TXData x, TYData y)> Data,
    Func<TXData, TXData, TXData, float> getXMinMaxRatio,
    Func<TYData, TYData, TYData, float> getYMinMaxRatio,
    TXData xMin, TXData xMax, TYData yMin, TYData yMax, Color color);
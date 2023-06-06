// <copyright file="LineGraphCategory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record LineGraphCategory<TXValue, TYValue>(string CategoryName, IEnumerable<(TXValue x, TYValue y)> Data, Color LegendColour);
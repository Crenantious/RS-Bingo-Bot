// <copyright file="LineGraphCategory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record LineGraphCategory<TXValue, TYValue>(LineGraphCategoryLegend LegendInfo, IEnumerable<(TXValue x, TYValue y)> Data)
{
    public LineGraphCategory(string CategoryName, Color LegendColour, IEnumerable<(TXValue x, TYValue y)> Data) :
        this(new(CategoryName, LegendColour), Data) { }
}
// <copyright file="LineGraphCategory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record LineGraphCategory(string CategoryName, IEnumerable<(float xValue, float yValue)> dataPoints, Color legendColour);
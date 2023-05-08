// <copyright file="GridImageDimensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record GridImageDimensions(IEnumerable<int> ColumnWidths, IEnumerable<int> RowHeights);
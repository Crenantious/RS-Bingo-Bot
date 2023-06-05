// <copyright file="GraphAxisInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

public record GraphAxisInfo(string Title, int DivisionCount, IEnumerable<string> CategoryLabels);
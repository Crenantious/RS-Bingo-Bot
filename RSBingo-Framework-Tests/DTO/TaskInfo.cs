// <copyright file="TaskInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using static RSBingo_Framework.Records.BingoTaskRecord;

namespace RSBingo_Framework_Tests.DTO;

public record TaskInfo(string Name, Difficulty Difficulty, int Amount, string? ImageURL = null);

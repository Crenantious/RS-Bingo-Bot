// <copyright file="TaskInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DTO;

using static RSBingo_Framework.Records.BingoTaskRecord;

public record TaskInfo(string Name, Difficulty Difficulty, int Amount, string? ImageURL = null);
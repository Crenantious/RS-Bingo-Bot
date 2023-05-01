// <copyright file="LeaderboardRowDimensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using RSBingoBot.Leaderboard;

/// <param name="widths">The width of each <see cref="LeaderboardRowBackgroundComponent"/> in order left to right.</param>
/// <param name="height">The height of the <see cref="LeaderboardRowBackground"/>.</param>
internal record LeaderboardRowDimensions(IEnumerable<int> widths, int height);
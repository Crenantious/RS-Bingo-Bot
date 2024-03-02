// <copyright file="LeaderboardRowDimensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Leaderboard;

// TODO: JR - see if this is needed.

/// <param name="widths">The width of each <see cref="LeaderboardCellBackground"/> in order left to right.</param>
/// <param name="height">The height of the <see cref="LeaderboardRowBackground"/>.</param>
internal record LeaderboardRowDimensions(IEnumerable<int> widths, int height);
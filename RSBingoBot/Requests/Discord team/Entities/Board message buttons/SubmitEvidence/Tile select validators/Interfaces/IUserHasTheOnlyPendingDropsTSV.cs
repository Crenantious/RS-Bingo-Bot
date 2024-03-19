// <copyright file="IUserHasTheOnlyPendingDropsTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface IUserHasTheOnlyPendingDropsTSV
{
    public bool Validate(Tile tile, User user);
}
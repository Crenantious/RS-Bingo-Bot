// <copyright file="IRequestWithUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface IRequestWithUser
{
    public User User { get; }
}
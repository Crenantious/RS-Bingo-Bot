﻿// <copyright file="IUserHasNoAcceptedVerificationEvidenceForTileTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface IUserHasNoAcceptedVerificationEvidenceForTileTSV
{
    public bool Validate(Tile tile, User user);
}
// <copyright file="IOtherUsersDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public interface IOtherUsersDropEvidenceTSV
{
    public bool Validate(IEnumerable<Evidence> dropEvidence, ulong userId);
}
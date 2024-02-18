// <copyright file="DiscordTeamBoardButtonErrors.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

internal static class DiscordTeamBoardButtonErrors
{
    public const string ChangeTilesWithActiveSubmitDropOrViewEvidence = "Tiles cannot be changed while drops are being submitted or viewed.";
    public const string SubmitDropOrViewEvidenceWithActiveChangeTiles = "Drops cannot be submitted or viewed while tiles are being changed.";
    public const string SubmitDropWithActiveViewEvidence = "You cannot submit and view evidence at the same time.";
    public const string ViewEvidenceWithActiveSubmitDrop = "You cannot view and submit evidence at the same time.";
}
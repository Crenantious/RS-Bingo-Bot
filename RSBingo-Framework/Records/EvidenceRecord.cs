// <copyright file="EvidenceRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records;

using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public static class EvidenceRecord
{
    #region enums & lookups

    public enum EvidenceType
    {
        Undefined,
        TileVerification,
        Drop,
    }

    public enum EvidenceStatus
    {
        PendingReview,
        Accepted,
        Rejected,
    }

    public static readonly EnumDict<EvidenceType> EvidenceTypeLookup = new EnumDict<EvidenceType>(
        EvidenceType.Undefined)
        .Add(EvidenceType.TileVerification, 1)
        .Add(EvidenceType.Drop, 2);

    public static readonly EnumDict<EvidenceStatus> EvidenceStatusLookup = new EnumDict<EvidenceStatus>(
        EvidenceStatus.PendingReview)
        .Add(EvidenceStatus.Accepted, 1)
        .Add(EvidenceStatus.Rejected, 2);

    #endregion

    public static Evidence CreateEvidence(IDataWorker dataWorker, User user, Tile tile, string url,
        EvidenceType type, ulong discordMessageId) =>
        dataWorker.Evidence.Create(user, tile, url, type, discordMessageId);

    public static Evidence? GetByTileUserAndType(IDataWorker dataWorker, Tile tile, User user, EvidenceType evidenceType) =>
        dataWorker.Evidence.FirstOrDefault(e =>
        e.Tile == tile &&
        e.User == user &&
        e.EvidenceType == EvidenceTypeLookup.Get(evidenceType));

    public static Evidence? GetByMessageId(IDataWorker dataWorker, ulong messageId) =>
        dataWorker.Evidence.FirstOrDefault(e => e.DiscordMessageId == messageId);

    public static bool IsVerified(this Evidence evidence) =>
        EvidenceStatusLookup.Get(evidence.Status) == EvidenceStatus.Accepted;

    public static bool IsRejected(this Evidence evidence) =>
        EvidenceStatusLookup.Get(evidence.Status) == EvidenceStatus.Rejected;
}
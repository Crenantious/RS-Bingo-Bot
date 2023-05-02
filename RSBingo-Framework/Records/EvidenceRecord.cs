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
        Verified,
        Rejected,
    }

    private static readonly EnumDict<EvidenceType> EvidenceTypeLookup = new EnumDict<EvidenceType>(EvidenceType.Undefined)
     .Add(EvidenceType.TileVerification, 1)
     .Add(EvidenceType.Drop, 2);

    private static readonly EnumDict<EvidenceStatus> EvidenceStatusLookup = new EnumDict<EvidenceStatus>(
        EvidenceStatus.PendingReview)
        .Add(EvidenceStatus.Verified, 1)
        .Add(EvidenceStatus.Rejected, 2);

    #endregion

    public static Evidence CreateEvidence(IDataWorker dataWorker, User user, Tile tile, string url,
        EvidenceType type, ulong discordMessageId) =>
        dataWorker.Evidence.Create(user, tile, url, type, discordMessageId);

    public static Evidence? GetByTileUserAndType(IDataWorker dataWorker, Tile tile, User user, EvidenceType evidenceType) =>
        dataWorker.Evidence.FirstOrDefault(e =>
        e.Tile == tile &&
        e.DiscordUser == user &&
        e.EvidenceType == EvidenceTypeLookup.Get(evidenceType));

    public static IEnumerable<Evidence> GetByMessageId(IDataWorker dataWorker, ulong messageId) =>
        dataWorker.Evidence.Where(e => e.DiscordMessageId == messageId);
}
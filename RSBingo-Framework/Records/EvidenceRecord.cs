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

    #region Extensions

    public static bool IsPending(this Evidence evidence) =>
        evidence.HasStatus(EvidenceStatus.PendingReview);

    public static bool IsRejected(this Evidence evidence) =>
        evidence.HasStatus(EvidenceStatus.Rejected);

    public static bool IsAccepted(this Evidence evidence) =>
        evidence.HasStatus(EvidenceStatus.Accepted);

    public static bool HasStatus(this Evidence evidence, EvidenceStatus evidenceStatus) =>
        EvidenceStatusLookup.Get(evidence.Status) == evidenceStatus;

    public static bool IsType(this Evidence evidence, EvidenceType evidenceType) =>
        EvidenceTypeLookup.Get(evidence.EvidenceType) == evidenceType;

    #region IEnumerable inputs

    public static IEnumerable<Evidence> GetVerificationEvidence(this IEnumerable<Evidence> evidence) =>
        evidence.Where(e => e.IsType(EvidenceType.TileVerification));

    public static IEnumerable<Evidence> GetDropEvidence(this IEnumerable<Evidence> evidence) =>
        evidence.Where(e => e.IsType(EvidenceType.Drop));

    public static IEnumerable<Evidence> GetUserEvidence(this IEnumerable<Evidence> evidence, ulong userId) =>
        evidence.Where(e => e.DiscordUserId == userId);

    public static IEnumerable<Evidence> GetOtherUsersEvidence(this IEnumerable<Evidence> evidence, ulong userId) =>
        evidence.Where(e => e.DiscordUserId != userId);

    public static IEnumerable<Evidence> GetAcceptedEvidence(this IEnumerable<Evidence> evidence) =>
        evidence.Where(e => e.IsAccepted());

    public static IEnumerable<Evidence> GetPendingEvidence(this IEnumerable<Evidence> evidence) =>
        evidence.Where(e => e.IsPending());

    #endregion

    #endregion
}
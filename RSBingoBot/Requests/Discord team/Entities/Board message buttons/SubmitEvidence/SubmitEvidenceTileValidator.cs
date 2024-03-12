// <copyright file="SubmitEvidenceTileValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

public static class SubmitEvidenceTileValidator
{
    public static bool Validate(IEnumerable<Tile> tiles, EvidenceType evidenceType, ulong userId) =>
        !tiles.Any(t => Validate(t, evidenceType, userId) is false);

    public static bool Validate(Tile tile, EvidenceType evidenceType, ulong userId) =>
        evidenceType switch
        {
            EvidenceType.TileVerification => TileVerificationValidation(tile, userId),
            EvidenceType.Drop => DropValidation(tile, userId),
            _ => throw new ArgumentOutOfRangeException(),
        };

    private static bool TileVerificationValidation(Tile tile, ulong userId) =>
        tile.Evidence.Any(e => e.IsType(EvidenceType.Drop) ||
                               DoesUserHaveAcceptedVerificationEvidence(userId, e))
        is false;

    private static bool DoesUserHaveAcceptedVerificationEvidence(ulong userId, Evidence e) =>
        e.DiscordUserId == userId &&
        e.IsType(EvidenceType.TileVerification) &&
        e.IsVerified();

    private static bool DropValidation(Tile tile, ulong userId)
    {
        if (tile.IsCompleteAsBool() || DoAllUsersHaveAcceptedVerificationEvidence(tile) is false)
        {
            return false;
        }

        return !tile.Evidence.Where(e => e.IsType(EvidenceType.Drop))
                             .Any(e => e.IsStatus(EvidenceStatus.Accepted) ||
                                       IsPendingFromDifferentUser(e, userId));
    }

    private static bool DoAllUsersHaveAcceptedVerificationEvidence(Tile tile) =>
        !tile.Team.Users.Any(u => DoesUserHaveAcceptedVerificationEvidence(u) is false);

    private static bool DoesUserHaveAcceptedVerificationEvidence(User user) =>
        user.Evidence.Any(e => e.IsType(EvidenceType.TileVerification) &&
                               e.IsStatus(EvidenceStatus.Accepted));

    private static bool IsPendingFromDifferentUser(Evidence e, ulong userId) =>
        e.IsStatus(EvidenceStatus.PendingReview) &&
        e.DiscordUserId != userId;
}
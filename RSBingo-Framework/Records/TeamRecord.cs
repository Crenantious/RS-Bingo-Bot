// <copyright file="TeamRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records;

using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public static class TeamRecord
{
    public enum SubmissionState
    {
        Verification,
        Drops
    }

    public static Team CreateTeam(IDataWorker dataWorker, string name, ulong categoryChannelId,
        ulong boardChannelId, ulong generalChannelId, ulong evidenceChannelId, ulong voiceChannelId, ulong boardMessageId, ulong roleId) =>
        dataWorker.Teams.Create(name, categoryChannelId, boardChannelId, generalChannelId, evidenceChannelId, voiceChannelId, boardMessageId, roleId);

    public static bool IsBoardVerfied(this Team team) =>
        team.Tiles.All(t => t.IsVerified());

    public static SubmissionState GetEvidenceSubmissionState(this Team team) =>
        General.HasCompetitionStarted && team.IsBoardVerfied() ?
            SubmissionState.Drops :
            SubmissionState.Verification;
}
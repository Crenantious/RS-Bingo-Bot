// <copyright file="TileFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Imaging.Board;

using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using SixLabors.ImageSharp;

public class TileFactory : TileFactoryBase<Team, int>
{
    private static NoTaskTileFactory NoTaskTileFactory => General.DI.Get<NoTaskTileFactory>();
    private static PlainTaskTileFactory PlainTaskTileFactory => General.DI.Get<PlainTaskTileFactory>();
    private static EvidencePendingTileFactory EvidencePendingTileFactory => General.DI.Get<EvidencePendingTileFactory>();
    private static CompletedTileFactory CompletedTileFactory => General.DI.Get<CompletedTileFactory>();

    public override Image Create(Team team, int boardIndex)
    {
        Tile? tile = team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);
        Image tileImage = CreateTileImage(tile);
        return tileImage;
    }

    private static Image CreateTileImage(Tile? tile)
    {
        if (tile is null)
        {
            return NoTaskTileFactory.Create();
        }

        return tile.Team.GetEvidenceSubmissionState() switch
        {
            TeamRecord.SubmissionState.Verification => CreateVerificationTile(tile),
            TeamRecord.SubmissionState.Drops => CreateDropTile(tile),
            _ => throw new ArgumentOutOfRangeException($"Invalid enum value for {nameof(TeamRecord.SubmissionState)}"),
        };
    }

    private static Image CreateVerificationTile(Tile tile)
    {
        if (tile.IsVerified())
        {
            return CompletedTileFactory.Create(tile.Task);
        }

        return PlainTaskTileFactory.Create(tile.Task);
    }

    private static Image CreateDropTile(Tile tile)
    {
        if (tile.IsCompleteAsBool())
        {
            return CompletedTileFactory.Create(tile.Task);
        }

        if (tile.Evidence.GetDropEvidence().GetPendingEvidence().Any())
        {
            return EvidencePendingTileFactory.Create(tile.Task);
        }

        return PlainTaskTileFactory.Create(tile.Task);
    }
}
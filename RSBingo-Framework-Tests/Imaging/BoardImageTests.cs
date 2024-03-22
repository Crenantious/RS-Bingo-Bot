// <copyright file="BoardImageTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.Imaging.Board;

using global::Imaging.Board;
using Microsoft.Extensions.DependencyInjection;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using System.Diagnostics;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingo_Framework.Records.TileRecord;

// These tests are intended to be used for visual inspection only; make sure it is ignored when committed.

[TestClass]
public class BoardImageTests : MockDBBaseTestClass
{
    private string imagePath = null!;
    private IDataWorker dataWorker = null!;
    private Team team = null!;
    private Tile tileOne = null!;
    private Tile tileTwo = null!;
    private User user = null!;
    private Board board = null!;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);

        services.AddSingleton<TileFactory>();
        services.AddSingleton<NoTaskTileFactory>();
        services.AddSingleton<PlainTaskTileFactory>();
        services.AddSingleton<EvidencePendingTileFactory>();
        services.AddSingleton<CompletedTileFactory>();
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        imagePath = Path.Combine(Paths.ResourcesTestOutputFolder, "Test board.png");

        dataWorker = CreateDW();
        team = MockDBSetup.Add_Team(dataWorker);
        user = MockDBSetup.Add_User(dataWorker, 0, team);

        // Tile two is just to ensure the markers are only being placed on tile one.
        BingoTask taskOne = MockDBSetup.Add_BingoTask(dataWorker, "Test 1");
        BingoTask taskTwo = MockDBSetup.Add_BingoTask(dataWorker, "Test 2");
        tileOne = MockDBSetup.Add_Tile(dataWorker, team, taskOne);
        tileTwo = MockDBSetup.Add_Tile(dataWorker, team, taskTwo);

        BoardFactory boardFactory = new();
        board = boardFactory.Create();
    }

    [TestMethod]
    [Ignore]
    public void VerificationStage_TileIsNotVerified_NoMarker()
    {
        SetVerificationStage();
        SetTileVerified(false);

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void VerificationStage_TileHasPendingVerificationEvidence_NoMarker()
    {
        SetVerificationStage();
        SetTileVerified(false);
        AddPendingVerificationEvidence();

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void VerificationStage_TileHasPendingDropsEvidence_NoMarker()
    {
        SetVerificationStage();
        SetTileVerified(false);
        AddPendingDropEvidence();

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void VerificationStage_TileIsVerified_CompletedMarker()
    {
        SetVerificationStage();
        SetTileVerified(true);

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void DropsStage_TileIsNotComplete_NoMarker()
    {
        SetDropsStage();
        SetTileCompleted(false);

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void DropsStage_TileHasPendingVerificationEvidence_NoMarker()
    {
        SetDropsStage();
        SetTileCompleted(false);
        AddPendingVerificationEvidence();

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void DropsStage_TileHasPendingDropsEvidence_PendingEvidenceMarker()
    {
        SetDropsStage();
        SetTileCompleted(false);
        AddPendingDropEvidence();

        UpdateBoard();

        ShowBoard();
    }

    [TestMethod]
    [Ignore]
    public void DropsStage_TileIsComplete_CompletedMarkerMarker()
    {
        SetDropsStage();
        SetTileCompleted(true);

        UpdateBoard();

        ShowBoard();
    }

    private void SetVerificationStage()
    {
        General.HasCompetitionStarted = false;
        SetTileVerified(tileOne, false);
        SetTileVerified(tileTwo, false);
    }

    private void SetDropsStage()
    {
        General.HasCompetitionStarted = true;
        SetTileVerified(tileOne, true);
        SetTileVerified(tileTwo, true);
    }

    private void AddPendingVerificationEvidence()
    {
        MockDBSetup.Add_Evidence(dataWorker, user, tileOne, EvidenceType.TileVerification, EvidenceStatus.PendingReview);
    }

    private void AddPendingDropEvidence()
    {
        MockDBSetup.Add_Evidence(dataWorker, user, tileOne, EvidenceType.Drop, EvidenceStatus.PendingReview);
    }

    private void SetTileVerified(bool isVerified)
    {
        SetTileVerified(tileOne, isVerified);
    }

    private void SetTileVerified(Tile tile, bool isVerified)
    {
        VerifiedStatus verifiedStatus = isVerified ? VerifiedStatus.Yes : VerifiedStatus.No;
        tile.IsVerified = VerifiedLookup.Get(verifiedStatus);
    }

    private void SetTileCompleted(bool isCompleted)
    {
        CompleteStatus completeStatus = isCompleted ? CompleteStatus.Yes : CompleteStatus.No;
        tileOne.IsComplete = CompleteLookup.Get(completeStatus);
    }

    private void UpdateBoard()
    {
        BoardUpdater.UpdateTile(board, team, 0);
        BoardUpdater.UpdateTile(board, team, 1);
    }

    private void ShowBoard()
    {
        board.Image.SaveAsPng(imagePath);

        // Opens the image using the system's default program.
        Process.Start("explorer.exe", imagePath);
    }
}
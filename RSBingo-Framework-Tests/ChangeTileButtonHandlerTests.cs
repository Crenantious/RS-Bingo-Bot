//// <copyright file="ChangeTileButtonHandlerTests.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingo_Framework_Tests
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;
//    using RSBingo_Framework.Interfaces;
//    using RSBingo_Framework.Models;
//    using static RSBingo_Framework.Records.TileRecord;
//    using static RSBingo_Framework.Records.BingoTaskRecord;
//    using RSBingo_Framework.Records;
//    using RSBingoBot.RequestHandlers;

//    [TestClass]
//    public class ChangeTileButtonHandlerTests : MockDBBaseTestClass
//    {
//        private const string testTeamName = "Test";

//        IDataWorker dataWorkerBefore = null!;
//        IDataWorker dataWorkerAfter = null!;
//        Team teamBefore = null!;
//        private static BingoTask bingoTask1 = null!;
//        private static BingoTask bingoTask2 = null!;
//        int boardIndex1 = 0;
//        int boardIndex2 = 1;

//        [TestInitialize]
//        public override void TestInitialize()
//        {
//            base.TestInitialize();
//            dataWorkerBefore = CreateDW();
//            dataWorkerAfter = CreateDW();
//            teamBefore = MockDBSetup.Add_Team(dataWorkerBefore, testTeamName);
//            bingoTask1 = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test1", Difficulty.Easy);
//            bingoTask2 = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test2", Difficulty.Easy);
//        }

//        [TestMethod]
//        public void AddTile_TileExistsWithGivenParameters()
//        {
//            ChangeTileButtonHandler.ChangeDBTiles(dataWorkerBefore, teamBefore, null, bingoTask1, boardIndex1);
//            dataWorkerBefore.SaveChanges();
//            Team teamAfter = GetTeamAfter();
//            Tile tileAfter = teamAfter.Tiles.ElementAt(0);

//            AssertTile(tileAfter, bingoTask1, boardIndex1);
//        }

//        [TestMethod]
//        public void SwapExistingBoardTaskWithNoTile_TileExistsWithGivenParameters()
//        {
//            Tile tileBefore = TileRecord.CreateTile(dataWorkerBefore, teamBefore, bingoTask1, boardIndex1);
//            ChangeTileButtonHandler.ChangeDBTiles(dataWorkerBefore, teamBefore, null, bingoTask1, boardIndex2);
//            dataWorkerBefore.SaveChanges();
//            Team teamAfter = GetTeamAfter();
//            Tile tileAfter = teamAfter.Tiles.ElementAt(0);

//            AssertTile(tileAfter,  bingoTask1, boardIndex2);
//            Assert.AreEqual(teamAfter.Tiles.Count, 1);
//        }

//        [TestMethod]
//        public void ChangeTileTask_TileExistsWithGivenParameters()
//        {
//            Tile tileBefore = TileRecord.CreateTile(dataWorkerBefore, teamBefore, bingoTask1, boardIndex1);

//            ChangeTileButtonHandler.ChangeDBTiles(dataWorkerBefore, teamBefore, tileBefore, bingoTask2);
//            dataWorkerBefore.SaveChanges();
//            Team teamAfter = GetTeamAfter();
//            Tile tileAfter = teamAfter.Tiles.ElementAt(0);

//            AssertTile(tileAfter, bingoTask2, boardIndex1);
//        }

//        [TestMethod]
//        public void SwapTileTasks_TilesExistsWithGivenParametersAndSwappedTasks()
//        {
//            Tile tileBefore1 = TileRecord.CreateTile(dataWorkerBefore, teamBefore, bingoTask1, boardIndex1);
//            Tile tileBefore2 = TileRecord.CreateTile(dataWorkerBefore, teamBefore, bingoTask2, boardIndex2);

//            ChangeTileButtonHandler.ChangeDBTiles(dataWorkerBefore, teamBefore, tileBefore1, bingoTask2);
//            dataWorkerBefore.SaveChanges();
//            Team teamAfter = GetTeamAfter();
//            Tile tileAfter1 = teamAfter.Tiles.ElementAt(0);
//            Tile tileAfter2 = teamAfter.Tiles.ElementAt(1);

//            AssertTile(tileAfter1, bingoTask1, boardIndex2);
//            AssertTile(tileAfter2, bingoTask2, boardIndex1);
//        }

//        private Team GetTeamAfter() =>
//            dataWorkerAfter.Teams.GetByName(testTeamName)!;

//        private void AssertTile(Tile tile, BingoTask task, int boardIndex)
//        {
//            Assert.AreEqual(tile.Team.RowId, teamBefore.RowId);
//            Assert.AreEqual(tile.Task.RowId, task.RowId);
//            Assert.AreEqual(tile.BoardIndex, boardIndex);
//        }
//    }
//}

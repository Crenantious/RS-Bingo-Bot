// <copyright file="DbContextTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    [TestClass]
    public class DbContextTests : MockDBBaseTestClass
    {
        // TODO: JCH - Once we have real tests delete this class as it is only here as an example.

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        #region Cascade

        // NOTE: This just is to check DB integrity with cascade. When working with a real DB we will need to update our remove method to fetch Dependants.

        [TestMethod]
        public void CascadeDelete_TeamToUsers_Valid()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            Team teamBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userOneBefore = MockDBSetup.Add_User(dataWorkerBefore, 1, teamBefore);
            User userTwoBefore = MockDBSetup.Add_User(dataWorkerBefore, 2, teamBefore);
            Team teamToKeepBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userToKeepOneBefore = MockDBSetup.Add_User(dataWorkerBefore, 3, teamToKeepBefore);
            User userToKeepTwoBefore = MockDBSetup.Add_User(dataWorkerBefore, 4, teamToKeepBefore);

            dataWorkerBefore.Teams.Remove(teamBefore);
            dataWorkerBefore.SaveChanges();
            IDataWorker dataWorkerAfter = CreateDW();
            Team? teamAfter = dataWorkerAfter.Teams.GetTeamByID(teamBefore.RowId);
            User? userOneAfter = dataWorkerAfter.Users.GetByDiscordId(userOneBefore.DiscordUserId);
            User? userTwoAfter = dataWorkerAfter.Users.GetByDiscordId(userTwoBefore.DiscordUserId);
            Team? teamToKeepAfter = dataWorkerAfter.Teams.GetTeamByID(teamToKeepBefore.RowId);
            User? userToKeepOneAfter = dataWorkerAfter.Users.GetByDiscordId(userToKeepOneBefore.DiscordUserId);
            User? userToKeepTwoAfter = dataWorkerAfter.Users.GetByDiscordId(userToKeepTwoBefore.DiscordUserId);

            Assert.IsNull(teamAfter);
            Assert.IsNull(userOneAfter);
            Assert.IsNull(userTwoAfter);
            Assert.IsNotNull(teamToKeepAfter);
            Assert.IsNotNull(userToKeepOneAfter);
            Assert.IsNotNull(userToKeepTwoAfter);
        }

        [TestMethod]
        public void CascadeDelete_UserToTeam_Valid()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            Team teamBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userOneBefore = MockDBSetup.Add_User(dataWorkerBefore, 1, teamBefore);
            User userTwoBefore = MockDBSetup.Add_User(dataWorkerBefore, 2, teamBefore);

            dataWorkerBefore.Users.Remove(userOneBefore);
            dataWorkerBefore.SaveChanges();
            IDataWorker dataWorkerAfter = CreateDW();
            Team? teamAfter = dataWorkerAfter.Teams.GetTeamByID(teamBefore.RowId);
            User? userOneAfter = dataWorkerAfter.Users.GetByDiscordId(userOneBefore.DiscordUserId);
            User? userTwoAfter = dataWorkerAfter.Users.GetByDiscordId(userTwoBefore.DiscordUserId);

            Assert.IsNotNull(teamAfter);
            Assert.IsNotNull(userTwoAfter);
            Assert.IsNull(userOneAfter);
        }

        [TestMethod]
        public void CascadeDelete_UserToEvidence_Valid()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            Team teamBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userBefore = MockDBSetup.Add_User(dataWorkerBefore, 1, teamBefore);
            BingoTask bingoTask = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test");
            Tile tileBefore = MockDBSetup.Add_Tile(dataWorkerBefore, teamBefore, bingoTask);
            Evidence evidenceBefore = MockDBSetup.Add_Evidence(dataWorkerBefore, userBefore, tileBefore);

            dataWorkerBefore.Users.Remove(userBefore);
            dataWorkerBefore.SaveChanges();
            IDataWorker dataWorkerAfter = CreateDW();
            User? userAfter = dataWorkerAfter.Users.GetByDiscordId(userBefore.DiscordUserId);
            Evidence? evidenceAfter = dataWorkerAfter.Evidence.GetById(evidenceBefore.RowId);

            Assert.IsNull(userAfter);
            Assert.IsNull(evidenceAfter);
        }

        [TestMethod]
        public void CascadeDelete_EvidenceToUserAndTile_Valid()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            Team teamBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userBefore = MockDBSetup.Add_User(dataWorkerBefore, 1, teamBefore);
            BingoTask bingoTask = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test");
            Tile tileBefore = MockDBSetup.Add_Tile(dataWorkerBefore, teamBefore, bingoTask);
            Evidence evidenceBefore = MockDBSetup.Add_Evidence(dataWorkerBefore, userBefore, tileBefore);

            dataWorkerBefore.Evidence.Remove(evidenceBefore);
            dataWorkerBefore.SaveChanges();
            IDataWorker dataWorkerAfter = CreateDW();
            User? userAfter = dataWorkerAfter.Users.GetByDiscordId(userBefore.DiscordUserId);
            Evidence? evidenceAfter = dataWorkerAfter.Evidence.GetById(evidenceBefore.RowId);
            Tile? tileAfter = dataWorkerAfter.Tiles.GetById(tileBefore.RowId);

            Assert.IsNotNull(tileAfter);
            Assert.IsNotNull(userAfter);
            Assert.IsNull(evidenceAfter);
        }

        [TestMethod]
        public void CascadeDelete_TileToEvidenceAndUser_Valid()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            Team teamBefore = MockDBSetup.Add_Team(dataWorkerBefore);
            User userBefore = MockDBSetup.Add_User(dataWorkerBefore, 1, teamBefore);
            BingoTask bingoTask = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test");
            Tile tileBefore = MockDBSetup.Add_Tile(dataWorkerBefore, teamBefore, bingoTask);
            Evidence evidenceBefore = MockDBSetup.Add_Evidence(dataWorkerBefore, userBefore, tileBefore);

            dataWorkerBefore.Tiles.Remove(tileBefore);
            dataWorkerBefore.SaveChanges();
            IDataWorker dataWorkerAfter = CreateDW();
            User? userAfter = dataWorkerAfter.Users.GetByDiscordId(userBefore.DiscordUserId);
            Evidence? evidenceAfter = dataWorkerAfter.Evidence.GetById(evidenceBefore.RowId);
            Tile? tileAfter = dataWorkerAfter.Tiles.GetById(tileBefore.RowId);

            Assert.IsNotNull(userAfter);
            Assert.IsNull(tileAfter);
            Assert.IsNull(evidenceAfter);
        }

        #endregion
    }
}

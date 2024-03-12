// <copyright file="SubmitEvidenceTileValidatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingoBot.Requests.SubmitEvidenceTileValidator;

public record EvidenceDTO(EvidenceType EvidenceType, EvidenceStatus EvidenceStatus);

[TestClass]
public class SubmitEvidenceTileValidatorTests : MockDBBaseTestClass
{
    private const string testTeamName = "Test";

    private static EvidenceDTO pendingVerification = new(EvidenceType.TileVerification, EvidenceStatus.PendingReview);
    private static EvidenceDTO rejectedVerification = new(EvidenceType.TileVerification, EvidenceStatus.Rejected);
    private static EvidenceDTO acceptedVerification = new(EvidenceType.TileVerification, EvidenceStatus.Accepted);
    private static EvidenceDTO pendingDrop = new(EvidenceType.Drop, EvidenceStatus.PendingReview);
    private static EvidenceDTO rejectedDrop = new(EvidenceType.Drop, EvidenceStatus.Rejected);
    private static EvidenceDTO acceptedDrop = new(EvidenceType.Drop, EvidenceStatus.Accepted);

    private IDataWorker dataWorker = null!;
    private Team team = null!;
    private User userOne = null!;
    private User userTwo = null!;
    private TeamScore teamScore = null!;
    private BingoTask taskOne = null!;
    private BingoTask taskTwo = null!;
    private Tile tileOne = null!;
    private Tile tile2 = null!;

    private Dictionary<EvidenceEnum, EvidenceDTO> evidenceDTOLookup = new()
    {
        { EvidenceEnum.pendingVerification, pendingVerification},
        { EvidenceEnum.rejectedVerification, rejectedVerification},
        { EvidenceEnum.acceptedVerification, acceptedVerification},
        { EvidenceEnum.pendingDrop, pendingDrop},
        { EvidenceEnum.rejectedDrop,rejectedDrop },
        { EvidenceEnum.acceptedDrop,acceptedDrop }
    };

    [Flags]
    public enum EvidenceEnum
    {
        pendingVerification = 1 << 0,
        rejectedVerification = 1 << 1,
        acceptedVerification = 1 << 2,
        pendingDrop = 1 << 3,
        rejectedDrop = 1 << 4,
        acceptedDrop = 1 << 5
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorker = CreateDW();

        team = MockDBSetup.Add_Team(dataWorker, testTeamName);

        taskOne = MockDBSetup.Add_BingoTask(dataWorker, "Test1", Difficulty.Easy);
        taskTwo = MockDBSetup.Add_BingoTask(dataWorker, "Test2", Difficulty.Easy);

        tileOne = MockDBSetup.Add_Tile(dataWorker, team, taskOne, 0);
        tile2 = MockDBSetup.Add_Tile(dataWorker, team, taskTwo, 1);

        dataWorker.SaveChanges();
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.TileVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.TileVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.Drop, true)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.pendingDrop, EvidenceType.Drop, true)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.rejectedDrop, EvidenceType.Drop, true)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.TileVerification, false)]
    public void ValidateEvidenceForOneUser(EvidenceEnum evidenceEnum, EvidenceType evidenceType, bool isValidExpected)
    {
        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        AddEvidence(userOne, tileOne, evidenceEnum);

        bool isValidActual = Validate(tileOne, evidenceType, userOne.DiscordUserId);

        Assert.AreEqual(isValidExpected, isValidActual);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.TileVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.TileVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.TileVerification, true)]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.pendingDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.rejectedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.TileVerification, false)]
    public void TestUserHasNoAcceptedVerificationEvidence_ValidateEvidenceForTwoUsers(EvidenceEnum evidenceEnum,
        EvidenceType evidenceType, bool isValidExpected)
    {
        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        userTwo = MockDBSetup.Add_User(dataWorker, 1, team);
        AddEvidence(userOne, tileOne, evidenceEnum);

        bool isValidActual = Validate(tileOne, evidenceType, userTwo.DiscordUserId);

        Assert.AreEqual(isValidExpected, isValidActual);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.pendingVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedVerification, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification, EvidenceType.Drop, true)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.pendingDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.rejectedDrop, EvidenceType.Drop, true)]
    [DataRow(EvidenceEnum.acceptedVerification | EvidenceEnum.acceptedDrop, EvidenceType.Drop, false)]
    [DataRow(EvidenceEnum.pendingDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.rejectedDrop, EvidenceType.TileVerification, false)]
    [DataRow(EvidenceEnum.acceptedDrop, EvidenceType.TileVerification, false)]
    public void TestUserHasAcceptedVerificationEvidence_ValidateEvidenceForTwoUsers(EvidenceEnum evidenceEnum,
        EvidenceType evidenceType, bool isValidExpected)
    {
        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        userTwo = MockDBSetup.Add_User(dataWorker, 1, team);
        AddEvidence(userOne, tileOne, evidenceEnum);
        AddEvidence(userTwo, tileOne, EvidenceEnum.acceptedVerification);

        bool isValidActual = Validate(tileOne, evidenceType, userTwo.DiscordUserId);

        Assert.AreEqual(isValidExpected, isValidActual);
    }

    private void AddEvidence(User user, Tile tile, EvidenceEnum evidence)
    {
        foreach (EvidenceEnum evidenceEnum in Enum.GetValues(typeof(EvidenceEnum)))
        {
            var a = (int)evidenceEnum;
            if ((evidence & evidenceEnum) == evidenceEnum)
            {
                EvidenceDTO evidenceDTO = evidenceDTOLookup[evidenceEnum];
                AddEvidence(user, tile, evidenceDTO.EvidenceType, evidenceDTO.EvidenceStatus);
            }
        }
    }

    private Evidence AddEvidence(User user, Tile tile, EvidenceType evidenceType, EvidenceStatus evidenceStatus) =>
       MockDBSetup.Add_Evidence(dataWorker, user, tile, evidenceType, evidenceStatus);
}
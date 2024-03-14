// <copyright file="SubmitEvidenceTileValidatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.Records.EvidenceRecord;

public record EvidenceDTO(EvidenceType EvidenceType, EvidenceStatus EvidenceStatus);

[TestClass]
public class TSVTestsBase : MockDBBaseTestClass
{
    private const string testTeamName = "Test";

    private static EvidenceDTO pendingVerification = new(EvidenceType.TileVerification, EvidenceStatus.PendingReview);
    private static EvidenceDTO rejectedVerification = new(EvidenceType.TileVerification, EvidenceStatus.Rejected);
    private static EvidenceDTO acceptedVerification = new(EvidenceType.TileVerification, EvidenceStatus.Accepted);
    private static EvidenceDTO pendingDrop = new(EvidenceType.Drop, EvidenceStatus.PendingReview);
    private static EvidenceDTO rejectedDrop = new(EvidenceType.Drop, EvidenceStatus.Rejected);
    private static EvidenceDTO acceptedDrop = new(EvidenceType.Drop, EvidenceStatus.Accepted);

    private ISubmitEvidenceTSV submitEvidenceTSV = null!;
    private BingoTask taskOne = null!;
    private BingoTask taskTwo = null!;
    private bool? isValid = null;

    protected IDataWorker dataWorker { get; set; } = null!;
    protected Team team { get; set; } = null!;
    protected User userOne { get; set; } = null!;
    protected User userTwo { get; set; } = null!;
    protected Tile tileOne { get; set; } = null!;
    protected Tile tileTwo { get; set; } = null!;

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

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);
        services.AddSingleton(typeof(ISubmitEvidenceTSV), typeof(SubmitEvidenceTSV));
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
        tileTwo = MockDBSetup.Add_Tile(dataWorker, team, taskTwo, 1);

        submitEvidenceTSV = General.DI.Get<ISubmitEvidenceTSV>();

        dataWorker.SaveChanges();
    }

    protected void AddEvidence(User user, Tile tile, EvidenceEnum evidence)
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


    protected void Validate(Tile tile, User user, EvidenceType evidenceType)
    {
        isValid = submitEvidenceTSV.Validate(tile, user, evidenceType);
    }

    public void AssertValidation(bool expected)
    {
        if (isValid is null)
        {
            throw new InvalidOperationException("Must validate the data before asserting.");
        }

        Assert.AreEqual(expected, isValid);
    }
}
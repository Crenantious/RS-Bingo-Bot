// <copyright file="SubmitEvidenceTileValidatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

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

    protected bool? IsValid { get; set; } = null;
    protected IDataWorker DataWorker { get; set; } = null!;
    protected Team Team { get; set; } = null!;
    protected User UserOne { get; set; } = null!;
    protected User UserTwo { get; set; } = null!;
    protected Tile TileOne { get; set; } = null!;
    protected Tile TileTwo { get; set; } = null!;

    private Dictionary<EvidenceEnum, EvidenceDTO> evidenceDTOLookup = new()
    {
        { EvidenceEnum.PendingVerification, pendingVerification},
        { EvidenceEnum.RejectedVerification, rejectedVerification},
        { EvidenceEnum.AcceptedVerification, acceptedVerification},
        { EvidenceEnum.PendingDrop, pendingDrop},
        { EvidenceEnum.RejectedDrop,rejectedDrop },
        { EvidenceEnum.AcceptedDrop,acceptedDrop }
    };

    [Flags]
    public enum EvidenceEnum
    {
        None = 0,
        PendingVerification = 1 << 0,
        RejectedVerification = 1 << 1,
        AcceptedVerification = 1 << 2,
        PendingDrop = 1 << 3,
        RejectedDrop = 1 << 4,
        AcceptedDrop = 1 << 5
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        DataWorker = CreateDW();

        Team = MockDBSetup.Add_Team(DataWorker, testTeamName);

        taskOne = MockDBSetup.Add_BingoTask(DataWorker, "Test1", Difficulty.Easy);
        taskTwo = MockDBSetup.Add_BingoTask(DataWorker, "Test2", Difficulty.Easy);

        TileOne = MockDBSetup.Add_Tile(DataWorker, Team, taskOne, 0);
        TileTwo = MockDBSetup.Add_Tile(DataWorker, Team, taskTwo, 1);

        submitEvidenceTSV = General.DI.Get<ISubmitEvidenceTSV>();

        DataWorker.SaveChanges();
    }

    protected List<Evidence> AddEvidence(Tile tile, User user, EvidenceEnum evidenceEnum)
    {
        List<Evidence> addedEvidence = new();
        IEnumerable<EvidenceDTO> DTOs = EvidenceEnumToDTO(evidenceEnum);

        foreach (EvidenceDTO DTO in DTOs)
        {
            Evidence evidence = MockDBSetup.Add_Evidence(DataWorker, user, tile, DTO.EvidenceType, DTO.EvidenceStatus);
            addedEvidence.Add(evidence);
        }

        return addedEvidence;
    }

    protected void AssertEvidence(IEnumerable<Evidence> evidence, EvidenceEnum evidenceEnum)
    {
        List<EvidenceDTO> expected = EvidenceEnumToDTO(evidenceEnum).ToList();
        List<EvidenceDTO> actual = new();

        foreach (Evidence e in evidence)
        {
            actual.Add(new(EvidenceTypeLookup.Get(e.EvidenceType), EvidenceStatusLookup.Get(e.Status)));
        }

        CollectionAssert.AreEquivalent(expected, actual);
    }

    public void AssertValidation(bool expected)
    {
        if (IsValid is null)
        {
            throw new InvalidOperationException("Must validate the data before asserting.");
        }

        Assert.AreEqual(expected, IsValid);
    }

    private IEnumerable<EvidenceDTO> EvidenceEnumToDTO(EvidenceEnum evidenceEnum)
    {
        List<EvidenceDTO> DTOs = new();

        foreach (EvidenceEnum value in Enum.GetValues(typeof(EvidenceEnum)))
        {
            if (value == EvidenceEnum.None)
            {
                continue;
            }

            if ((evidenceEnum & value) == value)
            {
                DTOs.Add(evidenceDTOLookup[value]);
            }
        }

        return DTOs;
    }
}
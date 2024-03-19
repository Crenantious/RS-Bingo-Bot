// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class UserHasNoAcceptedVerificationEvidenceForTileDPTests : TSVDataParserTestBase
{
    private UserHasNoAcceptedVerificationEvidenceForTileDP parser = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);
        UserTwo = MockDBSetup.Add_User(DataWorker, 1, Team);

        parser = new();
    }

    [TestMethod]
    public void UserHasNoEvidence_NoEvidenceParsed()
    {
        ParseData();

        AssertData(Enumerable.Empty<Evidence>());
    }

    [TestMethod]
    [DataRow(EvidenceEnum.PendingVerification, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.RejectedVerification, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.PendingDrop, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.RejectedDrop, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.AcceptedDrop, EvidenceEnum.None)]
    public void UserHasOneEvidence(EvidenceEnum addedEvidence, EvidenceEnum expectedEvidence)
    {
        AddEvidence(TileOne, UserOne, addedEvidence);

        ParseData();

        AssertEvidence(parser.Evidence, expectedEvidence);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.PendingVerification | EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.RejectedVerification | EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.PendingDrop | EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.RejectedDrop | EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.AcceptedDrop | EvidenceEnum.AcceptedVerification, EvidenceEnum.AcceptedVerification)]
    public void UserHasTwoEvidence(EvidenceEnum addedEvidence, EvidenceEnum expectedEvidence)
    {
        AddEvidence(TileOne, UserOne, addedEvidence);

        ParseData();

        AssertEvidence(parser.Evidence, expectedEvidence);
    }

    [TestMethod]
    public void UserHasTwoAcceptedVerificationEvidence_ShouldParseBoth()
    {
        var evidenceOne = AddEvidence(TileOne, UserOne, EvidenceEnum.AcceptedVerification);
        var evidenceTwo = AddEvidence(TileOne, UserOne, EvidenceEnum.AcceptedVerification);

        ParseData();

        AssertData(evidenceOne.Concat(evidenceTwo));
    }

    [TestMethod]
    public void TwoUsersHaveAcceptedVerificationEvidence_ShouldParseOnlyForUserOne()
    {
        var evidenceOne = AddEvidence(TileOne, UserOne, EvidenceEnum.AcceptedVerification);
        var evidenceTwo = AddEvidence(TileOne, UserTwo, EvidenceEnum.AcceptedVerification);

        ParseData();

        AssertData(evidenceOne);
    }

    private void AddEvidence(EvidenceType evidenceType, EvidenceStatus evidenceStatus)
    {
        MockDBSetup.Add_Evidence(DataWorker, UserOne, TileOne, evidenceType, evidenceStatus);
    }

    /// <summary>
    /// Parses the data for <see cref="tileOne"/> and <see cref="userOne"/>.
    /// </summary>
    private void ParseData()
    {
        parser.Parse(TileOne, UserOne);
    }

    private void AssertData(IEnumerable<Evidence> expectedEvidence)
    {
        CollectionAssert.AreEquivalent(expectedEvidence.ToList(), parser.Evidence.ToList());
    }
}
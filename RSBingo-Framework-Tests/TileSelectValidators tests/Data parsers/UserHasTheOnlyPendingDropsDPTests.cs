// <copyright file="UserHasTheOnlyPendingDropsDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class UserHasTheOnlyPendingDropsDPTests : TSVDataParserTestBase
{
    private UserHasTheOnlyPendingDropsDP parser = null!;

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
    [DataRow(EvidenceEnum.AcceptedVerification, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.RejectedDrop, EvidenceEnum.None)]
    [DataRow(EvidenceEnum.AcceptedDrop, EvidenceEnum.None)]
    public void UserHasOneEvidence(EvidenceEnum addedEvidence, EvidenceEnum expectedEvidence)
    {
        AddEvidence(TileOne, UserOne, addedEvidence);

        ParseData();

        AssertEvidence(parser.Evidence, expectedEvidence);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.PendingVerification | EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.RejectedVerification | EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.AcceptedVerification | EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.RejectedDrop | EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.AcceptedDrop | EvidenceEnum.PendingDrop, EvidenceEnum.PendingDrop)]
    public void UserHasTwoEvidence(EvidenceEnum addedEvidence, EvidenceEnum expectedEvidence)
    {
        AddEvidence(TileOne, UserOne, addedEvidence);

        ParseData();

        AssertEvidence(parser.Evidence, expectedEvidence);
    }

    [TestMethod]
    public void UserHasTwoPendingDropEvidence_ShouldParseBoth()
    {
        var evidenceOne = AddEvidence(TileOne, UserOne, EvidenceEnum.PendingDrop);
        var evidenceTwo = AddEvidence(TileOne, UserOne, EvidenceEnum.PendingDrop);

        ParseData();

        AssertData(evidenceOne.Concat(evidenceTwo));
    }

    [TestMethod]
    public void TwoUsersHavePendingDropEvidence_ShouldParseOnlyForUserOne()
    {
        var evidenceOne = AddEvidence(TileOne, UserOne, EvidenceEnum.PendingDrop);
        var evidenceTwo = AddEvidence(TileOne, UserTwo, EvidenceEnum.PendingDrop);

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
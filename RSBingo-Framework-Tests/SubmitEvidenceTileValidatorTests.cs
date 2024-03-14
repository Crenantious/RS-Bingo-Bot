// <copyright file="SubmitEvidenceTileValidatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class SubmitEvidenceTileValidatorTests : TSVTestsBase
{
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

        Validate(tileOne, userOne, evidenceType);

        AssertValidation(isValidExpected);
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

        Validate(tileOne, userTwo, evidenceType);

        AssertValidation(isValidExpected);
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

        Validate(tileOne, userTwo, evidenceType);

        AssertValidation(isValidExpected);
    }
}
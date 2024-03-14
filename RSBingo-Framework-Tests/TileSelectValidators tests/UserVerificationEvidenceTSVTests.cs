// <copyright file="VerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

[TestClass]
public class UserVerificationEvidenceTSVTests : TSVTestsBase
{
    private IUserVerificationEvidenceTSV validator = null!;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);
        services.AddSingleton(typeof(IUserVerificationEvidenceTSV), typeof(UserVerificationEvidenceTSV));
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        userTwo = MockDBSetup.Add_User(dataWorker, 1, team);

        validator = General.DI.Get<IUserVerificationEvidenceTSV>();
    }

    [TestMethod]
    public void NoEvidenceSubmitted_Invalid()
    {
        Validate(tileOne, userOne);

        AssertValidation(false);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, false)]
    [DataRow(EvidenceEnum.rejectedVerification, false)]
    [DataRow(EvidenceEnum.acceptedVerification, true)]
    [DataRow(EvidenceEnum.pendingDrop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, false)]
    public void FirstUserHasEvidenceSubmitted_ValidateForFirstUser(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileOne, userOne);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, false)]
    [DataRow(EvidenceEnum.rejectedVerification, false)]
    [DataRow(EvidenceEnum.acceptedVerification, false)]
    [DataRow(EvidenceEnum.pendingDrop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, false)]
    public void FirstUserHasEvidenceSubmitted_ValidateForSecondUser(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileOne, userTwo);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, false)]
    [DataRow(EvidenceEnum.rejectedVerification, false)]
    [DataRow(EvidenceEnum.acceptedVerification, false)]
    [DataRow(EvidenceEnum.pendingDrop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, false)]
    public void FirstUserHasEvidenceSubmittedForFirstTile_ValidateForFirstUserAndSecondTile(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileTwo, userOne);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, false)]
    [DataRow(EvidenceEnum.rejectedVerification, false)]
    [DataRow(EvidenceEnum.acceptedVerification, false)]
    [DataRow(EvidenceEnum.pendingDrop, false)]
    [DataRow(EvidenceEnum.rejectedDrop, false)]
    [DataRow(EvidenceEnum.acceptedDrop, false)]
    public void FirstUserHasEvidenceSubmittedForFirstTile_ValidateForSecondUserAndSecondTile(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileTwo, userTwo);

        AssertValidation(expected);
    }

    private void Validate(Tile tile, User user)
    {
        isValid = validator.Validate(tile, user);
    }
}
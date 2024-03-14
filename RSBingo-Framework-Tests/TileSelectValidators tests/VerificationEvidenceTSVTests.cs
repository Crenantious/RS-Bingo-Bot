// <copyright file="VerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class VerificationEvidenceTSVTests : TSVTestsBase
{
    private IVerificationEvidenceTSV validator = null!;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);
        services.AddSingleton(typeof(IVerificationEvidenceTSV), typeof(VerificationEvidenceTSV));
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        userTwo = MockDBSetup.Add_User(dataWorker, 1, team);

        validator = General.DI.Get<IVerificationEvidenceTSV>();
    }

    [TestMethod]
    public void NoEvidenceSubmitted_TrySubmitEvidenceForFirstUser_Allowed()
    {
        Validate(tileOne, userOne);

        AssertValidation(true);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, false)]
    public void FirstUserHasEvidenceSubmitted_TrySubmitEvidenceForFirstUser(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileOne, userOne);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, true)]
    public void FirstUserHasEvidenceSubmitted_TrySubmitForSecondUser(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileOne, userTwo);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, true)]
    public void FirstUserHasEvidenceSubmittedForFirstTile_TrySubmitEvidenceForFirstUserForSecondTile(EvidenceEnum evidenceEnum, bool expected)
    {
        AddEvidence(tileOne, userOne, evidenceEnum);

        Validate(tileTwo, userOne);

        AssertValidation(expected);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.pendingVerification, true)]
    [DataRow(EvidenceEnum.rejectedVerification, true)]
    [DataRow(EvidenceEnum.acceptedVerification, true)]
    public void FirstUserHasEvidenceSubmittedForFirstTile_TrySubmitEvidenceForSecondUserForSecondTile(EvidenceEnum evidenceEnum, bool expected)
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
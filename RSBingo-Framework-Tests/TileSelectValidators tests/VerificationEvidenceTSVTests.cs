//// <copyright file="VerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingo_Framework_Tests;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RSBingo_Common;
//using RSBingo_Framework.Models;
//using RSBingoBot.Requests;
//using static RSBingo_Framework.Records.EvidenceRecord;

//[TestClass]
//public class VerificationEvidenceTSVTests : TSVTestsBase
//{
//    private ISubmitVerificationEvidenceTSV validator = null!;

//    protected override void AddServices(ServiceCollection services)
//    {
//        base.AddServices(services);
//        services.AddSingleton(typeof(ISubmitVerificationEvidenceTSV), typeof(SubmitVerificationEvidenceTSV));
//    }

//    [TestInitialize]
//    public override void TestInitialize()
//    {
//        base.TestInitialize();

//        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);
//        UserTwo = MockDBSetup.Add_User(DataWorker, 1, Team);

//        validator = General.DI.Get<ISubmitVerificationEvidenceTSV>();
//    }

//    [TestMethod]
//    public void NoEvidenceSubmitted_TrySubmitEvidenceForFirstUser_Allowed()
//    {
//        Validate(TileOne, UserOne);

//        AssertValidation(true);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, true)]
//    [DataRow(EvidenceEnum.RejectedVerification, true)]
//    [DataRow(EvidenceEnum.AcceptedVerification, false)]
//    public void FirstUserHasEvidenceSubmitted_TrySubmitEvidenceForFirstUser(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileOne, UserOne);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, true)]
//    [DataRow(EvidenceEnum.RejectedVerification, true)]
//    [DataRow(EvidenceEnum.AcceptedVerification, true)]
//    public void FirstUserHasEvidenceSubmitted_TrySubmitForSecondUser(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileOne, UserTwo);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, true)]
//    [DataRow(EvidenceEnum.RejectedVerification, true)]
//    [DataRow(EvidenceEnum.AcceptedVerification, true)]
//    public void FirstUserHasEvidenceSubmittedForFirstTile_TrySubmitEvidenceForFirstUserForSecondTile(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileTwo, UserOne);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, true)]
//    [DataRow(EvidenceEnum.RejectedVerification, true)]
//    [DataRow(EvidenceEnum.AcceptedVerification, true)]
//    public void FirstUserHasEvidenceSubmittedForFirstTile_TrySubmitEvidenceForSecondUserForSecondTile(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileTwo, UserTwo);

//        AssertValidation(expected);
//    }

//    private void Validate(Tile tile, User user)
//    {
//        IsValid = validator.Validate(tile, user);
//    }
//}
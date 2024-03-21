//// <copyright file="VerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingo_Framework_Tests;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RSBingo_Common;
//using RSBingo_Framework.Models;
//using RSBingoBot.Requests;

//[TestClass]
//public class UserVerificationEvidenceTSVTests : TSVTestsBase
//{
//    private IUserVerificationEvidenceTSV validator = null!;

//    protected override void AddServices(ServiceCollection services)
//    {
//        base.AddServices(services);
//        services.AddSingleton(typeof(IUserVerificationEvidenceTSV), typeof(UserVerificationEvidenceTSV));
//    }

//    [TestInitialize]
//    public override void TestInitialize()
//    {
//        base.TestInitialize();

//        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);
//        UserTwo = MockDBSetup.Add_User(DataWorker, 1, Team);

//        validator = General.DI.Get<IUserVerificationEvidenceTSV>();
//    }

//    [TestMethod]
//    public void NoEvidenceSubmitted_Invalid()
//    {
//        Validate(TileOne, UserOne);

//        AssertValidation(false);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, false)]
//    [DataRow(EvidenceEnum.RejectedVerification, false)]
//    [DataRow(EvidenceEnum.AcceptedVerification, true)]
//    [DataRow(EvidenceEnum.PendingDrop, false)]
//    [DataRow(EvidenceEnum.RejectedDrop, false)]
//    [DataRow(EvidenceEnum.AcceptedDrop, false)]
//    public void FirstUserHasEvidenceSubmitted_ValidateForFirstUser(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileOne, UserOne);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, false)]
//    [DataRow(EvidenceEnum.RejectedVerification, false)]
//    [DataRow(EvidenceEnum.AcceptedVerification, false)]
//    [DataRow(EvidenceEnum.PendingDrop, false)]
//    [DataRow(EvidenceEnum.RejectedDrop, false)]
//    [DataRow(EvidenceEnum.AcceptedDrop, false)]
//    public void FirstUserHasEvidenceSubmitted_ValidateForSecondUser(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileOne, UserTwo);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, false)]
//    [DataRow(EvidenceEnum.RejectedVerification, false)]
//    [DataRow(EvidenceEnum.AcceptedVerification, false)]
//    [DataRow(EvidenceEnum.PendingDrop, false)]
//    [DataRow(EvidenceEnum.RejectedDrop, false)]
//    [DataRow(EvidenceEnum.AcceptedDrop, false)]
//    public void FirstUserHasEvidenceSubmittedForFirstTile_ValidateForFirstUserAndSecondTile(EvidenceEnum evidenceEnum, bool expected)
//    {
//        AddEvidence(TileOne, UserOne, evidenceEnum);

//        Validate(TileTwo, UserOne);

//        AssertValidation(expected);
//    }

//    [TestMethod]
//    [DataRow(EvidenceEnum.PendingVerification, false)]
//    [DataRow(EvidenceEnum.RejectedVerification, false)]
//    [DataRow(EvidenceEnum.AcceptedVerification, false)]
//    [DataRow(EvidenceEnum.PendingDrop, false)]
//    [DataRow(EvidenceEnum.RejectedDrop, false)]
//    [DataRow(EvidenceEnum.AcceptedDrop, false)]
//    public void FirstUserHasEvidenceSubmittedForFirstTile_ValidateForSecondUserAndSecondTile(EvidenceEnum evidenceEnum, bool expected)
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
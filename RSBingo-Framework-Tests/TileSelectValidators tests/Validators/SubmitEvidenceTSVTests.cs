// <copyright file="SubmitEvidenceTSVTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.TSV.Validators;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RSBingo_Common;
using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class SubmitEvidenceTSVTests : TSVTestBase
{
    private SubmitEvidenceTSV tsv = null!;
    private Mock<ISubmitEvidenceDP> parser = new();
    private Mock<ISubmitDropEvidenceTSV> dropTSV = new();
    private Mock<ISubmitVerificationEvidenceTSV> verificationTSV = new();
    private bool? isValid = null;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);

        services.AddSingleton<ISubmitDropEvidenceTSV>(s => dropTSV.Object);
        services.AddSingleton<ISubmitVerificationEvidenceTSV>(s => verificationTSV.Object);
        services.AddSingleton<ISubmitEvidenceDP>(s => parser.Object);
        services.AddTransient<SubmitEvidenceTSV>();
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        tsv = General.DI.Get<SubmitEvidenceTSV>();
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public void ParserHasDropEvidence_TSVValidationOnlyDependsOnTheDropTSV(bool isDropTSVValid, bool isVerificationTSVValid)
    {
        parser.Setup(p => p.EvidenceType)
            .Returns(EvidenceType.Drop);
        dropTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(isDropTSVValid);
        verificationTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(isVerificationTSVValid);

        Validate();

        AssertValidation(isDropTSVValid);
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public void ParserHasVerificationEvidence_TSVValidationOnlyDependsOnTheVerificationTSV(bool isDropTSVValid, bool isVerificationTSVValid)
    {
        parser.Setup(p => p.EvidenceType)
            .Returns(EvidenceType.TileVerification);
        dropTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(isDropTSVValid);
        verificationTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(isVerificationTSVValid);

        Validate();

        AssertValidation(isVerificationTSVValid);
    }

    [TestMethod]
    public void TestTSVPassesCorrectDataToDropTSV()
    {
        bool isValid = false;
        parser.Setup(p => p.EvidenceType)
            .Returns(EvidenceType.Drop);

        parser.Setup(p => p.Tile)
            .Returns(TileOne);

        parser.Setup(p => p.User)
            .Returns(UserOne);

        dropTSV.Setup(t => t.Validate(It.Is<Tile>(t => t == TileOne), It.Is<User>(u => u == UserOne)))
            .Callback(() => isValid = true);

        Validate();

        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void TestTSVPassesCorrectDataToVerificationTSV()
    {
        bool isValid = false;
        parser.Setup(p => p.EvidenceType)
            .Returns(EvidenceType.TileVerification);

        parser.Setup(p => p.Tile)
            .Returns(TileOne);

        parser.Setup(p => p.User)
            .Returns(UserOne);

        verificationTSV.Setup(t => t.Validate(It.Is<Tile>(t => t == TileOne), It.Is<User>(u => u == UserOne)))
            .Callback(() => isValid = true);

        Validate();

        Assert.IsTrue(isValid);
    }

    private void Validate()
    {
        // We don't care about the parameters because we've mocked
        // the parser that contains the data to be used.
        isValid = tsv.Validate(null!, null!, EvidenceType.Undefined);
    }

    private void AssertValidation(bool expected)
    {
        if (isValid is null)
        {
            throw new InvalidOperationException($"Must call {nameof(Validate)} before {nameof(AssertValidation)}.");
        }

        Assert.AreEqual(expected, isValid);
    }
}
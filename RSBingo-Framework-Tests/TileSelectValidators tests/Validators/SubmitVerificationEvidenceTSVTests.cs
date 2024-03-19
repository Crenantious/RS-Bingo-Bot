// <copyright file="SubmitVerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
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

[TestClass]
public class SubmitVerificationEvidenceTSVTests : TSVTestBase
{
    private SubmitVerificationEvidenceTSV tsv = null!;
    private Mock<IUserHasNoAcceptedVerificationEvidenceForTileTSV> dependentTSV = new();
    private Mock<ISubmitVerificationEvidenceDP> parser = new();
    private bool? isValid = null;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);

        services.AddSingleton<IUserHasNoAcceptedVerificationEvidenceForTileTSV>(s => dependentTSV.Object);
        services.AddSingleton<ISubmitVerificationEvidenceDP>(s => parser.Object);
        services.AddTransient<SubmitVerificationEvidenceTSV>();
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        tsv = General.DI.Get<SubmitVerificationEvidenceTSV>();
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void TestTSVValidationMatchesDependentTSVValidation(bool valid)
    {
        dependentTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(valid);

        Validate();

        AssertValidation(valid);
    }

    [TestMethod]
    public void TestTSVPassesCorrectDataToDependentTSV()
    {
        parser.Setup(p => p.Tile)
            .Returns(TileOne);

        parser.Setup(p => p.User)
            .Returns(UserOne);

        dependentTSV.Setup(t => t.Validate(It.Is<Tile>(t => t == TileOne), It.Is<User>(u => u == UserOne)))
            .Returns(true);
        dependentTSV.Setup(t => t.Validate(It.Is<Tile>(t => t != TileOne), It.Is<User>(u => u != UserOne)))
            .Returns(false);

        Validate();

        AssertValidation(true);
    }

    private void Validate()
    {
        // We don't care about the parameters because we've mocked
        // the parser that contains the data to be used.
        isValid = tsv.Validate(null!, null!);
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
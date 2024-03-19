// <copyright file="SubmitDropEvidenceTSVTests.cs" company="PlaceholderCompany">
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
public class SubmitDropEvidenceTSVTests : TSVTestBase
{
    private SubmitDropEvidenceTSV tsv = null!;
    private Mock<ISubmitDropEvidenceDP> parser = new();
    private Mock<ITileCanRecieveDropsTSV> tileDropsTSV = new();
    private Mock<IUserHasTheOnlyPendingDropsTSV> userDropsTSV = new();
    private bool? isValid = null;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);

        services.AddSingleton<ISubmitDropEvidenceDP>(s => parser.Object);
        services.AddSingleton<ITileCanRecieveDropsTSV>(s => tileDropsTSV.Object);
        services.AddSingleton<IUserHasTheOnlyPendingDropsTSV>(s => userDropsTSV.Object);
        services.AddTransient<SubmitDropEvidenceTSV>();
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        tsv = General.DI.Get<SubmitDropEvidenceTSV>();
    }

    [TestMethod]
    [DataRow(true, true)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(false, false)]
    public void TestTSVValidationIsOnlyValidIfBothDepenentTSVsAreValid(bool isTileDropsTSV, bool isUserDropsTSV)
    {
        tileDropsTSV.Setup(t => t.Validate(It.IsAny<Tile>()))
            .Returns(isTileDropsTSV);
        userDropsTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(isUserDropsTSV);

        Validate();

        AssertValidation(isTileDropsTSV && isUserDropsTSV);
    }

    [TestMethod]
    public void TestTSVPassesCorrectDataToTileDropTSV()
    {
        bool isValid = false;

        parser.Setup(p => p.Tile)
            .Returns(TileOne);

        parser.Setup(p => p.User)
            .Returns(UserOne);

        // This is required in case there's a guard statement in the TSV that causes the tileDropsTSV to not be checked.
        userDropsTSV.Setup(t => t.Validate(It.IsAny<Tile>(), It.IsAny<User>()))
            .Returns(true);

        tileDropsTSV.Setup(t => t.Validate(It.Is<Tile>(t => t == TileOne)))
            .Callback(() => isValid = true);

        Validate();

        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void TestTSVPassesCorrectDataToUserDropTSV()
    {
        bool isValid = false;

        parser.Setup(p => p.Tile)
            .Returns(TileOne);

        parser.Setup(p => p.User)
            .Returns(UserOne);

        // This is required in case there's a guard statement in the TSV that causes the userDropsTSV to not be checked.
        tileDropsTSV.Setup(t => t.Validate(It.IsAny<Tile>()))
            .Returns(true);

        userDropsTSV.Setup(t => t.Validate(It.Is<Tile>(t => true), It.Is<User>(u => true)))
            .Callback(() => isValid = true);

        Validate();

        Assert.IsTrue(isValid);
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
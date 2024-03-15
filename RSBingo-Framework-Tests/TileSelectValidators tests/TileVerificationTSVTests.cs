// <copyright file="VerificationEvidenceTSVTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

[TestClass]
public class TileVerificationTSVTests : TSVTestsBase
{
    private ITileVerificationTSV validator = null!;
    private Mock<IUserVerificationEvidenceTSV> userVefification = new();

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);
        services.AddSingleton(typeof(ITileVerificationTSV), typeof(TileValidatedTSV));
        services.AddSingleton(typeof(IUserVerificationEvidenceTSV), userVefification.Object);
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        userOne = MockDBSetup.Add_User(dataWorker, 0, team);
        userTwo = MockDBSetup.Add_User(dataWorker, 1, team);

        validator = General.DI.Get<ITileVerificationTSV>();

        SetVerificationValue(tileOne, userOne, false);
        SetVerificationValue(tileOne, userTwo, false);
        SetVerificationValue(tileTwo, userOne, false);
        SetVerificationValue(tileTwo, userTwo, false);
    }

    [TestMethod]
    public void BothUsersHaveNoVerifiedEvidenceForTileOne_TileOneIsInValid()
    {
        Validate(tileOne);

        AssertValidation(false);
    }

    [TestMethod]
    public void FirstUserHasVerifiedEvidenceForTileOne_SecondUserDoesNot_TileOneIsInValid()
    {
        SetVerificationValue(tileOne, userOne, true);

        Validate(tileOne);

        AssertValidation(false);
    }

    [TestMethod]
    public void BothUserHaveVerifiedEvidenceForTileOne_TileOneIsValid()
    {
        SetVerificationValue(tileOne, userOne, true);
        SetVerificationValue(tileOne, userTwo, true);

        Validate(tileOne);

        AssertValidation(true);
    }

    [TestMethod]
    public void BothUserHaveVerifiedEvidenceForTileOne_TileTwoIsInValid()
    {
        SetVerificationValue(tileOne, userOne, true);
        SetVerificationValue(tileOne, userTwo, true);

        Validate(tileTwo);

        AssertValidation(false);
    }

    private void SetVerificationValue(Tile tile, User user, bool value = true)
    {
        userVefification.Setup(x => x.Validate(It.Is<Tile>(t => t == tile), It.Is<User>(u => u == user)))
           .Returns(value);
    }

    private void Validate(Tile tile)
    {
        isValid = validator.Validate(tile);
    }
}
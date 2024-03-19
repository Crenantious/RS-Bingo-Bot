// <copyright file="SubmitVerificationEvidenceDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.TSV.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;

[TestClass]
public class SubmitVerificationEvidenceDPTests : TSVTestBase
{
    private SubmitVerificationEvidenceDP parser = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        parser = new();
    }

    [TestMethod]
    public void GiveTileAndUser_TheTileAndUserAreParsedAndGivenBack()
    {
        parser.Parse(TileOne, UserOne);

        Assert.AreEqual(TileOne, parser.Tile);
        Assert.AreEqual(UserOne, parser.User);
    }
}
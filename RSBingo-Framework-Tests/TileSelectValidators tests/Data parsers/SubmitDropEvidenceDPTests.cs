// <copyright file="SubmitDropEvidenceDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.TSV.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;

[TestClass]
public class SubmitDropEvidenceDPTests : TSVTestBase
{
    private SubmitDropEvidenceDP parser = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        parser = new();
    }

    [TestMethod]
    public void GiveData_Parse_TheSameDataIsGottenBack()
    {
        parser.Parse(TileOne, UserOne);

        Assert.AreEqual(TileOne, parser.Tile);
        Assert.AreEqual(UserOne, parser.User);
    }
}
// <copyright file="TileCanRecieveDropsDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.TSV.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;

[TestClass]
public class TileCanRecieveDropsDPTests : TSVTestBase
{
    private TileCanRecieveDropsDP parser = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        parser = new();
    }

    [TestMethod]
    public void TileGiven_TheSameTileIsGivenBack()
    {
        parser.Parse(TileOne);

        Assert.AreEqual(TileOne, parser.Tile);
    }
}
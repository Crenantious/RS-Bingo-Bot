// <copyright file="SubmitEvidenceDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DataParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.DataParsers;
using static RSBingo_Framework.Records.EvidenceRecord;

[TestClass]
public class SubmitEvidenceDPTests : TSVDataParserTestBase
{
    private SubmitEvidenceDP parser = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        parser = new();
    }

    [TestMethod]
    [DataRow(EvidenceType.Undefined)]
    [DataRow(EvidenceType.TileVerification)]
    [DataRow(EvidenceType.Drop)]
    public void GiveData_Parse_TheSameDataIsGottenBack(EvidenceType evidenceType)
    {
        parser.Parse(TileOne, UserOne, evidenceType);

        Assert.AreEqual(TileOne, parser.Tile);
        Assert.AreEqual(UserOne, parser.User);
        Assert.AreEqual(evidenceType, parser.EvidenceType);
    }
}
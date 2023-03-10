// <copyright file="ChangeTileButtonHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.TileRecord;
    using static RSBingo_Framework.Records.BingoTaskRecord;
    using RSBingo_Framework.Records;
    using RSBingoBot.Component_interaction_handlers;
    using RSBingo_Framework.CSV;
    using RSBingo_Framework.Repository;
    using RSBingo_Framework.CSV.Lines;
    using RSBingo_Framework.Exceptions;

    [TestClass]
    public class AddTaskRestrictionsCSVReaderTests : MockDBBaseTestClass
    {
        private const string fileName = "TestCSV.csv";

        private IDataWorker dataWorkerBefore = null!;
        private IDataWorker dataWorkerAfter = null!;
        private FileStream CSVFile = null!;
        private AddTaskRestrictionsCSVOperator csvOperator = null!;
        private bool didCSVReaderError = false;
        private bool didCSVOperatorError = false;
        private CSVData<AddTaskRestrictionCSVLine> data;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            dataWorkerBefore = CreateDW();
            dataWorkerAfter = CreateDW();
            csvOperator = new();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void AddRestrictionToFile_Parse_IsAddedToDBCorrectlyWithNoErrorMessage()
        {
            CreateCSVFile("1, Description 1");

            ParseFileAndOperate();
            Restriction? restrictionAfter = dataWorkerAfter.Restrictions.GetByName("1");
            int restrictionCount = dataWorkerAfter.Restrictions.GetAll().Count();

            Assert.AreEqual("1", restrictionAfter.Name);
            Assert.AreEqual("Description 1", restrictionAfter.Description);
            Assert.AreEqual(1, restrictionCount);
        }

        [TestMethod]
        public void AddTwoRestrictionsWithTheSameNameToFile_Parse_OnlyTheFirstIsAddedToDBAndAnErrorMessageIsReturned()
        {
            CreateCSVFile("1, Description 1", "1, Description 2");

            ParseFileAndOperate();
            Restriction? restrictionAfter = dataWorkerAfter.Restrictions.GetByName("1");
            int restrictionCount = dataWorkerAfter.Restrictions.GetAll().Count();

            Assert.IsTrue(didCSVOperatorError);
            Assert.AreEqual("1", restrictionAfter.Name);
            Assert.AreEqual("Description 1", restrictionAfter.Description);
            Assert.AreEqual(1, restrictionCount);
        }

        private void CreateCSVFile(params string[] lines) =>
            File.WriteAllLines(fileName, lines);

        private void ParseFile()
        {
            try
            {
                data = CSVReader.Parse<AddTaskRestrictionCSVLine>(fileName);
            }
            catch(CSVReaderException e)
            {
                didCSVReaderError = true;
            }
        }

        private void Operate()
        {
            try
            {
                csvOperator.Operate(data);
            }
            catch (CSVOperatorException e)
            {
                didCSVOperatorError = true;
            }
        }

        private void ParseFileAndOperate()
        {
            ParseFile();
            Operate();
        }
    }
}
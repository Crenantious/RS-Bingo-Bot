//// <copyright file="AddTasksCSVLineTests.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingo_Framework_Tests.CSV;

//using RSBingo_Framework.CSV.Lines;
//using RSBingo_Framework.Exceptions.CSV;
//using static RSBingo_Framework.Records.BingoTaskRecord;
//using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;

//[TestClass]
//public class AddTasksCSVLineTests : CSVTestsBase<AddTasksCSVLine>
//{
//    private record TaskInfo(string Name, Difficulty Difficulty, int Amount, string ImageURL);

//    [TestMethod]
//    public void AddMinNumberOfTasksToFile_Parse_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
//    {
//        CreateAndParseCSVFile(new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, "URL"));

//        AssertReader(null);
//        AssertCSVValue(new TaskInfo("Task 1", Difficulty.Easy, MinNumberOfTasks, "URL"));
//    }

//    [TestMethod]
//    public void AddMinValueToCSVFile_Parse_NoErrorsOrWarnings()
//    {
//        CreateAndParseCSVFile(ComparableValueMin.ToString());

//        AssertReader(null);
//        AssertCSVValue(ComparableValueMin);
//    }

//    [TestMethod]
//    public void AddMaxValueToCSVFile_Parse_NoErrorsOrWarnings()
//    {
//        CreateAndParseCSVFile(ComparableValueMax.ToString());

//        AssertReader(null);
//        AssertCSVValue(ComparableValueMax);
//    }

//    [TestMethod]
//    public void AddValueInbetweenMinAndMaxToCSVFile_Parse_NoErrorsOrWarnings()
//    {
//        CreateAndParseCSVFile((ComparableValueMax - 1).ToString());

//        AssertReader(null);
//        AssertCSVValue(ComparableValueMax - 1);
//    }

//    [TestMethod]
//    public void AddValueLessThanMinToCSVFile_Parse_GetCSVValueOutOfRangeException()
//    {
//        CreateAndParseCSVFile((ComparableValueMin - 1).ToString());

//        AssertReader(typeof(CSVValueOutOfRangeException));
//    }

//    [TestMethod]
//    public void AddValueGreaterThanMaxToCSVFile_Parse_GetCSVValueOutOfRangeException()
//    {
//        CreateAndParseCSVFile((ComparableValueMax + 1).ToString());

//        AssertReader(typeof(CSVValueOutOfRangeException));
//    }

//    [TestMethod]
//    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetInvalidValueTypeException()
//    {
//        CreateAndParseCSVFile("1.23");

//        AssertReader(typeof(InvalidCSVValueTypeException));
//    }

//    private void CreateAndParseCSVFile(TaskInfo taskInfo) =>
//        CreateAndParseCSVFile($"{taskInfo.Name}, {taskInfo.Difficulty}, {taskInfo.Amount}, {taskInfo.ImageURL},");

//    private void AssertCSVValue(TaskInfo taskInfo)
//    {
//        Assert.AreEqual(taskInfo.Name, ParsedCSVData.Lines.ElementAt(0).TaskName);
//        Assert.AreEqual(taskInfo.Difficulty, ParsedCSVData.Lines.ElementAt(0).TaskDifficulty);
//        Assert.AreEqual(taskInfo.Amount, ParsedCSVData.Lines.ElementAt(0).AmountOfTasks);
//        Assert.AreEqual(taskInfo.ImageURL, ParsedCSVData.Lines.ElementAt(0).TaskImageUrl);
//    }
//}
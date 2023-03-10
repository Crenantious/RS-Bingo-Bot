// <copyright file="AddTasksCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using System.Net;
using System.Threading;
using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.BingoTaskRecord;
using static RSBingo_Framework.CSV.Lines.AddOrRemoveTasksCSVLine;
using static RSBingo_Common.General;

[TestClass]
public class AddTasksCSVOperatorTests : CSVOperatorTestsBase<AddTasksCSVOperator, AddTasksCSVLine>
{
    private const string UriPrefix = "http://localhost:8000/";
    private const string TestImageURL = "http://localhost:8000/Test image.png";

    private string TestImageFilePath = Path.Combine(Directory.GetCurrentDirectory());
    private HttpListener httpListener;

    private record TaskInfo(string Name, Difficulty Difficulty, int Amount, string ImageURL);

    [TestMethod]
    public void AddTasksToFile_ParseAndOperate_AddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        using (CreateLocalServerWithImage())
        {
            TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MaxNumberOfTasks, TestImageURL);
            CreateAndParseTasksInCSVFile(taskInfo);

            Operate();

            AssertReaderAndOperator(null, null, 0);
            AssertTasks(taskInfo);
        }
    }

    [TestMethod]
    public void AddTasksWithInvalidImageURLToFile_ParseAndOperate_GetAWarning()
    {
        TaskInfo taskInfo = new TaskInfo("Task 1", Difficulty.Easy, MaxNumberOfTasks, "Invalid URL");
        CreateAndParseTasksInCSVFile(taskInfo);

        Operate();

        AssertReaderAndOperator(null, null, 1);
        AssertTasks(taskInfo);
    }

    private void AssertTasks(params TaskInfo[] expectedTasksInDB)
    {
        int expectedNumberOfTasks = 0;

        foreach (TaskInfo taskInfo in expectedTasksInDB)
        {
            IEnumerable<BingoTask> tasks = DataWorkerAfter.BingoTasks.GetByName(taskInfo.Name);
            expectedNumberOfTasks += taskInfo.Amount;
            Assert.AreEqual(taskInfo.Amount, tasks.Count());

            foreach (BingoTask task in tasks)
            {
                Assert.AreEqual(taskInfo.Name, task.Name);
                Assert.AreEqual(taskInfo.Difficulty, task.Difficulty);
                Assert.IsTrue(Directory.Exists(GetTaskImagePath(taskInfo.Name)));
            }
        }

        Assert.AreEqual(expectedNumberOfTasks, DataWorkerAfter.BingoTasks.CountAll());
    }

    private void CreateAndParseTasksInCSVFile(params TaskInfo[] tasks) =>
        CreateAndParseCSVFile(tasks.Select(t => $"{t.Name}, {t.Difficulty}, {t.Amount}, {t.ImageURL}").ToArray());

    public static Task CreateLocalServerWithImage() =>
        Task.Run(() =>
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(UriPrefix);
            listener.Start();

            HttpListenerContext context = listener.GetContext();
            //HttpListenerResponse response = context.Response;

            //Stream stream = response.OutputStream;
            //var writer = new StreamWriter(stream);
            //writer.Write(outputHtml);
            //writer.Close();
        });

    //private static void ResponseThread()
    //{
    //    while (true)
    //    {
    //        HttpListenerContext context = httpListener.GetContext(); 
    //        byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Localhost server -- port 5000</title></head>" +
    //        "<body>Welcome to the <strong>Localhost server</strong> -- <em>port 5000!</em></body></html>");
    //        context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
    //        context.Response.KeepAlive = false;
    //        context.Response.Close();
    //        Console.WriteLine("Respone given to a request.");
    //    }
    //}
}
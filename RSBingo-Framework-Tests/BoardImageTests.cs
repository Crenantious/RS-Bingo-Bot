// <copyright file="BoardImageTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingoBot.Imaging;
using RSBingo_Common;
using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using RSBingo_Framework.DAL;

[TestClass]
public class BoardImageTests : MockDBBaseTestClass
{
    private readonly string imagePath = Path.Combine(Paths.ResourcesTestOutputFolder, "Test board.png");

    private IDataWorker dataWorker = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorker = CreateDW();
    }

    [TestMethod]
    //[Ignore]
    // This test is intended to be used for visual inspection only; make sure it is ignored when committed.
    public void CreateTasks_CreateTeamBoard_DisplayImageForInspection()
    {
        CreateTasks();
        Team team = CreateTeam();

        BoardImage.Create(team).Save(imagePath);

        // Opens the image using the system's default program.
        Process.Start("explorer.exe", imagePath);

        // Assert true since the test is for visual inspection.
        Assert.IsTrue(true);
    }

    private void CreateTasks()
    {
        foreach(string imagePath in Directory.GetFiles(Paths.TaskImageFolder))
        {
            string taskName = imagePath.Split("\\")[^1].Split(".")[^2];
            dataWorker.BingoTasks.Create(taskName, BingoTaskRecord.Difficulty.Easy);
        }
        dataWorker.SaveChanges();
    }

    private Team CreateTeam()
    {
        Team team = TeamRecord.CreateTeam(dataWorker, "Test", 0, 0, 0, 0, 0, 0);
        team.CreateDefaultTiles(dataWorker); 
        dataWorker.SaveChanges();
        return team;
    }
}
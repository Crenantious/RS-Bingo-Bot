// <copyright file="LeaderboardImageTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Imaging.Leaderboard;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;
using SixLabors.ImageSharp;
using System.Diagnostics;

[TestClass]
public class LeaderboardImageTests : MockDBBaseTestClass
{
    private readonly string imagePath = Path.Combine(Paths.ResourcesTestOutputFolder, "Leaderboard.png");

    private IDataWorker dataWorker = null!;
    private List<(string name, int score)> teams = new();

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorker = CreateDW();
    }

    [TestMethod]
    [Ignore]
    // This test is intended to be used for visual inspection only; make sure it is ignored when committed.
    public void CreateTeams_DisplayImageForInspection()
    {
        CreateTeam("Team 1", 50);
        CreateTeam("pppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppp", 50);
        CreateTeam("a", int.MaxValue);
        CreateTeam("b", -int.MaxValue);
        CreateTeam("", 0);

        ShowLeaderboard();
    }

    private void CreateTeam(string name, int score)
    {
        teams.Add((name, score));
    }

    private void ShowLeaderboard()
    {
        LeaderboardImage.Create(teams)
            .SaveAsPng(imagePath);

        // Opens the image using the system's default program.
        Process.Start("explorer.exe", imagePath);
    }
}
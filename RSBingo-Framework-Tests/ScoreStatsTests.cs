// <copyright file="ScoreStatsTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingoBot.DTO;
using RSBingoBot.Imaging.Graphs;
using RSBingo_Common;
using SixLabors.ImageSharp;

[TestClass]
public class ScoreStatsTests : MockDBBaseTestClass
{
    [TestMethod]
    //[Ignore]
    // This test is intended to be used for visual inspection only; make sure it is ignored when committed.
    public void CreateTestGraph()
    {
        List<DataPoint> points = new();

        for (int i = 0; i < 5; i++)
        {
            points.Add(new("Name " + i, i));
        }

        GraphAxis axisX = new GraphXAxisBuilder(7).Build();
        GraphAxis axisY = new GraphYAxisBuilder(7).Build();

        axisX.Image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Score stats x.png"));
        axisY.Image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Score stats y.png"));

        Image imageXLabels = new GraphXAxisLabelsBuilder(points.Select(p => p.Name),
            axisX.Image.Width,
            axisX.Image.Height,
            axisX.DivisionPositions.Where((p, i) => i != 0 && i != axisX.DivisionPositions.Length - 1)
                .Select(p => p.X))
            .Build();

        Image imageYLabels = new GraphYAxisLabelsBuilder(points.Select(p => p.Name),
            axisY.Image.Width,
            axisY.Image.Height,
            axisY.DivisionPositions.Where((p, i) => i != 0 && i != axisY.DivisionPositions.Length - 1)
                .Select(p => p.Y))
            .Build();

        imageXLabels.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Score stats x labels.png"));
        imageYLabels.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Score stats y labels.png"));
    }
}
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
        List<LineGraphCategory> categories = new();
        Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue };

        for (int i = 0; i < colours.Length; i++)
        {
            (float, float)[] dataPoints = new(float, float)[] 
            {
                (5 + i, 5 + i),
                (7 + i, 16 + i),
                (11 + i, 24 + i),
                (3 + i, -2 + i),
            };
            categories.Add(new("Name " + i, dataPoints, colours[i]));
        }

        (var xLabels, var yLabels) = GraphUtiilties.GetAxisLabelsFromData(categories, 15, 15);
        GraphAxisInfo xAxisInfo = new("Date", 17, xLabels);
        GraphAxisInfo yAxisInfo = new("Score", 17, yLabels);

        LineGraphBuilder graphBuilder = new("Test", xAxisInfo, yAxisInfo, categories, "Team name");
        Image image = graphBuilder.Build();
        image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Test graph.png"));
    }
}
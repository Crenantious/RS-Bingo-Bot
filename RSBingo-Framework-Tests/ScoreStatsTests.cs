// <copyright file="ScoreStatsTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingoBot.DTO;
using RSBingoBot.Imaging.Graphs;
using RSBingo_Common;
using SixLabors.ImageSharp;
using Discord.Rest;

[TestClass]
public class ScoreStatsTests : MockDBBaseTestClass
{
    [TestMethod]
    //[Ignore]
    // This test is intended to be used for visual inspection only; make sure it is ignored when committed.
    public void CreateTestGraph()
    {
        List<LineGraphCategory<DateOnly, int>> categories = new();
        Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue };
        for (int i = 0; i < colours.Length; i++)
        {
            (DateOnly, int)[] data = new (DateOnly, int)[]
            {
                (new DateOnly(2023, 5, 23),5 + i),
                (new DateOnly(2023, 5, 29),7 + i),
                (new DateOnly(2023, 6, 1),11 + i),
                (new DateOnly(2023, 6, 3),3 + i),
                (new DateOnly(2023, 6, 3), 0),
                (new DateOnly(2023, 6, 3), 30),
            };

            categories.Add(new("Name " + i, colours[i], data));
        }

        IEnumerable<string> xLabels = GetXLabels(new DateOnly(2023, 5, 22), new DateOnly(2023, 6, 5), 8);
        IEnumerable<string> yLabels = GetYLabels(0, 30, 11);
        GraphAxisInfo xAxisInfo = new("Date", 10, xLabels);
        GraphAxisInfo yAxisInfo = new("Score", 13, yLabels);

        LineGraphBuilder<DateOnly, int> graphBuilder = new("Test", xAxisInfo, yAxisInfo, categories,
            new DateOnly(2023, 5, 22), new DateOnly(2023, 6, 5), 0, 30, GetXMinMaxRatio, GetYMinMaxRatio, "Team name");
        Image image = graphBuilder.Build();
        image.Save(Path.Combine(Paths.ResourcesTestOutputFolder, "Test graph.png"));
    }

    private static IEnumerable<string> GetXLabels(DateOnly min, DateOnly max, int count)
    {
        List<string> labels = new(count);
        int dayStep = (max.DayNumber - min.DayNumber) / (count - 1);

        for (int i = 0; i < count; i++)
        {
            labels.Add(DateOnly.FromDayNumber(min.DayNumber + dayStep * i).ToString());
        }

        return labels;
    }

    private static IEnumerable<string> GetYLabels(int min, int max, int count)
    {
        List<string> labels = new(count);
        int step = (max - min) / (count - 1);

        for (int i = 0; i < count; i++)
        {
            labels.Add((min + step * i).ToString());
        }

        return labels;
    }

    private static float GetXMinMaxRatio(DateOnly xValue, DateOnly min, DateOnly max)
    {
        if (xValue == min) { return 0; }
        var a = (float)max.DayNumber - min.DayNumber;
        var b = xValue.DayNumber - min.DayNumber;
        var c = b/a;
        var d = (float)b / a;
        var e = (float)b / (float)a;
        return ((float)xValue.DayNumber - min.DayNumber)/(max.DayNumber - min.DayNumber);
    }

    private static float GetYMinMaxRatio(int yValue, int min, int max)
    {
        if (yValue == min) { return 0; }
        return ((float)yValue - min)/(max - min);
    }
}
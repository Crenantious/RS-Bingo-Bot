// <copyright file="GraphAxisLabelsBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;
using RSBingoBot.Leaderboard;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using static GraphPreferences;

public abstract class GraphAxisLabelsBuilder
{
    private Image image = null!;

    private IEnumerable<string> labels;
    private List<Point> labelPositions;
    private bool ignoreEndpoints;

    protected int LabelSpacing { get; }
    protected int LabelCount { get; }
    protected int Width { get; set; } = 0;
    protected int Height { get; set; } = 0;

    protected abstract HorizontalAlignment horizontalAlignment{get;}
    protected abstract VerticalAlignment verticalAlignment { get; }

    /// <param name="ignoreEndpoints">Ignore the outermost axis divisions.</param>
    /// <exception cref="ArgumentException"></exception>
    public GraphAxisLabelsBuilder(IEnumerable<string> labels, int axisWidth, int axisHeight, bool ignoreEndpoints)
    {
        if (ignoreEndpoints && labels.Count() < 2)
        {
            throw new ArgumentException($"{nameof(labels)} must have a count of at least 2 if {nameof(ignoreEndpoints)} is true.");
        }

        this.labels = labels;
        this.ignoreEndpoints = ignoreEndpoints;

        LabelCount = labels.Count();
        labelPositions = new(LabelCount);
        Width = axisWidth;
        Height = axisHeight;
    }
    
    public Image Build()
    {
        Setup();
        return Create();
    }

    protected abstract void IncreaseSizeFromNewLabel(FontRectangle labelRect);
    protected abstract Point GetLabelPosition(int divisionIndex);

    private void Setup()
    {
        for (int i = 0; i < LabelCount; i++)
        {
            // TODO: JR - make the font a graph preference. Most likely have a Fonts class that sets up
            // and contains each front used, then the preferences can reference the desired one.
            TextOptions textOptions = new(LeaderboadPreferences.Font);
            FontRectangle labelRect = TextMeasurer.Measure(labels.ElementAt(i), textOptions);
            IncreaseSizeFromNewLabel(labelRect);
            labelPositions.Add(GetLabelPosition(ignoreEndpoints ? i + 1 : i));
        }

        image = new Image<Rgba32>(Width, Height);
    }

    private Image Create()
    {
        for (int i = 0; i < LabelCount; i++)
        {
            CreateLabel(i);
        }
        return image;
    }

    private void CreateLabel(int index)
    {
        TextOptions textOptions = new(LeaderboadPreferences.Font)
        {
            Origin = labelPositions[index],
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment
        };
        image.Mutate(x => x.DrawText(textOptions, labels.ElementAt(index), TextColour));
    }
}
//// <copyright file="GraphUtilities.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingoBot.Imaging.Graphs;

//using RSBingoBot.DTO;
//using RSBingoBot.Leaderboard;
//using SixLabors.Fonts;
//using SixLabors.ImageSharp.Drawing.Processing;
//using static GraphPreferences;

//internal class GraphUtilities
//{
//    private float scaleMin;
//    private float scaleMax;
//    private int divisionCount;

//    private IEnumerable<string> horizontalLabels;
//    private Image image;

//    public GraphUtilities(float scaleMin, float scaleMax, IEnumerable<string> labels, int verticalDivisionCount = VerticalDivisionCount)
//    {
//        this.scaleMin = scaleMin;
//        this.scaleMax = scaleMax;
//        this.horizontalLabels = labels;
//        this.verticalDivisionCount = verticalDivisionCount;

//        image = new Image<Rgba32>(DivisionWidth + 1, height);
//    }

//    public Image Build()
//    {
//        (var labels, var yPositions) = SetupYAxis();
//        CreateYAxis();

//        return GetDivisions();
//    }

//    private (string[] labels, int[] yPositions) SetupYAxis()
//    {
//        string[] labels = new string[verticalDivisionCount];
//        int[] yPositionsCentre = new int[verticalDivisionCount];
//        float width = 0;
//        float height = DivisionLength * this.verticalDivisionCount + DivisionSpacing * (this.verticalDivisionCount - 1);
//        TextOptions textOptions = new(LeaderboadPreferences.Font);

//        for (int i = 0; i < verticalDivisionCount; i++)
//        {
//            labels[i] = ((scaleMax - scaleMin) / verticalDivisionCount).ToString();
//            yPositionsCentre[i] = (DivisionSpacing + DivisionLength) * i + DivisionLength / 2;

//            FontRectangle rect = TextMeasurer.Measure(labels[i], textOptions);

//            // TODO - JR: check how this should be rounded.
//            if (i == 0 || i == verticalDivisionCount - 1) { height += rect.Height / 2; }
//            if (rect.Width > width) { width = rect.Width; }
//        }

//        return (labels, yPositionsCentre);
//    }

//    private void CreateYAxis()
//    {
//        for (int i = 0; i < verticalDivisionCount; i++)
//        {
//            CreateVericalDivision(i);
//            AddLabel(vertialLables[i], new PointF());
//        }
//    }

//    private void CreateVericalDivision(int index)
//    {
//        var points = new PointF[] { new(0, y), new(DivisionWidth, y) };
//        image.Mutate(x => x.DrawLines(axisColour, DivisionLength, points));
//    }

//    private Image GetScale(float min, float max)
//    {
//        Image image = new Image<Rgba32>(DivisionSpacing, ScaleWidth);
//        TextOptions textOptions = new(LeaderboadPreferences.Font);
//        float scaleValuesWidth = 0;
//        float height = 0;

//        for (int i = 0; i < DivisionCount; i++)
//        {
//            FontRectangle rect = TextMeasurer.Measure(((max - min) / DivisionCount).ToString(), textOptions);
//            float width = rect.Width;
//            if (width > scaleValuesWidth) { scaleValuesWidth = width; }

//            height += rect.Height;
//        }
//        return image;
//    }

//    private static void DrawText(Image image, string text, Point position)
//    {
//        TextOptions textOptions = new(LeaderboadPreferences.Font)
//        {
//            Origin = position,
//            HorizontalAlignment = HorizontalAlignment.Center,
//            VerticalAlignment = VerticalAlignment.Center
//        };

//        image.Mutate(x => x.DrawText(textOptions, text, textColour));
//    }
//}
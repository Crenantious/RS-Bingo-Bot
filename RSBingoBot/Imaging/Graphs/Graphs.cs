//// <copyright file="RequestGetStats.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingoBot.Imaging.Graphs;

//using RSBingoBot.DTO;
//using RSBingoBot.Leaderboard;
//using SixLabors.Fonts;
//using SixLabors.ImageSharp.Drawing.Processing;

//public class BarChartBuilder
//{
//    private const int Width = 500;
//    private const int Height = 500;
//    private const int XPadding = 20;
//    private const int YPadding = 20;

//    private const int TextMinSpacing = 5;

//    private const int ScaleWidth = 50;

//    private static Color textColour = Color.Black;

//    private Image image;
//    private BarChartInfo chartInfo;

//    public BarChartBuilder(BarChartInfo chartInfo)
//    {
//        this.chartInfo = chartInfo;
//    }

//    public Image Build()
//    {
//        // TODO: JR - make this optionally set from a derived ChartInfo
//        float min = chartInfo.min;
//        float max = chartInfo.max ?? chartInfo.DataPoints.Max(p => p.Value);
//        return GetDivisions();
//    }

//    private Image GetDivisions()
//    {
//        int height = DivisionHeight * DivisionCount + DivisionSpacing * (DivisionCount - 1);
//        Image image = new Image<Rgba32>(DivisionWidth + 1, height);

//        for (int i = 0; i < DivisionCount; i++)
//        {
//            divisionYPositions[i] = (DivisionSpacing + DivisionHeight) * i + DivisionHeight/2;
//            var points = new PointF[] { new(0, divisionYPositions[i]), new(DivisionWidth, divisionYPositions[i]) };
//            image.Mutate(x => x.DrawLines(frameColour, DivisionHeight, points));
//        }
//        return image;
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
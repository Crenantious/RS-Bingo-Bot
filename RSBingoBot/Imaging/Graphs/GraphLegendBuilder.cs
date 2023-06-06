// <copyright file="GraphLegendBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using static GraphPreferences;

internal class GraphLegendBuilder
{
    private string title;
    private IEnumerable<LineGraphCategoryLegend> categories;

    private Point titlePosition;
    private FontRectangle titleTextRect;
    private List<FontRectangle> categoriesTextRect = null!;
    private int width;
    private int height;
    private Image image = null!;

    private TextOptions titleTextOptions = new(TitleFont)
    {
        HorizontalAlignment = HorizontalAlignment.Center
    };
    private TextOptions categoryTextOptions = new(TextFont)
    {
        HorizontalAlignment = HorizontalAlignment.Center
    };

    public GraphLegendBuilder(string title, IEnumerable<LineGraphCategoryLegend> categories)
    {
        this.title = title;
        this.categories = categories;

        categoriesTextRect = new(categories.Count());
    }

    public Image Build()
    {
        SetupSizes();
        CreateImage();
        AddBorder();
        return image;
    }

    private void SetupSizes()
    {
        FontRectangle textRect = TextMeasurer.Measure(title, titleTextOptions);
        AdjustSize(textRect);
        titleTextRect = textRect;

        foreach (LineGraphCategoryLegend category in categories)
        {
            textRect = TextMeasurer.Measure(category.Name, categoryTextOptions);
            AdjustSize(textRect);
            categoriesTextRect.Add(textRect);
        }

        width += TextSpacing * 2 + LegendBorderThickness * 2;
        height += TextSpacing + LegendBorderThickness * 2;
    }

    private void AdjustSize(FontRectangle text)
    {
        if (text.Width > width) { width = FloatPosToInt(text.Width); }
        height += TextSpacing + FloatPosToInt(text.Height);
    }

    private Image CreateImage()
    {
        image = new Image<Rgba32>(width, height);
        int x = width / 2;
        int y = DrawText(image, x, LegendBorderThickness, titleTextRect, TitleFont, title, TextColour);

        for (int i = 0; i < categoriesTextRect.Count; i++)
        {
            LineGraphCategoryLegend category = categories.ElementAt(i);
            y = DrawText(image, x, y, categoriesTextRect[i], TextFont, category.Name, category.LegendColour);
        }
        return image;
    }

    private static int DrawText(Image image, int x, int y, FontRectangle textRect, Font font, string text, Color colour)
    {
        TextOptions textOptions = new(font)
        {
            Origin = new Point(x, y),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        y += FloatPosToInt(TextSpacing + MathF.Floor(textRect.Height / 2));
        image.Mutate(x => x.DrawText(textOptions, text, colour));
        y += FloatPosToInt(MathF.Ceiling(textRect.Height / 2));
        return y;
    }

    private void AddBorder()
    {
        // TODO: JR - fix. This doesn't work well for even values of LegendBorderThickness.
        // The border fades too early making it look thinner than expected.
        image.Mutate(x => DrawRectangleExtensions.Draw(x, LegendBorderColour, LegendBorderThickness,
            new Rectangle(
                LegendBorderThickness/2,
                LegendBorderThickness/2,
                width - LegendBorderThickness,
                height - LegendBorderThickness)));
    }

    private static int FloatPosToInt(float pos) =>
        (int)MathF.Ceiling(pos);
}
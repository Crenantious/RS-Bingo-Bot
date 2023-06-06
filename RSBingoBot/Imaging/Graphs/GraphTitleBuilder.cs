// <copyright file="GraphTitleBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using static GraphPreferences;

internal abstract class GraphTitleBuilder
{
    private string title;
    private Image image = null!;

    public GraphTitleBuilder(string title)
    {
        this.title = title;
    }

    public Image Build()
    {
        TextOptions textOptions = new(GraphPreferences.TitleFont);
        FontRectangle rect = TextMeasurer.Measure(title, textOptions);

        image = new Image<Rgba32>((int)MathF.Ceiling(rect.Width), (int)MathF.Ceiling(rect.Height));

        image.Mutate(x => x.DrawText(textOptions, title, TextColour));
        MutateTitle(image);
        return image;
    }

    protected virtual void MutateTitle(Image image) { }
}
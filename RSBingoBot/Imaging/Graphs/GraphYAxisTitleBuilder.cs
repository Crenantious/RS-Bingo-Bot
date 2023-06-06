// <copyright file="GraphYAxisTitleBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using System.Numerics;

internal class GraphYAxisTitleBuilder : GraphTitleBuilder
{
    public GraphYAxisTitleBuilder(string title) : base(title) { }

    protected override void MutateTitle(Image titleImage)
    {
        AffineTransformBuilder transformBuilder = new();
        Vector2 origin = new Vector2(titleImage.Width, titleImage.Height) / 2;
        transformBuilder.AppendTranslation(-origin);
        transformBuilder.AppendRotationDegrees(270);
        transformBuilder.AppendTranslation(origin);

        titleImage.Mutate(c => c.Transform(transformBuilder));
    }
}
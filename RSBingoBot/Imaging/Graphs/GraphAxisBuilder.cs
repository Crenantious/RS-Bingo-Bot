// <copyright file="GraphAxisBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;
using SixLabors.ImageSharp.Drawing.Processing;
using static GraphPreferences;

public abstract class GraphAxisBuilder
{
    private Image image;

    public Point[] divisionPositions;

    protected int Width { get; }
    protected int Height { get; }
    protected int LongestSide { get; }

    public GraphAxisBuilder(int divisionCount)
    {
        divisionPositions = new Point[divisionCount];
        LongestSide = DivisionThickness * divisionCount + DivisionSpacing * (divisionCount - 1);
        (Width, Height) = GetImageSize();
        image = new Image<Rgba32>(Width, Height);
    }
    
    public GraphAxis Build()
    {
        Setup();
        return Create();
    }

    protected int GetDivisionOffset(int index) =>
        (DivisionSpacing + DivisionThickness) * index;

    protected abstract (int width, int height) GetImageSize();
    protected abstract Point GetDivisionPosition(int index);
    protected abstract (int width, int height) GetDivisionSize();
    protected abstract (int width, int height) GetSpinePosition();
    protected abstract (int width, int height) GetSpineSize();
    
    private void Setup()
    {
        for (int i = 0; i < divisionPositions.Length; i++)
        {
            divisionPositions[i] = GetDivisionPosition(i);
        }
    }

    private GraphAxis Create()
    {
        CreateSpine();
        for (int i = 0; i < divisionPositions.Length; i++)
        {
            CreateDivision(i);
        }
        return new(image, divisionPositions);
    }

    private void CreateSpine()
    {
        (int x, int y) = GetSpinePosition();
        (int width, int height) = GetSpineSize();
        Rectangle rect = new(x, y, width, height);
        image.Mutate(x => FillRectangleExtensions.Fill(x, AxisColour, rect));
    }

    private void CreateDivision(int index)
    {
        (int x, int y) = divisionPositions[index];
        (int width, int height) = GetDivisionSize();
        Rectangle rect = new(x, y, width, height);
        image.Mutate(x => FillRectangleExtensions.Fill(x, AxisColour, rect));
    }
}
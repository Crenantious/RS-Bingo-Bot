// <copyright file="GridImageBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingoBot.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

internal class GridImageBuilder
{
    public Image Image { get; private set; } = null!;

    private int cellXPosition;
    private int cellYPosition;
    private GridImageDimensions dimensions;
    private ImageBorderInfo borderInfo;
    private Action<Image, int, int>? mutateCell;

    public GridImageBuilder(GridImageDimensions dimensions, ImageBorderInfo borderInfo, Action<Image, int, int>? mutateCell = null)
    {
        this.dimensions = dimensions;
        this.borderInfo = borderInfo;
        this.mutateCell = mutateCell;

        CreateEmptyImage();
        CreateCells();
    }

    private void CreateEmptyImage()
    {
        (int width, int height) = GetImageSize();
        Image = new Image<Rgba32>(width, height);
    }

    private void CreateCells()
    {
        int cellWidth = 0;
        int cellHeight = 0;

        for (int i = 0; i < dimensions.ColumnWidths.Count(); i++)
        {
            SetCellXPosition(cellWidth, i);

            for (int j = 0; j < dimensions.RowHeights.Count(); j++)
            {
                SetCellYPosition(cellHeight, j);

                Image cell = CreateCell(dimensions.ColumnWidths.ElementAt(i), dimensions.RowHeights.ElementAt(j), i, j);
                cellWidth = cell.Width;
                cellHeight = cell.Height;

                AddCell(cell);
            }
        }
    }

    private (int width, int height) GetImageSize()
    {
        int width = GetDimensionSize(dimensions.ColumnWidths);
        int height = GetDimensionSize(dimensions.RowHeights);
        return (width, height);
    }

    private int GetDimensionSize(IEnumerable<int> cellDimensionSizes)
    {
        int size = cellDimensionSizes.Sum();
        if (cellDimensionSizes.Any()) { size -= borderInfo.Thickness * (cellDimensionSizes.Count() - 1); }
        return size;
    }

    private void AddCell(Image cell) =>
        Image.Mutate(x => x.DrawImage(cell, new Point(cellXPosition, cellYPosition), 1));

    private void SetCellXPosition(int width, int column) =>
        cellXPosition = column == 0 ? 0 : cellXPosition + width - borderInfo.Thickness;
    private void SetCellYPosition(int height, int row) =>
        cellYPosition = row == 0 ? 0 : cellYPosition + height - borderInfo.Thickness;

    private Image CreateCell(int width, int height, int column, int row)
    {
        Image cellInside = new Image<Rgba32>(width, height);
        mutateCell?.Invoke(cellInside, column, row);

        int border = borderInfo.Thickness;
        Image cellWithBorder = new Image<Rgba32>(width + border, height + border);
        cellWithBorder.Mutate(x => x.DrawImage(cellInside, new Point(border, border), 1));
        AddCellBorder(cellWithBorder);

        return cellWithBorder;
    }

    private void AddCellBorder(Image cell) =>
        cell.Mutate(x =>
            DrawRectangleExtensions.Draw(x,
                borderInfo.Colour,
                borderInfo.Thickness,
                new RectangleF(0, 0, cell.Width - borderInfo.Thickness, cell.Height - borderInfo.Thickness)));
}
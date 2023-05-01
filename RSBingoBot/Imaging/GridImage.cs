// <copyright file="BoardImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using SixLabors.ImageSharp;
using RSBingoBot.DTO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

internal class GridImage
{
    public Image Image { get; private set; }

    private int columns;
    private int rows;
    private int cellXPosition;
    private int cellYPosition;

    public GridImage(GridImageDimensions dimensions, ImageBorderInfo borderInfo, Action<Image>? mutateCell = null)
    {
        columns = dimensions.ColumnWidths.Count();
        rows = dimensions.RowHeights.Count();

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Image cell = CreateCell(dimensions.ColumnWidths.ElementAt(i), dimensions.RowHeights.ElementAt(j), borderInfo, mutateCell);
                AddCell(cell, i, j, borderInfo);
            }
        }
    }

    private void AddCell(Image cell, int column, int row, ImageBorderInfo borderInfo)
    {
        Image.Mutate(x => x.DrawImage(cell, new Point(cellXPosition, cellYPosition), 1));
        cellXPosition = column == columns ? 0 : cellXPosition + Image.Width - borderInfo.Thickness;
        cellYPosition = row == rows ? 0 : cellYPosition + Image.Height - borderInfo.Thickness;
    }

    private static Image CreateCell(int width, int height, ImageBorderInfo borderInfo, Action<Image>? mutateCell)
    {
        Image cell = new Image<Rgba32>(width, height);
        mutateCell?.Invoke(cell);
        AddCellBorder(cell, borderInfo);
        return cell;
    }

    private static void AddCellBorder(Image cell, ImageBorderInfo borderInfo)
    {
        cell.Mutate(x =>
            DrawRectangleExtensions.Draw(x,
                borderInfo.Colour,
                borderInfo.Thickness,
                new RectangleF(0, 0, cell.Width - borderInfo.Thickness, cell.Height - borderInfo.Thickness)));
    }
}
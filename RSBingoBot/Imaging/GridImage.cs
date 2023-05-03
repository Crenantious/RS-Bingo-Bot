// <copyright file="GridImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using RSBingoBot.DTO;
using SixLabors.ImageSharp;

internal static class GridImage
{
    public static Image Create(GridImageDimensions dimensions, ImageBorderInfo borderInfo, Action<Image, int, int>? mutateCell = null) =>
        new GridImageBuilder(dimensions, borderInfo, mutateCell).Image;
}
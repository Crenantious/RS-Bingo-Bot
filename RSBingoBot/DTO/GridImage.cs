// <copyright file="GridImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using SixLabors.ImageSharp;

public record GridImage(Image Image, GridImageDimensions Dimensions);
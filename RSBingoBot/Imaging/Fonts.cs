// <copyright file="GraphMainTitleBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using static RSBingo_Common.General;

internal static class Fonts
{
    #region ChampagneAndLimousinesBold info

    private const string ChampagneAndLimousinesBoldPath = "Fonts/Champagne & Limousines/Champagne & Limousines Bold.ttf";
    private const FontStyle ChampagneAndLimousinesBoldStyle = FontStyle.Bold;

    #endregion

    public static Font CreateChampagneAndLimousines(int size)
    {
        FontCollection collection = new();
        FontFamily family = collection.Add(Path.Combine(Paths.ResourcesFolder, ChampagneAndLimousinesBoldPath));
        return family.CreateFont(size, ChampagneAndLimousinesBoldStyle);
    }
}

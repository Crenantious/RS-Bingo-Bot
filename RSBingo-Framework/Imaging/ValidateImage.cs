// <copyright file="ValidateImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Imaging;

using SixLabors.ImageSharp;

public class ValidateImage
{
    /// <returns><see langword="true"/> if the file is a valid image, <see langword="false"/> otherwise.</returns>
    public static bool ValidatePath(string filePath)
    {
        try
        {
            if (Image.Identify(File.ReadAllBytes(filePath)) is null) { return false; }
        }
        catch { return false; }
        return true;
    }
}
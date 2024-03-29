﻿// <copyright file="TaskTemplatePopulator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// TODO: Use resources folder from General.

namespace RSBingo_Framework;

using RSBingo_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static RSBingo_Common.General;

public class TaskTemplatePopulator
{
    public static void Run()
    {
        Dictionary<string, string[]> difficultyToTask = new() {
            { "Easy", Array.Empty<string>() },
            { "Medium", Array.Empty<string>() },
            { "Hard", Array.Empty<string>() } };

        string text = string.Empty;

        foreach (string difficulty in difficultyToTask.Keys)
        {
            IEnumerable<string?>? files = Directory.GetFiles(GetImagePath(difficulty), "*.png")
                .Select(Path.GetFileNameWithoutExtension);

            foreach (string? fileName in files)
            {
                if (fileName == null) { continue; }
                text += $"{fileName}, {difficulty.ToLower()}, 1, {Environment.NewLine}";
            }
        }
        File.WriteAllText(GetFilePath("Bronze reaper tasks template.csv"), text);
    }

    public static string GetFilePath(string fileName) =>
        Path.Combine(AppRootPath, fileName);

    private static string GetImagePath(string difficulty) =>
        Path.Combine(Paths.TaskImagesFolder, difficulty);
}

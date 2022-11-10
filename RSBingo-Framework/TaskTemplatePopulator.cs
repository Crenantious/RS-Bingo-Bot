// <copyright file="TaskTemplatePopulator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
                    text += $"{fileName}, {difficulty.ToLower()}, 1{Environment.NewLine}";
                }
            }
            File.WriteAllText(GetFilePath("Tasks template.csv"), text);
        }

        private static string GetFilePath(string fileName) =>
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, fileName);

        private static string GetImagePath(string name) =>
            GetFilePath("Tile images\\" + name);
    }
}

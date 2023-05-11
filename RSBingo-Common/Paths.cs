// <copyright file="Paths.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

using static General;

public class Paths
{
    private const string ImageExtension = ".png";

    private static readonly Dictionary<PathType, string> pathExtension = new()
    {
        { PathType.Folder, ""},
        { PathType.Image, ImageExtension }
    };

    private static string resourcesFolder;
    private static string resourcesTestFolder;
    private static string taskImageFolder;
    private static string teamBoardFolder;

    public static string BaseBoardPath { get; private set; }
    public static string TileCompletedMarkerPath { get; private set; }

    private enum PathType
    {
        Folder,
        Image
    }

    public static void Initialise(bool asMock = false)
    {
        resourcesFolder = Path.Combine(AppRootPath, "Resources");
        resourcesTestFolder = GetPath("Test", PathType.Folder);

        taskImageFolder = GetPath("Task images", PathType.Folder, asMock);
        teamBoardFolder = GetPath("Team boards", PathType.Folder, asMock);

        BaseBoardPath = GetPath("Base board", PathType.Image);
        TileCompletedMarkerPath = GetPath("Tile completed marker", PathType.Image);
    }

    public static string GetTaskImagePath(string taskName) =>
        GetPath(taskImageFolder, taskName, PathType.Image);

    public static string GetTeamBoardPath(string teamName) =>
        GetPath(teamBoardFolder, teamName, PathType.Image);

    private static string GetPath(string pathEnding, PathType pathType, bool isInMockResources = false)
    {
        string root = isInMockResources ? resourcesTestFolder : resourcesFolder;
        string extension = pathExtension[pathType];
        return Path.Combine(root, pathEnding + extension);
    }

    private static string GetPath(string pathBegining, string pathEnding, PathType pathType)
    {
        string extension = pathExtension[pathType];
        return Path.Combine(pathBegining, pathEnding + extension);
    }
}
// <copyright file="Paths.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

using static General;

public static class Paths
{
    private const string ImageExtension = ".png";

    private static readonly string resourcesFolderPath;
    private static readonly Dictionary<PathType, string> pathExtension = new()
    {
        { PathType.Folder, ""},
        { PathType.Image, ImageExtension }
    };

    private static string TaskImageFolder { get; }
    private static string TeamBoardFolder { get; }
    public static string BaseBoardPath { get; }
    public static string TileCompletedMarkerPath { get; }

    private enum PathType
    {
        Folder,
        Image
    }

    static Paths()
    {
        resourcesFolderPath = GetPath("Resources", PathType.Folder, false);
        TaskImageFolder = GetPath("Task images", PathType.Folder);
        TeamBoardFolder = GetPath("Team boards", PathType.Folder);
        BaseBoardPath = GetPath("Base board", PathType.Image);
        TileCompletedMarkerPath = GetPath("Tile completed marker", PathType.Image);
    }

    public static string GetTaskImagePath(string taskName) =>
        GetPath(TaskImageFolder, taskName, PathType.Image);

    public static string GetTeamBoardPath(string teamName) =>
        GetPath(TeamBoardFolder, teamName, PathType.Image);

    private static string GetPath(string pathEnding, PathType pathType, bool isInResources = true)
    {
        string root = isInResources ? resourcesFolderPath : AppRootPath;
        string extension = pathExtension[pathType];
        return Path.Combine(root, pathEnding + extension);
    }

    private static string GetPath(string pathBegining, string pathEnding, PathType pathType)
    {
        string extension = pathExtension[pathType];
        return Path.Combine(pathBegining, pathEnding + extension);
    }
}
// <copyright file="Paths.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

public class Paths
{
    private const string ImageExtension = ".png";

    private static readonly Dictionary<PathType, string> pathExtension = new()
    {
        { PathType.Folder, ""},
        { PathType.Image, ImageExtension }
    };

    private static bool isMock;

    #region PATHS

    public static string ResourcesFolder { get; private set; } = null!;
    public static string ResourcesTestInputFolder { get; private set; } = null!;
    public static string ResourcesTestOutputFolder { get; private set; } = null!;
    public static string TaskImageFolder { get; private set; } = null!;
    public static string TeamBoardFolder { get; private set; } = null!;
    public static string BoardBackgroundPath { get; private set; } = null!;
    public static string TileCompletedMarkerPath { get; private set; } = null!;

#endregion

    private enum PathType
    {
        Folder,
        Image
    }

    public static void Initialise(bool asMock = false)
    {
        isMock = asMock;

        ResourcesFolder = Path.Combine(General.AppRootPath, "Resources");
        ResourcesTestInputFolder = GetPath("Test input", PathType.Folder);
        ResourcesTestOutputFolder = GetPath("Test output", PathType.Folder);

        TaskImageFolder = GetPath("Task images", PathType.Folder, asMock);
        TeamBoardFolder = GetPath("Team boards", PathType.Folder, asMock);

        BoardBackgroundPath = GetPath("Board background", PathType.Image);
        TileCompletedMarkerPath = GetPath("Tile completed marker", PathType.Image);
    }

    public static string GetTaskImagePath(string taskName) =>
        GetPath(TaskImageFolder, taskName, PathType.Image);

    public static string GetTeamBoardPath(string teamName) =>
        GetPath(TeamBoardFolder, teamName, PathType.Image);

    /// <summary>
    /// Gets a path with <see cref="ResourcesFolder"/> as the root,
    /// or <see cref="ResourcesTestFolder"/> if <see cref="Paths"/> was initialised as a mock.
    /// </summary>
    /// <param name="path">The path suffix.</param>
    /// <returns>The combined path.</returns>
    public static string FromResources(string path) =>
        GetPath(path, PathType.Folder, isMock);

    private static string GetPath(string pathEnding, PathType pathType, bool isInMockResources = false)
    {
        string root = isInMockResources ? ResourcesTestInputFolder : ResourcesFolder;
        string extension = pathExtension[pathType];
        return Path.Combine(root, pathEnding + extension);
    }

    private static string GetPath(string pathBegining, string pathEnding, PathType pathType)
    {
        string extension = pathExtension[pathType];
        return Path.Combine(pathBegining, pathEnding + extension);
    }
}
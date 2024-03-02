// <copyright file="Paths.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Common;

public class Paths
{
    private const string DefaultImageExtension = ".png";

    private static readonly Dictionary<PathType, string> pathExtension = new()
    {
        { PathType.Folder, ""},
        { PathType.Image, DefaultImageExtension }
    };

    private static bool isMock;

    #region Paths

    public static string ResourcesFolder { get; private set; } = null!;
    public static string ResourcesTestInputFolder { get; private set; } = null!;
    public static string ResourcesTestOutputFolder { get; private set; } = null!;
    public static string TaskImagesResizedFolder { get; private set; } = null!;
    public static string TaskImagesFolder { get; private set; } = null!;
    public static string TeamBoardFolder { get; private set; } = null!;
    public static string UserTempEvidenceFolder { get; private set; } = null!;
    public static string BoardBackgroundPath { get; private set; } = null!;
    public static string TileCompletedMarkerPath { get; private set; } = null!;
    public static string EvidencePendingMarkerPath { get; private set; } = null!;

    #endregion

    private enum PathType
    {
        Folder,
        Image
    }

    public enum ImageType
    {
        Png,
        Jpeg,
        Bmp
    }

    public static void Initialise(bool asMock = false)
    {
        isMock = asMock;

        ResourcesFolder = Path.Combine(General.AppRootPath, "Resources");
        ResourcesTestInputFolder = GetPath("Test input", PathType.Folder);
        ResourcesTestOutputFolder = GetPath("Test output", PathType.Folder);

        TaskImagesFolder = GetPath(Path.Combine("Task images", "Bronze reaper"), PathType.Folder, asMock);
        TaskImagesResizedFolder = GetPath("Task images resized", PathType.Folder, asMock);
        TeamBoardFolder = GetPath("Team boards", PathType.Folder, asMock);
        UserTempEvidenceFolder = GetPath("User temp evidence", PathType.Folder, asMock);

        BoardBackgroundPath = GetPath(Path.Combine("Board images", "Board background bronze reaper"), PathType.Image);
        TileCompletedMarkerPath = GetPath(Path.Combine("Board images", "Tile completed marker"), PathType.Image);
        EvidencePendingMarkerPath = GetPath(Path.Combine("Board images", "Evidence pending"), PathType.Image);
    }

    public static string GetTaskImagePath(string taskName) =>
        GetPath(TaskImagesFolder, taskName, PathType.Image);

    public static string GetTaskImagesResizedPath(string taskName) =>
        GetPath(TaskImagesResizedFolder, taskName, PathType.Image);

    public static string GetTeamBoardPath(string teamName) =>
        GetPath(TeamBoardFolder, teamName, PathType.Image);

    public static string GetUserTempEvidencePath(ulong userId, string extension) =>
        GetPath(UserTempEvidenceFolder, userId.ToString(), extension);

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

    private static string GetPath(string pathBegining, string pathEnding, string extension)
    {
        return Path.Combine(pathBegining, pathEnding + extension);
    }
}
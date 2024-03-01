// <copyright file="MessageFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using SixLabors.ImageSharp;

public class MessageFile
{
    private bool isTempPath = false;
    private FileStream stream = null!;

    internal string DiscordName => Name + System.IO.Path.GetExtension(Path);

    public string Name { get; private set; } = null!;
    public string Path { get; private set; } = string.Empty;
    public bool HasContent => string.IsNullOrEmpty(Path) is false;

    public MessageFile(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Sets the file contents to that of <paramref name="image"/> with image type <paramref name="extension"/>.
    /// </summary>
    public void SetContent(Image image, string extension)
    {
        string tempPath = System.IO.Path.GetTempFileName();
        string imagePath = System.IO.Path.ChangeExtension(tempPath, extension);

        image.Save(imagePath);

        SetPath(imagePath, true);
    }

    /// <summary>
    /// Sets the file contents to the file that is at <paramref name="path"/>.
    /// </summary>
    public void SetContent(string path)
    {
        SetPath(path, false);
    }

    private void SetPath(string path, bool isNewPathTemp)
    {
        DisposeStream(stream);

        if (isTempPath && Path != path)
        {
            DeletePath(Path);
        }

        Path = path;
        isTempPath = isNewPathTemp;
    }

    public FileStream Open()
    {
        stream = new(Path, FileMode.Open);
        return stream;
    }

    public void Close()
    {
        stream?.Close();
        stream?.Dispose();
    }

    private static void DeletePath(string path)
    {
        try
        {
            System.IO.File.Delete(path);
        }
        catch
        {

        }
    }

    private static void DisposeStream(FileStream stream)
    {
        if (stream is null)
        {
            return;
        }

        try
        {
            stream.Close();
        }
        catch
        {

        }

        stream.Dispose();
    }
}
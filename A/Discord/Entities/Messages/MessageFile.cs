// <copyright file="MessageFile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SixLabors.ImageSharp;

namespace DiscordLibrary.DiscordEntities;

public class MessageFile
{
    private FileStream stream = null!;

    public string Name { get; set; }
    public string Path { get; private set; } = string.Empty;

    public MessageFile(string name = "File")
    {
        Name = name;
        Path = System.IO.Path.GetTempFileName();
    }

    public void SetContents(Image image, string extension, string name)
    {
        Path = System.IO.Path.ChangeExtension(Path, extension);
        image.Save(Path);
    }

    public void SetContents(string path)
    {
        Path = path;
    }

    /// <summary>
    /// Sets <see cref="Name"/> to be the file name at <see cref="Path"/>.
    /// </summary>
    public void SetNameFromPath()
    {
        Name = System.IO.Path.GetFileName(Path);
    }

    public FileStream Open()
    {
        if (string.IsNullOrEmpty(Path))
        {
            throw new InvalidOperationException("Must set file contents before opening it.");
        }

        stream = new(Path, FileMode.Open, FileAccess.ReadWrite);
        return stream;
    }

    public void Close()
    {
        stream.Close();
    }
}
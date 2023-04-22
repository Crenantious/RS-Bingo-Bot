// <copyright file="LocalServerUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using RSBingo_Common;
using System.Net;

public class LocalServerUtilities
{
    public const string ImageFolderPath = "RSBingo-Framework-Tests\\CSV\\Operator tests\\Local server\\Server images\\";

    /// <summary>
    /// Opens <paramref name="fileName"/> in <see cref="ImageFolderPath"/> and sends it to <paramref name="response"/>.
    /// </summary>
    public static void SendImage(HttpListenerResponse response, string fileName)
    {
        byte[] buf = File.ReadAllBytes(GetImagePath(fileName));
        response.ContentLength64 = buf.Length;
        Stream stream = response.OutputStream;
        stream.Write(buf, 0, buf.Length);
    }

    private static string GetImagePath(string fileName) =>
       $"{General.AppRootPath}\\{ImageFolderPath}{fileName}";
}
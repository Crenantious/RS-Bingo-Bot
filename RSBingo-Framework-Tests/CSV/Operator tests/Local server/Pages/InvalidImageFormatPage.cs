// <copyright file="InvalidImageFormatPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

public class InvalidImageFormatPage : ILocalServerPage
{
    public string URL => "Invalid image format";

    public void SendResponse(HttpListenerContext context) =>
        LocalServerUtilities.SendImage(context.Response, "Png as jpg.jpg");
}
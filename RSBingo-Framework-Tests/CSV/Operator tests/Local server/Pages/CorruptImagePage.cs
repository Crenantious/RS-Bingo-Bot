// <copyright file="CorruptImagePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

public class CorruptImagePage : ILocalServerPage
{
    public string URL => "Corrupt image";

    public void SendResponse(HttpListenerContext context) =>
        LocalServerUtilities.SendImage(context.Response, "Corrupt png.png");
}
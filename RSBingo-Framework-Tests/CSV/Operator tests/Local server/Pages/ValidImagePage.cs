// <copyright file="ValidImagePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

public class ValidImagePage : ILocalServerPage
{
    public string URL => "Valid image";

    public void SendResponse(HttpListenerContext context) =>
        LocalServerUtilities.SendImage(context.Response, "Valid png.png");
}
// <copyright file="ILocalServerPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

public interface ILocalServerPage
{
    public string URL { get; }

    public void SendResponse(HttpListenerContext context);
}
﻿// <copyright file="IWebServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;

public interface IWebServices : IRequestService
{
    public Task<Result> DownloadFile(string url, string path);
}
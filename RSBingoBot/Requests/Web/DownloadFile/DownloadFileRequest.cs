﻿// <copyright file="DownloadFileRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using MediatR;

public record DownloadFileRequest(string Url, string Path) : IRequest<Result>;
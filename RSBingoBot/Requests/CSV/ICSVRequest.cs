// <copyright file="ICSVRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using MediatR;

public interface ICSVRequest : IRequest<Result>
{
    public DiscordAttachment Attachment { get; }
    public string FileName { get; }
}
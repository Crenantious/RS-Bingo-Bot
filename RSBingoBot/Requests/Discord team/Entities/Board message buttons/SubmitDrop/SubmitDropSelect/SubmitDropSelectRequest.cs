﻿// <copyright file="SubmitDropSelectRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

public record SubmitDropSelectRequest(SubmitDropButtonDTO DTO) : ISelectComponentRequest;
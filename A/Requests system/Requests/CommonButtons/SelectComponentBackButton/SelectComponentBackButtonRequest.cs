// <copyright file="SelectComponentBackButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

public record SelectComponentBackButtonRequest(SelectComponent SelectComponent) : IButtonRequest;
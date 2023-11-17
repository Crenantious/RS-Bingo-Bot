// <copyright file="ViewEvidenceButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.RequestHandlers;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;

internal record ViewEvidenceButtonRequest(Team Team, IInteractionHandler? ParentHandler) : IButtonRequest;
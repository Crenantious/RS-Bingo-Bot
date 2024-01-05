// <copyright file="GetDiscordMemberRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record GetDiscordMemberRequest(ulong Id) : IDiscordRequest<DiscordMember>;
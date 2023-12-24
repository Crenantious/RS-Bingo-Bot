// <copyright file="GetDiscordMemberRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record GetDiscordMemberRequest(ulong Id) : IDiscordRequest<DiscordMember>;
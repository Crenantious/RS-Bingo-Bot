// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public record DiscordTeam(DiscordChannel CategoryChannel, DiscordChannel GeneralChannel,
                          DiscordChannel BoardChannel, DiscordChannel EvidenceChannel, DiscordChannel VoiceChannel,
                          Message BoardMessage, DiscordRole Role);
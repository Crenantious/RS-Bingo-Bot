// <copyright file="AllDiscordTeamsChoiceProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Commands;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

internal class AllDiscordTeamsChoiceProvider : IChoiceProvider
{
    public async Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider() =>
        RSBingoBot.Discord.DiscordTeam.ExistingTeams.Select(t => new DiscordApplicationCommandOptionChoice(t.Key, t.Value));
}
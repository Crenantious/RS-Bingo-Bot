// <copyright file="DisableDuringCompetitiontAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands.Attributes;

using DSharpPlus.SlashCommands;

public class DisableDuringCompetitionAttribute : BingoBotSlashCheckAttribute
{
    public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx) =>
        General.HasCompetitionStarted is false;

    public override string GetErrorMessage() =>
        "This command cannot be ran during the competition.";
}
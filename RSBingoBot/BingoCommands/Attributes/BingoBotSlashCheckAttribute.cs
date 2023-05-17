// <copyright file="BingoBotSlashCheckAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands.Attributes;

using DSharpPlus.SlashCommands;

public abstract class BingoBotSlashCheckAttribute : SlashCheckBaseAttribute
{
    public abstract string GetErrorMessage();
}
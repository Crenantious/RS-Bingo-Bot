﻿// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands.Attributes;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

public class RequireRoleAttribute : SlashCheckBaseAttribute
{
    public string name;

    public RequireRoleAttribute(string name)
    {
        this.name = name;
    }

    public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx) =>
        ctx.Member.Roles.FirstOrDefault(r => r.Name == name) is DiscordRole;
}
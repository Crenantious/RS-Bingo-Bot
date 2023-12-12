// <copyright file="GrantDiscordRoleValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class GrantDiscordRoleValidator : Validator<GrantDiscordRoleRequest>
{
    public GrantDiscordRoleValidator()
    {
        MemberNotNull(r => r.Member);
        RoleNotNull(r => r.Role);
    }
}
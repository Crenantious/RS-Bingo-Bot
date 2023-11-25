// <copyright file="DiscordComponentExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordExtensions;

using DSharpPlus;
using DSharpPlus.Entities;

public static class DiscordComponentExtensions
{
    public static bool IsSelect(this DiscordComponent component)
    {
        ComponentType[] selectTypes = new ComponentType[]
        {
            ComponentType.ChannelSelect, ComponentType.MentionableSelect, ComponentType.RoleSelect,
            ComponentType.StringSelect, ComponentType.UserSelect
        };
        return selectTypes.Contains(component.Type);
    }
}
// <copyright file="IReasonExtentions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

public static class IReasonExtentions
{
    public static IEnumerable<T> GetDiscordResponses<T>(this IEnumerable<T> reasons) where T : IReason =>
        reasons.Where(r => typeof(IDiscordResponse).IsAssignableFrom(r.GetType()));
}
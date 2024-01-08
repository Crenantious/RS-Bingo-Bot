// <copyright file="IReasonExtentions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

public static class IReasonExtentions
{
    public static IEnumerable<TReason> GetDiscordResponses<TReason>(this IEnumerable<TReason> reasons) 
        where TReason : IReason =>
        reasons.Where(r => typeof(IDiscordResponse).IsAssignableFrom(r.GetType()));
}
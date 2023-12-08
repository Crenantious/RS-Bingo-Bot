// <copyright file="RequestSemaphores.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

public class RequestSemaphores
{
    public SemaphoreSlim UpdateTeam { get; } = new(1, 1);
    public SemaphoreSlim UpdateEvidence { get; } = new(1, 1);
}
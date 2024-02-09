// <copyright file="RequestSemaphores.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingo_Framework.Models;

public class RequestSemaphores
{
    private Dictionary<int, SemaphoreSlim> team = new();
    private Dictionary<int, SemaphoreSlim> evidence = new();
    private Dictionary<ulong, SemaphoreSlim> teamRegistration = new();

    /// <summary>
    /// Gets the <see cref="SemaphoreSlim"/> associated with updating <see cref="Team"/> with id <paramref name="teamId"/>.
    /// </summary>
    public SemaphoreSlim GetTeamDatabase(int teamId) =>
        Get(team, teamId);

    /// <summary>
    /// Gets the <see cref="SemaphoreSlim"/> associated with updating <see cref="Evidence"/> with team id <paramref name="teamId"/>.
    /// </summary>
    public SemaphoreSlim GetEvidence(int teamId) =>
        Get(evidence, teamId);

    /// <summary>
    /// Gets the <see cref="SemaphoreSlim"/> associated with interacting with a team registration button.
    /// </summary>
    public SemaphoreSlim GetTeamRegistration(ulong discordUserId) =>
        Get(teamRegistration, discordUserId);

    private SemaphoreSlim Get<T>(Dictionary<T, SemaphoreSlim> semaphores, T key, int initialCount = 1, int maxCount = 1)
        where T : notnull
    {
        if (semaphores.ContainsKey(key) is false)
        {
            semaphores.Add(key, new(initialCount, maxCount));
        }
        return semaphores[key];
    }
}
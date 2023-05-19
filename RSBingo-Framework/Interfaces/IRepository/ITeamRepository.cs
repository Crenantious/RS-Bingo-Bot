// <copyright file="ITeamRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Interfaces.IRepository;

using RSBingo_Framework.Models;

/// <summary>
/// Interface detailing use of <see cref="Team"/>as a repository.
/// </summary>
public interface ITeamRepository : IRepository<Team>
{
    public Team Create(string name, ulong categoryChannelId, ulong boardChannelId,
        ulong generalChannelId, ulong evidenceChannelId, ulong voiceChannelId, ulong boardMessageId, ulong roleId);

    public bool DoesTeamExist(string name);

    public Team? GetByName(string name);

    public IEnumerable<Team> GetTeams();

    public Team? GetTeamByID(int id);
}
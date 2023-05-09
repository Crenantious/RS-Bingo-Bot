// <copyright file="TeamRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository;

using Microsoft.EntityFrameworkCore;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Interfaces.IRepository;
using RSBingo_Framework.Models;
using System.Linq;

/// <summary>
/// Class detailing use of <see cref="Team"/> as a repository.
/// </summary>
public class TeamRepository : RepositoryBase<Team>, ITeamRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamRepository"/> class.
    /// </summary>
    /// <param name="dataWorker">Reference to the dataworker.</param>
    public TeamRepository(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    public override Team Create() =>
        Add(new Team());

    public Team Create(string name, ulong categoryChannelId, ulong boardChannelId,
        ulong generalChannelId, ulong voiceChannelId, ulong boardMessageId, ulong roleId) =>
        Add(new Team()
        {
            Name = name,
            CategoryChannelId = categoryChannelId,
            BoardChannelId = boardChannelId,
            GeneralChannelId = generalChannelId,
            VoiceChannelId = voiceChannelId,
            BoardMessageId = boardMessageId,
            RoleId = roleId
        });

    public bool DoesTeamExist(string name) =>
        GetByName(name) != null;

    public Team? GetByName(string name) =>
        FirstOrDefault(t => t.Name == name);

    public IEnumerable<Team> GetTeams() =>
        GetAll();

    public Team? GetTeamByID(int id) => FirstOrDefault(t => t.RowId == id);

    /// <inheritdoc/>
    public override void LoadCascadeNavigations(Team team)
    {
        DataWorker.Teams.Where(t => t.RowId == team.RowId)
            .Include(t => t.Tiles)
            .ThenInclude(ti => ti.Evidence)
            .Include(t => t.Users);
    }
}
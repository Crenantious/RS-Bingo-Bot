// <copyright file="RestrictionRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository;

using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Interfaces.IRepository;
using RSBingo_Framework.Models;

/// <summary>
/// Class detailing use of <see cref="Restrciton"/> as a repository.
/// </summary>
public class RestrictionRepository : RepositoryBase<Restriction>, IRestrictionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RestrictionRepository"/> class.
    /// </summary>
    /// <param name="dataWorker">Reference to the dataWorker.</param>
    public RestrictionRepository(IDataWorker dataWorker)
        : base(dataWorker) { }

    /// <inheritdoc/>
    public override Restriction Create() =>
        Add(new Restriction());

    public Restriction Create(string name, string description) =>
        Add(new Restriction()
        {
            Name = name,
            Description = description
        });

    public Restriction? GetByName(string name) =>
        FirstOrDefault(r => r.Name == name);
}
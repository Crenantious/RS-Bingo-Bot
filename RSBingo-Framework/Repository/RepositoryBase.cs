// <copyright file="RepositoryBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Repository
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Interfaces.IRepository;
    using RSBingo_Framework.Models;

    /// <inheritdoc />
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, IRepositoryBase
        where TEntity : BingoRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class.
        /// </summary>
        /// <param name="dataWorker">DataWorker used to initialize the repository.</param>
        public RepositoryBase(IDataWorker dataWorker)
        {
            DataWorker = dataWorker;
        }

        /// <inheritdoc/>
        public IDataWorker DataWorker { get; }

        /// <summary>
        /// Gets the <see cref="Microsoft.EntityFrameworkCore.DbSet"/> for type <see cref="TEntity"/>.
        /// </summary>
        internal DbSet<TEntity> DBSet
        {
            get
            {
                // Used to allow access via property in Locals during Dev as well as generic calls
                return DataWorker!.Context.Set<TEntity>();
            }
        }

        /// <inheritdoc/>
        public BingoRecord? FindByPK(object id)
        {
            return DBSet.Find(id);
        }

        /// <inheritdoc/>
        public TEntity? Find(int id)
        {
            return DBSet.Find(id);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return DBSet.ToList();
        }

        /// <inheritdoc/>
        public int CountAll()
        {
            return DBSet.Count();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, int? limitRows = null)
        {
            return limitRows.HasValue ? DBSet.Where(predicate).Take(limitRows.Value) : DBSet.Where(predicate);
        }

        /// <inheritdoc/>
        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool checkContext = false)
        {
            if (checkContext)
            {
                TEntity? result = DBSet.Local.FirstOrDefault(predicate.Compile());
                if (result is not null) { return result; }
            }

            return DBSet.FirstOrDefault(predicate);
        }

        /// <inheritdoc/>
        public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DBSet.SingleOrDefault(predicate);
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DBSet.Count(predicate);
        }

        /// <inheritdoc/>
        public abstract TEntity Create();

        /// <inheritdoc/>
        public virtual void LoadCascadeNavigations(TEntity entity) { return; }

        /// <inheritdoc/>
        public TEntity Add(TEntity entity)
        {
            DBSet.Add(entity);
            return entity;
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            LoadCascadeNavigations(entity);

            DBSet.Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities) { LoadCascadeNavigations(entity); }

            DBSet.RemoveRange(entities);
        }
    }
}

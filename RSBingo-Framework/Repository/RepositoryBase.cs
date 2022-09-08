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
        /// <inheritdoc/>
        public IDataWorker DataWorker => throw new NotImplementedException();

        /// <summary>
        /// Gets the <see cref="Microsoft.EntityFrameworkCore.DbSet"/> for type <see cref="TEntity"/>.
        /// </summary>
        internal DbSet<TEntity> DBSet
        {
            get
            {
                // Used to allow access via property in Locals during Dev as well as generic calls
                return this.DataWorker!.Context.Set<TEntity>();
            }
        }

        /// <inheritdoc/>
        public BingoRecord? FindByPK(object id)
        {
            return this.DBSet.Find(id);
        }

        /// <inheritdoc/>
        public TEntity Find(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int CountAll()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, int? limitRows = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool checkContext = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public TEntity Create()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}

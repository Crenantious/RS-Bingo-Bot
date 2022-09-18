using RSBingo_Framework.Models;
using System.Linq.Expressions;

namespace RSBingo_Framework.Interfaces.IRepository
{
    /// <summary>
    /// Wrapper around the DbSet implementing the Repository Pattern and providing common access methods.
    /// </summary>
    /// <typeparam name="TEntity">The type of Model which to extend.</typeparam>
    public interface IRepository<TEntity> : IRepositoryBase
        where TEntity : BingoRecord
    {
        /// <summary>
        /// Diverted reference to the DBSet Find method of the given Model type.
        /// </summary>
        /// <param name="id">Finds the entity of the given primary key value.</param>
        /// <returns>The entity if found.</returns>
        TEntity? Find(int id);

        /// <summary>
        /// Diverted reference to the DBSet requesting all of the given Model types.
        /// Warning, this will retrieve all of the data in the given repository and should be used sparingly.
        /// </summary>
        /// <returns>The list of all entities.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Diverted reference to the DBSet Count method of all entities.
        /// </summary>
        /// <returns>The  if found.</returns>
        int CountAll();

        /// <summary>
        /// Diverted reference to the DBSet Where method of the given predicate.
        /// </summary>
        /// <param name="predicate">Finds the entities of the given predicate.</param>
        /// <param name="limitRows">Optional filter to limit number of rows required.</param>
        /// <returns>The query from the expression.</returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, int? limitRows = null);

        /// <summary>
        /// Diverted reference to the DBSet FirstOrDefault method of the given predicate.
        /// </summary>
        /// <param name="predicate">Finds the entities of the given predicate.</param>
        /// <param name="checkContext">Checks the context first for unsaved records before testing the DB.</param>
        /// <returns>The entity if found or null.</returns>
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool checkContext = false);

        /// <summary>
        /// Diverted reference to the DBSet SingleOrDefault method of the given predicate.
        /// </summary>
        /// <param name="predicate">Finds the entities of the given predicate.</param>
        /// <returns>The entity if found or null.</returns>
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Diverted reference to the DBSet Count method of the given predicate.
        /// </summary>
        /// <param name="predicate">Finds the count of the given predicate.</param>
        /// <returns>The  if found.</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Add an entity of the given type to the DBSet after adding initial default values.
        /// </summary>
        /// <returns>The newly created entity.</returns>
        TEntity Create();

        /// <summary>
        /// Add an entity of the given type to the DBSet.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The passed in entity to allow chaining.</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Remove an entity from the DBSet.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove a range of entities from the DBSet.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}

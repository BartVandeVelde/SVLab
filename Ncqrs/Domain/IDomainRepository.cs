using System;
using System.Diagnostics.Contracts;

namespace Ncqrs.Domain.Storage
{
    /// <summary>
    /// A repository that can be used to get and save aggregate roots.
    /// </summary>
    public interface IDomainRepository
    {
        /// <summary>
        /// Gets aggregate root by id.
        /// </summary>
        /// <param name="aggregateRootType">Type of the aggregate root.</param>
        /// <param name="id">The id of the aggregate root.</param>
        /// <param name="version">The version of the aggregate root to retrieve.</param>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        AggregateRoot GetById(Type aggregateRootType, Guid id, long version);

        /// <summary>
        /// Gets aggregate root by id.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate root.</typeparam>
        /// <param name="id">The id of the aggregate root.</param>
        /// <param name="version">The version of the aggregate root to retrieve.</param>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        T GetById<T>(Guid id, long version) where T : AggregateRoot;

        /// <summary>
        /// Saves the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRootToSave">The aggregate root to save.</param>
        void Add(AggregateRoot aggregateRootToSave);
    }
}
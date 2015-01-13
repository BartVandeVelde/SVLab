using System;
using System.Collections.Generic;

using Ncqrs.Domain;
using Ncqrs.Domain.Storage;

namespace Ncqrs.EventSourcing.Storage
{
    /// <summary>
    /// A event store. Can store and load events from an <see cref="IEventSource"/>.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Get all events provided by an specified event source for a particular version.
        /// </summary>
        /// <returns>All the events from the event source.</returns>
        IEnumerable<IEvent> GetAllEventsForVersion(Guid eventSourceId, long version);

        /// <summary>
        /// Get all events provided by an specified event source for a particular version, since an earlier version.
        /// </summary>
        /// <param name="eventSourceId">The id of the event source that owns the events.</param>
        /// <returns>All the events from the event source.</returns>
        IEnumerable<IEvent> GetAllEventsForVersionSince(Guid eventSourceId, long fromVersion, long version);

        /// <summary>
        /// Gets the current version of an aggregate root from the store.
        /// </summary>
        /// <remarks>
        /// Returns <c>0</c> if the specified aggregate root does not exist (yet).
        /// </remarks>
        long GetVersion(Guid id);

        /// <summary>
        /// Save all events from a specific event source.
        /// </summary>
        /// <exception cref="ConcurrencyException">Occurs when there is already a newer version of the event provider stored in the event store.</exception>
        /// <param name="source">The source that should be saved.</param>
        IEnumerable<IEvent> Save(IEventSource source);

        /// <summary>
        /// Saves a snapshot of the specified event source.
        /// </summary>
        void SaveShapShot(IEventSource eventSource);

        /// <summary>
        /// Gets the most recent snapshot for a particular event source, if one exists. 
        /// Otherwise, returns <c>null</c>.
        /// </summary>
        ISnapshot GetSnapShot(Guid eventSourceId, long version);
    }
}
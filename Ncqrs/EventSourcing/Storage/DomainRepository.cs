using System;
using System.Collections;
using System.Collections.Generic;

using Ncqrs.Bus;
using Ncqrs.Domain;
using Ncqrs.Domain.Storage;

namespace Ncqrs.EventSourcing.Storage
{
    public class DomainRepository : IDomainRepository
    {
        #region Private Definition

        private const int SnapshotIntervalInEvents = 10;
        private readonly IEventBus eventBus;
        private readonly IEventStore store;

        #endregion

        public DomainRepository(IEventStore store, IEventBus eventBus)
        {
            this.store = store;
            this.eventBus = eventBus;
        }

        /// <summary>
        /// Adds the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        public void Add(AggregateRoot aggregateRoot)
        {
            ResolveConcurrencyConflicts(aggregateRoot);

            IEnumerable<IEvent> events = store.Save(aggregateRoot);

            if (ShouldCreateSnapshot(aggregateRoot))
            {
                store.SaveShapShot(aggregateRoot);
            }

            eventBus.Publish(events);

            aggregateRoot.AcceptChanges();
        }

        private void ResolveConcurrencyConflicts(AggregateRoot aggregateRoot)
        {
            long storedVersion = store.GetVersion(aggregateRoot.Id);

            if (storedVersion > aggregateRoot.UncommittedVersion)
            {
                IEnumerable<IEvent> concurrentEvents = 
                    store.GetAllEventsForVersionSince(aggregateRoot.Id, aggregateRoot.UncommittedVersion, storedVersion);

                IEnumerable<IEvent> rejectedEvents = aggregateRoot.MergeEvents(concurrentEvents);

                // TODO: report the rejected events back to the client
            }
        }

        private static bool ShouldCreateSnapshot(AggregateRoot aggregateRoot)
        {
            return (aggregateRoot.Version % SnapshotIntervalInEvents) == 0;
        }

        /// <summary>
        /// Gets aggregate root by id.
        /// </summary>
        /// <param name="aggregateRootType">Type of the aggregate root.</param>
        /// <param name="id">The id of the aggregate root.</param>
        /// <param name="version">The version of the aggregate root to retrieve.</param>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        public AggregateRoot GetById(Type aggregateRootType, Guid id, long version)
        {
            var aggregateRoot = (AggregateRoot) Activator.CreateInstance(aggregateRootType, true);

            IEnumerable<IEvent> historicalEvents;
            
            var snapshot = store.GetSnapShot(id, version);
            if (snapshot != null)
            {
                aggregateRoot.RestoreFromMemento(snapshot.Memento);

                historicalEvents = store.GetAllEventsForVersionSince(id, snapshot.Version, version);
            }
            else
            {
                historicalEvents = store.GetAllEventsForVersion(id, version);
            }
            
            aggregateRoot.InitializeFromHistory(historicalEvents);

            return aggregateRoot;
        }

        /// <summary>
        /// Gets aggregate root by id.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate root.</typeparam>
        /// <param name="id">The id of the aggregate root.</param>
        /// <param name="version">The version of the aggregate root to retrieve.</param>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        public T GetById<T>(Guid id, long version) where T : AggregateRoot
        {
            return (T) GetById(typeof (T), id, version);
        }
    }
}
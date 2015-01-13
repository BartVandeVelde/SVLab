using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using Ncqrs.Domain;
using Ncqrs.EventSourcing.Storage;

namespace Ncqrs.EventSourcing.EntityFramework
{
    public class EntityFrameworkEventStore : IEventStore
    {
        private const int FirstVersion = 0;
        private readonly IPropertyBagConverter converter;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        /// <param name = "converter">
        ///   An object that must be used to convert the events and snapshots to a property bag
        /// </param>
        public EntityFrameworkEventStore(IPropertyBagConverter converter)
        {
            this.converter = converter;
        }

        /// <summary>
        /// Get all events provided by an specified event source for a particular version.
        /// </summary>
        /// <param name="eventSourceId"></param>
        /// <param name="version"></param>
        /// <returns>All the events from the event source.</returns>
        public IEnumerable<IEvent> GetAllEventsForVersion(Guid eventSourceId, long version)
        {
            return GetAllEventsForVersionSince(eventSourceId, FirstVersion, version);
        }

        /// <summary>
        /// Get all events provided by an specified event source for a particular version, since an earlier version.
        /// </summary>
        /// <param name="eventSourceId">The id of the event source that owns the events.</param>
        /// <param name="fromVersion"></param>
        /// <param name="version"></param>
        /// <returns>All the events from the event source.</returns>
        public IEnumerable<IEvent> GetAllEventsForVersionSince(Guid eventSourceId, long fromVersion, long version)
        {
            using (var ctx = new EventStoreContext())
            {
                var q = from evt in ctx.Events
                        where (evt.AggregateId == eventSourceId) && (evt.Version > fromVersion) && (evt.Version <= version)
                        orderby evt.Version ascending
                        select evt.Data;

                return q.ToList().Select(data => converter.Convert(FromBinary(data))).Cast<IEvent>().ToArray();
            }
        }

        /// <summary>
        ///   Gets the current version of an aggregate root from the store.
        /// </summary>
        public long GetVersion(Guid id)
        {
            using (var ctx = new EventStoreContext())
            {
                var q = from evt in ctx.Events
                        where (evt.AggregateId == id)
                        select evt.Version;

                return (q.Count() > 0) ? q.Max() : 0;
            }
        }

        /// <summary>
        ///   Save all uncommited events from a specific aggregate root.
        /// </summary>
        /// <param name = "source">
        ///   The aggregate root for which all uncommited events should be saved.
        /// </param>
        public IEnumerable<IEvent> Save(IEventSource source)
        {
            var uncommittedEvents = source.GetUncommittedEvents();

            SaveEvents(source.Id, source.UncommittedVersion, uncommittedEvents);

            source.AcceptChanges();

            return uncommittedEvents;
        }

        private void SaveEvents(Guid aggregateId, long uncomittedVersion, IEnumerable<IEvent> uncommittedEvents)
        {
            using (var ctx = new EventStoreContext())
            {
                long eventVersion = uncomittedVersion;

                foreach (IEvent evnt in uncommittedEvents)
                {
                    eventVersion++;

                    evnt.Id = aggregateId;
                    evnt.Version = eventVersion;
                    
                    ctx.AddToEvents(new Event
                    {
                        AggregateId = evnt.Id,
                        Version = evnt.Version,
                        Data = ToBinary(converter.Convert(evnt)),
                        Type = evnt.GetType().Name
                    });
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        ///   Saves a snapshot of the specified event source.
        /// </summary>
        public void SaveShapShot(IEventSource source)
        {
            using (var ctx = new EventStoreContext())
            {
                IMemento memento = source.GetMemento();

                ctx.AddToSnapshots(new Snapshot
                {
                    Data = ToBinary(converter.Convert(memento)),
                    AggregateId = source.Id,
                    Version = source.Version,
                    Type = memento.GetType().Name
                });

                ctx.SaveChanges();
            }
        }

        /// <summary>
        ///   Gets a snapshot of a particular event source, if one exists. Otherwise, returns <c>null</c>.
        /// </summary>
        public ISnapshot GetSnapShot(Guid eventSourceId, long version)
        {
            Snapshot mostRecentSnapshot = FindSnapshotForVersion(eventSourceId, version);
            if (mostRecentSnapshot != null)
            {
                return new Domain.Snapshot(
                    mostRecentSnapshot.AggregateId,
                    mostRecentSnapshot.Version,
                    (IMemento) converter.Convert(FromBinary(mostRecentSnapshot.Data)));
            }
            else
            {
                return null;
            }
        }

        private Snapshot FindSnapshotForVersion(Guid eventSourceId, long version)
        {
            using (var ctx = new EventStoreContext())
            {
                var snapshots =
                    from snapshot in ctx.Snapshots
                    where (snapshot.AggregateId == eventSourceId) && (snapshot.Version <= version)
                    orderby snapshot.Version descending
                    select snapshot;

                return snapshots.FirstOrDefault();
            }
        }

        private static byte[] ToBinary(PropertyBag document)
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, document);
                return memoryStream.ToArray();
            }
        }

        private static PropertyBag FromBinary(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                return (PropertyBag) new BinaryFormatter().Deserialize(memoryStream);
            }
        }
    }
}
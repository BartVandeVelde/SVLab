using System;
using System.Collections.Generic;

using Ncqrs.Domain;

namespace Ncqrs.EventSourcing
{
    /// <summary>
    /// An object that represents all his state changes via a sequence of events.
    /// </summary>
    public interface IEventSource
    {
        /// <summary>
        /// Gets the globally unique identifier.
        /// </summary>
        Guid Id
        {
            get;
        }

        /// <summary>
        /// Gets the version of this source.
        /// </summary>
        long Version
        {
            get;
        }

        long UncommittedVersion { get; }

        /// <summary>
        /// Gets the events that occurred since the last time <see cref="AcceptChanges"/> was called.
        /// </summary>
        IEnumerable<IEvent> GetUncommittedEvents();

        /// <summary>
        /// Restores an object from a collection of events that happended in history.
        /// </summary>
        void InitializeFromHistory(IEnumerable<IEvent> history);

        /// <summary>
        /// Makes all events returned by <see cref="GetUncommittedEvents"/> as commited to a 
        /// persistent store.
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Gets an object representing all of the internal state of the object.
        /// </summary>
        IMemento GetMemento();

        /// <summary>
        /// Restores an object to an earlier point in time. 
        /// </summary>
        void RestoreFromMemento(IMemento memento);
    }
}

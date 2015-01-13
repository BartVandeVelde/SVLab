using Microsoft.Practices.Unity;
using Ncqrs.EventSourcing;
using Ncqrs.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Ncqrs.Domain
{
    /// <summary>
    ///   The abstract concept of an aggregate root.
    /// </summary>
    public abstract class AggregateRoot : IEventSource
    {
        /// <summary>
        ///   Holds the events that are not yet committed.
        /// </summary>
        private readonly Stack<IEvent> uncommittedEvents = new Stack<IEvent>();

        /// <summary>
        ///   Gets the globally unique identifier.
        /// </summary>
        /// <exception cref = "InvalidOperationException">Thrown when setting this
        ///   value when the version of this aggregate root is not 0 or this
        ///   instance contains are any uncommitted events.</exception>
        public Guid Id { get; protected set; }

        /// <summary>
        ///   Gets the current version of the aggregate root as it is known in the event store.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This represents the committed version of this instance. When this instance was retrieved
        ///     via history, it contains the version as it was at that time. For new instances this value is always 0.
        ///   </para>
        ///   <para>
        ///     The version does not change until changes are accepted via the <see cref = "AcceptChanges" /> method.
        ///   </para>
        /// </remarks>
        /// <value>An <see cref = "int" /> representing the current version of this aggregate root.</value>
        public long Version
        {
            get { return UncommittedVersion + uncommittedEvents.Count; }
        }

        public long UncommittedVersion { get; private set; }

        /// <summary>
        ///   A list that contains all the event handlers.
        /// </summary>
        private readonly List<IDomainEventHandler> _eventHandlers = new List<IDomainEventHandler>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AggregateRoot" /> class.
        /// </summary>
        protected AggregateRoot()
        {
            UncommittedVersion = 0;

            var idGenerator = ServiceLocator.Current.Resolve<IUniqueIdentifierGenerator>();
            Id = idGenerator.GenerateNewId(this);
        }

        /// <summary>
        ///   Initializes from history.
        /// </summary>
        /// <param name = "history">The history.</param>
        public virtual void InitializeFromHistory(IEnumerable<IEvent> history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("history");
            }

            if (history.Count() == 0)
            {
                throw new ArgumentException("The provided history does not contain any historical event.", "history");
            }

            if (Version != 0 || uncommittedEvents.Count > 0)
            {
                throw new InvalidOperationException("Cannot load from history when a event source is already loaded.");
            }

            foreach (var historicalEvent in history)
            {
                ApplyEvent(historicalEvent, true);
                UncommittedVersion++; 
            }
        }

        protected void RegisterHandler(IDomainEventHandler handler)
        {
            _eventHandlers.Add(handler);
        }

        protected void ApplyEvent(IEvent evnt)
        {
            ApplyEvent(evnt, false);
        }

        private void ApplyEvent(IEvent evnt, bool historical)
        {
            if (!historical)
            {
                uncommittedEvents.Push(evnt);

                RegisterCurrentInstanceAsDirty();
            }

            HandleEvent(evnt);
        }

        private void RegisterCurrentInstanceAsDirty()
        {
            if (UnitOfWork.Current == null)
            {
                throw new NoUnitOfWorkAvailableInThisContextException();
            }

            UnitOfWork.Current.RegisterDirtyInstance(this);
        }

        protected virtual void HandleEvent(IEvent evnt)
        {
            bool handled = false;

            foreach (var handler in _eventHandlers)
            {
                handled |= handler.HandleEvent(evnt);
            }

            if (!handled)
            {
                throw new EventNotHandledException(evnt);
            }
        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return uncommittedEvents.ToArray();
        }

        public void AcceptChanges()
        {
            UncommittedVersion = Version;

            // Clear the unaccepted event list.
            uncommittedEvents.Clear();
        }

        /// <summary>
        /// Restores an object to an earlier point in time. 
        /// </summary>
        public abstract void RestoreFromMemento(IMemento memento);

        /// <summary>
        /// Gets an object representing all of the internal state of the object.
        /// </summary>
        public abstract IMemento GetMemento();

        /// <summary>
        /// Merges the events that occurred since the version this aggregate root represents.
        /// </summary>
        /// <param name="concurrentEvents">
        /// The concurrent events that have occurred since this version was loaded.
        /// </param>
        /// <returns>
        /// Returns the events that were rejected because of the business rules enforced by the 
        /// aggregate root.
        /// </returns>
        public IEnumerable<IEvent> MergeEvents(IEnumerable<IEvent> concurrentEvents)
        {
            UpdateVersionToMatchLatestStoredAggregateRoot(concurrentEvents);

            var rejectedEvents = new List<IEvent>();

            IEvent[] eventsToCheckForConflicts = uncommittedEvents.ToArray();
            uncommittedEvents.Clear();

            foreach (var eventToCheckForConflict in eventsToCheckForConflicts)
            {
                if (concurrentEvents.Any(evt => IsConflict(eventToCheckForConflict, evt)))
                {
                    rejectedEvents.Add(eventToCheckForConflict);
                }
                else
                {
                    uncommittedEvents.Push(eventToCheckForConflict);
                }
            }

            return rejectedEvents;
        }

        private void UpdateVersionToMatchLatestStoredAggregateRoot(IEnumerable<IEvent> concurrentEvents)
        {
            UncommittedVersion = concurrentEvents.Max(evt => evt.Version);
        }

        /// <summary>
        /// Determines whether the specified event conflicts with another event that has happened before.
        /// </summary>
        protected virtual bool IsConflict(IEvent evnt, IEvent concurrentEvent)
        {
            return false;
        }
    }
}
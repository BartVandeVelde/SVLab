﻿using System;
using System.Diagnostics.Contracts;

namespace Ncqrs.Domain
{
    /// <summary>
    ///   An event handler that uses a specified action as handler, but only calls this when the event
    ///   is of a certain type, or is inherited from it.
    /// </summary>
    public class TypeThresholdedActionBasedDomainEventHandler : IDomainEventHandler
    {
        /// <summary>
        ///   The event type that should be used as threshold.
        /// </summary>
        private readonly Type _eventTypeThreshold;

        /// <summary>
        ///   Specifies whether the event type threshold should be used as an exact or at least threshold.
        /// </summary>
        /// <remarks>
        ///   <c>true</c> means that the event type threshold should be the same type as the event, otherwise
        ///   the event handler will not be executed. <c>false</c> means that the threshold type should be in 
        ///   the inheritance hierarchy of the event type, or that the threshold type should be an interface 
        ///   that the event type implements, otherwise the handler will not be executed.
        /// </remarks>
        private readonly bool _exact;

        /// <summary>
        ///   The handler that should be called when the threshold did not hold the event.
        /// </summary>
        private readonly Action<IEvent> _handler;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TypeThresholdedActionBasedDomainEventHandler" /> class.
        /// </summary>
        /// <param name = "handler">The handler that will be called to handle a event when the threshold did not hold the event.</param>
        /// <param name = "eventTypeThreshold">The event type that should be used as threshold.</param>
        /// <param name = "exact">if set to <c>true</c> the threshold will hold all types that are not the same type; otherwise it hold 
        /// all types that are not inhered from the event type threshold or implement the interface that is specified by the threshold type.</param>
        public TypeThresholdedActionBasedDomainEventHandler(Action<IEvent> handler, Type eventTypeThreshold,
                                                              Boolean exact = false)
        {
            _handler = handler;
            _eventTypeThreshold = eventTypeThreshold;
            _exact = exact;
        }

        /// <summary>
        ///   Handles the event.
        /// </summary>
        /// <param name = "evnt">The event to handle.
        ///   <remarks>
        ///     This value should not be <c>null</c>.
        ///   </remarks>
        /// </param>
        /// <returns>
        ///   <c>true</c> when the event was handled; otherwise, <c>false</c>.
        ///   <remarks>
        ///     <c>false</c> does not mean that the handling failed, but that the
        ///     handler was not interested in handling this event.
        ///   </remarks>
        /// </returns>
        public bool HandleEvent(IEvent evnt)
        {
            

            var handled = false;

            if (ShouldHandleThisEvent(evnt))
            {
                _handler(evnt);
                handled = true;
            }

            return handled;
        }

        /// <summary>
        ///   This method holds all the objects invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handler != null);
            Contract.Invariant(_eventTypeThreshold != null);
        }

        /// <summary>
        ///   Determine whether the event should be handled or not.
        /// </summary>
        /// <param name = "evnt">The event.</param>
        /// <returns><c>true</c> when this event should be handled; otherwise, <c>false</c>.</returns>
        private bool ShouldHandleThisEvent(IEvent evnt)
        {
            Contract.Assume(evnt != null, "The evnt should not be null.");

            var shouldHandle = false;

            var evntType = evnt.GetType();

            // This is true when the eventTypeThreshold is 
            // true if event type and the threshold type represent the same type, or if the theshold type is in the inheritance hierarchy 
            // of the event type, or if the threshold type is an interface that event type implements.
            if (_eventTypeThreshold.IsAssignableFrom(evntType))
            {
                if (_exact)
                {
                    // Only handle the event when there is an exact match.
                    shouldHandle = (_eventTypeThreshold == evntType);
                }
                else
                {
                    // Handle the event, since it the threshold is assignable from the event type.
                    shouldHandle = true;
                }
            }

            return shouldHandle;
        }
    }
}
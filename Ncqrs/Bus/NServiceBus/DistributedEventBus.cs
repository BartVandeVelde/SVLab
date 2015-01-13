using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ncqrs.Bus.NServiceBus
{
    /// <summary>
    ///   An implementation of <see cref = "IEventBus" /> that uses NServiceBus for distributing events 
    ///   to multiple subscribers.
    /// </summary>
    public class DistributedEventBus : IEventBus
    {
        readonly IBus serviceBus;

        public DistributedEventBus(IBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        /// <summary>
        ///   Publishes the event to the handlers.
        /// </summary>
        /// <param name = "eventMessage">The message to publish.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <i>message</i> was null.</exception>
        /// <exception cref = "NoHandlerRegisteredForMessageException">Thrown when no handler was found for the specified message.</exception>
        public void Publish(Ncqrs.Domain.IEvent eventMessage)
        {
            serviceBus.Publish(eventMessage);
        }

        /// <summary>
        ///   Publishes the messages to the handlers.
        /// </summary>
        /// <param name = "eventMessages"></param>
        /// <exception cref = "ArgumentNullException">Thrown when <i>messages</i> was null.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when a instance in <i>messages</i> was null.</exception>
        /// <exception cref = "NoHandlerRegisteredForMessageException">Thrown when no handler was found for one of the specified messages.</exception>
        public void Publish(IEnumerable<Ncqrs.Domain.IEvent> eventMessages)
        {
            serviceBus.Publish(eventMessages.ToArray());
        }
    }
}
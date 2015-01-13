using System;
using System.Collections.Generic;

using Ncqrs.Domain;

namespace Ncqrs.Bus
{
    /// <summary>
    /// A bus that can publish events to handlers.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes the event to the handlers.
        /// </summary>
        /// <param name="eventMessage">The message to publish.</param>
        /// <exception cref="ArgumentNullException">Thrown when <i>message</i> was null.</exception>
        void Publish(IEvent eventMessage);

        /// <summary>
        /// Publishes the messages to the handlers.
        /// </summary>
        /// <param name="eventMessage">The messages to publish.</param>
        /// <exception cref="ArgumentNullException">Thrown when <i>messages</i> was null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when a instance in <i>messages</i> was null.</exception>
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}
using System;

using NServiceBus;

namespace Ncqrs.Domain
{
    /// <summary>
    /// Represents an event
    /// </summary>
    public interface IEvent : IMessage
    {
        Guid Id { get; set; }
        long Version { get; set; }
    }
}
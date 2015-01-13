using System;

namespace Ncqrs.Domain
{
    public interface ISnapshot 
    {
        IMemento Memento { get; }
        Guid EventSourceId { get; }
        long Version { get; }
    }
}
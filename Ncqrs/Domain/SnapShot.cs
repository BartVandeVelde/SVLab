using System;

namespace Ncqrs.Domain
{
    [Serializable]
    public class Snapshot : ISnapshot
    {
        public Snapshot(Guid eventSourceId, long version, IMemento memento)
        {
            EventSourceId = eventSourceId;
            Version = version;
            Memento = memento;
        }

        public Guid EventSourceId { get; private set; }
        public long Version { get; private set; }
        public IMemento Memento { get; private set; }
    }
}
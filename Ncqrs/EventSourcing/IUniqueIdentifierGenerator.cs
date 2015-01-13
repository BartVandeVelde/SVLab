using System;

using Ncqrs.EventSourcing;

namespace Ncqrs.Domain
{
    public interface IUniqueIdentifierGenerator
    {
        Guid GenerateNewId(IEventSource eventSource);
    }
}
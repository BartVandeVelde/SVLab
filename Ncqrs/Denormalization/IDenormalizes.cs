using Ncqrs.Domain;

namespace Ncqrs.Denormalization
{
    public interface IDenormalizes<TEvent> where TEvent : IEvent
    {
        void DenormalizeEvent(TEvent evnt);        
    }
}

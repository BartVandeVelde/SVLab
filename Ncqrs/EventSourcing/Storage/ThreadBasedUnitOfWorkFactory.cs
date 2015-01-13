using Ncqrs.Bus;
using Ncqrs.Domain.Storage;
using Ncqrs.Domain.Bus;
using Ncqrs.EventSourcing.Storage;

namespace Ncqrs.Domain
{
    public class ThreadBasedUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IDomainRepository _repository;

        public ThreadBasedUnitOfWorkFactory(IEventStore eventStore, IEventBus eventBus)
        {
            _repository = new DomainRepository(eventStore, eventBus);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_repository);
        }

        public IUnitOfWork GetUnitOfWorkInCurrentContext()
        {
            return UnitOfWork.Current;
        }
    }
}

using NServiceBus;

namespace SVLab.UI.Infrastructure.Services
{
    public class CommandService
    {
        private readonly IBus bus;

        public CommandService(IBus bus)
        {
            this.bus = bus;
        }

        public void Send(ICommand command)
        {
            bus.Send(command);
        }
    }
}
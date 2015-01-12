using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace SVLab.Server.CommandService
{
    class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
                   {
                       ErrorQueue = "error"
                   };
        }
    }
}
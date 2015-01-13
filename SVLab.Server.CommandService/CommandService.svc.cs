using Microsoft.Practices.Unity;
using Ncqrs.CommandHandling;
using Ncqrs.Commands;
using Ncqrs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace SVLab.Server.CommandService
{
    [ServiceContract(Namespace = "")]
    [ServiceKnownType("GetKnownTypes")]
    public class CommandService
    {
        private static ICommandHandler executor;

        /// <summary>
        /// Initializes the <see cref="CommandService"/> class.
        /// </summary>
        static CommandService()
        {
            new Bootstrapper().Start();

            executor = ServiceLocator.Current.Resolve<ICommandHandler>();
        }

        /// <summary>
        /// Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        [OperationContract]
        public void Execute(IEnumerable<ICommand> commands)
        {
            try
            {
                foreach (var command in commands)
                {
                    executor.Handle(command);
                }
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            var query =
                from type in typeof(ICommand).Assembly.GetTypes()
                where typeof(ICommand).IsAssignableFrom(type)
                select type;

            return query.ToArray();
        }
    }
}

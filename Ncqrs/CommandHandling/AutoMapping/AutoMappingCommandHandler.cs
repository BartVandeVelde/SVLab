using System;

using Ncqrs.CommandHandling;
using Ncqrs.Commands;
using Ncqrs.Domain.Storage;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Ncqrs.Commands.Attributes;

namespace Ncqrs.CommandHandling.AutoMapping
{
    /// <summary>
    /// A command handler that execute an action based on the mapping of a command.
    /// </summary>
    public class AutoMappingCommandHandler : ICommandHandler
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command to execute. This should not be null.</param>
        /// <exception cref="ArgumentNullException">Occurs when <i>command</i> is null.</exception>
        public void Handle(ICommand command)
        {
            var factory = new ActionFactory();
            ICommandHandler executor = factory.CreateHandlerForCommand(command);

            if (command.GetType().GetCustomAttributes(typeof(TransactionalAttribute), true).Length > 0)
            {
                executor = new TransactionalCommandHandlerWrapper(executor);
            }

            executor.Handle(command);
        }
    }
}

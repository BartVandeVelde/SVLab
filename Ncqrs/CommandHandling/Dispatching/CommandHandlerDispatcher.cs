using System;

using Ncqrs.CommandHandling;
using Ncqrs.Commands;

using log4net;

using System.Reflection;

namespace Ncqrs.CommandHandling.Dispatching
{
    /// <summary>
    /// A command handler that dispatch the command execution to other command handlers.
    /// </summary>
    public abstract class CommandHandlerDispatcher : ICommandHandler
    {
        /// <summary>
        /// The logger to use to log messages.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Execute a <see cref="ICommand"/> by giving it to the registered <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <exception cref="ArgumentNullException">Occurs when the <i>command</i> was a <c>null</c> dereference.</exception>
        /// <exception cref="CommandHandlerNotFoundException">Occurs when the <see cref="ICommandHandler"/> was not found for on the given <see cref="ICommand"/>.</exception>
        public void Handle(ICommand command)
        {
            Type commandType = command.GetType();

            Logger.InfoFormat("Received {0} command and will now start processing it.", commandType.FullName);

            ICommandHandler handler = GetCommandHandlerForCommand(commandType);

            if (handler == null)
            {
                throw new CommandHandlerNotFoundException(commandType);
            }

            Logger.DebugFormat("Found command handler {0} to execute the {1} command. Will start executing it now.", handler.GetType().FullName, commandType.FullName);

            handler.Handle(command);

            Logger.DebugFormat("Execution complete.");
        }

        /// <summary>
        /// Gets the command handler for command.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A command handler to use to execute the command or <c>null</c> if not found.</returns>
        protected abstract ICommandHandler GetCommandHandlerForCommand(Type commandType);
    }
}

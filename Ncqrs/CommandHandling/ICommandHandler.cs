using System;

using Ncqrs.Commands;

namespace Ncqrs.CommandHandling
{
    /// <summary>
    /// Executes a command. This means that the handles 
    /// executes the correct action based on the command.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command to execute. This should not be null.</param>
        /// <exception cref="ArgumentNullException">Occurs when <i>command</i> is null.</exception>
        void Handle(ICommand command);
    }

}

using Ncqrs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Ncqrs.CommandHandling
{
    /// <summary>
    /// Wraps transactional behavior around the execution of command executor.
    /// </summary>
    /// <remarks>
    /// The transaction logic uses TransactionScope.
    /// </remarks>
    public class TransactionalCommandHandlerWrapper : ICommandHandler
    {
        /// <summary>
        /// The executor to use to execute the command.
        /// </summary>
        private readonly ICommandHandler _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalCommandHandlerWrapper"/> class.
        /// </summary>
        /// <param name="executor">The executor to use to execute the command.</param>
        public TransactionalCommandHandlerWrapper(ICommandHandler executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Executes the command within a transaction. The transaction logic uses TransactionScope.
        /// </summary>
        /// <param name="command">The command to execute. This should not be null.</param>
        /// <exception cref="ArgumentNullException">Occurs when <i>command</i> is null.</exception>
        public void Handle(ICommand command)
        {
            using (var transaction = new TransactionScope())
            {
                _executor.Handle(command);
            }
        }
    }
}

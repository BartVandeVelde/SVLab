using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ncqrs.CommandHandling;
using Ncqrs.CommandHandling.AutoMapping;
using Ncqrs.Commands;

namespace Ncqrs.CommandHandling.Dispatching
{
    /// <summary>
    ///   A dispatcher that dispatch command objects to their appropriate command handler. Command handlers can subscribe and
    ///   unsubscribe to specific command types. Only a single handler may be subscribed for a single type of command at any time.
    /// </summary>
    public class InProcessCommandHandlerDispatcher : CommandHandlerDispatcher
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers = new Dictionary<Type, ICommandHandler>();

        /// <summary>
        ///   Registers the handler for the specified command type. The handler will be called for every command of the specified type.
        /// </summary>
        /// <typeparam name = "TCommand">The type of the command.</typeparam>
        /// <param name = "handler">The handler that will be called for every command of the specified type.</param>
        /// <exception cref = "ArgumentNullException">Occurs when the <i>commandType</i> or <i>handler</i> was a <c>null</c> dereference.</exception>
        public void RegisterHandler<TCommand>(ICommandHandler handler) where TCommand : ICommand
        {
            RegisterHandler(typeof (TCommand), handler);
        }

        /// <summary>
        ///   Registers the handler for the specified command type. The handler will be called for every command of the specified type.
        /// </summary>
        /// <param name = "commandType">Type of the command.</param>
        /// <param name = "handler">The handler that will be called for every command of the specified type.</param>
        /// <exception cref = "ArgumentNullException">Occurs when the <i>commandType</i> or <i>handler</i> was a <c>null</c> dereference.</exception>
        public void RegisterHandler(Type commandType, ICommandHandler handler)
        {
            _handlers.Add(commandType, handler);
        }

        /// <summary>
        ///   Unregisters the handler of the specified command type. The handler will not be called any more.
        /// </summary>
        /// <param name = "commandType">Type of the command.</param>
        /// <param name = "handler">The handler to unregister.</param>
        /// <exception cref = "ArgumentNullException">Occurs when the <i>commandType</i> or <i>handler</i> was a <c>null</c> dereference.</exception>
        /// <exception cref = "InvalidOperationException">Occurs when the <i>handler</i> is not the same as the registered handler for the specified command type.</exception>
        public void UnregisterHandler(Type commandType, ICommandHandler handler)
        {
            ICommandHandler registeredHandler = null;

            if (_handlers.TryGetValue(commandType, out registeredHandler))
            {
                if (handler != registeredHandler)
                {
                    throw new InvalidOperationException(
                        "The specified handler does not match with the registered handler for command type " +
                            commandType.FullName + ".");
                }

                _handlers.Remove(commandType);
            }
        }

        /// <summary>
        ///   Gets the command handler for command.
        /// </summary>
        /// <param name = "commandType">Type of the command.</param>
        /// <returns>
        ///   A command handler to use to execute the command or <c>null</c> if not found.
        /// </returns>
        protected override ICommandHandler GetCommandHandlerForCommand(Type commandType)
        {
            ICommandHandler result = null;
            _handlers.TryGetValue(commandType, out result);

            return result;
        }

        public void RegisterHandlerForAllCommandsIn(Assembly commandAssembly, ICommandHandler handler)
        {
            var commands =
                from type in commandAssembly.GetTypes()
                where IsConcreteCommand(type)
                select type;

            foreach (var command in commands)
            {
                RegisterHandler(command, new AutoMappingCommandHandler());
            }
        }

        private static bool IsConcreteCommand(Type type)
        {
            return !type.IsAbstract && type.GetInterfaces().Any(t => t == typeof(ICommand));
        }
    }
}
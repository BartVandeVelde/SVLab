﻿using System;
using System.Linq;
using System.Reflection;

using Ncqrs.CommandHandling;
using Ncqrs.Commands;
using Ncqrs.Domain;
using Ncqrs.Domain.Storage;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Ncqrs.CommandHandling.AutoMapping.Actions
{
    /// <summary>
    /// An auto mapped action for a command. It created the object as specified by the mapping.
    /// </summary>
    public class ObjectCreationAction : ICommandHandler
    {
        private IUnitOfWorkFactory factory;

        public ObjectCreationAction(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Executes this action.
        /// </summary>
        public void Handle(ICommand command)
        {
            var commandInfo = ObjectCreationCommandInfo.CreateFromDirectMethodCommand(command);

            using (var work = factory.CreateUnitOfWork())
            {
                var targetCtor = GetConstructorBasedOnCommand(commandInfo, command);

                var parameterValues = CommandAutoMappingConfiguration.GetParameterValues(command, targetCtor.GetParameters());
                targetCtor.Invoke(parameterValues);

                work.Accept();
            }
        }

        private ConstructorInfo GetConstructorBasedOnCommand(ObjectCreationCommandInfo commandInfo, ICommand command)
        {
            var aggregateType = commandInfo.AggregateType;
            var propertiesToMap = CommandAutoMappingConfiguration.GetCommandProperties(command);
            var ctorQuery = from ctor in aggregateType.GetConstructors()
                            where ctor.GetParameters().Length == propertiesToMap.Count()
                            where ParametersDoMatchPropertiesToMap(ctor.GetParameters(), propertiesToMap)
                            select ctor;

            if (ctorQuery.Count() == 0)
            {
                var message = String.Format("No constructor found with {0} parameters on aggregate root {1}.",
                                            propertiesToMap.Count(), aggregateType.FullName);
                throw new CommandMappingException(message);
            }
            if (ctorQuery.Count() > 1)
            {
                var message = String.Format("Multiple constructors found with {0} parameters on aggregate root {1}.",
                                            propertiesToMap.Count(), aggregateType.FullName);
                throw new CommandMappingException(message);
            }

            return ctorQuery.First();
        }

        // TODO: Remove this duplicate method.
        private Boolean ParametersDoMatchPropertiesToMap(ParameterInfo[] parameterInfo, IEnumerable<PropertyInfo> propertiesToMap)
        {
            var enumerator = propertiesToMap.GetEnumerator();

            for (int i = 0; i < parameterInfo.Length; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return false;
                }

                if (!parameterInfo[i].ParameterType.IsAssignableFrom(enumerator.Current.PropertyType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
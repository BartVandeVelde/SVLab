using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ncqrs.CommandHandling.AutoMapping;
using Ncqrs.Commands;
using Ncqrs.Commands.Attributes;

namespace Ncqrs.CommandHandling.AutoMapping
{
    internal class DirectMethodCommandInfo
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DirectMethodCommandInfo" /> struct.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "aggregateType">Type of the aggregate.</param>
        /// <param name = "aggregateRootId">The aggregate root id.</param>
        /// <param name = "methodName">Name of the method.</param>
        public DirectMethodCommandInfo(ICommand command, Type aggregateType, Guid aggregateRootId, long aggregateRootVersion,
            string methodName)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (aggregateType == null)
            {
                throw new ArgumentNullException("aggregateType");
            }

            if (String.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            Command = command;
            AggregateType = aggregateType;
            AggregateRootId = aggregateRootId;
            AggregateRootVersion = aggregateRootVersion;
            MethodName = methodName;
        }

        /// <summary>
        ///   Gets the type of the aggregate root.
        /// </summary>
        /// <value>The type of the aggregate root.</value>
        public Type AggregateType { get; private set; }

        /// <summary>
        ///   Gets the aggregate root id.
        /// </summary>
        /// <value>The id of the aggregate root.</value>
        public Guid AggregateRootId { get; private set; }

        public long AggregateRootVersion { get; set; }

        public ICommand Command { get; private set; }

        public String MethodName { get; private set; }

        public static DirectMethodCommandInfo CreateFromDirectMethodCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Type aggregateRootType = GetAggregateRootType(command);
            long aggregateRootVersion = GetAggregateRootVersion(command);
            Guid aggregateRootId = GetAggregateRootId(command);
            MapsToAggregateRootMethodAttribute attribute = GetMappingAttribute(command);
            string methodName = String.IsNullOrEmpty(attribute.MethodName) ? command.GetType().Name : attribute.MethodName;

            return new DirectMethodCommandInfo(command, aggregateRootType, aggregateRootId, aggregateRootVersion, methodName);
        }

        private static long GetAggregateRootVersion(ICommand command)
        {
            var prop = GetPropertyMarkedAsAggregateRootVersion(command);

            if (prop.PropertyType != typeof (long))
            {
                string message = 
                    string.Format("Property {0} that marked as aggregate root version is not of type long.", prop.Name);
                
                throw new CommandMappingException(message);
            }

            return (long) prop.GetValue(command, null);
        }

        private static PropertyInfo GetPropertyMarkedAsAggregateRootVersion(ICommand command)
        {
            Type type = command.GetType();
            
            IEnumerable<PropertyInfo> propertyQuery =
                from prop in type.GetProperties()
                where prop.GetCustomAttributes(typeof (AggregateRootVersionAttribute), true).Count() > 0
                select prop;

            if (propertyQuery.Count() == 0)
            {
                string message = String.Format("Missing AggregateRootVersionAttribute on {0} command.", type.Name);
                throw new CommandMappingException(message);
            }

            if (propertyQuery.Count() > 1)
            {
                string message =
                    String.Format("Multiple AggregateRootVersionAttribute found on {0} command, only one attribute is allowed.",
                        type.Name);

                throw new CommandMappingException(message);
            }

            return propertyQuery.First();
        }

        private static Type GetAggregateRootType(ICommand command)
        {
            MapsToAggregateRootMethodAttribute mappingAttribute = GetMappingAttribute(command);

            try
            {
                return Type.GetType(mappingAttribute.TypeName, true);
            }
            catch (Exception e)
            {
                var message = String.Format("Couldn't determ aggregate root type from typename '{0}'.", mappingAttribute.TypeName);
                throw new AutoMappingException(message, e);
            }
        }

        private static Guid GetAggregateRootId(ICommand command)
        {
            var prop = GetPropertyMarkedAsAggregateRootId(command);

            if (prop.PropertyType != typeof (Guid))
            {
                String message = String.Format("Property {0} that marked as aggregate root id is not of type Guid.", prop.Name);
                throw new CommandMappingException(message);
            }

            return (Guid) prop.GetValue(command, null);
        }

        private static PropertyInfo GetPropertyMarkedAsAggregateRootId(ICommand command)
        {
            var type = command.GetType();
            var propertyQuery = from prop in type.GetProperties()
                                where prop.GetCustomAttributes(typeof (AggregateRootIdAttribute), true).Count() > 0
                                select prop;

            if (propertyQuery.Count() == 0)
            {
                String message = String.Format("Missing AggregateRootIdAttribute on {0} command.", type.Name);
                throw new CommandMappingException(message);
            }
            if (propertyQuery.Count() > 1)
            {
                String message =
                    String.Format("Multiple AggregateRootIdAttribute found on {0} command, only one attribute is allowed.",
                        type.Name);
                throw new CommandMappingException(message);
            }

            return propertyQuery.First();
        }

        private static MapsToAggregateRootMethodAttribute GetMappingAttribute(ICommand command)
        {
            var type = command.GetType();
            var mappingAttributes =
                (MapsToAggregateRootMethodAttribute[]) type.GetCustomAttributes(typeof (MapsToAggregateRootMethodAttribute), true);

            if (mappingAttributes.Length == 0)
            {
                String message = String.Format("Missing MapsToAggregateRootMethodAttribute on {0} command.", type.Name);
                throw new CommandMappingException(message);
            }
            if (mappingAttributes.Length > 1)
            {
                String message =
                    String.Format(
                        "Multiple MapsToAggregateRootMethodAttribute found on {0} command, only one attribute is allowed.",
                        type.Name);
                throw new CommandMappingException(message);
            }

            return mappingAttributes[0];
        }
    }
}
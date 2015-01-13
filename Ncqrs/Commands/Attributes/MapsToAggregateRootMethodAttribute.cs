﻿using System;

namespace Ncqrs.Commands.Attributes
{
    /// <summary>
    /// Defines that the command maps directly to a method on an aggregate root.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MapsToAggregateRootMethodAttribute : Attribute
    {
        /// <summary>
        /// Get or sets the full qualified type name of the target aggregate root.
        /// </summary>
        public string TypeName
        {
            get; set;
        }

        /// <summary>
        /// Get or sets the full qualified name of the target method.
        /// </summary>
        /// <remarks>Leave this null or empty to automap the target method based on the command name.</remarks>
        public string MethodName
        {
            get; set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapsToAggregateRootMethodAttribute"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        public MapsToAggregateRootMethodAttribute(string typeName)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapsToAggregateRootMethodAttribute"/> class.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        public MapsToAggregateRootMethodAttribute(string typeName, string methodName)
        {
            if (String.IsNullOrEmpty(typeName)) throw new ArgumentNullException("typeName");
            if (String.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");

            TypeName = typeName;
            MethodName = methodName;
        }

        public MapsToAggregateRootMethodAttribute(Type typeName, string methodName)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (String.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");

            TypeName = typeName.AssemblyQualifiedName;
            MethodName = methodName;
        }
    }
}
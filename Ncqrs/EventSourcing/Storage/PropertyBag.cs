using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ncqrs.EventSourcing.Storage
{
    [Serializable]
    public class PropertyBag
    {
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        public PropertyBag(Type type)
        {
            TypeName = type.Name;
            Namespace = type.Namespace;
            AssemblyName = type.Assembly.FullName;
        }

        /// <summary>
        ///   Gets or sets the type name of the original object from which this document was constructed.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        ///   Gets or sets the name of the assembly from which the original object originated from.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        ///   Gets or sets the namespace of the original object from which this document was constructed.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        ///   Gets the assembly-qualified name of the System.Type, which includes the name of the assembly from which the System.Type was loaded.
        /// </summary>
        public string AssemblyQualfiedName
        {
            get { return Namespace + "." + TypeName + "," + AssemblyName; }
        }

        /// <summary>
        ///   Gets or sets the names and values of all public properties of the original object.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get { return properties; }
        }

        public void AddPropertyValue(string propertyName, object value)
        {
            properties.Add(propertyName, value);
        }
    }
}
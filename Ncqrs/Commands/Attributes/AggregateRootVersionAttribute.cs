using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ncqrs.Commands.Attributes
{
    /// <summary>
    /// Marks a property as the propery that contains the version of the aggregate root it applies on.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AggregateRootVersionAttribute : ExcludeInMappingAttribute
    {
    }
}

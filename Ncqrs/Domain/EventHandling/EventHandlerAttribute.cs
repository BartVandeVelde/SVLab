﻿using System;

namespace Ncqrs.Domain.Mapping
{
    /// <summary>
    /// Indicates that a method is an event handler.
    /// </summary>
    /// <remarks>
    /// The marked method should not be static and should have on parameter that is of the type <see cref="DomainEvent"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandlerAttribute : Attribute
    {
        public Boolean Exact
        {
            get; set;
        }

        public EventHandlerAttribute(Boolean exact = false)
        {
            Exact = exact;            
        }
    }
}
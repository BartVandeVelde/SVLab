using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVLab.Shared.Events
{
    public class EntityUpdated
    {
        public Guid EventId { get; set; }
        public Entity OldEntity { get; set; }
        public Entity NewEntity { get; set; }
    }
}

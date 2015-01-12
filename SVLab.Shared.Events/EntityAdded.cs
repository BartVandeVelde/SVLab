﻿using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVLab.Shared.Events
{
    public class EntityAdded
    {
        public Guid EventId { get; set; }
        public Entity Entity { get; set; }
    }
}

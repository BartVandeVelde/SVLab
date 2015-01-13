using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVLab.Shared.Commands
{
    public class CreateEntity
    {
        public Guid CommandId { get; set; }
        public Entity Entity { get; set; }
    }
}
using System.Collections.Generic;

namespace SVLab.Shared.Entities
{
    public class Campaign : Entity
    {
        public string Name { get; set; }

        public virtual Project ParentProject { get; set; }
        public virtual ICollection<Measurement> SubMeasurements { get; set; }
    }
}
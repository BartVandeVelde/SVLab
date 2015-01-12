using System.Collections.Generic;

namespace SVLab.Shared.Entities
{
    public class MeasurementFile : Entity
    {
        public string Path { get; set; }

        public virtual Project ParentProject { get; set; }
        public virtual Campaign ParentCampaign { get; set; }
        public virtual ICollection<Measurement> ParentMeasurements { get; set; }
    }
}
using System.Collections.Generic;

namespace SVLab.Shared.Entities
{
    public class Measurement : Entity
    {
        public string Name { get; set; }

        public virtual Project ParentProject { get; set; }
        public virtual Campaign ParentCampaign { get; set; }
        public virtual ICollection<MeasurementFile> SubMeasurementFiles { get; set; }
    }
}
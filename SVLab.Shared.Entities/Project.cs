using System.Collections.Generic;

namespace SVLab.Shared.Entities
{
    public class Project : Entity
    {
        public int ParentId { get; set; }

        public string Name { get; set; }
        public string Number { get; set; }

        public virtual Project ParentProject { get; set; }
        public virtual ICollection<Project> SubProjects { get; set; }
        public virtual ICollection<Campaign> SubCampaigns { get; set; }
    }
}
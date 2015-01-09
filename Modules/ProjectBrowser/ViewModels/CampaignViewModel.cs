using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBrowser.ViewModels
{
    public class CampaignViewModel : TreeViewItemViewModel 
    {
        readonly Campaign campaign;

        public CampaignViewModel(Campaign campaign, ProjectViewModel parentProject)
            : base(parentProject, true)
        {
            this.campaign = campaign;
        }

        public string Name
        {
            get { return this.campaign.Name; }
        }

        protected override void LoadChildren()
        {
            if (this.campaign.SubMeasurements != null)
            {
                foreach (Measurement m in this.campaign.SubMeasurements)
                {
                    base.Children.Add(new MeasurementViewModel(m, this));
                }
            }
        }
    }
}

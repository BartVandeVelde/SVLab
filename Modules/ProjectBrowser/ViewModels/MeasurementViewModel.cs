using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBrowser.ViewModels
{
    public class MeasurementViewModel : TreeViewItemViewModel 
    {
        readonly Measurement measurement;

        public MeasurementViewModel(Measurement measurement, CampaignViewModel parentCampaign)
            : base(parentCampaign, true)
        {
            this.measurement = measurement;
        }

        public string Name
        {
            get { return this.measurement.Name; }
        }

        protected override void LoadChildren()
        {
            if (this.measurement.SubMeasurementFiles != null)
            {
                foreach (MeasurementFile f in this.measurement.SubMeasurementFiles)
                {
                    base.Children.Add(new MeasurementFileViewModel(f, this));
                }
            }
        }
    }
}

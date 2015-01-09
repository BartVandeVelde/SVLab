using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectBrowser.ViewModels
{
    public class MeasurementFileViewModel : TreeViewItemViewModel 
    {
        readonly MeasurementFile measurementFile;

        public MeasurementFileViewModel(MeasurementFile measurementFile, MeasurementViewModel parentMeasurement)
            : base(parentMeasurement, true)
        {
            this.measurementFile = measurementFile;
        }

        public string FileName
        {
            get { return Path.GetFileName(this.measurementFile.Path); }
        }
    }
}

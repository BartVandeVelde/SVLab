using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SVLab.UI.Infrastructure.Interfaces
{
    public interface ILogViewerLogger
    {
        ObservableCollection<ILogMessage> Messages { get; }
    }
}

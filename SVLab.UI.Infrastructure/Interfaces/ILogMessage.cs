using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVLab.UI.Infrastructure.Interfaces
{
    public interface ILogMessage
    {
        string Message { get; }
        Category Category { get; }
        Priority Priority { get; }
        DateTime Timestamp { get; }
    }
}

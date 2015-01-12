using Microsoft.Practices.Prism.Logging;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SVLab.UI.Infrastructure.Logging
{
    public class LogViewerLogger : ILoggerFacade, ILogViewerLogger
    {
        private ObservableCollection<ILogMessage> messages = null;
        public LogViewerLogger()
        {
            messages = new ObservableCollection<ILogMessage>();
        }

        public ObservableCollection<ILogMessage> Messages
        {
            get
            {
                return this.messages;
            }
        }

        #region ILoggerFacade Members
        public void Log(string message, Category category, Priority priority)
        {
            this.messages.Add(new LogMessage(message, category, priority));
        }
        #endregion
    }
}
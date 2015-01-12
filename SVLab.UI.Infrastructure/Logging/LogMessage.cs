using Microsoft.Practices.Prism.Logging;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SVLab.UI.Infrastructure.Logging
{
    public class LogMessage : ILogMessage
    {
        private string message;
        private Category category;
        private Priority priority;
        private DateTime timestamp;
        public LogMessage(string message, Category category, Priority priority)
        {
            this.message = message;
            this.category = category;
            this.priority = priority;
            this.timestamp = DateTime.Now;
        }

        public string Message
        {
            get { return this.message; }
        }

        public Category Category
        {
            get { return this.category; }
        }

        public Priority Priority
        {
            get { return this.priority; }
        }

        public DateTime Timestamp
        {
            get { return this.timestamp; }
        }
    }
}

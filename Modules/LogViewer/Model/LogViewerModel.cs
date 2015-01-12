using Microsoft.Practices.Prism.Logging;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LogViewer.Model
{
    public class LogViewerModel : ILogMessage
    {
        private string message;
        private Category category;
        private Priority priority;
        private DateTime timestamp;
        public LogViewerModel(string message, Category category, Priority priority, DateTime timestamp)
        {
            this.message = message;
            this.category = category;
            this.priority = priority;
            this.timestamp = timestamp;
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
        [DisplayFormatAttribute(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Timestamp
        {
            get { return this.timestamp; }
        }
    }
}

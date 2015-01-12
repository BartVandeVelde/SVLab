using LogViewer.Model;
using Microsoft.Practices.Prism.Mvvm;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace LogViewer.ViewModels
{
    public class LogViewerViewModel : BindableBase
    {
        private ObservableCollection<ILogMessage> messages;
        private ObservableCollection<LogViewerModel> logs;
        private LogViewerModel lastEntry;

        public LogViewerViewModel(ILogViewerLogger logger)
        {
            logs = new ObservableCollection<LogViewerModel>();

            messages = logger.Messages;
            messages.CollectionChanged += messages_CollectionChanged;

            foreach (ILogMessage message in messages)
            {
                LogViewerModel log = new LogViewerModel(message.Message, message.Category, message.Priority, message.Timestamp);
                logs.Add(log);
            }

            this.lastEntry = logs.Last();
        }

        void messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if( e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(ILogMessage message in e.NewItems )
                {
                    LogViewerModel log = new LogViewerModel(message.Message, message.Category, message.Priority, message.Timestamp);
                    logs.Add(log);
                }

                this.lastEntry = logs.Last();
            }
        }

        public ObservableCollection<LogViewerModel> Logs
        {
            get
            {
                return logs;
            }
            private set
            {
                SetProperty(ref this.logs, value);
            }
        }

        public LogViewerModel LastEntry
        {
            get
            {
                return this.lastEntry;
            }
            private set
            {
                SetProperty(ref this.lastEntry, value);
            }
        }
    }
}
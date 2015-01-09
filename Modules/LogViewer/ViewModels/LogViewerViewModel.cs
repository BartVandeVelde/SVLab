using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogViewer.ViewModels
{
    public class LogViewerViewModel : BindableBase
    {
        private ObservableCollection<string> logs; 

        public LogViewerViewModel()
        {
            logs = new ObservableCollection<string>();
            logs.Add("TEST1");
            logs.Add("TEST2");
            logs.Add("TEST3");
        }

        public ObservableCollection<string> Logs
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
    }
}
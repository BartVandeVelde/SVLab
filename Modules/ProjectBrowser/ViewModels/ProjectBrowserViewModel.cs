using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Mvvm;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectBrowser.ViewModels
{
    public class ProjectBrowserViewModel : BindableBase
    {
        private ObservableCollection<ProjectViewModel> projects;
        private readonly ILoggerFacade logger = null;

        public ProjectBrowserViewModel(IEntityService entityService, ILoggerFacade logger)
        {
            this.projects = new ObservableCollection<ProjectViewModel>((from project in entityService.GetRootProjects() select new ProjectViewModel(project)).ToList());
            this.logger = logger;
        }

        public ObservableCollection<ProjectViewModel> Projects
        {
            get
            {
                return projects;
            }
            private set
            {
                SetProperty(ref this.projects, value);
            }
        }
    }
}
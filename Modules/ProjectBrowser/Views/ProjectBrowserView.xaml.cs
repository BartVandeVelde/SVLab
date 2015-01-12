using ProjectBrowser.ViewModels;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ProjectBrowser.Views
{
    /// <summary>
    /// Interaction logic for ProjectBrowser.xaml
    /// </summary>
    public partial class ProjectBrowserView : UserControl, IView
    {
        public ProjectBrowserView(ProjectBrowserViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        #region IView
        public string Caption
        {
            get { return "Project Browser"; }
        }

        public string RegionName
        {
            get { return "ProjectBrowser"; }
        }

        public string ViewName
        {
            get { return "ProjectBrowserView"; }
        }
        #endregion IView
    }
}
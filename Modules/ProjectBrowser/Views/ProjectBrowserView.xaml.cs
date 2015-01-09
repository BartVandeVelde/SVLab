using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
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
    public partial class ProjectBrowserView : UserControl, IPanelInfo, IView
    {
        private readonly IRegionManager regionManager = null;

        public ProjectBrowserView(ProjectBrowserViewModel viewModel, IRegionManager regionManager)
        {
            InitializeComponent();

            DataContext = viewModel;
            this.regionManager = regionManager;
        }

        public string GetPanelCaption()
        {
            return "Project Browser";
        }
    }
}
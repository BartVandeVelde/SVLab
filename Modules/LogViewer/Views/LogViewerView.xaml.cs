using LogViewer.ViewModels;
using SVLab.UI.Infrastructure.Interfaces;
using System.Windows.Controls;

namespace LogViewer.Views
{
    /// <summary>
    /// Interaction logic for LogViewerView.xaml
    /// </summary>
    public partial class LogViewerView : UserControl, IView
    {
        public LogViewerView(LogViewerViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        #region IView
        public string Caption
        {
            get { return "Log"; }
        }

        public string RegionName
        {
            get { return "LogViewer"; }
        }

        public string ViewName
        {
            get { return "LogViewerView"; }
        }
        #endregion IView
    }
}
using Microsoft.Practices.Prism.Mvvm;
using SVLab.UI.Infrastructure.Interfaces;
using System.Windows.Controls;

namespace LogViewer.Views
{
    /// <summary>
    /// Interaction logic for LogViewerView.xaml
    /// </summary>
    public partial class LogViewerView : UserControl, IPanelInfo, IView
    {
        public LogViewerView()
        {
            InitializeComponent();
        }

        public string GetPanelCaption()
        {
            return "Log";
        }
    }
}

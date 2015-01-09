using DevExpress.Xpf.Core;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace SVLab.UI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : DXWindow
    {
        private readonly IUnityContainer container = null;
        private readonly IRegionManager regionManager = null;
        private readonly IEventAggregator eventAggregator = null;

        public Shell(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            InitializeComponent();
        }
    }
}
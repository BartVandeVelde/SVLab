using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Specialized;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class LayoutPanelAdapter : RegionAdapterBase<LayoutPanel>
    {
        private readonly ILoggerFacade logger = null;

        public LayoutPanelAdapter(IRegionBehaviorFactory behaviorFactory, ILoggerFacade logger)
            : base(behaviorFactory)
        {
            this.logger = logger;
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
        }

        void OnViewsCollectionChanged(IRegion region, LayoutPanel regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                regionTarget.Content = e.NewItems[0];
                regionTarget.Visibility = System.Windows.Visibility.Visible;

                DockLayoutManager dockLayoutManager = regionTarget.GetDockLayoutManager();
                if (dockLayoutManager.ClosedPanels.Contains(regionTarget))
                {
                    dockLayoutManager.DockController.Restore(regionTarget);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                regionTarget.Content = null;
                regionTarget.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
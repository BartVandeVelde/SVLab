﻿using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class DockManagerAdapter : RegionAdapterBase<DockLayoutManager>
    {
        private readonly ILoggerFacade logger = null;

        public DockManagerAdapter(IRegionBehaviorFactory behaviorFactory, ILoggerFacade logger) :
            base(behaviorFactory)
        {
            this.logger = logger;
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }

        protected override void Adapt(IRegion region, DockLayoutManager regionTarget)
        {
            BaseLayoutItem[] items = regionTarget.GetItems();

            foreach (BaseLayoutItem item in items)
            {
                string regionName = RegionManager.GetRegionName(item);

                if (!String.IsNullOrEmpty(regionName))
                {
                    LayoutPanel panel = item as LayoutPanel;

                    if (panel != null && panel.Content == null)
                    {
                        ContentControl control = new ContentControl();
                        RegionManager.SetRegionName(control, regionName);
                        panel.Content = control;
                    }
                }
            }

            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
            regionTarget.DockOperationCompleted += (s, e) => OnDockOperationCompleted(region, regionTarget, s, e);
        }

        void OnViewsCollectionChanged(IRegion region, DockLayoutManager regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            this.logger.Log("DockManagerAdapter: OnViewsCollectionChanged called.", Category.Debug, Priority.None);
        }

        void OnDockOperationCompleted(IRegion region, DockLayoutManager regionTarget, object sender, DockOperationCompletedEventArgs e)
        {
            if (e.DockOperation == DockOperation.Close && e.Item.GetType() == typeof(LayoutPanel))
            {
                IView view = ((LayoutPanel)e.Item).Content as IView;

                if (view != null)
                {
                    if (region.RegionManager.Regions.ContainsRegionWithName(view.RegionName) && region.RegionManager.Regions[view.RegionName].Views.Contains(view))
                    {
                        region.RegionManager.Regions[view.RegionName].Remove(view);
                        logger.Log(String.Format("Removed view '{0}' from '{1}'", view.ViewName, view.RegionName), Category.Debug, Priority.None);
                    }
                }
            }
        }
    }
}
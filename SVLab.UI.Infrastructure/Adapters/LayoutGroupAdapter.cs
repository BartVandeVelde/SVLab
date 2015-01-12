using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using SVLab.UI.Infrastructure.Interfaces;
using System.Collections.Specialized;
using System;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class LayoutGroupAdapter : RegionAdapterBase<LayoutGroup>
    {
        private readonly ILoggerFacade logger = null;

        public LayoutGroupAdapter(IRegionBehaviorFactory behaviorFactory, ILoggerFacade logger)
            : base(behaviorFactory)
        {
            this.logger = logger;
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, LayoutGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
        }

        void OnViewsCollectionChanged(IRegion region, LayoutGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var panel = new LayoutPanel();
                    IView viewInfo = view as IView;

                    if (viewInfo != null)
                    {
                        panel.Caption = viewInfo.Caption;

                        region.Remove(view);
                        RegionManager.SetRegionName(panel, viewInfo.RegionName);
                        RegionManager.SetRegionManager(panel, region.RegionManager);
                        region.RegionManager.RegisterViewWithRegion(viewInfo.RegionName, () => view);
                    }
                    else
                    {
                        panel.Caption = "";
                    }

                    regionTarget.Add(panel);
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var view in e.OldItems)
                {
                    LayoutPanel viewPanel = null;

                    foreach (LayoutPanel panel in regionTarget.Items)
                    {
                        if (panel.Content == view)
                        {
                            viewPanel = panel;
                            break;
                        }
                    }

                    if (viewPanel == null) continue;
                    viewPanel.Content = null;

                    regionTarget.Remove(viewPanel);
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }
    }
}
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using SVLab.UI.Infrastructure.Interfaces;
using System.Collections.Specialized;
using System;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class FloatGroupAdapter : RegionAdapterBase<FloatGroup>
    {
        private readonly ILoggerFacade logger = null;

        public FloatGroupAdapter(IRegionBehaviorFactory behaviorFactory, ILoggerFacade logger)
            : base(behaviorFactory)
        {
            this.logger = logger;
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, FloatGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => OnViewsCollectionChanged(region, regionTarget, s, e);
            regionTarget.Items.CollectionChanged += (s, e) => OnItemsCollectionChanged(region, regionTarget, s, e);
        }

        bool _lockItemsChanged;
        bool _lockViewsChanged;

        void OnItemsCollectionChanged(IRegion region, FloatGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_lockItemsChanged)
                return;

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                _lockViewsChanged = true;
                var lp = (LayoutPanel)e.OldItems[0];
                var view = lp.Content;

                if (region.Views.Contains(view))
                {
                    region.Remove(view);
                    this.logger.Log(String.Format("FloatGroupAdapter: View '{0}' removed from region '{1}'", view, region.Name), Category.Debug, Priority.None);
                }
                else
                {
                    this.logger.Log(String.Format("FloatGroupAdapter: Could not remove view '{0}' from region '{1}'", view, region.Name), Category.Exception, Priority.Medium);
                }

                _lockViewsChanged = false;
            }
        }

        void OnViewsCollectionChanged(IRegion region, FloatGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_lockViewsChanged)
                return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var panel = new LayoutPanel { Content = view };
                    IPanelInfo panelInfo = view as IPanelInfo;

                    if (panelInfo != null)
                    {
                        panel.Caption = panelInfo.GetPanelCaption();
                    }
                    else
                    {
                        panel.Caption = "";
                    }

                    _lockItemsChanged = true;
                    regionTarget.Items.Add(panel);
                    this.logger.Log(String.Format("FloatGroupAdapter: View '{0}' added to region '{1}'", view, region.Name), Category.Debug, Priority.None);
                    _lockItemsChanged = false;

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
                    _lockItemsChanged = true;
                    regionTarget.Items.Remove(viewPanel);
                    this.logger.Log(String.Format("FloatGroupAdapter: View '{0}' removed from region '{1}'", view, region.Name), Category.Debug, Priority.None);
                    _lockItemsChanged = false;
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }
    }
}
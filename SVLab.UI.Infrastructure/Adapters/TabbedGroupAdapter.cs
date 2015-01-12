using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using SVLab.UI.Infrastructure.Interfaces;
using System.Collections.Specialized;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class TabbedGroupAdapter : RegionAdapterBase<TabbedGroup>
    {
        public TabbedGroupAdapter(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, TabbedGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => { OnViewsCollectionChanged(region, regionTarget, s, e); };
        }

        void OnViewsCollectionChanged(IRegion region, TabbedGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object view in e.NewItems)
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
        }
    }
}
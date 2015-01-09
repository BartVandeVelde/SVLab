using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
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
                    LayoutPanel panel = new LayoutPanel() { Content = view, Caption = "" };

                    regionTarget.Items.Add(panel);
                    regionTarget.SelectedTabIndex = regionTarget.Items.Count - 1;
                }
            }
        }
    }
}
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System;

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
            region.Views.CollectionChanged += (d, e) =>
            {
                if (e.NewItems != null)
                {
                    regionTarget.Content = e.NewItems[0];
                    this.logger.Log(String.Format("LayoutPanelAdapter: View '{0}' added to region '{1}'", e.NewItems[0], region.Name), Category.Debug, Priority.None);
                }
            };
        }
    }
}
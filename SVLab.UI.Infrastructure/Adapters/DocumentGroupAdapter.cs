using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using SVLab.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Specialized;

namespace SVLab.UI.Infrastructure.Adapters
{
    public class DocumentGroupAdapter : RegionAdapterBase<DocumentGroup>
    {
        private readonly ILoggerFacade logger = null;

        public DocumentGroupAdapter(IRegionBehaviorFactory behaviorFactory, ILoggerFacade logger)
            : base(behaviorFactory)
        {
            this.logger = logger;
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, DocumentGroup regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => { OnViewsCollectionChanged(region, regionTarget, s, e); };
        }

        void OnViewsCollectionChanged(IRegion region, DocumentGroup regionTarget, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object view in e.NewItems)
                {
                    DockLayoutManager manager = regionTarget.GetDockLayoutManager();
                    DocumentPanel panel = manager.DockController.AddDocumentPanel(regionTarget);
                    
                    IPanelInfo panelInfo = panel.Content as IPanelInfo;

                    if (panelInfo != null)
                    {
                        panel.Caption = panelInfo.GetPanelCaption();
                    }
                    else
                    {
                        panel.Caption = "";
                    }

                    manager.DockController.Activate(panel);

                    this.logger.Log(String.Format("DocumentGroupAdapter: View '{0}' added to region '{1}'", view, region.Name), Category.Debug, Priority.None);
                }
            }
        }
    }
}
using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using System.Windows;

namespace SVLab.UI.Infrastructure.RegionBehaviors
{
	public class TabbedGroupRegionBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
		public IRegionManager RegionManager { get; set; }
		public DependencyObject HostControl { get; set; }
        public static string BehaviorKey
        {
            get { return "TabbedGroupRegionBehavior"; }
        }

        protected override void OnAttach()
        {
			RegisterRegion();
		}

	    void RegisterRegion()
        {
			DependencyObject targetElement = HostControl;
			
            if(targetElement.CheckAccess())
            {
				TabbedGroup tg = targetElement as TabbedGroup;

                if(tg != null && RegionManager != null)
                {
                    RegionManager.Regions.Add(Region);
                }
			}
		}
	}
}
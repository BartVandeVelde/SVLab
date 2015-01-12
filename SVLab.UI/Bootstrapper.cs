using DevExpress.Xpf.Docking;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using SVLab.UI.Infrastructure.Adapters;
using SVLab.UI.Infrastructure.Interfaces;
using SVLab.UI.Infrastructure.Logging;
using SVLab.UI.Infrastructure.Menubar;
using SVLab.UI.Infrastructure.RegionBehaviors;
using SVLab.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SVLab.UI
{
    public class Bootstrapper : UnityBootstrapper
    {
        //private readonly EnterpriseLibraryLogger logger = new EnterpriseLibraryLogger();
        private readonly LogViewerLogger logger = new LogViewerLogger();

        protected override ILoggerFacade CreateLogger()
        {
            return logger;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            //return new ModuleService(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Modules"));
            return new DirectoryModuleCatalog() { ModulePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Modules") };
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(DockLayoutManager), Container.Resolve<DockManagerAdapter>());
            mappings.RegisterMapping(typeof(DocumentGroup), Container.Resolve<DocumentGroupAdapter>());
            mappings.RegisterMapping(typeof(LayoutGroup), Container.Resolve<LayoutGroupAdapter>());
            mappings.RegisterMapping(typeof(LayoutPanel), Container.Resolve<LayoutPanelAdapter>());
            mappings.RegisterMapping(typeof(TabbedGroup), Container.Resolve<TabbedGroupAdapter>());

            return mappings;
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            IRegionBehaviorFactory behaviors = base.ConfigureDefaultRegionBehaviors();

            behaviors.AddIfMissing(TabbedGroupRegionBehavior.BehaviorKey, typeof(TabbedGroupRegionBehavior));

            return behaviors;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IMenuService, MenuService>();
            Container.RegisterInstance(typeof(ILogViewerLogger), logger);
        }

        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();
            Container.RegisterInstance(shell);
            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }
    }
}

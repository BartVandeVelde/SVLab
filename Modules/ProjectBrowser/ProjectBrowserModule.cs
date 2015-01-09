using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using ProjectBrowser.Services;
using ProjectBrowser.ViewModels;
using ProjectBrowser.Views;
using SVLab.UI.Infrastructure.Constants;
using SVLab.UI.Infrastructure.Interfaces;
using SVLab.UI.Infrastructure.Menubar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBrowser
{
    [Module(ModuleName = "ProjectBrowser", OnDemand = false)]
    public class ProjectBrowserModule : IModule
    {
        private readonly IUnityContainer container = null;
        private readonly IRegionManager regionManager = null;
        private readonly IEventAggregator eventAggregator = null;
        private readonly IMenuService menuService = null;
        private readonly ILoggerFacade logger = null;

        private DelegateCommand cmdOpenProjectBrowserModule;
        private DelegateCommand cmdLogCurrentRegionName;

        public ProjectBrowserModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, IMenuService menuService, ILoggerFacade logger)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.menuService = menuService;
            this.logger = logger;
        }

        #region IModule implementation
        public void Initialize()
        {
            RegisterTypes();
            SetupCommands();
            SetupModuleMenu();
            RegisterViewWithRegion();
        }
        #endregion

        #region Register
        private void RegisterTypes()
        {
            this.container.RegisterType<ProjectBrowserView>();
            this.container.RegisterType<ProjectBrowserViewModel>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<IEntityService, EntityService>(new ContainerControlledLifetimeManager());
        }

        private void RegisterViewWithRegion()
        {
            // View discovery
            //this.regionManager.RegisterViewWithRegion(RegionNames.LeftRegion, typeof(ProjectBrowserView));

            // View injection
            //this.regionManager.RequestNavigate(RegionNames.LeftRegion, );
            // or
            this.regionManager.Regions[RegionNames.LeftRegion].Add(this.container.Resolve<ProjectBrowserView>());
        }
        #endregion

        #region Setup
        private void SetupCommands()
        {
            logger.Log(String.Format("SetupCommands voor '{0}'", this.GetType()), Category.Debug, Priority.None);

            cmdOpenProjectBrowserModule = new DelegateCommand(OpenProjectBrowserModule, CanOpenProjectBrowserModule);
            cmdLogCurrentRegionName = new DelegateCommand(LogCurrentRegionName, CanLogCurrentRegionName);
        }

        private void SetupModuleMenu()
        {
            logger.Log(String.Format("SetupModuleMenu voor '{0}'", this.GetType()), Category.Debug, Priority.None);

            menuService.Add(new MenuItem { Parent = "View", Title = "Project Browser", Command = cmdOpenProjectBrowserModule });
            menuService.Add(new MenuItem { Parent = "Project", Title = "Log current region", Command = cmdLogCurrentRegionName  });
        }
        #endregion

        #region Commands
        private bool CanOpenProjectBrowserModule()
        {
            return true;
        }

        private void OpenProjectBrowserModule()
        {
            RegisterViewWithRegion();
        }

        private bool CanLogCurrentRegionName()
        {
            return this.regionManager != null && this.logger != null;
        }

        private void LogCurrentRegionName()
        {
            bool found = false;

            foreach( IRegion region in this.regionManager.Regions)
            {
                if( region.Views.Any(v => v.GetType() == typeof(ProjectBrowserView)))
                {
                    found = true;
                    this.logger.Log(String.Format("ProjectBrowserView is currently hosted in the region '{0}'", region.Name), Category.Debug, Priority.None);
                }
            }

            if( !found)
            {
                this.logger.Log("The current region for ProjectBrowserView could not be found.", Category.Debug, Priority.None);
            }
        }
        #endregion
    }
}
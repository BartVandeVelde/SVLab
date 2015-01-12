using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using LogViewer.ViewModels;
using LogViewer.Views;
using SVLab.UI.Infrastructure.Constants;
using SVLab.UI.Infrastructure.Menubar;
using System;
using System.Collections.Generic;
using System.Linq;
using SVLab.UI.Infrastructure.Interfaces;

namespace LogViewer
{
    [Module(ModuleName = "LogViewer", OnDemand = false)]
    public class LogViewerModule : IModule
    {
        private readonly IUnityContainer container = null;
        private readonly IRegionManager regionManager = null;
        private readonly IEventAggregator eventAggregator = null;
        private readonly IMenuService menuService = null;
        private readonly ILoggerFacade logger = null;

        private DelegateCommand cmdOpenLogViewerModule;

        private LogViewerView view;

        public LogViewerModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, IMenuService menuService, ILoggerFacade logger)
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
            this.container.RegisterType<LogViewerView>();
            this.container.RegisterType<LogViewerViewModel>();

            this.view = this.container.Resolve<LogViewerView>();
        }

        private void RegisterViewWithRegion()
        {
            this.regionManager.Regions[RegionNames.TabRegion].Add(view, ((IView)view).ViewName);
        }

        private void ToggleRegisterView()
        {
            if (this.regionManager.Regions.ContainsRegionWithName(this.view.RegionName))
            {
                if (this.regionManager.Regions[view.RegionName].Views.Contains(this.view))
                {
                    this.regionManager.Regions[view.RegionName].Remove(this.view);
                    logger.Log(String.Format("Removed view '{0}' from '{1}'", view.ViewName, view.RegionName), Category.Debug, Priority.None);
                }
                else
                {
                    this.regionManager.Regions[view.RegionName].Add(this.view);
                    logger.Log(String.Format("Added view '{0}' from '{1}'", view.ViewName, view.RegionName), Category.Debug, Priority.None);
                }
            }
            else
            {
                RegisterViewWithRegion();
                logger.Log(String.Format("Registered view '{0}' with region '{1}'", view.ViewName, view.RegionName), Category.Debug, Priority.None);
            }
        }
        #endregion

        #region Setup
        private void SetupCommands()
        {
            logger.Log(String.Format("SetupCommands voor '{0}'", this.GetType()), Category.Debug, Priority.None);

            cmdOpenLogViewerModule = new DelegateCommand(OpenLogViewerModule, CanOpenLogViewerModule);
        }

        private void SetupModuleMenu()
        {
            logger.Log(String.Format("SetupModuleMenu voor '{0}'", this.GetType()), Category.Debug, Priority.None);

            menuService.Add(new MenuItem { Parent = "View", Title = "Log Viewer", Command = cmdOpenLogViewerModule });
            menuService.Add(new MenuItem { Parent = "Log", Title = "Clear log", Command = null });
        }
        #endregion Setup

        #region Commands
        private bool CanOpenLogViewerModule()
        {
            return true;
        }

        private void OpenLogViewerModule()
        {
            ToggleRegisterView();
        }
        #endregion
    }
}

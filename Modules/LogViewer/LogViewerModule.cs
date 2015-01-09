using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using LogViewer.ViewModels;
using LogViewer.Views;
using SVLab.UI.Infrastructure.Constants;
using SVLab.UI.Infrastructure.Interfaces;
using SVLab.UI.Infrastructure.Menubar;
using System;
using System.Collections.Generic;
using System.Linq;

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
        }

        private void RegisterViewWithRegion()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.RightRegion, typeof(LogViewerView));
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
            RegisterViewWithRegion();
        }
        #endregion
    }
}

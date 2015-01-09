using DevExpress.Xpf.Bars;
using Microsoft.Practices.Prism.Logging;
using SVLab.UI.Infrastructure.Menubar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SVLab.UI.Services
{
    public class MenuService : IMenuService
    {
        private readonly BarManager manager;
        private readonly Bar bar;
        private readonly ILoggerFacade logger = null;

        public MenuService(Shell shell, ILoggerFacade logger)
        {
            this.manager = shell.BarManager;
            this.bar = shell.MainMenu;
            this.logger = logger;
        }

        public void Add(MenuItem item)
        {
            BarSubItem parent = GetParent(item.Parent);
            BarButtonItem button = new BarButtonItem { Content = item.Title, Command = item.Command, Name = "bbi" + Regex.Replace(item.Title, "[^a-zA-Z0-9]", "") };
            manager.Items.Add(button);
            parent.ItemLinks.Add(new BarButtonItemLink { BarItemName = button.Name });

            logger.Log(String.Format("MENU: Added '{0}' to '{1}'", item.Title, parent.Content), Category.Debug, Priority.None);
        }

        BarSubItem GetParent(string parentName)
        {
            foreach (BarItem item in manager.Items)
            {
                BarSubItem button = item as BarSubItem;
                if (button != null && button.Content.ToString() == parentName)
                    return button;
            }
            BarSubItem newParent = new BarSubItem { Content = parentName, Name = "bsi" + Regex.Replace(parentName, "[^a-zA-Z0-9]", "") };
            manager.Items.Add(newParent);
            bar.ItemLinks.Add(new BarSubItemLink { BarItemName = newParent.Name });
            return newParent;
        }
    }
}

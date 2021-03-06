// Copyright 2012 Telerik AD
// 
// This program is free software: you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Windows.Media;
using JustDecompile.API.CompositeEvents;
using JustDecompile.API.Core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Reflexil.Utils;

namespace Reflexil.JustDecompile
{
    internal abstract class MenuItemBase : MenuItem
    {
        public JustDecompileCecilStudioPackage StudioPackage;

        public MenuItemBase(IEventAggregator eventAggregator)
            : base()
        {
            this.StudioPackage = new JustDecompileCecilStudioPackage();

            this.Header = StudioPackage.GetProductVersion();

            this.StudioPackage.NewItemInjected += OnNewItemInjected;

            eventAggregator.GetEvent<SelectedTreeViewItemChangedEvent>().Subscribe(SetReflexilHandler);
        }

        public override void AddMenuItems()
        {
            this.MenuItems.Add(new MenuItem { Header = "Inject class", Command = new DelegateCommand(OnInjectClass) });
            this.MenuItems.Add(new MenuItem { Header = "Inject interface", Command = new DelegateCommand(OnInterfaceClass) });
            this.MenuItems.Add(new MenuItem { Header = "Inject struct", Command = new DelegateCommand(OnStructClass) });
            this.MenuItems.Add(new MenuItem { Header = "Inject enum", Command = new DelegateCommand(OnEnumClass) });
        }

        public void AddRenameDeleteNodes()
        {
            this.MenuItems.Add(new MenuItem { Header = "Rename...", Command = new DelegateCommand(OnRename) });
            this.MenuItems.Add(new MenuItem { Header = "Delete", Command = new DelegateCommand(OnDelete) });
        }

        private void OnDelete()
        {
            StudioPackage.Delete();
        }

        private void OnEnumClass()
        {
            StudioPackage.Inject(EInjectType.Enum);
        }

        private void OnInjectClass()
        {
            StudioPackage.Inject(EInjectType.Class);
        }

        private void OnInterfaceClass()
        {
            StudioPackage.Inject(EInjectType.Interface);
        }

        private void OnRename()
        {
            StudioPackage.Rename();
        }

        private void OnStructClass()
        {
            StudioPackage.Inject(EInjectType.Struct);
        }

        private void SetReflexilHandler(ITreeViewItem selectedTreeItem)
        {
            if (selectedTreeItem != null)
            {
                this.StudioPackage.SelectedTreeViewItem = selectedTreeItem;
            }
        }

        private void OnNewItemInjected(object sender, EventArgs e)
        {
            JustDecompileCecilStudioPackage.UpdatedItems.Add(StudioPackage.SelectedTreeViewItem);

            StudioPackage.SelectedTreeViewItem.TreeNodeVisuals.SetForeground(new SolidColorBrush(Colors.Red));
        }
    }
}
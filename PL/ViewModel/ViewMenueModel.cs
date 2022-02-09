using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewMenueModel : ViewModelBase
    {
        public string ButtonA_Content { get { return "Drones List"; } }
        public string ButtonB_Content { get { return "Parcels List"; } }
        public string ButtonC_Content { get{ return "Customers List"; } }
        public string ButtonD_Content { get{ return "Stations List"; } }
        public ICommand ButtonA_Command
        {
            get
            {
                new TextBlock() { Text = "Drones List" };
                new Button() { Content = "x", Command = CloseCd };
                TabItem newTab = new TabItem();
                newTab.Header = new DockPanel() {DataContext= "{}" };
                newTab.Content = new View.ViewListDrone(AddTab, RemoveTab);
                return new DelegateCommand((o) =>
                {
                    AddTab(newTab);
                });
            }
        }
        public ICommand ButtonB_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Parcels List";
                newTab.Content = new View.ViewParcelList(AddTab, RemoveTab);
                return new DelegateCommand((o) =>
                {
                    AddTab(newTab);
                });
            }
        }
        public ICommand ButtonC_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Customers List";
                newTab.Content = new View.VIewCustomerList(AddTab, RemoveTab);
                return new DelegateCommand((o) =>
                {
                    AddTab(newTab);
                });
            }
        }

        public ICommand ButtonD_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Stations List";
                newTab.Content = new View.ViewStationList(AddTab, RemoveTab);
                return new DelegateCommand((o) =>
                {
                    AddTab(newTab);
                });
            }
        }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    Close();
                });
            }
        }


        public ViewMenueModel(Action<object> addTab, Action<string> removeTab, Action close)
        {
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = close;
        }
    }
}

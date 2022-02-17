using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace PL.ViewModel
{
    class ViewMenueModelCompany : ViewMenueModel
    {
        public ICommand ButtonA_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Drones List";
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

        public ViewMenueModelCompany(Action<object> addTab, Action<object> removeTab, Action close)
        {
            baseModel = new Model.MenueModel();
            baseModel.ButtonA_Content = "Drones List";
            baseModel.ButtonB_Content = "Parcels List";
            baseModel.ButtonC_Content = "Customers List";
            baseModel.ButtonD_Content = "Stations List";
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = close;
        }
    }
}

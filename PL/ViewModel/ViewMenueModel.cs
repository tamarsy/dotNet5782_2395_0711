using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace PL.ViewModel
{
    abstract class ViewMenueModel : ViewModelBase
    {
        public Model.MenueModel baseModel;
        public string ButtonA_Content { get { return baseModel.ButtonA_Content; } }
        public string ButtonB_Content { get { return baseModel.ButtonB_Content; } }
        public string ButtonC_Content { get{ return baseModel.ButtonC_Content; } }
        public string ButtonD_Content { get{ return baseModel.ButtonD_Content; } }
        public int SelectedTab { get { return baseModel.selectedTab; } set { baseModel.selectedTab = value; OnPropertyChange("SelectedTab"); } }
        //public ICommand ButtonA_Command
        //{
        //    get
        //    {
        //        TabItem newTab = new TabItem();
        //        newTab.Header = "Drones List";
        //        newTab.Content = new View.ViewListDrone(AddTab, RemoveTab);
        //        return new DelegateCommand((o) =>
        //        {
        //            AddTab(baseModel.ButtonA_Command(););
        //        });
        //    }
        //}
        //public ICommand ButtonB_Command
        //{
        //    get
        //    {
        //        TabItem newTab = new TabItem();
        //        newTab.Header = "Parcels List";
        //        newTab.Content = new View.ViewParcelList(AddTab, RemoveTab);
        //        return new DelegateCommand((o) =>
        //        {
        //            AddTab(newTab);
        //        });
        //    }
        //}
        //public ICommand ButtonC_Command
        //{
        //    get
        //    {
        //        TabItem newTab = new TabItem();
        //        newTab.Header = "Customers List";
        //        newTab.Content = new View.VIewCustomerList(AddTab, RemoveTab);
        //        return new DelegateCommand((o) =>
        //        {
        //            AddTab(newTab);
        //        });
        //    }
        //}

        //public ICommand ButtonD_Command
        //{
        //    get
        //    {
        //        TabItem newTab = new TabItem();
        //        newTab.Header = "Stations List";
        //        newTab.Content = new View.ViewStationList(AddTab, RemoveTab);
        //        return new DelegateCommand((o) =>
        //        {
        //            AddTab(newTab);
        //        });
        //    }
        //}

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
    }
}

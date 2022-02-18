using BO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewMenueModelCustomer : ViewMenueModel
    {
        public int Id { get; set; }
        public ICommand ButtonA_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Update your details";
                newTab.Content = new View.ViewCustomer(Id, close: () => RemoveTab("Update your details"), AddTab, RemoveTab, UpdateWindows);
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
                newTab.Header = $"Parcels to {BLApi.FactoryBL.GetBL().GetCustomer(Id).Name}";
                newTab.Content = new View.ViewParcelList(AddTab, RemoveTab, UpdateWindows, (ParcelToList p) => p.GetterId.Equals(Id), (string)newTab.Header);
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
                newTab.Header = $"Parcels From {BLApi.FactoryBL.GetBL().GetCustomer(Id).Name}";
                newTab.Content = new View.ViewParcelList(AddTab, RemoveTab, UpdateWindows, (ParcelToList p) => p.SenderId.Equals(Id), (string)newTab.Header);
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
                newTab.Header = "Add parcel";
                newTab.Content = new View.ViewParcel(() => {UpdateWindows(); RemoveTab(newTab.Header); }, Id);
                return new DelegateCommand((o) =>
                {
                    AddTab(newTab);
                });
            }
        }

        public ViewMenueModelCustomer(Action<object> addTab, Action<object> removeTab, int customerId, Action close) : base()
        {
            baseModel = new Model.MenueModel();
            baseModel.ButtonA_Content = "Update your details";
            baseModel.ButtonB_Content = "gets parcels";
            baseModel.ButtonC_Content = "sends parcels";
            baseModel.ButtonD_Content = "Add parcel";
            AddTab = addTab;
            RemoveTab = removeTab;
            Id = customerId;
            Close = close;
        }
    }
}

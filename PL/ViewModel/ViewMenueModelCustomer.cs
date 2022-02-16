using BO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewMenueModelCustomer : ViewModelBase
    {
        public int Id { get; set; }
        public string ButtonA_Content { get { return "Update your details"; } }
        public string ButtonB_Content { get { return "gets parcels"; } }
        public string ButtonC_Content { get { return "sends parcels"; } }
        public string ButtonD_Content { get { return "Add parcel"; } }
        public ICommand ButtonA_Command
        {
            get
            {
                TabItem newTab = new TabItem();
                newTab.Header = "Update your details";
                newTab.Content = new View.ViewCustomer(Id, close:()=>RemoveTab("Update your details"), AddTab, RemoveTab);
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
                newTab.Content = new View.ViewParcelList(AddTab, RemoveTab, (ParcelToList p) => p.GetterId.Equals(Id), (string)newTab.Header);
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
                newTab.Content = new View.ViewParcelList(AddTab, RemoveTab, (ParcelToList p) => p.SenderId.Equals(Id), (string)newTab.Header);
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
                newTab.Content = new View.ViewParcel(() => RemoveTab("Add parcel"), Id);
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

        public ViewMenueModelCustomer(Action<object> addTab, Action<object> removeTab, int customerId, Action close)
        {
            AddTab = addTab;
            RemoveTab = removeTab;
            Id = customerId;
            Close = close;
        }
    }
}

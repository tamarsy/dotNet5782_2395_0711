using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewCustomerModel : ViewModelBase
    {
        private Model.CustomerModel customerModel;

        #region Command
        public DelegateCommand UpDateCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    customerModel.UpDateCommand();
                    OnPropertyChange("IsEnableUpdateCommand");
                    if (UpDatePWindow != default)
                        UpDatePWindow();
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

        public void NewViewParcel(int o)
        {
            TabItem tabitem = new TabItem();
            if (o is int PId)
            {
                tabitem.Header = "Parcel: " + PId;
                tabitem.Content = new View.ViewParcel(PId, updatePWindow: () => Details = BLApi.FactoryBL.GetBL().GetCustomer(Id).ToString()
                , close: () => RemoveTab(tabitem.Header), addTab: AddTab, removeTab: RemoveTab);
            }
            AddTab(tabitem);
        }


        #endregion

        #region Enable and Visibility
        public bool IsEnableUpdateCommand
        {
            get
            {
                if (customerModel.IsDetailsPanelVisibility)
                {
                    try
                    {
                        BO.Customer c = BLApi.FactoryBL.GetBL().GetCustomer(customerModel.CustomerId);
                        return customerModel.Name.Length > 1 && customerModel.Phone.Length > 7 && customerModel.Phone.Length < 11 &&
                            (!customerModel.Name.Equals(c.Name) || !customerModel.Phone.Equals(c.Phone));
                    }
                    catch (BO.ObjectNotExistException e) { Details = e.Message; }
                }
                return false;
            }
        }


        public Visibility AddPanelVisibility { get { return customerModel.IsDetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility DetailsPanelVisibility { get { return customerModel.IsDetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }

        #endregion

        public ListCollectionView ParcelsFrom { get => customerModel.ParcelsFrom; set { customerModel.ParcelsFrom = value; OnPropertyChange("ParcelsFrom"); } }
        public ListCollectionView ParcelsTo { get => customerModel.ParcelsTo; set { customerModel.ParcelsTo = value; OnPropertyChange("ParcelsTo"); } }

        public string Details
        {
            get { return customerModel.Details; }
            set
            {
                if (OnlyUpdatesCange(value))
                {
                    customerModel.Details = value;
                    OnPropertyChange("Details");
                    OnPropertyChange("IsEnableUpdateCommand");
                }
            }
        }

        private bool OnlyUpdatesCange(string str)
        {
            int pIndex = customerModel.IStartPhone, nIndex = customerModel.IStartName, nameLength, phonLength;
            string subDetails = Details.Remove(nIndex, customerModel.Name.Length)
                .Remove(pIndex - customerModel.Name.Length, customerModel.Phone.Length);
            if (customerModel.IStartPhone > customerModel.IStartName)
            {
                pIndex = str.IndexOf("Phone: ") + "Phone: ".Length;
                nameLength = customerModel.Name.Length + pIndex - customerModel.IStartPhone;
                phonLength = str.Length - subDetails.Length - nameLength;
            }
            else
            {
                nIndex = str.IndexOf("Name: ") + "Name: ".Length;
                phonLength = customerModel.Phone.Length + nIndex - customerModel.IStartName;
                nameLength = str.Length - subDetails.Length - phonLength;
            }
            if (nIndex > -1 && pIndex > -1 && phonLength > -1 && nameLength > -1)
            {

                if (nameLength < 40 && phonLength < 11 && subDetails.Equals(str.Remove(nIndex, nameLength)
                        .Remove(pIndex - nameLength, phonLength)))
                {
                    customerModel.Name = str.Substring(nIndex, nameLength);
                    customerModel.Phone = str.Substring(pIndex, phonLength);
                    customerModel.IStartName = nIndex;
                    customerModel.IStartPhone = pIndex;
                    return true;
                }
            }
            return false;
        }

        private void InitializeData(int customerId)
        {
            BO.Customer c = BLApi.FactoryBL.GetBL().GetCustomer(customerId);
            customerModel.CustomerId = customerId;
            customerModel.Name = c.Name;
            customerModel.Phone = c.Phone;
            customerModel.Details = c.ToString();
            customerModel.IStartPhone = Details.IndexOf("Phone: ") + "Phone: ".Length;
            customerModel.IStartName = Details.IndexOf("Name") + "Name: ".Length;
            customerModel.Phone = c.Phone;
            customerModel.Name = c.Name;
            ParcelsFrom = new ListCollectionView(BLApi.FactoryBL.GetBL().GetCustomer(Id).FromCustomer);
            ParcelsTo = new ListCollectionView(BLApi.FactoryBL.GetBL().GetCustomer(Id).ToCustomer);
        }

        public ViewCustomerModel(int customerId, Action upDatePList, Action close, Action<object> addTab, Action<object> removeTab)
        {
            customerModel = new Model.CustomerModel();
            Close = close;
            AddTab = addTab;
            RemoveTab = removeTab;
            UpDatePWindow = upDatePList;
            customerModel.IsDetailsPanelVisibility = true;
            updateCurrentWindow = () => InitializeData(customerId);
            updateCurrentWindow();
        }
    }
}

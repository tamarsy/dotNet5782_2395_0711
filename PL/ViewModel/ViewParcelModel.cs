using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewParcelModel : ViewModelBase
    {
        private Model.ParcelModel parcelModel;

        #region Commands

        /// <summary>
        /// Close window 
        /// </summary>
        public ICommand CloseCd
        {
            get => new DelegateCommand((o) =>
            {
                Close();
            });
        }

        /// <summary>
        /// Open customer window
        /// </summary>
        public ICommand ViewCustomer
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    if (o is string i)
                    {
                        BO.DeliveryCustomer customer = i.Equals("0") ? BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).GetterId :
                        BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).SenderId;
                        TabItem viewCustomer = new TabItem();
                        viewCustomer.Header = "Customer: " + customer.Name;
                        viewCustomer.Content = new View.ViewCustomer(customer.Id, () => RemoveTab(viewCustomer.Header), AddTab, RemoveTab, updateCurrentWindow);
                        AddTab(viewCustomer);
                    }
                });
            }
        }

        /// <summary>
        /// Open drone window
        /// </summary>
        public ICommand DroneDetails
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    int droneId = BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).DroneDelivery.Id;
                    TabItem viewDrone = new TabItem();
                    viewDrone.Header = "Drone: " + droneId;
                    viewDrone.Content = new View.ViewDrone(droneId, updateCurrentWindow, () => RemoveTab(viewDrone.Header), AddTab, RemoveTab);
                    AddTab(viewDrone);
                });
            }
        }

        #endregion Commands

        /// <summary>
        /// parcel details
        /// </summary>
        public string Details { get { return parcelModel.Details; } set { parcelModel.Details = value; } }

        /// <summary>
        /// Customer Details Visibility
        /// </summary>
        public Visibility CustomerVisibility { get; }

        /// <summary>
        /// View Drone Visibility
        /// </summary>
        public Visibility ViewDroneVisibility { get { return BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).DroneDelivery == null? Visibility.Collapsed: Visibility.Visible; } }

        /// <summary>
        /// DeleteCommand
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    parcelModel.Delete();
                    if (UpDatePWindow is not null)
                        UpDatePWindow();
                    Close();
                });
            }
        }

        private void InitializeData(int parcelId)
        {
            parcelModel.Details = BLApi.FactoryBL.GetBL().GetParcel(parcelId).ToString();
            if (BLApi.FactoryBL.GetBL().GetParcel(parcelId).DroneDelivery == default)
                parcelModel.DD_ButtonVisibility = Visibility.Hidden;
        }


        public ViewParcelModel(int parcelId, Action upDateParcelsWindow, Action close, Action<object> addTab, Action<object> removeTab, bool IsCustomerI = true)
        {
            parcelModel = new Model.ParcelModel();
            Close = close;
            AddTab = addTab;
            RemoveTab = removeTab;
            UpDatePWindow = upDateParcelsWindow;
            parcelModel.UpDatePWindow = upDateParcelsWindow;
            parcelModel.ParcelId = parcelId;
            parcelModel.DetailsPanelVisibility = true;
            CustomerVisibility = IsCustomerI ? Visibility.Collapsed : Visibility.Visible;
            updateCurrentWindow = ()=>InitializeData(parcelId);
            updateCurrentWindow();
        }
    }
}

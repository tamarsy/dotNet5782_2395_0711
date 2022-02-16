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
        public string Details { get { return parcelModel.Details; } set { parcelModel.Details = value; } }
        public Visibility CustomerVisibility { get; }
        public Visibility ViewDroneVisibility { get { return BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).DroneDelivery == null? Visibility.Collapsed: Visibility.Visible; } }
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

        public ICommand ViewCustomer
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    if (o is string i)
                    {
                        BO.DeliveryCustomer customer = i.Equals("0")? BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).GetterId:
                        BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).SenderId;
                        TabItem viewCustomer = new TabItem();
                        viewCustomer.Header = "Customer: " + customer.Name;
                        viewCustomer.Content = new View.ViewCustomer(customer.Id, () => RemoveTab(viewCustomer.Header), AddTab, RemoveTab, ()=>InitializeData(parcelModel.ParcelId));
                        AddTab(viewCustomer);
                    }
                });
            }
        }

        public ICommand DroneDetails
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    int droneId = BLApi.FactoryBL.GetBL().GetParcel(parcelModel.ParcelId).DroneDelivery.Id;
                    TabItem viewDrone = new TabItem();
                    viewDrone.Header = "Drone: " + droneId;
                    viewDrone.Content = new View.ViewDrone(droneId, () => InitializeData(parcelModel.ParcelId), ()=>RemoveTab(viewDrone.Header), AddTab,RemoveTab);
                    AddTab(viewDrone);
                });
            }
        }



        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        if (MessageBox.Show($"delete parcel {parcelModel.ParcelId} ?", $"delete {parcelModel.ParcelId}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            BLApi.FactoryBL.GetBL().DeleteParcel(parcelModel.ParcelId);
                            MessageBox.Show("Successfully delete");
                            if (UpDatePWindow != default)
                                UpDatePWindow();
                            Close();
                        }
                    }
                    catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update parcel details:" + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
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
            InitializeData(parcelId);
        }
    }
}

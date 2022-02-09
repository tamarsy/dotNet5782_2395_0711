using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewDroneModel : ViewModelBase
    {
        private Model.DroneModel droneModel;
        public string DroneDetails
        {
            get {return droneModel.DroneStr;}
            set
            {
                if (OnlyModelCange(value))
                {
                    string newModel = value.Substring(droneModel.IStartModel, value.Length - DroneDetails.Length + droneModel.ModelLength);
                    droneModel.DroneStr = value;
                    droneModel.ModelLength = newModel.Length;
                    if (droneModel.DetailsPanelVisibility)
                    {
                        if (newModel.Length > 3 && !newModel.Equals(BLApi.FactoryBL.GetBL().GetDrone(droneModel.DroneId).Model))
                            IsEnablUpdateModel = true;
                        else
                            IsEnablUpdateModel = false;
                    }
                    OnPropertyChange("DroneDetails");
                    OnPropertyChange("IsEnablUpdateModel");
                }
            }
        }


        private bool OnlyModelCange(string str)
        {
            if(str.Length >= DroneDetails.Length - droneModel.ModelLength)
            {
                string start = DroneDetails.Substring(0, droneModel.IStartModel);
                string end = DroneDetails.Substring(droneModel.IStartModel + droneModel.ModelLength);
                int ModelLength = str.Length - DroneDetails.Length + droneModel.ModelLength;

                if (ModelLength < 20 && start.Equals(str.Substring(0, droneModel.IStartModel))
                    && end.Equals(str.Substring(droneModel.IStartModel + ModelLength))
                    )
                    return true;
            }
            return false;
        }

        public Visibility AddPanelVisibility { get { return droneModel.DetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility DetailsPanelVisibility { get { return droneModel.DetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }
        public bool IsEnablUpdateModel { get { return droneModel.IsEnablButton; }
            private set { droneModel.IsEnablButton = value; OnPropertyChange("IsEnablUpdateModel"); } }
        public Visibility DeliveryAndCollectedVisibility
        {
            get { return droneModel.DC_ButtonVisibility; }
            private set { droneModel.DC_ButtonVisibility = value; OnPropertyChange("DeliveryAndCollectedVisibility"); }
        }

        public Visibility ChargeAndSuppliedVisibility
        {
            get { return droneModel.CS_ButtonVisibility; }
            private set { droneModel.CS_ButtonVisibility = value; OnPropertyChange("ChargeAndSuppliedVisibility"); }
        }

        public string ChargeAndSuppliedContext
        {
            get { return droneModel.CS_ButtonContext; }
            private set { droneModel.CS_ButtonContext = value; OnPropertyChange("ChargeAndSuppliedContext"); }
        }
        public string DeliveryAndCollectedContext
        {
            get { return droneModel.DC_ButtonContext; }
            private set { droneModel.DC_ButtonContext = value; OnPropertyChange("DeliveryAndCollectedContext"); }
        }




        private void InitializeData(int droneId)
        {
            try
            {
                BO.Drone currentDrone = BLApi.FactoryBL.GetBL().GetDrone(droneId);
                droneModel.DroneStr = currentDrone.ToString();
                droneModel.IStartModel = droneModel.DroneStr.IndexOf("Model:") + "Model: ".Length;
                droneModel.ModelLength = droneModel.DroneStr.IndexOf("\n", droneModel.IStartModel) - droneModel.IStartModel;
                OnPropertyChange("DroneDetails");
                OnPropertyChange("IsEnablUpdateModel");
                ButtonsContent(currentDrone.DroneStatuses, currentDrone.Parcel == default);
            }
            catch (BO.ObjectNotExistException e) { DroneDetails = "can't view drone " + e.Message; }
        }

        private void ButtonsContent(BO.DroneStatuses droneStatuses, bool IsNoParcel)
        {
            Action currentActionDelivery = default, currentActionCharge = default;
            DeliveryAndCollectedVisibility = Visibility.Visible;
            ChargeAndSuppliedVisibility = Visibility.Visible;
            if (droneStatuses == BO.DroneStatuses.vacant)
            {
                ChargeAndSuppliedContext = "Charge on";
                currentActionCharge = ChargeOn;
                DeliveryAndCollectedContext = "Send to delivery";
                currentActionDelivery = ParcelToDrone;
            }
            else if (droneStatuses == BO.DroneStatuses.sending)
            {
                if (IsNoParcel)
                {
                    DeliveryAndCollectedContext = "collect Parcel";
                    currentActionDelivery = PickParcel;
                    currentActionCharge = default;
                    ChargeAndSuppliedVisibility = Visibility.Collapsed;
                }
                else
                {
                    DeliveryAndCollectedContext = "Parcel delivery";
                    currentActionDelivery = Destination;
                    ChargeAndSuppliedContext = "Parcel details";
                    // up date ??????????????????????????????????????????
                    //currentActionCharge = () => new View.ViewParcel(droneModel.DroneId, () => DroneDetails = DroneDetails);
                }
            }
            else if (droneStatuses == BO.DroneStatuses.maintanance)
            {
                ChargeAndSuppliedContext = "Charge of";
                currentActionCharge = ChargeOf;
                currentActionDelivery = default;
                DeliveryAndCollectedVisibility = Visibility.Collapsed;
            }
            SetButtonsAction(currentActionCharge, currentActionDelivery);
        }


        private void SetButtonsAction(Action currentActionCharge = default, Action currentActionDelivery = default)
        {
            if (currentActionCharge != null)
                ChargeAndSuppliedAction = new DelegateCommand((o) =>
                {
                    try
                    {
                        currentActionCharge();
                        InitializeData(droneModel.DroneId);
                        UpDatePWindow();
                    }
                    catch (BO.NoChangesToUpdateException ex) { MessageBox.Show(ex.Message); }
                    catch (BO.ObjectNotExistException ex) { MessageBox.Show(ex.Message); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                });

            if (currentActionDelivery != null)
                DeliveryAndCollectedAction = new DelegateCommand((o) =>
                {
                    try
                    {
                        currentActionDelivery();
                        InitializeData(droneModel.DroneId);
                        UpDatePWindow();
                    }
                    catch (BO.NoChangesToUpdateException ex) { MessageBox.Show(ex.Message); }
                    catch (BO.ObjectNotExistException ex) { MessageBox.Show(ex.Message); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                });
        }

        public ICommand UpdateModelCommand
        {
            get
            {
                droneModel.UpDateModelComand = new DelegateCommand((o) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().UpdateDrone(droneModel.DroneId, droneModel.DroneStr.Substring(droneModel.IStartModel, droneModel.ModelLength));
                        IsEnablUpdateModel = false;
                        MessageBox.Show("Successfully update drone model");
                        UpDatePWindow();
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("can't update drone model: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
                return droneModel.UpDateModelComand;
            }
        }

        public ICommand ChargeAndSuppliedAction
        {
            get
            {
                return droneModel.CS_Comand;
            }
            set
            {
                droneModel.CS_Comand = (DelegateCommand)value;
                OnPropertyChange("ChargeAndSuppliedAction");
            }
        }

        public ICommand DeliveryAndCollectedAction
        {
            get
            {
                return droneModel.DC_Comand;
            }
            set
            {
                droneModel.DC_Comand = (DelegateCommand)value;
                OnPropertyChange("DeliveryAndCollectedAction");
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

        private void ChargeOn()
        {
            BLApi.FactoryBL.GetBL().ChargeOn(droneModel.DroneId);
            MessageBox.Show("successfully charge on");
        }
        private void ChargeOf()
        {
            BLApi.FactoryBL.GetBL().ChargeOf(droneModel.DroneId);
            MessageBox.Show("successfully charge of");

        }
        private void ParcelToDrone()
        {

            BLApi.FactoryBL.GetBL().ParcelToDrone(droneModel.DroneId);
            MessageBox.Show("successfully connect parcel to drone");

        }
        private void PickParcel()
        {
            BLApi.FactoryBL.GetBL().PickParcel(droneModel.DroneId);
            MessageBox.Show("successfully pick the parcel by the drone");
        }
        private void Destination()
        {
            BLApi.FactoryBL.GetBL().Destination(droneModel.DroneId);
            MessageBox.Show("successfully supplay the parcel by drone");
        }

        public ViewDroneModel(int droneId, Action upDateDronesWindow, Action close)
        {
            Close = close;
            droneModel = new Model.DroneModel();
            droneModel.DroneId = droneId;
            UpDatePWindow = upDateDronesWindow;
            droneModel.DetailsPanelVisibility = true;
            InitializeData(droneId);
        }
    }
}

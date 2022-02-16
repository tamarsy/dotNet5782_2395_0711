using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewDroneModel : ViewModelBase
    {
        private Model.DroneModel droneModel;
        BackgroundWorker worker;

        private void updateDrone() => worker.ReportProgress(0);
        private bool checkStop() => worker.CancellationPending;

        public DelegateCommand AutomaticCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        if (!droneModel.IsAutomatic)
                        {
                            droneModel.IsAutomatic = true;
                            worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
                            worker.DoWork += (sender, args) => BLApi.FactoryBL.GetBL().StartSimulator((int)args.Argument, updateDrone, checkStop);
                            worker.RunWorkerCompleted += (sender, args) => droneModel.IsAutomatic = false;
                            worker.ProgressChanged += (sender, args) => { InitializeData(DroneId); UpDatePWindow(); };
                            worker.RunWorkerAsync(droneModel.DroneId);
                        }
                        else
                        {
                            worker?.CancelAsync();
                            droneModel.IsAutomatic = false;
                        }
                    }
                    catch (BO.ObjectNotExistException e) { MessageBox.Show("failed Automatic:" + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                    OnPropertyChange("AutomaticText");
                    InitializeData(DroneId);
                });
            }
        }
        
        public string AutomaticText { get { return droneModel.IsAutomatic ? "Regular" : "Automatic"; } }

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


        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        if (MessageBox.Show($"delete drone {DroneId} ?", $"delete {DroneId}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            BLApi.FactoryBL.GetBL().DeleteDrone(DroneId);
                            MessageBox.Show("Successfully delete");
                            if (UpDatePWindow != default)
                                UpDatePWindow();
                            Close();
                        }
                    }
                    catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update drone details:" + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
            }
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
                ButtonsContent(currentDrone.DroneStatuses, currentDrone.Parcel == default? default: currentDrone.Parcel.Id, currentDrone.Parcel == default ? default : currentDrone.Parcel.StatusParcel);
            }
            catch (BO.ObjectNotExistException e) { DroneDetails = "can't view drone " + e.Message; }
        }

        private void ButtonsContent(BO.DroneStatuses droneStatuses, int? parcelId, bool StatusParcel)
        {
            Action currentActionDelivery = default, currentActionCharge = default;
            if (!droneModel.IsAutomatic)
            {
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
                    if (!StatusParcel)
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
                        currentActionCharge = () =>
                        {
                            TabItem parcelTab = new TabItem();
                            parcelTab.Header = "Parcel: " + parcelId;
                            parcelTab.Content = new View.ViewParcel((int)parcelId, () => InitializeData(DroneId), () => RemoveTab(parcelTab.Header), AddTab, RemoveTab);
                            AddTab(parcelTab);
                        };
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
            else { DeliveryAndCollectedVisibility = Visibility.Collapsed; ChargeAndSuppliedVisibility = Visibility.Collapsed; }
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

        public ViewDroneModel(int droneId, Action upDateDronesWindow, Action close, Action<object> addTab, Action<object> removeTab)
        {
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = close;
            droneModel = new Model.DroneModel();
            droneModel.DroneId = droneId;
            UpDatePWindow = upDateDronesWindow;
            droneModel.DetailsPanelVisibility = true;
            droneModel.IsAutomatic = false;
            InitializeData(droneId);
        }
    }
}

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

        #region BackgroundWorker

        BackgroundWorker worker;
        private void updateDrone() => worker.ReportProgress(0);
        private bool checkStop() => worker.CancellationPending;

        /// <summary>
        /// start Automatic
        /// </summary>
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

        #endregion BackgroundWorker

        #region Command
        /// <summary>
        /// Delete Command
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    droneModel.DeleteCommand();
                    UpDatePWindow();
                    Close();

                });
            }
        }
        /// <summary>
        /// Update Model Command
        /// </summary>
        public ICommand UpdateModelCommand
        {
            get => new DelegateCommand((o) =>
            {
                droneModel.UpDateModelComand();
                IsEnablUpdateModel = false;
                UpDatePWindow();
            });
        }
        /// <summary>
        /// close window
        /// </summary>
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
        /// <summary>
        /// One button Command
        /// </summary>
        public ICommand ChargeAndSuppliedAction
        {
            get => droneModel.CS_Comand;
            set
            {
                droneModel.CS_Comand = (DelegateCommand)value;
                OnPropertyChange("ChargeAndSuppliedAction");
            }
        }
        /// <summary>
        /// Secound button Command
        /// </summary>
        public ICommand DeliveryAndCollectedAction
        {
            get => droneModel.DC_Comand;
            set
            {
                droneModel.DC_Comand = (DelegateCommand)value;
                OnPropertyChange("DeliveryAndCollectedAction");
            }
        }
        #endregion

        #region Enable and Visibility
        public Visibility AddPanelVisibility { get { return droneModel.DetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility DetailsPanelVisibility { get { return droneModel.DetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }
        public bool IsEnablUpdateModel
        {
            get { return droneModel.IsEnablButton; }
            private set { droneModel.IsEnablButton = value; OnPropertyChange("IsEnablUpdateModel"); }
        }
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
        #endregion

        #region texts
        public string DroneDetails
        {
            get { return droneModel.DroneStr; }
            set
            {
                if (droneModel.OnlyModelChange(value))
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

        #endregion

        #region private functions

        /// <summary>
        /// Initialize Data
        /// </summary>
        /// <param name="droneId">droneId</param>
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
                ButtonsContent(currentDrone.DroneStatuses, currentDrone.Parcel == default ? default : currentDrone.Parcel.Id, currentDrone.Parcel == default ? default : currentDrone.Parcel.StatusParcel);
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
                    currentActionCharge = droneModel.ChargeOn;
                    DeliveryAndCollectedContext = "Send to delivery";
                    currentActionDelivery = droneModel.ParcelToDrone;
                }
                else if (droneStatuses == BO.DroneStatuses.sending)
                {
                    if (!StatusParcel)
                    {
                        DeliveryAndCollectedContext = "collect Parcel";
                        currentActionDelivery = droneModel.PickParcel;
                        currentActionCharge = default;
                        ChargeAndSuppliedVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        DeliveryAndCollectedContext = "Parcel delivery";
                        currentActionDelivery = droneModel.Destination;
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
                    currentActionCharge = droneModel.ChargeOf;
                    currentActionDelivery = default;
                    DeliveryAndCollectedVisibility = Visibility.Collapsed;
                }
                ChargeAndSuppliedAction = ActionToIcomand(currentActionCharge);
                DeliveryAndCollectedAction = ActionToIcomand(currentActionDelivery);
            }
            else { DeliveryAndCollectedVisibility = Visibility.Collapsed; ChargeAndSuppliedVisibility = Visibility.Collapsed; }
        }

        private DelegateCommand ActionToIcomand(Action currentAction)
        {
            return new DelegateCommand((o) =>
            {
                try
                {
                    currentAction();
                }
                catch (BO.NoChangesToUpdateException ex) { MessageBox.Show("No Changes To Update: " + ex.Message); }
                catch (BO.ObjectNotExistException ex) { MessageBox.Show("No Exist: " + ex.Message); }
                catch (BO.ObjectAlreadyExistException ex) { MessageBox.Show("Already Existe: " + ex.Message); }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
                updateCurrentWindow();
                UpDatePWindow();
            });
        }

        #endregion private functions

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
            updateCurrentWindow = () => InitializeData(droneId);
            updateCurrentWindow();
        }
    }
}

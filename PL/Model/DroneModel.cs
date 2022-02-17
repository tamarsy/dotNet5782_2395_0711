using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Model
{
    partial class DroneModel
    {
        #region Actions

        public Action Addcommand
        {
            get => () =>
            {
                try
                {
                    BLApi.FactoryBL.GetBL().AddDrone(DroneId, DroneStr, (BO.WeightCategories)MaxWeightSelector_select, (int)StationSelector.GetValue(StationSelector_select));
                    MessageBox.Show("successfully add");
                }
                catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("Failed add drone: " + e.Message); }
                catch (Exception e) { MessageBox.Show("failed add drone: " + e.Message); }
            };
        }

        public Action ChargeOn
        {
            get => () =>
            {
                BLApi.FactoryBL.GetBL().ChargeOn(DroneId);
                MessageBox.Show("successfully charge on");
            };
        }

        public Action ChargeOf
        {
            get => () =>
            {
                BLApi.FactoryBL.GetBL().ChargeOf(DroneId);
                MessageBox.Show("successfully charge of");

            };
        }

        public Action ParcelToDrone
        {
            get => () =>
            {
                BLApi.FactoryBL.GetBL().ParcelToDrone(DroneId);
                MessageBox.Show("successfully connect parcel to drone");
            };
        }

        public Action PickParcel
        {
            get => () =>
            {
                BLApi.FactoryBL.GetBL().PickParcel(DroneId);
                MessageBox.Show("successfully pick the parcel by the drone");
            };
        }

        public Action Destination
        {
            get => () =>
            {
                BLApi.FactoryBL.GetBL().Destination(DroneId);
                MessageBox.Show("successfully supplay the parcel by drone");
            };
        }

        public Action DeleteCommand
        {
            get => () =>
            {
                try
                {
                    if (MessageBox.Show($"delete drone {DroneId} ?", $"delete {DroneId}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        BLApi.FactoryBL.GetBL().DeleteDrone(DroneId);
                        MessageBox.Show("Successfully delete");
                    }
                }
                catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update drone details:" + e.Message); }
                catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
            };
        }

        /// <summary>
        /// UpDateModel
        /// </summary>
        public Action UpDateModelComand
        {
            get => () =>
            {
                try
                {
                    BLApi.FactoryBL.GetBL().UpdateDrone(DroneId, DroneStr.Substring(IStartModel, ModelLength));
                    MessageBox.Show("Successfully update drone model");
                }
                catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("can't update drone model: " + e.Message); }
                catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
            };
        }

        #endregion Actions
        public Predicate<string> OnlyModelChange
        {
            get => (str) =>
            {
                if (DroneStr != null && str.Length >= DroneStr.Length - ModelLength)
                {
                    string start = DroneStr.Substring(0, IStartModel);
                    string end = DroneStr.Substring(IStartModel + ModelLength);
                    int NewModelLength = str.Length - DroneStr.Length + ModelLength;

                    if (NewModelLength < 20 && start.Equals(str.Substring(0, IStartModel))
                        && end.Equals(str.Substring(IStartModel + NewModelLength))
                        )
                        return true;
                }
                return false;
            };
        }
        public Array StationSelector { set; get; }
        public DelegateCommand DC_Comand { set; get; }
        public DelegateCommand CS_Comand { set; get; }
        public Visibility DC_ButtonVisibility { get; set; }
        public Visibility CS_ButtonVisibility { get; set; }
        public string CS_ButtonContext { get; set; }
        public string DroneStr { get; set; }
        public string DC_ButtonContext { get; set; }
        public int MaxWeightSelector_select { set; get; }
        public int StationSelector_select { set; get; }
        public int IStartModel { set; get; }
        public int ModelLength { set; get; }
        public int DroneId { get; set; }
        public bool IsEnablButton { get; set; }
        public bool IsAutomatic { set; get; }
        public bool DetailsPanelVisibility { get; set; }

    }
}

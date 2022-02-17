using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewDroneModel : ViewModelBase
    {

        #region Selectors

        public Array MaxWeightSelector
        {
            get { return Enum.GetValues(typeof(BO.WeightCategories)); }
        }

        public Array StationSelector { get => droneModel.StationSelector; set { droneModel.StationSelector = value; OnPropertyChange("StationSelector"); } }

        #endregion Selectors


        #region Items select

        public int MaxWeightSelector_select
        {
            get { return droneModel.MaxWeightSelector_select; }
            set { droneModel.MaxWeightSelector_select = value; OnPropertyChange("MaxWeightSelector_select"); }
        }

        public int StationSelector_select
        {
            get { return droneModel.StationSelector_select; }
            set { droneModel.StationSelector_select = value; OnPropertyChange("StationSelector_select"); }
        }
        #endregion  Items select

        public bool IsEnablAddDrone
        {
            get { return droneModel.IsEnablButton; }
            private set { droneModel.IsEnablButton = value; }
        }

        public int DroneId
        {
            get { return droneModel.DroneId; }
            set { droneModel.DroneId = value; }
        }

        /// <summary>
        /// AddCommand
        /// </summary>
        public ICommand AddCommand
        {
            get => new DelegateCommand((o) =>
                {
                    droneModel.Addcommand();
                    UpDatePWindow();
                });
        }

        /// <summary>
        /// ViewDroneModel
        /// </summary>
        /// <param name="updateAndClose">updateAndClose</param>
        public ViewDroneModel(Action updateAndClose)
        {
            droneModel = new Model.DroneModel();
            IsEnablAddDrone = false;
            droneModel.DetailsPanelVisibility = false;
            UpDatePWindow = updateAndClose;
            Close = updateAndClose;
            updateCurrentWindow = ()=> StationSelector = BLApi.FactoryBL.GetBL().StationsList().Select(s => s.Id).ToArray();
            updateCurrentWindow();
        }
    }
}

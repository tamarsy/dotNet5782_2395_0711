using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.ViewModel
{
    partial class ViewStationModel
    {
        /// <summary>
        /// AddCommand
        /// </summary>
        public DelegateCommand AddCommand
        {
            get => new DelegateCommand((o) => { stationModel.Addcommand(); UpDatePWindow(); });
        }

        /// <summary>
        /// true to Enable Add Command
        /// </summary>
        public bool IsEnableAddCommand
        {
            get=> Name.Length > 3 && NumCargeSlot > 0 && Latitude > 0 && Longitude > 0;
        }

        #region Add params
        public double Latitude { get => stationModel.Latitude;  set { stationModel.Latitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public double Longitude { get => stationModel.Longitude; set { stationModel.Longitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int Id { get => stationModel.Id;  set { stationModel.Id = value; OnPropertyChange("IsEnableAddCommand"); } }
        public string Name { get => stationModel.Name;  set { stationModel.Name = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int NumCargeSlot { get => stationModel.NumCargeSlot;  set { stationModel.NumCargeSlot = value; OnPropertyChange("IsEnableAddCommand"); } }
        #endregion

        /// <summary>
        /// ViewStationModel
        /// </summary>
        /// <param name="UpdateAndClosePWindow">Update And Close PWindow</param>
        public ViewStationModel(Action UpdateAndClosePWindow)
        {
            Close = UpdateAndClosePWindow;
            UpDatePWindow = UpdateAndClosePWindow;
            stationModel = new Model.StationModel();
            stationModel.IsDetailsPanelVisibility = false;
        }
    }
}

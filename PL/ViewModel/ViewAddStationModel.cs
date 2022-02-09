using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.ViewModel
{
    partial class ViewStationModel
    {
        public DelegateCommand AddCommand
        {
            get
            {
                stationModel.Addcommand = new DelegateCommand((o) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().AddStation(Id, Name, new BO.Location(Latitude, Longitude), NumCargeSlot);
                        MessageBox.Show("Successfully add station");
                        //?????????????????update window
                        UpDatePWindow();
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed add station: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
                return stationModel.Addcommand;
            }
        }

        public bool IsEnableAddCommand
        {
            get
            {
                return Name.Length > 3 && NumCargeSlot > 0 && Latitude > 0 && Longitude > 0;
            }
        }
        public double Latitude { get { return stationModel.Latitude; } set { stationModel.Latitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public double Longitude { get { return stationModel.Longitude; } set { stationModel.Longitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int Id { private get { return stationModel.Id; } set { stationModel.Id = value; OnPropertyChange("IsEnableAddCommand"); } }
        public string Name { private get { return stationModel.Name; } set { stationModel.Name = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int NumCargeSlot { private get { return stationModel.NumCargeSlot; } set { stationModel.NumCargeSlot = value; OnPropertyChange("IsEnableAddCommand"); } }

        public ViewStationModel(Action UpdateAndClosePWindow)
        {
            Close = UpdateAndClosePWindow;
            UpDatePWindow = UpdateAndClosePWindow;
            stationModel = new Model.StationModel();
            stationModel.IsDetailsPanelVisibility = false;
        }
    }
}

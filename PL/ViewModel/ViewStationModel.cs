using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewStationModel : ViewModelBase
    {
        private readonly Model.StationModel stationModel;

        public DelegateCommand UpDateComand
        {
            get
            {
                stationModel.UpDateComand = new DelegateCommand((o) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().UpdateStation(Id, Name, NumCargeSlot);
                        MessageBox.Show("Successfully update station details");
                        OnPropertyChange("IsEnableUpdateCommand");
                        //?????????????????update window
                        UpDatePWindow();
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed update station: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
                return stationModel.UpDateComand;
            }
        }
        public bool IsEnableUpdateCommand { get { return Name != default && NumCargeSlot > 0; } }
        public string Details{ get { return stationModel.Details; } set { stationModel.Details = value; } }
        public Visibility AddPanelVisibility { get { return stationModel.IsDetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility DetailsPanelVisibility { get { return stationModel.IsDetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }

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
        public ViewStationModel(int stationId, Action updatePList, Action close)
        {
            stationModel = new Model.StationModel();
            UpDatePWindow = updatePList;
            Close = close;
            stationModel.StationId = stationId;
            Details = BLApi.FactoryBL.GetBL().GetStation(stationId).ToString();
            stationModel.IsDetailsPanelVisibility = true;
        }


    }
}

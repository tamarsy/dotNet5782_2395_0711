using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        if (MessageBox.Show($"delete station {Id} ?", $"delete station {Id}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            BLApi.FactoryBL.GetBL().DeleteStation(Id);
                            MessageBox.Show("Successfully delete");
                            if (UpDatePWindow != default)
                                UpDatePWindow();
                            Close();
                        }
                    }
                    catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update station details:" + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
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

        public void ViewDrone(int dId)
        {
            TabItem tabitem = new TabItem();
            tabitem.Header = "Drone: " + dId;
            tabitem.Content = new View.ViewDrone(droneId: dId, UpDateDronesWindow:()=>OnPropertyChange("Details"), close: () => RemoveTab(tabitem.Header), AddTab, RemoveTab);
            AddTab(tabitem);
        }

        public List<BO.DroneCharge> DronesInCharge { get => BLApi.FactoryBL.GetBL().GetStation(stationModel.Id).DronesInCharge; }

        

        public ViewStationModel(int stationId, Action updatePList, Action close, Action<object> addTab, Action<object> removeTab)
        {
            stationModel = new Model.StationModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            UpDatePWindow = updatePList;
            Close = close;
            stationModel.Id = stationId;
            Details = BLApi.FactoryBL.GetBL().GetStation(stationId).ToString();
            stationModel.IsDetailsPanelVisibility = true;
            OnPropertyChange("DronesInCharge");
        }


    }
}

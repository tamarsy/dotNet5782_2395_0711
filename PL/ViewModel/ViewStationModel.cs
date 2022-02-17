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

        #region Commands
        /// <summary>
        /// UpDate station details
        /// </summary>
        public DelegateCommand UpDateComand
        {
            get => new DelegateCommand((o) => { stationModel.UpDateComand(); OnPropertyChange("IsEnableUpdateCommand"); UpDatePWindow(); });
        }


        /// <summary>
        /// Delete station
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    if (MessageBox.Show($"delete station {Id} ?", $"delete station {Id}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        stationModel.Delete();
                        if (UpDatePWindow is not null)
                            UpDatePWindow();
                        Close();
                    }
                });
            }
        }

        /// <summary>
        /// close window
        /// </summary>
        public ICommand CloseCd
        {
            get => new DelegateCommand((o) =>
            {
                Close();
            });
        }

        #endregion 

        #region Enable or visibility
        /// <summary>
        /// return true to Enable Update Command
        /// </summary>
        public bool IsEnableUpdateCommand { get { return Name != default && NumCargeSlot > 0; } }

        /// <summary>
        /// return AddPanelVisibility
        /// </summary>
        public Visibility AddPanelVisibility { get { return stationModel.IsDetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        /// <summary>
        /// return DetailsPanelVisibility
        /// </summary>
        public Visibility DetailsPanelVisibility { get { return stationModel.IsDetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }

        #endregion

        /// <summary>
        /// station details
        /// </summary>
        public string Details { get => stationModel.Details; set { stationModel.Details = value; OnPropertyChange("Details"); } }
        public List<BO.DroneCharge> DronesInCharge { get => stationModel.DronesInCharge; set { stationModel.DronesInCharge = value; OnPropertyChange("DronesInCharge"); } }

        /// <summary>
        /// open drone from DronesInCharge list
        /// </summary>
        /// <param name="dId"></param>
        public void ViewDrone(int dId)
        {
            TabItem tabitem = new TabItem();
            tabitem.Header = "Drone: " + dId;
            tabitem.Content = new View.ViewDrone(droneId: dId, UpDateDronesWindow: () => OnPropertyChange("Details"), close: () => RemoveTab(tabitem.Header), AddTab, RemoveTab);
            AddTab(tabitem);
        }

        /// <summary>
        /// ViewStationModel
        /// </summary>
        /// <param name="stationId">StationId</param>
        /// <param name="updatePList">Action to update previous window</param>
        /// <param name="close">Close window</param>
        /// <param name="addTab">Add tab</param>
        /// <param name="removeTab">Remove tab</param>
        public ViewStationModel(int stationId, Action updatePList, Action close, Action<object> addTab, Action<object> removeTab)
        {
            stationModel = new Model.StationModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            UpDatePWindow = updatePList;
            Close = close;
            stationModel.Id = stationId;
            stationModel.IsDetailsPanelVisibility = true;
            updateCurrentWindow = () =>
            {
                Details = BLApi.FactoryBL.GetBL().GetStation(stationId).ToString();
                DronesInCharge = BLApi.FactoryBL.GetBL().GetStation(stationModel.Id).DronesInCharge;
            };
            updateCurrentWindow();
        }
    }
}

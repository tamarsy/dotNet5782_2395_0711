using PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewStationListModel : ViewModelBase
    {
        Model.StationListModel stationListModel;
        public List<BO.StationToList> Stations
        {
            set { stationListModel.Stations = value; OnPropertyChange("Stations"); }
            get { return stationListModel.Stations; }
        }

        public Array OrderBySelector
        {
            get => typeof(BO.StationToList).GetProperties().Select(s=>s.Name).ToArray();
        }

        public int OrderBy_select
        {
            set
            {
                stationListModel.OrderBy_select = value;
                updateCurrentWindow();
            }
        }

        public ICommand NewViewCommand
        {
            get
            {
                return new DelegateCommand((s) =>
                {
                    TabItem newTab = new TabItem();
                    if (s is int stationId)
                    {
                        newTab.Header = "Station: " + stationId;
                        newTab.TabIndex = stationId;
                        newTab.Content = new ViewStation(UpDatePWindow, stationId, () => RemoveTab("Station: " + stationId), AddTab, RemoveTab);
                    }
                    else
                    {
                        newTab.Header = "Add station";
                        newTab.Content = new ViewStation(() => { UpDatePWindow(); RemoveTab("Add station"); });
                    }
                    AddTab(newTab);
                });
            }
        }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((choosenp) =>
                {
                    Close();
                });
            }
        }
        private void IntilizeStations()
        {
            Stations =
            stationListModel.OrderBy_select switch
            {
                0 => BLApi.FactoryBL.GetBL().StationsList()
                    .OrderBy(s => s.Id).OrderBy(p => p.Id).ToList(),
                1 => BLApi.FactoryBL.GetBL().StationsList()
                    .OrderBy(s => s.Id).OrderBy(p => p.Name).ToList(),
                2 => BLApi.FactoryBL.GetBL().StationsList()
                    .OrderBy(s => s.Id).OrderBy(p => p.NumOfEmptyChargeSlots).ToList(),
                _ => BLApi.FactoryBL.GetBL().StationsList()
                .OrderBy(s => s.Id).OrderBy(p => p.NumOfCatchChargeSlots).ToList()
            };
        }

        public ViewStationListModel(Action<object> addTab, Action<object> removeTab, Action upDateWindows)
        {
            stationListModel = new Model.StationListModel();
            UpDatePWindow = upDateWindows;
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = () => removeTab("Stations List");
            updateCurrentWindow = IntilizeStations;
            updateCurrentWindow();
        }
    }
}

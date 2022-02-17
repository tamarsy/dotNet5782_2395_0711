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

        public DelegateCommand NewViewCommand
        {
            get
            {
                stationListModel.NewViewCommand = new DelegateCommand((s) =>
                {
                    TabItem newTab = new TabItem();
                    if (s is int stationId)
                    {
                        newTab.Header = "Station: " + stationId;
                        newTab.TabIndex = stationId;
                        newTab.Content = new ViewStation(updateCurrentWindow, stationId, () => RemoveTab("Station: " + stationId), AddTab, RemoveTab);
                    }
                    else
                    {
                        newTab.Header = "Add station";
                        newTab.Content = new ViewStation(() => { updateCurrentWindow(); RemoveTab("Add station"); });
                    }
                    AddTab(newTab);
                });
                return stationListModel.NewViewCommand;
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
            //???????????????????
            Stations = BLApi.FactoryBL.GetBL().StationsList()
                .OrderBy(s=>s.Id).OrderBy(p=> p.Id).ToList();
        }

        public ViewStationListModel(Action<object> addTab, Action<object> removeTab)
        {
            stationListModel = new Model.StationListModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = () => removeTab("Stations List");
            updateCurrentWindow = IntilizeStations;
            IntilizeStations();
        }
    }
}

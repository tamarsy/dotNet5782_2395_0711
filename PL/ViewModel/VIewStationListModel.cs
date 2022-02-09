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
                        newTab.Content = new ViewStation(IntilizeStations, stationId, ()=>RemoveTab("Station: " + stationId));
                    }
                    else
                    {
                        newTab.Header = "Add station";
                        newTab.Content = new ViewStation(()=> { IntilizeStations(); RemoveTab("Add station"); });
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
            stationListModel = new Model.StationListModel();
            Stations = BLApi.FactoryBL.GetBL().StationsList().ToList();
        }

        public ViewStationListModel(Action<object> addTab, Action<string> removeTab)
        {
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = () => removeTab("Stations List");
            IntilizeStations();
        }
    }
}

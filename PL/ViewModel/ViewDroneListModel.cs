using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewDroneListModel : ViewModelBase
    {
        private Model.DroneListModel droneListModel;
        public List<BO.DroneToList> Drones
        {
            get { return droneListModel.Drones; }
            set
            {
                droneListModel.Drones = value;
                OnPropertyChange("Drones");
            }
        }

        public Array StatusSelector { get
            {
                List<object> statusSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.DroneStatuses)))
                {
                    statusSelector.Add(item);
                }
                statusSelector.Add("All");
                return statusSelector.ToArray();
            } }


        public Array MaxWeightSelector { get {
                List<object> maxWeightSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.WeightCategories)))
                {
                    maxWeightSelector.Add(item);
                }
                maxWeightSelector.Add("All");

                return maxWeightSelector.ToArray();
            } }


        public int StatusSelector_select
        {
            get { return droneListModel.StatusSelector_select; }
            set
            {
                droneListModel.StatusSelector_select = value;
                WeightAndStatudSelector_SelectionChanged();
            }
        }

        public int MaxWeightSelector_select
        { 
            get { return droneListModel.MaxWeightSelector_select; }
            set
            {
                droneListModel.MaxWeightSelector_select = value;
                WeightAndStatudSelector_SelectionChanged();
            }
        }

        private void WeightAndStatudSelector_SelectionChanged()
        {
            droneListModel.Drones = BLApi.FactoryBL.GetBL().DronesList().ToList();
            if (!MaxWeightSelector.GetValue(MaxWeightSelector_select).Equals("All"))
            {
                droneListModel.Drones = droneListModel.Drones.Where(
                    (d) => (d.MaxWeight).Equals((BO.WeightCategories)MaxWeightSelector_select)).ToList();
            }
            if (!StatusSelector.GetValue(StatusSelector_select).Equals("All"))
            {
                droneListModel.Drones = droneListModel.Drones.Where(
                    (d) => (d.DroneStatuses).Equals((BO.DroneStatuses)StatusSelector_select)).ToList();
            }
            Drones = droneListModel.Drones;
        }

        public ICommand NewViewDroneCommand
        {
            get
            {
                droneListModel.NewViewDroneCommand = new DelegateCommand((choosenDrone) =>
                {
                    TabItem newTabItem = new TabItem();
                    if (choosenDrone is int droneId)
                    {
                        newTabItem.Header = "Drone: " + droneId;
                        newTabItem.TabIndex = droneId;
                        newTabItem.Content = new ViewDrone(droneId, WeightAndStatudSelector_SelectionChanged, ()=>RemoveTab("Drone: " + droneId));
                    }
                    else
                    {
                        newTabItem.Header = "Add drone";
                        Action updateAndClose = WeightAndStatudSelector_SelectionChanged;
                        updateAndClose += () => RemoveTab((string)newTabItem.Header);
                        newTabItem.Content = new ViewDrone(updateAndClose);
                    }
                    AddTab(newTabItem);
                });
                return droneListModel.NewViewDroneCommand;
            }
        }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((choosenDrone) =>
                {
                    Close();
                });
            }
        }

        public ViewDroneListModel(Action<object> addTab, Action<string> removeTab)
        {
            droneListModel = new Model.DroneListModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = ()=>removeTab("Drones List");
            Drones = BLApi.FactoryBL.GetBL().DronesList().ToList();
            droneListModel.StatusSelector_select = StatusSelector.Length - 1;
            droneListModel.MaxWeightSelector_select = MaxWeightSelector.Length - 1;
            WeightAndStatudSelector_SelectionChanged();
        }
    }
}

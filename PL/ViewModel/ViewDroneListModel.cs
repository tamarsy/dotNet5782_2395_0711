using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewDroneListModel : ViewModelBase
    {
        private Model.DroneListModel droneListModel;

        #region Selctors
        public Array StatusSelector
        {
            get
            {
                List<object> statusSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.DroneStatuses)))
                {
                    statusSelector.Add(item);
                }
                statusSelector.Add("All Status");
                return statusSelector.ToArray();
            }
        }

        public Array MaxWeightSelector
        {
            get
            {
                List<object> maxWeightSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.WeightCategories)))
                {
                    maxWeightSelector.Add(item);
                }
                maxWeightSelector.Add("All Weights");

                return maxWeightSelector.ToArray();
            }
        }
        #endregion Selctors

        #region Item select
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

        #endregion

        #region Command
        public ICommand NewViewDroneCommand
        {
            get
            {
                return new DelegateCommand((choosenDrone) =>
                {
                    TabItem newTabItem = new TabItem();
                    if (choosenDrone is int droneId)
                    {
                        newTabItem.Header = "Drone: " + droneId;
                        newTabItem.TabIndex = droneId;
                        newTabItem.Content = new View.ViewDrone(droneId, UpDatePWindow, () => RemoveTab("Drone: " + droneId), addTab: AddTab, removeTab: RemoveTab);
                    }
                    else
                    {
                        newTabItem.Header = "Add drone";
                        Action updateAndClose = UpDatePWindow;
                        updateAndClose += () => RemoveTab((string)newTabItem.Header);
                        newTabItem.Content = new View.ViewDrone(updateAndClose);
                    }
                    AddTab(newTabItem);
                });
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
        #endregion

        public ListCollectionView Drones
        {
            get { return droneListModel.Drones; }
            set
            {
                droneListModel.Drones = value;
                OnPropertyChange("Drones");
            }
        }

        private void WeightAndStatudSelector_SelectionChanged()
        {
            List<BO.DroneToList> drones = BLApi.FactoryBL.GetBL().DronesList().ToList();
            if (MaxWeightSelector.Length - 1 > MaxWeightSelector_select)
                drones = drones.Where(
                    (d) => d.MaxWeight.Equals((BO.WeightCategories)MaxWeightSelector_select)).ToList();
            if (StatusSelector.Length - 1 > StatusSelector_select)
                drones = drones.Where(
                    (d) => d.DroneStatuses.Equals((BO.DroneStatuses)StatusSelector_select)).ToList();
            Drones = new ListCollectionView(drones);
        }

        public ViewDroneListModel(Action<object> addTab, Action<object> removeTab, Action UpdateWindows)
        {
            droneListModel = new Model.DroneListModel();
            UpDatePWindow = UpdateWindows;
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = ()=>removeTab("Drones List");
            droneListModel.StatusSelector_select = StatusSelector.Length - 1;
            droneListModel.MaxWeightSelector_select = MaxWeightSelector.Length - 1;
            updateCurrentWindow = WeightAndStatudSelector_SelectionChanged;
            updateCurrentWindow();
        }
    }
}

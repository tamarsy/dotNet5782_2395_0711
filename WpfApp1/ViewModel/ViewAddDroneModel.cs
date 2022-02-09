using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewDroneModel : ViewModelBase
    {
        public bool IsEnablAddDrone {
            get { return droneModel.IsEnablButton;}
            private set { droneModel.IsEnablButton = value; }
        }

        public Array MaxWeightSelector {
            get { return Enum.GetValues(typeof(BO.WeightCategories)); }
        }

        public Array StationSelector {
            get { return BLApi.FactoryBL.GetBL().StationsList().Select(s => s.Id).ToArray(); }
        }

        public int MaxWeightSelector_select {
            get { return droneModel.MaxWeightSelector_select; }
            set { droneModel.MaxWeightSelector_select = value; OnPropertyChange("MaxWeightSelector_select"); }
        }

        public int StationSelector_select { get { return droneModel.StationSelector_select; }
            set { droneModel.StationSelector_select = value; OnPropertyChange("StationSelector_select"); }
        }

        public int DroneId { get { return droneModel.DroneId; }
            set{ droneModel.DroneId = value; }
        }

        public ICommand AddCommand
        {
            get
            {
                droneModel.Addcommand = new DelegateCommand((o) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().AddDrone(droneModel.DroneId, droneModel.DroneStr, (BO.WeightCategories)droneModel.MaxWeightSelector_select, (int)StationSelector.GetValue(droneModel.StationSelector_select));
                        MessageBox.Show("successfully add");
                        UpDatePWindow();
                    }
                    catch (BO.ObjectAlreadyExistException e){ MessageBox.Show("Failed add drone: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("failed add drone: " + e.Message); }
                });
                return droneModel.Addcommand;
            }
        }

        public ViewDroneModel(Action updateAndClose)
        {
            droneModel = new Model.DroneModel();
            IsEnablAddDrone = false;
            droneModel.DetailsPanelVisibility = false;
            UpDatePWindow = updateAndClose;
            Close = updateAndClose;
        }
    }
}

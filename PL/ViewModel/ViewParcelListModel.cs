using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewParcelListModel : ViewModelBase
    {
        private Model.ParcelListModel ParcelListModel;
        Predicate<ParcelToList> customerSelector;
        public List<ParcelToList> Parcels
        {
            get { return ParcelListModel.Parcels; }
            set
            {
                ParcelListModel.Parcels = value;
                OnPropertyChange("Parcels");
            }
        }

        public Array StatusSelector
        {
            get
            {
                List<object> statusSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.ParcelStatuses)))
                {
                    statusSelector.Add(item);
                }
                statusSelector.Add("All");
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
                maxWeightSelector.Add("All");
                return maxWeightSelector.ToArray();
            }
        }
        public Array PrioritySelector
        {
            get
            {
                List<object> prioritySelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.Priorities)))
                {
                    prioritySelector.Add(item);
                }
                prioritySelector.Add("All");
                return prioritySelector.ToArray();
            }
}
        public int StatusSelector_select
        {
            get { return ParcelListModel.StatusSelector_select; }
            set
            {
                ParcelListModel.StatusSelector_select = value;
                ParcelsSelector_SelectionChanged();
            }
        }

        public int PrioritySelector_select
        {
            get { return ParcelListModel.PrioritySelector_select; }
            set
            {
                ParcelListModel.PrioritySelector_select = value;
                ParcelsSelector_SelectionChanged();
            }
        }

        public int MaxWeightSelector_select
        {
            get { return ParcelListModel.MaxWeightSelector_select; }
            set
            {
                ParcelListModel.MaxWeightSelector_select = value;
                ParcelsSelector_SelectionChanged();
            }
        }


        private void ParcelsSelector_SelectionChanged()
        {
            ParcelListModel.Parcels = BLApi.FactoryBL.GetBL().ParcelsList().ToList();
            if (customerSelector != default)
                ParcelListModel.Parcels = ParcelListModel.Parcels.Where(p => customerSelector(p)).ToList();
            if (!MaxWeightSelector.GetValue(MaxWeightSelector_select).Equals("All"))
            {
                ParcelListModel.Parcels = ParcelListModel.Parcels.Where(
                    (p) => (p.Weight).Equals((BO.WeightCategories)MaxWeightSelector_select)).ToList();
            }
            if (!StatusSelector.GetValue(StatusSelector_select).Equals("All"))
            {
                ParcelListModel.Parcels = ParcelListModel.Parcels.Where(
                    (p) => (p.ParcelStatuses).Equals((BO.ParcelStatuses)StatusSelector_select)).ToList();
            }
            if (!PrioritySelector.GetValue(PrioritySelector_select).Equals("All"))
            {
                ParcelListModel.Parcels = ParcelListModel.Parcels.Where(
                    (p) => (p.Priority).Equals((BO.Priorities)PrioritySelector_select)).ToList();
            }
            Parcels = ParcelListModel.Parcels;
        }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((choosen) =>
                {
                    Close();
                });
            }
        }

        public ICommand NewViewCommand
        {
            get
            {
                ParcelListModel.NewViewCommand = new DelegateCommand((parcel) =>
                {
                    TabItem tabitem = new TabItem();
                    if (parcel is int pId)
                    {
                        tabitem.Header = "Parcel: " + pId;
                        tabitem.TabIndex = pId;
                        tabitem.Content = new View.ViewParcel(pId, ParcelsSelector_SelectionChanged, ()=>RemoveTab("Parcel: " + pId));
                    }
                    else
                    {
                        tabitem.Header = "Add parcel";
                        tabitem.Content = new View.ViewParcel(()=> { ParcelsSelector_SelectionChanged(); RemoveTab("Add parcel"); });
                    }
                    AddTab(tabitem);
                });
                return ParcelListModel.NewViewCommand;
            }
        }


        public ViewParcelListModel(Action<object> addTab, Action<string> removeTab, Predicate<ParcelToList> selectorParcel = default)
        {
            ParcelListModel = new Model.ParcelListModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = () => removeTab("Parcels List");
            ParcelListModel.StatusSelector_select = StatusSelector.Length - 1;
            ParcelListModel.MaxWeightSelector_select = MaxWeightSelector.Length - 1;
            ParcelListModel.PrioritySelector_select = PrioritySelector.Length - 1;
            customerSelector = selectorParcel;
            ParcelsSelector_SelectionChanged();
        }
    }
}

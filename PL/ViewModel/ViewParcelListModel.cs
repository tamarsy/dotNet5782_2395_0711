using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;

namespace PL.ViewModel
{
    class ViewParcelListModel : ViewModelBase
    {
        private Model.ParcelListModel ParcelListModel;

        #region Selectors
        public Array StatusSelector
        {
            get
            {
                List<object> statusSelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(ParcelStatuses)))
                {
                    statusSelector.Add(item);
                }
                statusSelector.Add("All status");
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
                maxWeightSelector.Add("All weights");
                return maxWeightSelector.ToArray();
            }
        }

        public Array PrioritySelector
        {
            get
            {
                List<object> prioritySelector = new List<object>();
                foreach (var item in Enum.GetValues(typeof(BO.Priorities)))
                    prioritySelector.Add(item);
                prioritySelector.Add("All prioritys");
                return prioritySelector.ToArray();
            }
        }

        public Array GroupBySelector
        {
            get
            {
                return new List<object>() { nameof(ParcelToList.SenderId), nameof(ParcelToList.GetterId), "Not group" }.ToArray();
            }
        }
        #endregion

        #region SelectedItems
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

        public object GroupBy_select
        {
            get { return ParcelListModel.GroupBy_select; }
            set
            {
                ParcelListModel.GroupBy_select = value.ToString();
                ParcelListModel.groupingSelected = new PropertyGroupDescription(ParcelListModel.GroupBy_select);
                ParcelsSelector_SelectionChanged();
            }
        }
        #endregion

        public ListCollectionView Parcels
        {
            get { return ParcelListModel.Parcels; }
            set
            {
                ParcelListModel.Parcels = value;
                OnPropertyChange("Parcels");
            }
        }
        public Visibility IsAddVisibility { get { return ParcelListModel.CustomerSelector == null ? Visibility.Visible : Visibility.Collapsed; } }

        private void ParcelsSelector_SelectionChanged()
        {
            List<ParcelToList> parcels = BLApi.FactoryBL.GetBL().ParcelsList().ToList();
            if (ParcelListModel.CustomerSelector != default)
                parcels = parcels.Where(p => ParcelListModel.CustomerSelector(p)).ToList();
            if (Enum.GetValues(typeof(WeightCategories)).Length >  MaxWeightSelector_select)
                parcels = parcels.Where(
                    (p) => p.Weight.Equals((WeightCategories)MaxWeightSelector_select)).ToList();
            if (Enum.GetValues(typeof(ParcelStatuses)).Length > StatusSelector_select)
                parcels = parcels.Where(
                    (p) => p.ParcelStatuses.Equals((ParcelStatuses)StatusSelector_select)).ToList();
            if (Enum.GetValues(typeof(Priorities)).Length > PrioritySelector_select)
                parcels = parcels.Where(
                    (p) => p.Priority.Equals((Priorities)PrioritySelector_select)).ToList();

            ParcelListModel.Parcels = new ListCollectionView(parcels);

            if (ParcelListModel.groupingSelected is not null && !ParcelListModel.GroupBy_select.Equals(GroupBySelector.GetValue(GroupBySelector.Length - 1)))
                ParcelListModel.Parcels.GroupDescriptions.Add(ParcelListModel.groupingSelected);
            else if(ParcelListModel.groupingSelected is not null)
                ParcelListModel.Parcels.GroupDescriptions.Remove(ParcelListModel.groupingSelected);

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
                        tabitem.Content = new View.ViewParcel(pId, ParcelsSelector_SelectionChanged, () => RemoveTab(tabitem.Header), AddTab, RemoveTab, ParcelListModel.CustomerSelector != null);
                    }
                    else
                    {
                        tabitem.Header = "Add parcel";
                        tabitem.Content = new View.ViewParcel(() => { ParcelsSelector_SelectionChanged(); RemoveTab(tabitem.Header); });
                    }
                    AddTab(tabitem);
                });
                return ParcelListModel.NewViewCommand;
            }
        }


        public ViewParcelListModel(Action<object> addTab, Action<object> removeTab, Predicate<ParcelToList> selectorParcel = default, string header = "Parcels List")
        {
            ParcelListModel = new Model.ParcelListModel();
            AddTab = addTab;
            RemoveTab = removeTab;
            Close = () => removeTab(header);
            ParcelListModel.StatusSelector_select = StatusSelector.Length - 1;
            ParcelListModel.MaxWeightSelector_select = MaxWeightSelector.Length - 1;
            ParcelListModel.PrioritySelector_select = PrioritySelector.Length - 1;
            ParcelListModel.GroupBy_select = GroupBySelector.GetValue(GroupBySelector.Length - 1).ToString();
            ParcelListModel.CustomerSelector = selectorParcel;
            ParcelsSelector_SelectionChanged();
        }
    }
}

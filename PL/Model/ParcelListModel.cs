using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace PL.Model
{
    class ParcelListModel
    {
        public ListCollectionView Parcels { set; get; }
        public GroupDescription groupingSelected { set; get; }
        public DelegateCommand NewViewCommand { set; get; }
        public DelegateCommand AddCommand { set; get; }
        public int StatusSelector_select { set; get; }
        public int MaxWeightSelector_select { set; get; }
        public string GroupBy_select { get; set; }
        public int PrioritySelector_select { set; get; }
        public Predicate<ParcelToList> CustomerSelector { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PL.Model
{
    class ParcelListModel
    {
        public List<BO.ParcelToList> Parcels { set; get; }
        public DelegateCommand NewViewCommand { set; get; }
        public DelegateCommand AddCommand { set; get; }
        public int StatusSelector_select { set; get; }
        public int MaxWeightSelector_select { set; get; }
        public int PrioritySelector_select { set; get; }
    }
}

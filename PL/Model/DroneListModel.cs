using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.Model
{
    class DroneListModel
    {
        public ListCollectionView Drones { set; get; }
        public DelegateCommand Close { set; get; }
        public int StatusSelector_select { set; get; }
        public int MaxWeightSelector_select { set; get; }
    }
}

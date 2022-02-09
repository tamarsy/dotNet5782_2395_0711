using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.Model
{
    class ParcelModel
    {
        public Action UpDatePWindow { set; get; }
        public bool DetailsPanelVisibility { set; get; }
        public Visibility DD_ButtonVisibility { set; get; }
        public int SID { set; get; }
        public int TID_select { set; get; }
        public DelegateCommand AddComand { set; get; }
        public DelegateCommand DroneDetails_Comand { set; get; }
        public int PrioritySelector_select { set; get; }
        public int WeightSelector_select { set; get; }
        public string Details { get; set; }
        public int ParcelId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.Model
{
    class StationModel
    {
        public DelegateCommand Addcommand { set; get; }
        public DelegateCommand UpDateComand { set; get; }
        public bool IsDetailsPanelVisibility { set; get; }
        public int StatusSelector_select { set; get; }
        public int MaxWeightSelector_select { set; get; }
        public int StationSelector_select { set; get; }
        public string Details { get; set; }
        public int StationId { get; set; }
        public double Latitude { get; internal set; }
        public double Longitude { get; internal set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int NumCargeSlot { get; internal set; }
    }
}

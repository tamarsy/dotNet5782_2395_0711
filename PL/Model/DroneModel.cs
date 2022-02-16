using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Model
{
    partial class DroneModel
    {
        public DelegateCommand Addcommand { set; get; }
        public DelegateCommand DC_Comand { set; get; }
        public DelegateCommand CS_Comand { set; get; }
        public DelegateCommand UpDateModelComand { set; get; }
        public bool IsAutomatic { set; get; }
        public int MaxWeightSelector_select { set; get; }
        public int StationSelector_select { set; get; }
        public int IStartModel { set; get; }
        public int ModelLength { set; get; }
        public string DroneStr { get; set; }
        public int DroneId { get; set; }
        public bool IsEnablButton { get; set; }
        public Visibility DC_ButtonVisibility { get; set; }
        public Visibility CS_ButtonVisibility { get; set; }
        public bool DetailsPanelVisibility { get; set; }
        public string CS_ButtonContext { get; set; }
        public string DC_ButtonContext { get; set; }
    }
}

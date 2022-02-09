using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.Model
{
    class CustomerModel
    {
        internal DelegateCommand Addcommand { set; get; }
        internal DelegateCommand UpDateCommand { set; get; }
        internal string Details { get; set; }
        internal int CustomerId { get; set; }
        internal string Name { get; set; } = "";
        internal string Phone { get; set; } = "";
        public int IStartName { set; get; }
        public int IStartPhone { set; get; }
        internal double Latitude { get; set; }
        internal double Longitude { get; set; }
        internal bool IsDetailsPanelVisibility { set; get; }
    }
}

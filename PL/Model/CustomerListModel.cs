using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace PL.Model
{
    class CustomerListModel
    {
        public ListCollectionView Customers { set; get; }
        public DelegateCommand Close { set; get; }
        public DelegateCommand NewViewCommand { set; get; }
        public GroupDescription groupingSelected { set; get; }
    }
}

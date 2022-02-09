using System;
using System.Collections.Generic;
using System.Text;

namespace PL.Model
{
    class CustomerListModel
    {
        public List<BO.CustomerToList> Customer { set; get; }
        public DelegateCommand Close { set; get; }
        public DelegateCommand NewViewCommand { set; get; }
    }
}

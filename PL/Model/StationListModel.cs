using System;
using System.Collections.Generic;
using System.Text;

namespace PL.Model
{
    class StationListModel
    {
        public List<BO.StationToList> Stations { set; get; }
        public DelegateCommand Close { set; get; }
        public int OrderBy_select { set; get; }
    }
}

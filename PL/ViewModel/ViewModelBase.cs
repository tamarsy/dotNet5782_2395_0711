using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected Action UpDatePWindow = default;
        public Action<object> AddTab { get; set; }
        public Action<object> RemoveTab { get; set; }
        public Action Close { get; set; }

        protected string OnlyNumStr(string allString)
        {
            string newS = "";
            for(int i = 0; i < allString.Length; ++i)
            {
                if (char.IsDigit(allString[i]))
                    newS += allString[i];
            }
            if (newS.Length < allString.Length)
            {
                MessageBox.Show("ERROR enter only number for Id");
            }
            return newS;
        }


        public ViewModelBase()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

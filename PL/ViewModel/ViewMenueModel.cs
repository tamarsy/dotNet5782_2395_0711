﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace PL.ViewModel
{
    abstract class ViewMenueModel : ViewModelBase
    {
        public Model.MenueModel baseModel;
        public string ButtonA_Content { get { return baseModel.ButtonA_Content; } }
        public string ButtonB_Content { get { return baseModel.ButtonB_Content; } }
        public string ButtonC_Content { get{ return baseModel.ButtonC_Content; } }
        public string ButtonD_Content { get{ return baseModel.ButtonD_Content; } }
        public int SelectedTab { get { return baseModel.selectedTab; } set { baseModel.selectedTab = value; OnPropertyChange("SelectedTab"); } }
        public Action UpdateWindows;

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    Close();
                });
            }
        }
    }
}

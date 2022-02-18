using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewParcelModel : ViewModelBase
    {
        #region Selectors
        public Array WeightSelector { get { return Enum.GetValues(typeof(BO.WeightCategories)); } }
        public Array PrioritySelector { get { return Enum.GetValues(typeof(BO.Priorities)); } }
        public Array TID { get => parcelModel.TID; set { parcelModel.TID = value; OnPropertyChange("TID"); } }
        public Array SID { get => parcelModel.SID; set { parcelModel.SID = value; OnPropertyChange("SID"); } }
        #endregion Selectors

        #region Items select

        /// <summary>
        /// PrioritySelector item select
        /// </summary>
        public int PrioritySelector_select
        {
            get { return parcelModel.PrioritySelector_select; }
            set
            {
                parcelModel.PrioritySelector_select = value;
                OnPropertyChange("PrioritySelector_select");
            }
        }

        /// <summary>
        /// TID item select
        /// </summary>
        public object TID_select
        {
            get { return parcelModel.TID_select; }
            set
            {
                parcelModel.TID_select = (int)value;
                OnPropertyChange("PrioritySelector_select");
                OnPropertyChange("IsEnableAdd_Button");
            }
        }

        /// <summary>
        /// SID item select
        /// </summary>
        public object SID_select
        {
            get { return parcelModel.SID_select; }
            set
            {
                parcelModel.SID_select = (int)value;
                OnPropertyChange("SID");
                OnPropertyChange("IsEnableAdd_Button");
            }
        }

        /// <summary>
        /// WeightSelector item select
        /// </summary>
        public int WeightSelector_select
        {
            get { return parcelModel.WeightSelector_select; }
            set
            {
                parcelModel.WeightSelector_select = value;
                OnPropertyChange("WeightSelector_select");
            }
        }

        #endregion Items select


        #region Enable and Visibility
        public bool IsEnableAdd_Button
        {
            get
            {
                return !TID_select.Equals(0) && !SID_select.Equals(0) && !TID_select.Equals(SID_select);
            }
        }

        public Visibility DetailsPanelVisibility { get { return parcelModel.DetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility AddPanelVisibility { get { return parcelModel.DetailsPanelVisibility ? Visibility.Collapsed : Visibility.Visible; } }
        public bool IsEnableSID_select { get; }


        #endregion


        public ICommand AddComand
        {
            get
            {
                return new DelegateCommand((pram) =>
                {
                    parcelModel.AddComand();
                    UpDatePWindow();
                });
            }
        }


        private void InitializeData()
        {
            TID = BLApi.FactoryBL.GetBL().CustomersList().Select(c => c.Id).ToArray();
            SID = BLApi.FactoryBL.GetBL().CustomersList().Select(c => c.Id).ToArray();
        }

        public ViewParcelModel(Action upDateAndClose, int? id = null)
        {
            parcelModel = new Model.ParcelModel();
            Close = UpDatePWindow = upDateAndClose;
            parcelModel.DetailsPanelVisibility = false;
            if (id is not null)
            {
                parcelModel.SID_select = (int)id;
                IsEnableSID_select = false;
            }
            else
                IsEnableSID_select = true;
            updateCurrentWindow = InitializeData;
            InitializeData();
        }
    }
}

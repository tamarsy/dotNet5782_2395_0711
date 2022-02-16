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
        public Array WeightSelector { get { return Enum.GetValues(typeof(BO.WeightCategories)); } }
        public Array PrioritySelector { get { return Enum.GetValues(typeof(BO.Priorities)); } }
        public Array TID { get { return BLApi.FactoryBL.GetBL().CustomersList().Select(c=>c.Id).ToArray(); } }
        public Array SID { get { return BLApi.FactoryBL.GetBL().CustomersList().Select(c=>c.Id).ToArray(); } }

        public int PrioritySelector_select
        {
            get { return parcelModel.PrioritySelector_select; }
            set
            {
                parcelModel.PrioritySelector_select = value;
                OnPropertyChange("PrioritySelector_select");
            }
        }

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


        public int WeightSelector_select
        {
            get { return parcelModel.WeightSelector_select; }
            set
            {
                parcelModel.WeightSelector_select = value;
                OnPropertyChange("WeightSelector_select");
            }
        }

        public bool IsEnableAdd_Button
        {
            get {
                return !TID_select.Equals(0) && !SID_select.Equals(0) && !TID_select.Equals(SID_select);
            }
        }

        public ICommand AddComand
        {
            get
            {
                parcelModel.AddComand = new DelegateCommand((pram) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().GetCustomer(parcelModel.SID_select);
                        BLApi.FactoryBL.GetBL().AddParcel(parcelModel.SID_select, parcelModel.TID_select, (BO.WeightCategories)WeightSelector_select, (BO.Priorities)PrioritySelector_select);
                        MessageBox.Show("successfully add Parcel");
                        //?????????????????????????update from customer 
                        parcelModel.UpDatePWindow();
                    }
                    catch (BO.ObjectNotExistException) { MessageBox.Show("not exist sender id:" + SID); }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("can't add Parcel: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
                return parcelModel.AddComand;
            }
        }

        public Visibility DetailsPanelVisibility { get{ return parcelModel.DetailsPanelVisibility ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility AddPanelVisibility { get { return parcelModel.DetailsPanelVisibility ?Visibility.Collapsed:Visibility.Visible; } }
        public bool IsEnableSID_select { get; }

        public ViewParcelModel(Action upDateAndClose, int? id = null)
        {
            parcelModel = new Model.ParcelModel();
            Close = upDateAndClose;
            parcelModel.UpDatePWindow = upDateAndClose;
            parcelModel.DetailsPanelVisibility = false;
            if (id != null)
            {
                parcelModel.SID_select = (int)id;
                IsEnableSID_select = false;
            }
            else
                IsEnableSID_select = true;
        }
    }
}

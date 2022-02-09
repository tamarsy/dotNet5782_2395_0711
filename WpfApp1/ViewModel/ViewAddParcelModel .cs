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
                return TID_select != default && SID > 1111111 && SID < 999999999 && !TID_select.Equals(parcelModel.SID);
            }
        }

        public int SID { set { parcelModel.SID = value;OnPropertyChange("SID"); OnPropertyChange("IsEnableAdd_Button"); } get { return parcelModel.SID; } }

        public ICommand AddComand
        {
            get
            {
                parcelModel.AddComand = new DelegateCommand((pram) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().GetCustomer(SID);
                        BLApi.FactoryBL.GetBL().AddParcel(SID, parcelModel.TID_select, (BO.WeightCategories)WeightSelector_select, (BO.Priorities)PrioritySelector_select);
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


        public ViewParcelModel(Action upDateAndClose)
        {
            parcelModel = new Model.ParcelModel();
            Close = upDateAndClose;
            parcelModel.UpDatePWindow = upDateAndClose;
            parcelModel.DetailsPanelVisibility = false;
        }
    }
}

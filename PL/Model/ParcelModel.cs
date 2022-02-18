using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PL.Model
{
    class ParcelModel
    {
        public Action Delete
        {
            get => () =>
                 {
                     try
                     {
                         if (MessageBox.Show($"delete parcel {ParcelId} ?", $"delete {ParcelId}", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                         {
                             BLApi.FactoryBL.GetBL().DeleteParcel(ParcelId);
                             MessageBox.Show("Successfully delete");

                         }
                     }
                     catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update parcel details:" + e.Message); }
                     catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                 };
        }
        public Action AddComand
        {
            get => () =>
            {
                try
                {
                    BLApi.FactoryBL.GetBL().GetCustomer(SID_select);
                    BLApi.FactoryBL.GetBL().AddParcel(SID_select, TID_select, (BO.WeightCategories)WeightSelector_select, (BO.Priorities)PrioritySelector_select);
                    MessageBox.Show("successfully add Parcel");
                }
                catch (BO.ObjectNotExistException) { MessageBox.Show("not exist sender id:" + SID); }
                catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("can't add Parcel: " + e.Message); }
                catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
            };
        }
        public Array TID { set; get; }
        public Array SID { set; get; }
        public Visibility DD_ButtonVisibility { set; get; }
        public string Details { get; set; }
        public bool DetailsPanelVisibility { set; get; }
        public int SID_select { set; get; }
        public int TID_select { set; get; }
        public int PrioritySelector_select { set; get; }
        public int WeightSelector_select { set; get; }
        public int ParcelId { get; set; }
    }
}

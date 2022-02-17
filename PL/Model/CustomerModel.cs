using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PL.Model
{
    class CustomerModel
    {
        internal Action Addcommand { get => () =>
          {
              try
              {
                  BLApi.FactoryBL.GetBL().AddCustomer(CustomerId, Name, Phone, new BO.Location(Latitude, Longitude));
                  MessageBox.Show("Successfully add customer");

              }
              catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed add customer: " + e.Message); }
              catch (Exception e) { MessageBox.Show("ERROR " + e.Message); }
          };
        }
        internal Action UpDateCommand { get => () =>
          {
              try
              {
                  BLApi.FactoryBL.GetBL().UpdateCusomer(CustomerId, name: Name, phone: Phone);
                  MessageBox.Show("Successfully update customer details");
              }
              catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("can't update customer details:" + e.Message); }
              catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
          };
        }
        public ListCollectionView ParcelsFrom { set; get; }
        public ListCollectionView ParcelsTo { set; get; }
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

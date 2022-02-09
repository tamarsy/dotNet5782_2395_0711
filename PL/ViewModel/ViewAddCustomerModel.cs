using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.ViewModel
{
    partial class ViewCustomerModel
    {
        public DelegateCommand AddCommand
        {
            get
            {
                customerModel.Addcommand = new DelegateCommand((o) =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().AddCustomer(customerModel.CustomerId, Name, Phone, new BO.Location(Latitude, Longitude));
                        MessageBox.Show("Successfully add customer");
                        OnPropertyChange("IsEnableUpdateCommand");
                        UpDatePWindow();
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed add customer: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                });
                return customerModel.Addcommand;
            }
        }

        public bool IsEnableAddCommand
        {
            get
            {
                return Name.Length > 3 && Phone.Length > 7 && Phone.Length < 11 && Id.ToString().Length == 8 && Latitude > 0 && Longitude > 0;
            }
        }

        public string Name
        {
            get { return customerModel.Name; }
            set
            {
                customerModel.Name = value;
                OnPropertyChange("IsEnableAddCommand");
            }
        }

        public double Latitude { get { return customerModel.Latitude; } set { customerModel.Latitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public double Longitude { get { return customerModel.Longitude; } set { customerModel.Longitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int Id { private get { return customerModel.CustomerId; } set { customerModel.CustomerId = value; OnPropertyChange("IsEnableAddCommand"); } }
        public string Phone { private get { return customerModel.Phone; } set { customerModel.Phone = value; OnPropertyChange("IsEnableAddCommand"); } }

        public ViewCustomerModel(Action upDatePListAndClose)
        {
            Close = upDatePListAndClose;
            customerModel = new Model.CustomerModel();
            UpDatePWindow = upDatePListAndClose;
            customerModel.IsDetailsPanelVisibility = false;
        }
    }
}

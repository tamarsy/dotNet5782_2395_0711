using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PL.ViewModel
{
    partial class ViewCustomerModel
    {
        /// <summary>
        /// Add customer
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    customerModel.Addcommand();
                    OnPropertyChange("IsEnableUpdateCommand");
                    UpDatePWindow();
                });
            }
        }

        #region Property for add
        public double Latitude { get => customerModel.Latitude; set { customerModel.Latitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public double Longitude { get => customerModel.Longitude; set { customerModel.Longitude = value; OnPropertyChange("IsEnableAddCommand"); } }
        public int Id { private get => customerModel.CustomerId; set { customerModel.CustomerId = value; OnPropertyChange("IsEnableAddCommand"); } }
        public string Phone { private get => customerModel.Phone; set { customerModel.Phone = value; OnPropertyChange("IsEnableAddCommand"); } }
        public string Name { get => customerModel.Name;set { customerModel.Name = value; OnPropertyChange("IsEnableAddCommand"); }}
        #endregion

        /// <summary>
        /// return if Enable Add Command
        /// </summary>
        public bool IsEnableAddCommand
        {
            get => Name.Length > 1 && Phone.Length > 7 && Phone.Length < 11 && Id.ToString().Length == 8 && Latitude > 0 && Longitude > 0;
        }

        /// <summary>
        /// ViewCustomerModel
        /// </summary>
        /// <param name="upDatePListAndClose">upDatePListAndClose</param>
        public ViewCustomerModel(Action upDatePListAndClose)
        {
            Close = upDatePListAndClose;
            customerModel = new Model.CustomerModel();
            UpDatePWindow = upDatePListAndClose;
            customerModel.IsDetailsPanelVisibility = false;
        }
    }
}

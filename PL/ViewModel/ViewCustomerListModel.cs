using PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewCustomerListModel : ViewModelBase
    {
        Model.CustomerListModel customerListModel;
        public ListCollectionView Customers { get { return customerListModel.Customers; } set { customerListModel.Customers = value; OnPropertyChange("Customers"); } }
        #region Command
        public DelegateCommand NewViewCommand {
            get
            {
                customerListModel.NewViewCommand = new DelegateCommand((choosenCustomer) =>
                {
                    TabItem tabitem = new TabItem();
                    if (choosenCustomer is int cId)
                    {
                        tabitem.Header = "Customer: " + cId;
                        tabitem.TabIndex = cId;
                        tabitem.Content = new ViewCustomer(id:cId, close:()=> RemoveTab(tabitem.Header), AddTab, RemoveTab, UpDatePWindow: UpDatePWindow);
                    }
                    else
                    {
                        tabitem.Header = "Add customer";
                        tabitem.Content = new ViewCustomer(() => { UpDatePWindow(); RemoveTab(tabitem.Header); });
                    }
                    AddTab(tabitem);
                });
                return customerListModel.NewViewCommand;
            }
        }

        public DelegateCommand GoupByName
        {
            get
            {
                return new DelegateCommand((choosenCustomer) =>
                {
                    if (customerListModel.groupingSelected is null)
                    {
                        customerListModel.groupingSelected = new PropertyGroupDescription("Name");
                        customerListModel.Customers.GroupDescriptions.Add(customerListModel.groupingSelected);
                    }
                    else
                    {
                        customerListModel.Customers.GroupDescriptions.Remove(customerListModel.groupingSelected);
                        customerListModel.groupingSelected = null;
                    }
                    OnPropertyChange("GoupByNameText");
                });
            }
        }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((choosenp) =>
                {
                    Close();
                });
            }
        }

        #endregion

        public string GoupByNameText { get { return customerListModel.groupingSelected is null? "Group by name": "Remove group by name"; } }

        private void IntilizeCUstomerList()
        {
            Customers = new ListCollectionView(BLApi.FactoryBL.GetBL().CustomersList().ToList());
        }


        public ViewCustomerListModel(Action<object> addtab, Action<object> removeTab, Action upDateWindows)
        {
            customerListModel = new Model.CustomerListModel();
            UpDatePWindow = upDateWindows;
            AddTab = addtab;
            RemoveTab = removeTab;
            Close = () => removeTab("Customers List");
            updateCurrentWindow = IntilizeCUstomerList;
            updateCurrentWindow();
        }

    }
}

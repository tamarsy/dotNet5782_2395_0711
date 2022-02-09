using PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewCustomerListModel : ViewModelBase
    {
        Model.CustomerListModel customerListModel;
        public List<BO.CustomerToList> Customers { get { return customerListModel.Customer; } set { customerListModel.Customer = value; OnPropertyChange("Customers"); } }
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
                        tabitem.Content = new ViewCustomer(id:cId, close:()=> RemoveTab("Customer: " + cId), UpDatePWindow: IntilizeCUstomerList);
                    }
                    else
                    {
                        tabitem.Header = "Add customer";
                        tabitem.Content = new ViewCustomer(() => { IntilizeCUstomerList(); RemoveTab("Add customer"); });
                    }
                    AddTab(tabitem);
                });
                return customerListModel.NewViewCommand;
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


        private void IntilizeCUstomerList()
        {
            customerListModel = new Model.CustomerListModel();
            Customers = BLApi.FactoryBL.GetBL().CustomersList().ToList();
        }


        public ViewCustomerListModel(Action<object> addtab, Action<string> removeTab)
        {
            AddTab = addtab;
            RemoveTab = removeTab;
            Close = () => removeTab("Customers List");
            IntilizeCUstomerList();
        }

    }
}

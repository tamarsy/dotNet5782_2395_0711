using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for VIewCustomerList.xaml
    /// </summary>
    public partial class VIewCustomerList : UserControl
    {
        private readonly ViewModel.ViewCustomerListModel _viewCustomerListModel;
        public VIewCustomerList(Action<object> addTab, Action<object> removeTab)
        {
            _viewCustomerListModel = new ViewModel.ViewCustomerListModel(addTab, removeTab);
            DataContext = _viewCustomerListModel;
            InitializeComponent();
        }
        private void viewCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)=> _viewCustomerListModel.NewViewCommand.Execute(((BO.CustomerToList)viewCustomers.SelectedItem).Id);
    }
}

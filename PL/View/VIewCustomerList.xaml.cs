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
    public partial class VIewCustomerList : UserControl, Update
    {
        private readonly ViewModel.ViewCustomerListModel _viewCustomerListModel;
        public Action updateCurrentWindow { get; }
        public VIewCustomerList(Action<object> addTab, Action<object> removeTab)
        {
            _viewCustomerListModel = new ViewModel.ViewCustomerListModel(addTab, removeTab);
            updateCurrentWindow = _viewCustomerListModel.updateCurrentWindow;
            DataContext = _viewCustomerListModel;
            InitializeComponent();
        }

        private void viewCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewCustomers.SelectedItem is BO.CustomerToList c)
                _viewCustomerListModel.NewViewCommand.Execute(c.Id);
        }
    }
}

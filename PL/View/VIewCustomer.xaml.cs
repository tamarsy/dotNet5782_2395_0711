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
    /// Interaction logic for VIewCustomer.xaml
    /// </summary>
    public partial class ViewCustomer : UserControl , Update
    {
        private readonly ViewModel.ViewCustomerModel _viewCustomerModel;
        public Action updateCurrentWindow { get; }
        public ViewCustomer(Action upDatePWindowAndClose)
        {
            _viewCustomerModel = new ViewModel.ViewCustomerModel(upDatePWindowAndClose);
            updateCurrentWindow = _viewCustomerModel.updateCurrentWindow;
            DataContext = _viewCustomerModel;
            InitializeComponent();
        }

        public ViewCustomer(int id, Action close, Action<object> addTab, Action<object> removeTab, Action UpDatePWindow)
        {
            _viewCustomerModel = new ViewModel.ViewCustomerModel(id, UpDatePWindow, close, addTab, removeTab);
            updateCurrentWindow = _viewCustomerModel.updateCurrentWindow;
            DataContext = _viewCustomerModel;
            InitializeComponent();
        }


        private void viewParceld_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView l)
                if (l.SelectedItem is BO.CustomerDelivery c)
                    _viewCustomerModel.NewViewParcel(c.Id);

        }

    }
}

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
    public partial class ViewCustomer : UserControl
    {
        private readonly ViewModel.ViewCustomerModel _viewCustomerModel;
        public ViewCustomer(Action upDatePWindowAndClose)
        {
            _viewCustomerModel = new ViewModel.ViewCustomerModel(upDatePWindowAndClose);
            DataContext = _viewCustomerModel;
            InitializeComponent();
        }

        public ViewCustomer(int id, Action close, Action UpDatePWindow = default)
        {
            _viewCustomerModel = new ViewModel.ViewCustomerModel(id, UpDatePWindow, close);
            DataContext = _viewCustomerModel;
            InitializeComponent();
        }
    }
}

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
    /// Interaction logic for ViewParcelList.xaml
    /// </summary>
    public partial class ViewParcelList : UserControl
    {
        private readonly ViewModel.ViewParcelListModel _viewParcelListModel;
        public ViewParcelList(Action<object> addTab, Action<object> removeTab, Predicate<BO.ParcelToList> p = default, string header = "Parcels List")
        {
            _viewParcelListModel = new ViewModel.ViewParcelListModel(addTab, removeTab, p, header);
            DataContext = _viewParcelListModel;
            InitializeComponent();
        }

        private void viewParcel_MouseDoubleClick(object sender, MouseButtonEventArgs e) => _viewParcelListModel.NewViewCommand.Execute(((BO.ParcelToList)viewParcels.SelectedItem).Id);
    }
}

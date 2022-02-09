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
    /// Interaction logic for ViewStationList.xaml
    /// </summary>
    public partial class ViewStationList : UserControl
    {
        ViewModel.ViewStationListModel _viewStationList;
        public ViewStationList(Action<object> addTab, Action<string> removeTab)
        {
            _viewStationList = new ViewModel.ViewStationListModel(addTab, removeTab);
            DataContext = _viewStationList;
            InitializeComponent();
        }

        private void viewStation_MouseDoubleClick(object sender, MouseButtonEventArgs e) => _viewStationList.NewViewCommand.Execute(((BO.StationToList)viewStations.SelectedItem).Id);
    }
}

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
    public partial class ViewStationList : UserControl, Update
    {
        ViewModel.ViewStationListModel _viewStationList;
        public Action updateCurrentWindow { get; }
        public ViewStationList(Action<object> addTab, Action<object> removeTab)
        {
            _viewStationList = new ViewModel.ViewStationListModel(addTab, removeTab);
            updateCurrentWindow = _viewStationList.updateCurrentWindow;
            DataContext = _viewStationList;
            InitializeComponent();
        }


        private void viewStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewStations.SelectedItem is BO.StationToList d)
                _viewStationList.NewViewCommand.Execute(d.Id);
        }
    }
}

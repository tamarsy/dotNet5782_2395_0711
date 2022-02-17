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
    /// Interaction logic for ViewStation.xaml
    /// </summary>
    public partial class ViewStation : UserControl, Update
    {
        private readonly ViewModel.ViewStationModel _viewParcelModel;

        public Action updateCurrentWindow { get; }

        public ViewStation(Action UpdatePWindow, int parcelId, Action close, Action<object> addTab, Action<object> removeTab)
        {
            _viewParcelModel = new ViewModel.ViewStationModel(parcelId, UpdatePWindow, close, addTab, removeTab);
            updateCurrentWindow = _viewParcelModel.updateCurrentWindow;
            DataContext = _viewParcelModel;
            InitializeComponent();
        }

        public ViewStation(Action UpdateAndClosePWindow)
        {
            _viewParcelModel = new ViewModel.ViewStationModel(UpdateAndClosePWindow);
            updateCurrentWindow = _viewParcelModel.updateCurrentWindow;
            DataContext = _viewParcelModel;
            InitializeComponent();
        }

        private void ViewDrone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(ViewDrones.SelectedItem is BO.DroneCharge d)
                _viewParcelModel.ViewDrone(d.Id);
        }
    }
}

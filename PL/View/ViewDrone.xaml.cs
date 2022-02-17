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
    /// Interaction logic for ViewDrone.xaml
    /// </summary>
    public partial class ViewDrone : UserControl, Update
    {
        private readonly ViewModel.ViewDroneModel _viewDroneModel;
        public Action updateCurrentWindow {get; }
        public ViewDrone(Action updateAndClose)
        {
            _viewDroneModel = new ViewModel.ViewDroneModel(updateAndClose);
            updateCurrentWindow = _viewDroneModel.updateCurrentWindow;
            DataContext = _viewDroneModel;
            InitializeComponent();
        }
        public ViewDrone(int droneId, Action UpDateDronesWindow, Action close, Action<object> addTab, Action<object> removeTab)
        {
            _viewDroneModel = new ViewModel.ViewDroneModel(droneId, UpDateDronesWindow, close, addTab, removeTab);
            updateCurrentWindow = _viewDroneModel.updateCurrentWindow;
            DataContext = _viewDroneModel;
            InitializeComponent();
        }
    }
}

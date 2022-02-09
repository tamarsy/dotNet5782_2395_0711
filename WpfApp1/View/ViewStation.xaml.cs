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
    public partial class ViewStation : UserControl
    {
        private readonly ViewModel.ViewStationModel _viewParcelModel;
        public ViewStation(Action UpdatePWindow, int parcelId, Action close)
        {
            _viewParcelModel = new ViewModel.ViewStationModel(parcelId, UpdatePWindow, close);
            DataContext = _viewParcelModel;
            InitializeComponent();
        }

        public ViewStation(Action UpdateAndClosePWindow)
        {
            _viewParcelModel = new ViewModel.ViewStationModel(UpdateAndClosePWindow);
            DataContext = _viewParcelModel;
            InitializeComponent();
        }
    }
}

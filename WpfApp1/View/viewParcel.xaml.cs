using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for viewParcel.xaml
    /// </summary>
    public partial class ViewParcel : UserControl
    {
        private readonly ViewModel.ViewParcelModel _viewParcelModel;
        public ViewParcel(int parcelId, Action UpdatePWindow, Action close)
        {
            _viewParcelModel = new ViewModel.ViewParcelModel(parcelId, UpdatePWindow, close);
            DataContext = _viewParcelModel;
            InitializeComponent();
        }

        public ViewParcel(Action UpdatePWindow)
        {
            _viewParcelModel = new ViewModel.ViewParcelModel(UpdatePWindow);
            DataContext = _viewParcelModel;
            InitializeComponent();
        }
    }
}

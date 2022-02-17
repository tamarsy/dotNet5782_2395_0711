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
    public partial class ViewParcel : UserControl, Update
    {
        private readonly ViewModel.ViewParcelModel _viewParcelModel;
        public Action updateCurrentWindow { get; }
        public ViewParcel(int parcelId, Action updatePWindow, Action close, Action<object> addTab, Action<object> removeTab, bool IsCustomerI = true)
        {
            _viewParcelModel = new ViewModel.ViewParcelModel(parcelId, updatePWindow, close, addTab, removeTab, IsCustomerI);
            updateCurrentWindow = _viewParcelModel.updateCurrentWindow;
            DataContext = _viewParcelModel;
            InitializeComponent();
        }

        public ViewParcel(Action UpdateAndClose, int? id = null)
        {
            _viewParcelModel = new ViewModel.ViewParcelModel(UpdateAndClose, id);
            updateCurrentWindow = _viewParcelModel.updateCurrentWindow;
            DataContext = _viewParcelModel;
            InitializeComponent();
        }
    }
}


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
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for ViewDrones.xaml
    /// </summary>
    public partial class ViewListDrone : UserControl
    {

        private readonly ViewModel.ViewDroneListModel _viewListDrone;

        public ViewListDrone(Action<object> addTab, Action<object> removeTab)
        {
            _viewListDrone = new ViewModel.ViewDroneListModel(addTab, removeTab);
            DataContext = _viewListDrone;
            InitializeComponent();
        }

        private void viewDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e) => _viewListDrone.NewViewDroneCommand.Execute(((BO.DroneToList)viewDrones.SelectedItem).Id);
    }
}

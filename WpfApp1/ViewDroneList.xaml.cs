using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ViewListDrone.xaml
    /// </summary>
    public partial class ViewListDrone : Window
    {
        IBL.IBL bl;
        public ViewListDrone()
        {
            InitializeComponent();
            bl = IBL.BL.BLInstance;
            StatudSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            viewDrones.ItemsSource = bl.DronesList();
        }

        private void StatudSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Addrone_Click(object sender, RoutedEventArgs e)
        {
            new ViewDrone().Show();
        }

        private void viewDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ViewDrone(sender).Show();
        }
    }
}

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
        BlApi.IBL bl;
        IEnumerable<BO.DroneToList> drones;

        public ViewListDrone()
        {
            InitializeComponent();
            bl = BLApi.FactoryBL.GetBL();
            foreach (var item in Enum.GetValues(typeof(BO.DroneStatuses)))
            {
                StatusSelector.Items.Add(item);
            }
            StatusSelector.Items.Add("all");
            foreach (var item in Enum.GetValues(typeof(BO.WeightCategories)))
            {
                MaxWeightSelector.Items.Add(item);
            }
            MaxWeightSelector.Items.Add("all");

            InitializeData();
        }

        private void InitializeData(object sender = default, SelectionChangedEventArgs e = default)
        {
            drones = bl.DronesList().ToList();
            WeightAndStatudSelector_SelectionChanged();
            viewDrones.ItemsSource = drones;
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    MessageBox.Show("you cant cloze this window");
        //}

        private void WeightAndStatudSelector_SelectionChanged()
        {
            if (MaxWeightSelector.SelectedItem != null && !MaxWeightSelector.SelectedItem.Equals("all"))
            {
                drones = drones.Where((d) => (d.MaxWeight).Equals((BO.WeightCategories)MaxWeightSelector.SelectedItem)).ToList();
            }
            if (StatusSelector.SelectedItem != null && !StatusSelector.SelectedItem.Equals("all"))
            {
                drones = drones.Where((d) => (d.DroneStatuses).Equals((BO.DroneStatuses)StatusSelector.SelectedItem)).ToList();
            }
        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Addrone_Click(object sender, RoutedEventArgs e)
        {
            ViewDrone droneWindow = new ViewDrone();
            droneWindow.UpDateDronesWindow = () => InitializeData();
            droneWindow.Show();
        }


        private void viewDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewDrones.SelectedItem is BO.DroneToList drone)
            {
                ViewDrone droneWindow = new ViewDrone(drone.Id);
                droneWindow.UpDateDronesWindow = () => InitializeData();
                droneWindow.Show();
            }
        }
    }
}

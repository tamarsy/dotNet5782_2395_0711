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
    /// Interaction logic for ViewDrone.xaml
    /// </summary>
    public partial class ViewDrone : Window
    {

        public ViewDrone()
        {
            InitializeComponent();
            DroneDetails.Visibility = Visibility.Collapsed;
            bl = BLApi.FactoryBL.GetBL();
            weight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            Array stationsId = bl.StationsList().Select((s) => s.Id).ToArray();
            station.ItemsSource = stationsId;
            if (stationsId.Length != 0)
                station.SelectedItem = stationsId.GetValue(0);
        }
        private void AddDrone_click(object sender, RoutedEventArgs e)
        {
            int droneId = 0;
            foreach (var item in id.Text)
            {
                droneId = droneId*10 + (item - '0');
            }
            try
            {
                bl.AddDrone(droneId, model.Text, (BO.WeightCategories)weight.SelectedIndex, (int)station.SelectedItem);
                MessageBox.Show("successfully add");
                UpDateDronesWindow();
                Close();
            }
            catch (BO.ObjectAlreadyExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void is_num(object sender, TextChangedEventArgs e)
        {
            if (e.Source is TextBox t)
            {
                if ( t.CaretIndex!= 0 && id.Text.Length > 0 && !char.IsDigit(id.Text[t.CaretIndex - 1]))
                {
                    MessageBox.Show("ERROR enter only number for Id");
                    id.Text = id.Text.Remove(t.CaretIndex - 1, e.Changes.Count);
                }
            }
        }
    }
}

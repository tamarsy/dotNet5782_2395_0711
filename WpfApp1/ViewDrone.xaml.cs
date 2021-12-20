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
        BlApi.IBL bl;
        BO.Drone currentDrone;
        public Action UpDateDronesWindow { get; set; }

        public ViewDrone(int droneId)
        {
            InitializeComponent();
            AddDrone.Visibility = Visibility.Collapsed;
            bl = BLApi.FactoryBL.GetBL();
            try
            {
                InitializeData(droneId);
            }
            catch (BO.ObjectNotExistException e) { details.Text = e.Message; }
        }

        private void InitializeData(int droneId)
        {
            currentDrone = bl.GetDrone(droneId);
            string droneString = currentDrone.ToString();
            int i = droneString.IndexOf("Model:") + "Model: ".Length;
            droneString = droneString.Remove(i, droneString.IndexOf("\n", i) - i);
            details.DataContext = droneString;
            modelText.DataContext = currentDrone;
            ButtonsContent();
        }

        private void ButtonsContent()
        {
            DeliveryAndCollected.Visibility = Visibility.Visible;
            ChargeAndSupplied.Visibility = Visibility.Visible;
            if (currentDrone.DroneStatuses == BO.DroneStatuses.vacant)
            {
                ChargeAndSupplied.DataContext = "Chargh on";
                DeliveryAndCollected.DataContext = "Send to delivery";
            }
            else if (currentDrone.DroneStatuses == BO.DroneStatuses.sending)
            {
                if (currentDrone.Parcel == default)
                {
                    DeliveryAndCollected.DataContext = "collect Parcel";
                    ChargeAndSupplied.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DeliveryAndCollected.DataContext = "Parcel delivery";
                    ChargeAndSupplied.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ChargeAndSupplied.DataContext = "Chargh of";
                DeliveryAndCollected.Visibility = Visibility.Collapsed;
            }
        }

        private void ChargeOn()
        {
            try
            {
                bl.ChargeOn(currentDrone.Id);
                MessageBox.Show("successfully charge on");
            }
            catch (BO.ObjectNotExistException e) { MessageBox.Show(e.Message); }
        }

        private void ChargeOf()
        {
            bl.ChargeOf(id: currentDrone.Id);
            MessageBox.Show("successfully charge of");

        }
        private void ParcelToDrone()
        {

            bl.ParcelToDrone(currentDrone.Id);
            MessageBox.Show("successfully connect parcel to drone");

        }
        private void PickParcel()
        {
            bl.PickParcel(currentDrone.Id);
            MessageBox.Show("successfully pick the parcel by the drone");
        }
        private void Destination()
        {

            bl.Destination(currentDrone.Id);
            MessageBox.Show("successfully supplay the parcel by drone");

        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateDrone(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDrone(currentDrone.Id, modelText.Text);
                MessageBox.Show("Successfully update drone model");
                UpDateDronesWindow();
                updateModel.IsEnabled = false;
            }
            catch (BO.NoChangesToUpdateException ex) { MessageBox.Show(ex.Message); }
            catch (BO.ObjectNotExistException ex) { MessageBox.Show(ex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Bottuns_click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button b)
            {
                try
                {
                    if (b.Content.Equals("Chargh on"))
                        ChargeOn();
                    else if (b.Content.Equals("Send to delivery"))
                        ParcelToDrone();
                    else if (b.Content.Equals("collect Parcel"))
                        PickParcel();
                    else if (b.Content.Equals("Parcel delivery"))
                        Destination();
                    else if (b.Content.Equals("Chargh of"))
                        ChargeOf();
                    InitializeData(currentDrone.Id);
                    UpDateDronesWindow();
                }
                catch (BO.NoChangesToUpdateException ex) { MessageBox.Show(ex.Message); }
                catch (BO.ObjectNotExistException ex) { MessageBox.Show(ex.Message); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void Model_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (modelText.Text.Count() > 0 && !modelText.Text.Equals(currentDrone.Model))
                updateModel.IsEnabled = true;
            else
                updateModel.IsEnabled = false;
        }
    }
}


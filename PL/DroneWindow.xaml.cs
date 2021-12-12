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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private Ibl blObject;
        bool isInActionsState;
        public DroneWindow(Ibl obj) //add
        {
            InitializeComponent();
            isInActionsState = false;
            blObject = obj;
            //txtStatus.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //txtStatus.SelectedItem = DroneStatus.Maintenance;
            txtStationId.ItemsSource = blObject.DisplayListOfStations().Select(x => x.Id);
            txtStatus.Text = DroneStatus.Maintenance.ToString();
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        public DroneWindow(Ibl obj, int droneId) //actions
        {
            InitializeComponent();
            isInActionsState = true;
            blObject = obj;
            Drone drone = blObject.DisplayDrone(droneId);
            //לאתחל את כל הפקדים ולעשות את רובם readonly
            //להסתיר את כל הפקדים שלא צריך
            txtId.Text = drone.Id.ToString();
            txtLatti.Text= drone.CurrentLocation.Latti.ToString();
            txtLongi.Text= drone.CurrentLocation.Longi.ToString();
            txtModel.Text= drone.Model.ToString();
            txtBattery.Text = string.Format($"{drone.Battery:0.000}");
            txtStatus.Text= drone.Status.ToString();
            txtWeight.Text= drone.MaxWeight.ToString();

            txtId.IsReadOnly = true;
            txtLatti.IsReadOnly = true;
            txtModel.IsReadOnly = false;
            txtLongi.IsReadOnly = true;
            txtBattery.IsReadOnly = true;
            txtStatus.IsReadOnly = true;
            txtWeight.IsReadOnly = true;

            txtStationId.Visibility = Visibility.Hidden;
            StationLabel.Visibility = Visibility.Hidden;
        }

        private void txtStationId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location loc = blObject.DisplayStation((int)txtStationId.SelectedItem).Location;
            txtLatti.Text = loc.Latti.ToString();
            txtLongi.Text = loc.Longi.ToString();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)//להוסיף בדיקות תקינות וכו
        {
            int id;
            int.TryParse(txtId.Text, out id);
            int stationId;
            int.TryParse((string)txtStationId.SelectedItem, out stationId);
            blObject.AddDrone(id, txtModel.Text, (WeightCategories)txtWeight.SelectedItem, stationId);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (isInActionsState == false)
            {
                MessageBoxResult mb = MessageBox.Show("do you want to close the window? \n the drone will not be added", "cancel adding of drone", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                    this.Close();
            }
        }

        private void UpdateDroneModel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtId.Text, out id);
            try
            {
                blObject.UpdateDroneModel(id, txtModel.Text);
            }
            catch (IBL.NotExistIDException)
            {
                MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again");
            }
            MessageBox.Show("the model had updated!:)");
        }

        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);

            if (status != DroneStatus.Available)
            {
                MessageBox.Show("the drone is not available, it cant go to charge");
            }
            else
            {
                int id;
                int.TryParse(txtId.Text, out id);
                try
                {
                    blObject.SendDroneToCharge(id);
                }
                catch (IBL.NotExistIDException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (IBL.DroneCantGoToChargeException ex) 
                {
                    MessageBox.Show("Drone can't go to charge, apperantly there is not station that the drone can arrive to it");
                }

            }

        }

        private void ReleaseDroneFromCharge(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);
            if (txtStatus.Text != DroneStatus.Maintenance.ToString())
            {
                MessageBox.Show("the drone is not in maintenance, it cant realese from charge");
            }
            else
            {
                //timeInChargeLabel.Visibility = Visibility.Visible;
                //txtTimeInCharge.Visibility = Visibility.Visible;
                int id;
                int.TryParse(txtId.Text, out id);
                TimeSpan time;
                if (TimeSpan.TryParse(txtWeight.SelectedItem.ToString(), out time) == false)
                {
                    MessageBox.Show("the time is not good, change it");
                    return;
                }
                try
                {
                    blObject.ReleaseDroneFromeCharge(id, time);
                }
                catch (IBL.NotExistIDException ex)
                {
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    MessageBox.Show(ex.Message);
                }
                catch (IBL.DroneCantReleaseFromChargeException ex) { MessageBox.Show(ex.Message); }
            }

        }
    }
}

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
        public DroneWindow(Ibl obj)
        {
            InitializeComponent();
            blObject = obj;
            //txtStatus.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //txtStatus.SelectedItem = DroneStatus.Maintenance;
            txtStatus.Text = "Maintence";
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        public DroneWindow(Ibl obj, int droneId)
        {
            InitializeComponent();
            blObject = obj;
            Drone drone = blObject.DisplayDrone(droneId);
            //לאתחל את כל הפקדים ולעשות את רובם readonly
            //להסתיר את כל הפקדים שלא צריך
            txtId.Text = drone.Id.ToString();
            txtLatti.Text= drone.CurrentLocation.Latti.ToString();
            txtLongi.Text= drone.CurrentLocation.Longi.ToString();
            txtModel.Text= drone.Model.ToString();
            txtBattery.Text= drone.Battery.ToString();
            txtStatus.Text= drone.Status.ToString();
            txtWeight.Text= drone.MaxWeight.ToString();

            txtId.IsReadOnly = true;
            txtLatti.IsReadOnly = true;
            txtModel.IsReadOnly = true;
            txtLongi.IsReadOnly = true;
            txtBattery.IsReadOnly = true;
            txtStatus.IsReadOnly = true;
            txtWeight.IsReadOnly = true;

            txtStationId.Visibility = Visibility.Hidden;
            StationLabel.Visibility = Visibility.Hidden;
            txtStatus.Text = "Maintence";
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
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
            MessageBox.Show("do you want to close the window? \n the drone will not be added");
            this.Close();
        }
    }
}

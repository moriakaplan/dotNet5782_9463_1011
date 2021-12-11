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
            AddDroneOption.Visibility = Visibility.Visible;
            blObject = obj;
            //txtStatus.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //txtStatus.SelectedItem = DroneStatus.Maintenance;
            txtStatus.Text = "Maintence";
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)//להוסיף בדיקות תקינות וכו
        {
            int id;
            int.TryParse(txtId.Text, out id);
            int stationId;
            int.TryParse((string)txtStationId.SelectedItem, out stationId);
            blObject.AddDrone(id, txtModel.Text, (WeightCategories)txtWeight.SelectedItem, stationId);
        }
    }
}

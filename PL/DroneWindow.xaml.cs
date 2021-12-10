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
            //ActionsDroneOption.Visibility = Visibility.Hidden;
            blObject = obj;
            //txtStatus.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            IEnumerable<int> stationsId = blObject.DisplayListOfStations().Select(x => x.Id);
            txtStationId.ItemsSource = stationsId;
            
        }

        private void txtStationId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location loc = blObject.DisplayStation((int)txtStationId.SelectedItem).Location;
            txtLatti.Text = loc.Latti.ToString();
            txtLongi.Text = loc.Longi.ToString();
        }
    }
}

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
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        IBL blObject;
        public StationWindow(IBL obj, int stationId) //updating
        {
            InitializeComponent();
            blObject = obj;
            Station st = blObject.DisplayStation(stationId);
            DataContext = st;
            //locationGrid.DataContext = st.Location;
            //txtId.Text = st.Id.ToString();
            //txtName.Text = st.Name;
            //txtAvailableChargeSlots.Text = st.AvailableChargeSlots.ToString();
            //txtLatti.Text = st.Location.Latti.ToString();
            //txtLongi.Text = st.Location.Longi.ToString();
            txtDronesInCharge.DataContext = st.DronesInCharge;
        }

        public StationWindow(IBL obj) //adding
        {
            InitializeComponent();
            blObject = obj;
            txtId.Visibility = Visibility.Hidden;
        }

        private void viewDrone(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(blObject, ((BO.DroneToList)txtDronesInCharge.SelectedItem).Id).ShowDialog();
        }

        private void txtDronesInCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

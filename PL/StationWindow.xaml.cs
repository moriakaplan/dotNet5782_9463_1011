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
        /// <summary>
        /// constractor station window in update mode
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stationId"></param>
        public StationWindow(IBL obj, int stationId) //updating
        {
            InitializeComponent();
            blObject = obj;
            Station st = blObject.GetStation(stationId);
            DataContext = st;
            txtId.IsEnabled = false;
            txtLatti.IsEnabled = false;
            txtLongi.IsEnabled = false;
            gridDronesInCharge.DataContext = st.DronesInCharge;
            options.Content = "Update Station Data";
            options.Click -= addStation;
            options.Click += updateStation;
            
        }
        /// <summary>
        /// constractor station window in adding mode
        /// </summary>
        /// <param name="obj"></param>
        public StationWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            labelDronesInCharge.Visibility = Visibility.Collapsed;
            gridDronesInCharge.Visibility = Visibility.Collapsed;
            txtId.Text = "4 digits";
            options.Content = "Add Station";
            options.Click -= updateStation;
            options.Click += addStation;
        }
        /// <summary>
        /// add the station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addStation(object sender, RoutedEventArgs e)
        {
            int id, chargeSlots;
            if (int.TryParse(txtId.Text, out id) == false)//if the id is not good
            {
                MessageBox.Show("the id is not a valid number, please try again\n");
                return;
            }
            if (id < 1000 || id > 9999)//if the id is not 4 digits
            {
                MessageBox.Show("the id suppose to be a number with 4 digits, please choose another id and try again\n");
                return;
            }
            if (txtName.Text == null)//if there is no name
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (int.TryParse(txtAvailableChargeSlots.Text, out chargeSlots)==false || chargeSlots<0)//if the number of charge slots is not good
            {
                MessageBox.Show("the number of charge slots is not a valid number, please try again\n");
                return;
            }
            try
            {
                blObject.AddStation(id, txtName.Text, new Location { Latti = double.Parse(txtLatti.Text), Longi = double.Parse(txtLongi.Text) }, int.Parse(txtAvailableChargeSlots.Text));
                MessageBox.Show("The station added successfully");
                this.Close();
            }
            catch (ExistIdException)
            {
                MessageBox.Show("this id already exist, please choose another one and try again\n");
                txtId.Foreground = Brushes.Red;
            }
            
        }
        /// <summary>
        /// update the station dateils
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateStation(object sender, RoutedEventArgs e)
        {
            int chargeSlots;
            if (txtName.Text == null)
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (int.TryParse(txtAvailableChargeSlots.Text, out chargeSlots) == false || chargeSlots < 0)
            {
                MessageBox.Show("the number of charge slots is not a valid number, please try again\n");
                return;
            }
            blObject.UpdateStation(int.Parse(txtId.Text), txtName.Text, int.Parse(txtAvailableChargeSlots.Text));
        }

        /// <summary>
        /// determinate the color of the id text box, according to wich is valid or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idColor(object sender, TextChangedEventArgs e)
        {
            int id;
            if (txtId.IsEnabled == false) return;
            if (int.TryParse(txtId.Text, out id) && id >= 1000 && id <= 9999)
            {
                txtId.Foreground = Brushes.Green;
            }
            else
            {
                if (txtId.Text == "4 digits") txtId.Foreground = Brushes.Black;
                else txtId.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// By double-clicking on a row in the table of drone in charge a specific drone window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDrone(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(blObject, ((BO.DroneInCharge)gridDronesInCharge.SelectedItem).Id).ShowDialog();
        }   
    }
}

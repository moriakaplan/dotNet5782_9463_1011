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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        IBL blObject;
        public ManagerWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
        }
        private void displayDronesList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObject).Show();
        }

        private void displayStationList(object sender, RoutedEventArgs e)
        {
            new StationListWindow(blObject).ShowDialog();
        }

        private void displayParcelList(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(blObject).Show();
        }

        private void displayCustomerList(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(blObject).Show();
        }
    }
}
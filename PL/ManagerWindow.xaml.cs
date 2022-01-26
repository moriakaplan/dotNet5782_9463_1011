using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public ManagerWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            btnPass.DataContext = blObject;
        }
        
        /// <summary>
        /// open drones list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayDronesList(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObject).Show();
        }

        /// <summary>
        /// open stations list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayStationList(object sender, RoutedEventArgs e)
        {
            new StationListWindow(blObject).ShowDialog();
        }

        /// <summary>
        /// open parcels list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayParcelList(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(blObject).ShowDialog();
        }

        /// <summary>
        /// open customers window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayCustomerList(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(blObject).Show();
        }

        /// <summary>
        /// see or hide the manager password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seePassword(object sender, RoutedEventArgs e)
        {
            if (btnPass.Content.ToString() == "see managment password")
            {
                btnPass.Content = "hide managment password";
                pass.Content = blObject.GetManagmentPassword();
            }
            else
            {
                btnPass.Content = "see managment password";
                pass.Content = "";
            }
        }

        /// <summary>
        /// change the manager password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePass(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show($"Do you really want to change the password?\n it will changed in the computer of every manager", "change password", MessageBoxButton.OKCancel);
            if (mb == MessageBoxResult.OK)
            {
                string newPass = blObject.ChangeManagmentPassword();
                if (btnPass.Content.ToString() == "hide managment password")
                {
                    pass.Content = newPass;
                }
            }
        }

        /// <summary>
        /// open the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMain(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
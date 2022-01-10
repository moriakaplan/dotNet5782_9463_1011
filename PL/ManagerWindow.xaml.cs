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
        bool closeX = true;
        public ManagerWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            btnPass.DataContext = blObject;
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult mb;
            //mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);
            //if (mb == MessageBoxResult.No) e.Cancel=true;
            if (closeX) new MainWindow().Show();
        }

        private void displayDronesList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObject).Show();
            closeX = false;
            this.Close();
        }

        private void displayStationList(object sender, RoutedEventArgs e)
        {
            new StationListWindow(blObject).Show();
            closeX = false;
            this.Close();
            
        }

        private void displayParcelList(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(blObject).Show();
            closeX = false;
            this.Close();
        }

        private void displayCustomerList(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(blObject).Show();
             closeX = false;
            this.Close();
        }

        private void seePassword(object sender, RoutedEventArgs e)
        {
            if (btnPass.Content.ToString() == "see managment password")
            {
                btnPass.Content = "hide managment password";
                pass.Content = blObject.getManagmentPassword();
            }
            else
            {
                btnPass.Content = "see managment password";
                pass.Content = "";
            }
        }

        private void changePass(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show($"Do you really want to change the password?\n it will changed in the computer of every manager", "change password", MessageBoxButton.OKCancel);
            if (mb == MessageBoxResult.OK)
            {
                string newPass = blObject.changeManagmentPassword();
                if (btnPass.Content.ToString() == "hide managment password")
                {
                    pass.Content = newPass;
                }
            }
        }
    }
}
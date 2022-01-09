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
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window 
    {
        IBL blObject;
        public ParcelWindow(IBL obj, int Id)//update, need to add delete
        {
            InitializeComponent();
            blObject = obj;
            InitializeComponent();

            Parcel parcel = blObject.GetParcel(Id);
            DataContext = parcel;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;
            if (parcel.Drone != null) delete.Visibility = Visibility.Hidden;

            options.Content = "Update Parcel Data";
            options.Click -= AddParcel;
            options.Click += UpdateParcel;
        }

        public ParcelWindow(IBL obj) //add
        {
            InitializeComponent();
            blObject = obj;
            lblId.Visibility = Visibility.Collapsed;
            lblDrone.Visibility = Visibility.Collapsed;
            lblCreateTime.Visibility = Visibility.Collapsed;
            lblAssociateTime.Visibility = Visibility.Collapsed;
            lblPickUpTime.Visibility = Visibility.Collapsed;
            lblDeliverTime.Visibility = Visibility.Collapsed;
            txtId.Visibility = Visibility.Collapsed;
            txtDeliverTime.Visibility = Visibility.Collapsed;
            txtDrone.Visibility = Visibility.Collapsed;
            txtCreateTime.Visibility = Visibility.Collapsed;
            txtAssociateTime.Visibility = Visibility.Collapsed;
            txtPickUpTime.Visibility = Visibility.Collapsed;
            txtDeliverTime.Visibility = Visibility.Collapsed;
            delete.Visibility = Visibility.Hidden;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;
            options.Content = "Add Parcel";
            options.Click -= UpdateParcel;
            options.Click += AddParcel;
        }
        public ParcelWindow(IBL obj, bool isFromSender, int cusId) //updat
        {
            InitializeComponent();
            blObject = obj;

            options.Content = "Add Parcel";
            options.Click -= UpdateParcel;
            options.Click += AddParcel;
        }

        private void viewSender(object sender, MouseButtonEventArgs e)
        {
            if (txtSender.Text != "")
                new CustomerWindow(blObject, int.Parse(txtSender.Text)).ShowDialog();
        }

        private void viewTarget(object sender, MouseButtonEventArgs e)
        {
            if (txtTarget.Text != "")
                new CustomerWindow(blObject, int.Parse(txtTarget.Text)).ShowDialog();
        }

        private void viewDroneInParcel(object sender, MouseButtonEventArgs e)
        {
            if (txtDrone.Text != "")
                new DroneWindow(blObject, int.Parse(txtDrone.Text)).ShowDialog();
        }

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            blObject.AddParcelToDelivery((int)txtSender.SelectedItem, (int)txtTarget.SelectedItem, (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
        }

        private void UpdateParcel(object sender, RoutedEventArgs e)
        {

        }

        private void txtSender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTarget.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtSender.SelectedItem);
        }

        private void txtTarget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSender.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtTarget.SelectedItem);
        }

        private void DeleteParcel(object sender, RoutedEventArgs e)
        {
             MessageBoxResult mb= MessageBox.Show("Do you realy want to delete the parcel?", "delete parcel", MessageBoxButton.YesNo);
             if (mb == MessageBoxResult.Yes)
             {
                try
                {

                    blObject.DeleteParcel(int.Parse(txtId.Text));
                    MessageBox.Show("The parcel deleted successfully");
                    this.Close();
                }
                catch (NotExistIDException) { MessageBox.Show("something strange"); }
                catch (DeleteException) { MessageBox.Show("the parcel can't be deleted. apperently it associated to a drone."); }
            }
        }
            
    }
}

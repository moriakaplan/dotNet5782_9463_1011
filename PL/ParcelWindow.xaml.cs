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
    public partial class ParcelWindow : Window //update, need to add delete
    {
        IBL blObject;
        public ParcelWindow(IBL obj, int Id)
        {
            InitializeComponent();
            blObject = obj;
            InitializeComponent();

            Parcel parcel = blObject.GetParcel(Id);
            DataContext = parcel;
            //txtId.Text = parcel.Id.ToString();
            //txtDrone.Text = parcel.Drone.Id.ToString();
            //txtPriority.Text = parcel.Priority.ToString();
            //txtTarget.Text = parcel.Target.ToString();
            //txtWeight.Text = parcel.Weight.ToString();
            //txtCreateTime.Text = parcel.CreateTime.ToString();
            //txtAssociateTime.Text = parcel.AssociateTime.ToString();
            //txtPickUpTime.Text = parcel.PickUpTime.ToString();
            //txtDeliverTime.Text = parcel.DeliverTime.ToString();
            if (parcel.Drone == null) delete.Visibility = Visibility.Visible;

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
            txtId.Visibility = Visibility.Collapsed;
            txtDeliverTime.Visibility = Visibility.Collapsed;
            txtDrone.Visibility = Visibility.Collapsed;
            txtCreateTime.Visibility = Visibility.Collapsed;
            txtAssociateTime.Visibility = Visibility.Collapsed;
            txtPickUpTime.Visibility = Visibility.Collapsed;
            txtDeliverTime.Visibility = Visibility.Collapsed;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
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
            blObject.AddParcelToDelivery(int.Parse(txtSender.Text), int.Parse(txtTarget.Text), (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
        }

        private void UpdateParcel(object sender, RoutedEventArgs e)
        {

        }
    }
}

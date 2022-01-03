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
        public ParcelWindow(IBL obj, int Id, bool isFromSender=false)
        {
            InitializeComponent();
            blObject = obj;
            InitializeComponent();

            Parcel parcel = blObject.DisplayParcel(Id);
            DataContext = parcel;
            //txtId.Text = parcel.Id.ToString();
            //txtDrone.Text = parcel.Drone.Id.ToString();
            //txtPriority.Text = parcel.Priority.ToString();
            if (isFromSender)
            {
                txtSender.Text = Id.ToString();
                txtSender.IsEnabled = false;
            }
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

        }

        private void UpdateParcel(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
using System.Xml.Linq;
using System.Globalization;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL blObject;
        bool isUser;
        int userId;
        public ParcelWindow(IBL obj, int Id, bool flag = false)//update
        {
            if (!flag)
            {
                isUser = false;
                InitializeComponent();
                blObject = obj;
                InitializeComponent();
                grid.IsEnabled = false;
                delete.IsEnabled = true;
                timesVisibility.Visibility = Visibility.Visible;

                Parcel parcel = blObject.GetParcel(Id);
                DataContext = parcel;
                txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                var customersId = blObject.GetCustomersList().Select(x => x.Id);
                txtSender.ItemsSource = customersId;
                txtTarget.ItemsSource = customersId;
                if (parcel.Drone != null) delete.Visibility = Visibility.Hidden;
                add.Visibility = Visibility.Hidden;
                if (txtDrone.Text == "")
                {
                    btnDrone.Visibility = Visibility.Collapsed;
                    txtDrone.Visibility = Visibility.Collapsed;
                    lblDrone.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                isUser = true;
                InitializeComponent();

                blObject = obj;
                timesVisibility.Visibility = Visibility.Collapsed;
                lblDrone.Visibility = Visibility.Collapsed;
                txtDrone.Visibility = Visibility.Collapsed;
                btnDrone.Visibility = Visibility.Collapsed;
                delete.Visibility = Visibility.Collapsed;
                btnSender.Visibility = Visibility.Collapsed;
                txtSender.Visibility = Visibility.Collapsed;
                lblSender.Visibility = Visibility.Collapsed;


                txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                IEnumerable<int> /*var*/ customersId = blObject.GetCustomersList().Select(x => x.Id);
                txtSender.ItemsSource = customersId;
                //לתפוס חריגה
                txtSender.SelectedItem = Id/*id.ToString()*/ /*customersId.Where(x => x == id).SingleOrDefault()*/;
                //לתפוס חריגה
                txtTarget.ItemsSource = customersId;
                userId = Id;
                //txtSender.IsEnabled = false;
                InitializeComponent();

            }

        }

        public ParcelWindow(IBL obj) //add, need to add option to add from the user
        {
            InitializeComponent();
            blObject = obj;
            timesVisibility.Visibility = Visibility.Collapsed;
            isUser = false;
            lblDrone.Visibility = Visibility.Collapsed;
            txtDrone.Visibility = Visibility.Collapsed;
            btnDrone.Visibility = Visibility.Collapsed;
            delete.Visibility = Visibility.Collapsed;

            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;

        }

        //public ParcelWindow(IBL obj, int id, bool flag)
        //{
        //    InitializeComponent();

        //    blObject = obj;
        //    timesVisibility.Visibility = Visibility.Collapsed;
        //    lblDrone.Visibility = Visibility.Collapsed;
        //    txtDrone.Visibility = Visibility.Collapsed;
        //    btnDrone.Visibility = Visibility.Collapsed;
        //    delete.Visibility = Visibility.Collapsed;
        //    btnSender.Visibility = Visibility.Collapsed;

        //    txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        //    txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
        //    IEnumerable<int> /*var*/ customersId = blObject.GetCustomersList().Select(x => x.Id);
        //    txtSender.ItemsSource = customersId;
        //    //לתפוס חריגה
        //    txtSender.SelectedItem =  id/*id.ToString()*/ /*customersId.Where(x => x == id).SingleOrDefault()*/;
        //    //לתפוס חריגה
        //    txtTarget.ItemsSource = customersId;
        //    txtSender.IsEnabled = false;
        //    InitializeComponent();


        //}

        private void viewSender(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtSender.Text)).Show();
        }

        private void viewTarget(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtTarget.Text)).Show();
        }

        private void viewDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject, int.Parse(txtDrone.Text)).Show();
        }

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            if(!isUser)
            {
                int id = blObject.AddParcelToDelivery((int)txtSender.SelectedItem, (int)txtTarget.SelectedItem, (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
                MessageBox.Show($"your parcel added successfuly and got the number {id}");
            }
            else
            {
                int id = blObject.AddParcelToDelivery(userId, (int)txtTarget.SelectedItem, (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
                MessageBox.Show($"your parcel added successfuly and got the number {id}");
            }
            this.Close();
        }

        private void txtSender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isUser)
                txtTarget.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtSender.SelectedItem);
            //txtSenderName.Content = blObject.GetCustomer(int.Parse(txtSender.Text)).Name;
        }

        private void txtTarget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isUser)
                txtSender.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtTarget.SelectedItem);
            //txtTargetName.Content = blObject.GetCustomer(int.Parse(txtTarget.Text)).Name;
        }

        private void DeleteParcel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show("Do you realy want to delete the parcel?", "delete parcel", MessageBoxButton.YesNo);
            if (mb == MessageBoxResult.Yes)
            {
                try
                {

                    blObject.DeleteParcel(int.Parse(txtId.Text));
                    MessageBox.Show("The parcel deleted successfully");
                    this.Close();
                }
                catch (NotExistIDException) { MessageBox.Show("something strange"); }
                catch (DeleteException) { MessageBox.Show("the parcel can't be deleted. it associated to a drone."); }
            }
        }

    }
}

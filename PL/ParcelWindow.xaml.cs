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
        int userId;

        #region constructors
        /// <summary>
        /// constractor for update and add in user mode
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Id"></param>
        public ParcelWindow(IBL obj, int Id)//update
        {
            blObject = obj;
            InitializeComponent();
            grid.IsEnabled = false;
            options.IsEnabled = true;
            //timesVisibility.Visibility = Visibility.Visible;

            Parcel parcel = blObject.GetParcel(Id);
            DataContext = parcel;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;
            // if (parcel.Drone != null) options.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Hidden;
            //if (txtDrone.Text == "")
            //{
            //    lblDrone.Visibility = Visibility.Collapsed;
            //}
            refresh();
        }
        public ParcelWindow(IBL obj, int senderId, bool flag) //adding from customer
        {
            InitializeComponent();
            blObject = obj;
            //timesVisibility.Visibility = Visibility.Collapsed;
            hiddeTimes();
            lblId.Visibility = Visibility.Hidden;
            lblDrone.Visibility = Visibility.Collapsed;
            //txtDrone.Visibility = Visibility.Collapsed;
            //btnDrone.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;
            //btnSender.Visibility = Visibility.Collapsed;
            //txtSender.Visibility = Visibility.Collapsed;
            lblSender.Visibility = Visibility.Collapsed;


            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            userId = senderId;
            txtTarget.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != senderId);
            InitializeComponent();
        }

        /// <summary>
        /// constractor for adding in manager mode
        /// </summary>
        /// <param name="obj"></param>
        public ParcelWindow(IBL obj) //add -manager mode
        {
            InitializeComponent();
            blObject = obj;
            lblId.Visibility = Visibility.Hidden;
            //timesVisibility.Visibility = Visibility.Collapsed;
            hiddeTimes();
            lblDrone.Visibility = Visibility.Collapsed;
            //txtDrone.Visibility = Visibility.Collapsed;
            //btnDrone.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;

            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;

        }
        #endregion

        private void hiddeTimes()
        {
            lblCreateTime.Visibility = Visibility.Collapsed;
            lblAssociateTime.Visibility = Visibility.Collapsed;
            lblPickUpTime.Visibility = Visibility.Collapsed;
            lblDeliverTime.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// refresh the window when its in action state
        /// </summary>
        private void refresh()
        {
            Parcel parcel;
            lock (blObject)
            {
                parcel = blObject.GetParcel((DataContext as Parcel).Id);
                DataContext = parcel;
            }
            options.Visibility = Visibility;
            options.Click -= pickUpParcel;
            options.Click -= deliverParcel;
            options.Click -= deleteParcel;
            if (parcel.AssociateTime == null) //if the parcel is not accosiated yet
            {
                options.Content = "delete the parcel"; //option to delete the parcel 
                options.Click += deleteParcel;
            }
            else
            {
                if(parcel.PickUpTime==null)//if the parcel is accosiate but not picked up
                {
                    options.Content = "drone can \n pick the parcel"; //optin to pick up the parcel
                    options.Click += pickUpParcel;
                }
                else
                {
                    if (parcel.DeliverTime == null)//if the parcel id accosiated and picked up but not deliverd
                    {
                        options.Content = "deliver the parcel"; //option to deliver the parcel
                        options.Click += deliverParcel;
                    }
                    else//the parcel deliverd
                    {
                        options.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        /// <summary>
        /// deliver the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deliverParcel(object sender, RoutedEventArgs e)
        {
            int drone;
            int.TryParse(txtDrone.Text, out drone);
            if ((blObject.GetDrone(drone)).Status != DroneStatus.Delivery) //DroneStatus.Associated= didnt took the parcel.
            {
                MessageBox.Show("the drone is not in delivery");
                return;
            }
            else
            {
                try
                {
                    blObject.DeliverParcelByDrone(drone);//deliver the parcel
                }
                catch (NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (DroneCantTakeParcelException)
                {
                    MessageBox.Show("the drone cant deliver the parcel because its battery is not enugh. try to send the drone to charge");
                    return;
                }
                catch (TransferException)
                {
                    MessageBox.Show("there is a problem with the statuses of the parcel or the drone. please check the data and try again");
                    return;
                }
                catch (Exception ex)//***
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            MessageBox.Show("drone deliver the parcel successfully");
            refresh();
        }
        private void pickUpParcel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtDrone.Text, out id);
            if ((blObject.GetDrone(id)).Status != DroneStatus.Associated) //DroneStatus.Associated= didnt took the parcel.
            {
                MessageBox.Show("the drone is not Associated ");
                return;
            }
            else 
            {
                try
                {
                    blObject.PickParcelByDrone(id);
                }
                catch (NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                }
                catch (DroneCantTakeParcelException) { MessageBox.Show("drone cant deliver the parcel because its battery is not enugh. try to send the drone to charge"); return; }
                catch (TransferException) { MessageBox.Show("there is a problem with the statuses of the parcel or the drone. please check the data and try again"); return; }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }//***
            }
            MessageBox.Show("the drone piked up the parcel");
            refresh();
        }

        /// <summary>
        /// open the customer window in the sender details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewSender(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtSender.Text)).Show();
        }

        /// <summary>
        ///open the customer window in the target details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewTarget(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtTarget.Text)).Show();
        }

        /// <summary>
        /// open drone window with the drone in parcel details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject, int.Parse(txtDrone.Text)).Show();
        }

        /// <summary>
        /// open parcel window in add mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addParcel(object sender, RoutedEventArgs e)
        {
            int id;
            if (txtSender.Visibility==Visibility.Visible)
            {
                id = blObject.AddParcelToDelivery((int)txtSender.SelectedItem, (int)txtTarget.SelectedItem, (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
            }
            else
            {
                id = blObject.AddParcelToDelivery(userId, (int)txtTarget.SelectedItem, (WeightCategories)txtWeight.SelectedItem, (Priorities)txtPriority.SelectedItem);
            }
            MessageBox.Show($"your parcel added successfuly and got the number {id}");
            this.Close();
        }

        /// <summary>
        /// choose sender from the exist customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSenderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSender.Visibility == Visibility.Visible)
                txtTarget.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtSender.SelectedItem);
        }

        /// <summary>
        /// choose target from the exist customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTargetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSender.Visibility == Visibility.Visible)
                txtSender.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtTarget.SelectedItem);
        }

        /// <summary>
        /// delete the parcel - only if it not accosiated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteParcel(object sender, RoutedEventArgs e)
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

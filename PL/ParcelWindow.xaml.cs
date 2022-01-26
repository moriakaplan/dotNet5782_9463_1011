﻿using System;
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
        /// <summary>
        /// constractor for update and add in user mode
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Id"></param>
        /// <param name="flag"></param>
        public ParcelWindow(IBL obj, int Id, bool flag = false)
        {
            if (!flag)//update
            {
                isUser = false;
                InitializeComponent();
                blObject = obj;
                InitializeComponent();
                grid.IsEnabled = false;
                options.IsEnabled = true;
                timesVisibility.Visibility = Visibility.Visible;

                Parcel parcel = blObject.GetParcel(Id);
                DataContext = parcel;
                txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                var customersId = blObject.GetCustomersList().Select(x => x.Id);
                txtSender.ItemsSource = customersId;
                txtTarget.ItemsSource = customersId;
                add.Visibility = Visibility.Hidden;
                if (txtDrone.Text == "")
                {
                    btnDrone.Visibility = Visibility.Collapsed;
                    txtDrone.Visibility = Visibility.Collapsed;
                    lblDrone.Visibility = Visibility.Collapsed;
                }
                refresh();
            }
            else//user mode
            {
                isUser = true;
                InitializeComponent();

                blObject = obj;
                timesVisibility.Visibility = Visibility.Collapsed;
                lblDrone.Visibility = Visibility.Collapsed;
                txtDrone.Visibility = Visibility.Collapsed;
                btnDrone.Visibility = Visibility.Collapsed;
                options.Visibility = Visibility.Collapsed;
                btnSender.Visibility = Visibility.Collapsed;
                txtSender.Visibility = Visibility.Collapsed;
                lblSender.Visibility = Visibility.Collapsed;


                txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                IEnumerable<int>  customersId = blObject.GetCustomersList().Select(x => x.Id);
                txtSender.ItemsSource = customersId;
               
                txtSender.SelectedItem = Id;
                txtTarget.ItemsSource = customersId;
                userId = Id;
                InitializeComponent();

            }
        }

        /// <summary>
        /// constractor for adding in manager mode
        /// </summary>
        /// <param name="obj"></param>
        public ParcelWindow(IBL obj) //add -manager mode
        {
            InitializeComponent();
            blObject = obj;
            timesVisibility.Visibility = Visibility.Collapsed;
            isUser = false;
            lblDrone.Visibility = Visibility.Collapsed;
            txtDrone.Visibility = Visibility.Collapsed;
            btnDrone.Visibility = Visibility.Collapsed;
            options.Visibility = Visibility.Collapsed;

            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            var customersId = blObject.GetCustomersList().Select(x => x.Id);
            txtSender.ItemsSource = customersId;
            txtTarget.ItemsSource = customersId;

        }

        /// <summary>
        /// refresh the window
        /// </summary>
        private void refresh()
        {
            Parcel parcel;
            lock (blObject)
            {
                parcel = blObject.GetParcel((DataContext as Parcel).Id);
                DataContext = parcel;
            }

            if (parcel.AssociateTime == null)//if the parcel is not accosiate
            {
                lblPickUpTime.Visibility = Visibility.Collapsed;
                txtPickUpTime.Visibility = Visibility.Collapsed;
                lblDeliverTime.Visibility = Visibility.Collapsed;
                txtDeliverTime.Visibility = Visibility.Collapsed;
                lblAssociateTime.Visibility = Visibility.Collapsed;
                txtAssociateTime.Visibility= Visibility.Collapsed;
                options.Content = "delete";//option to delete the parcel 
                options.Click -= pickUpParcel;
                options.Click -= deliverParcel;
                options.Click -= deleteParcel;
                options.Click += deleteParcel;
            }
            else
            {
                if(parcel.PickUpTime==null)//if the parcel is accosiate but not picked up
                {
                    lblPickUpTime.Visibility = Visibility.Collapsed;
                    txtPickUpTime.Visibility = Visibility.Collapsed;
                    lblDeliverTime.Visibility = Visibility.Collapsed;
                    txtDeliverTime.Visibility = Visibility.Collapsed;
                    options.Content = " drone can &#xD;&#xA; pick the parcel";//optin to pick up the parcel
                    options.Click -= pickUpParcel;
                    options.Click -= deliverParcel;
                    options.Click -= deleteParcel;

                    options.Click += pickUpParcel;
                }
                else
                {
                    if (parcel.DeliverTime == null)//if the parcel id accosiated and picked up but not deliverd
                    {
                        lblPickUpTime.Visibility = Visibility.Visible;
                        txtPickUpTime.Visibility = Visibility.Visible;
                        lblDeliverTime.Visibility = Visibility.Collapsed;
                        txtDeliverTime.Visibility = Visibility.Collapsed;
                        options.Content = "the drone &#xD;&#xA; deliver the parcel";//option to deliver the parcel
                        options.Click -= pickUpParcel;
                        options.Click -= deliverParcel;
                        options.Click -= deleteParcel;

                        options.Click += deliverParcel;
                    }
                    else//the parcel deliverd
                    {
                        lblPickUpTime.Visibility = Visibility.Visible;
                        txtPickUpTime.Visibility = Visibility.Visible;
                        lblDeliverTime.Visibility = Visibility.Visible;
                        txtDeliverTime.Visibility = Visibility.Visible;
                        lblAssociateTime.Visibility = Visibility.Visible;
                        txtAssociateTime.Visibility = Visibility.Visible;
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
                    MessageBox.Show("drone cant deliver the parcel because its battery is not enugh. try to send the drone to charge");
                    return;
                }
                catch (TransferException)
                {
                    MessageBox.Show("there is a problem with the statuses of the parcel or the drone. please check the data and try again");
                    return;
                }
                catch (Exception ex)
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
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
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

        /// <summary>
        /// choose sender from the exist customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSenderSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isUser)
                txtTarget.ItemsSource = blObject.GetCustomersList().Select(x => x.Id).Where(x => x != (int)txtSender.SelectedItem);
        }

        /// <summary>
        /// choose target from the exist customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTargetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isUser)
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

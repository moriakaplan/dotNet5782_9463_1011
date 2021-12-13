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
using IBL;
using IBL.BO;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        enum Options
        {
            UpdateTheModel,
            SendDroneToCharge,
            ReleaseDroneFromCharge,
            SendDroneToDelivery,
            PickUpParcel,
            DeliverParcel,
        }
        private Ibl blObject;
        bool isInActionsState;
        bool canClose = false;
        /// <summary>
        /// constructor for making window for add drone
        /// </summary>
        /// <param name="obj"></param>
        public DroneWindow(Ibl obj) //add
        {
            InitializeComponent();
            isInActionsState = false;
            blObject = obj;

            rowBattery.Height = new GridLength(0);
            lblBattery.Visibility = Visibility.Hidden;
            txtBattery.Visibility = Visibility.Hidden;
            rowParcel.Height = new GridLength(0);
            lblParcel.Visibility = Visibility.Hidden;
            txtParcel.Visibility = Visibility.Hidden;

            txtId.Foreground = Brushes.Black;
            txtId.Text = "6 digits";
            update.Visibility = Visibility.Hidden;
            charge.Visibility = Visibility.Hidden;
            sendDeliver.Visibility = Visibility.Hidden;
            pickParcel.Visibility = Visibility.Hidden;
            deliver.Visibility = Visibility.Hidden;
            releaseFromCharge.Visibility = Visibility.Hidden;
            lblOptions.Visibility = Visibility.Hidden;
            updateOptions.Visibility = Visibility.Hidden;
            lblTimeInCharge.Visibility = Visibility.Hidden;
            txtTimeInCharge.Visibility = Visibility.Hidden;
            OKrelease.Visibility = Visibility.Hidden;

            txtStationId.ItemsSource = blObject.DisplayListOfStations().Select(x => x.Id);
            txtStatus.Text = DroneStatus.Maintenance.ToString();
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        /// <summary>
        /// constructor for making window for actions with specific drone
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="droneId"></param>
        public DroneWindow(Ibl obj, int droneId) //actions
        {
            InitializeComponent();
            isInActionsState = true;
            blObject = obj;
            Drone drone = blObject.DisplayDrone(droneId);
            //לאתחל את כל הפקדים ולעשות את רובם readonly
            //להסתיר את כל הפקדים שלא צריך
            txtId.Text = drone.Id.ToString();
            txtLatti.Text = drone.CurrentLocation.Latti.ToString();
            txtLongi.Text = drone.CurrentLocation.Longi.ToString();
            txtModel.Text = drone.Model.ToString();
            txtBattery.Text = string.Format($"{drone.Battery:0.000}");
            txtStatus.Text = drone.Status.ToString();
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            txtWeight.Text = drone.MaxWeight.ToString();
            if (drone.ParcelInT == null)
                txtParcel.Text = " -";
            else
                txtParcel.Text = drone.ParcelInT.Id.ToString();
            //Array optionsNames=new Array[6];
            //optionsNames.SetValue("update the model", 0);
            //optionsNames.SetValue("send drone to charge", 1);
            //optionsNames.SetValue("release drone from charge", 2);
            //optionsNames.SetValue("send drone to delivery", 3);
            //optionsNames.SetValue("pick up parcel", 4);
            //optionsNames.SetValue("deliver parcel", 5);
            //options.ItemsSource = optionsNames;
            updateOptions.ItemsSource = Enum.GetValues(typeof(Options));

            txtId.IsReadOnly = true;
            txtWeight.IsReadOnly = true;
            txtId.Foreground = Brushes.Gray;
            txtWeight.Foreground = Brushes.Gray;

            rowStation.Height = new GridLength(0);
            txtStationId.Visibility = Visibility.Hidden;
            lblStation.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Hidden;
            lblTimeInCharge.Visibility = Visibility.Hidden;
            txtTimeInCharge.Visibility = Visibility.Hidden;
            OKrelease.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// add new drone to the data source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtId.Text, out id) == false) 
            { 
                MessageBox.Show("the id is not a valid number, please try again\n"); 
                return; 
            }
            if (id < 100000 || id > 999999) 
            { 
                MessageBox.Show("the id suppose to be a number with 6 digits, please choose another id and try again\n"); 
                return; 
            }
            if (txtModel.Text == null) 
            { 
                MessageBox.Show("please enter a model\n"); 
                return; 
            }
            if (txtWeight == null) 
            { 
                MessageBox.Show("please enter maximum weight\n"); 
                return; 
            }
            int stationId = int.Parse(txtStationId.SelectedItem.ToString());
            try
            {
                blObject.AddDrone(id, txtModel.Text, (WeightCategories)txtWeight.SelectedItem, stationId);
                MessageBox.Show("The drone added successfully");
            }
            catch (IBL.ExistIdException)
            {
                MessageBox.Show("this id already exist, please choose another one and try again\n");
                txtId.Foreground = Brushes.Red;
            }
        }
        /// <summary>
        /// show the user the location of the station he choosed, where the drone will be for first charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStationId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location loc = blObject.DisplayStation((int)txtStationId.SelectedItem).Location;
            txtLatti.Text = loc.Latti.ToString();
            txtLongi.Text = loc.Longi.ToString();
        }
        /// <summary>
        /// update the model of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDroneModel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtId.Text, out id);
            if(txtModel.Text == blObject.DisplayDrone(id).Model)
            {
                MessageBox.Show("there nothing to update"); 
                return;
            }
            try
            {
                blObject.UpdateDroneModel(id, txtModel.Text);
            }
            catch (IBL.NotExistIDException)
            {
                MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            MessageBox.Show("the model updated successfully");
        }
        /// <summary>
        /// send the drone to charge in the closest station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);

            if (status != DroneStatus.Available)
            {
                MessageBox.Show("the drone is not available, it cant go to charge");
                return;
            }
            else
            {
                int id;
                int.TryParse(txtId.Text, out id);
                try
                {
                    blObject.SendDroneToCharge(id);
                }
                catch (IBL.NotExistIDException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (IBL.DroneCantGoToChargeException)
                {
                    MessageBox.Show("Drone can't go to charge, apperantly there is not station that the drone can arrive to it");
                    return;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            }
            MessageBox.Show("drone sent successfully");
        }
        /// <summary>
        /// ask the user how long the drone has been charging, realese it from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseDroneFromCharge(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);
            if (txtStatus.Text != DroneStatus.Maintenance.ToString())
            {
                MessageBox.Show("the drone is not in maintenance, it cant realese from charge");
            }
            else
            {
                //timeInChargeLabel.Visibility = Visibility.Visible;
                //txtTimeInCharge.Visibility = Visibility.Visible;
                lblTimeInCharge.Visibility = Visibility.Visible;
                txtTimeInCharge.Visibility = Visibility.Visible;
                OKrelease.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// after the user entered time- release the drone from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseAfterGetTime(object sender, RoutedEventArgs e)
        {
            TimeSpan time;
            if (TimeSpan.TryParse(txtTimeInCharge.Text.ToString(), out time) == false)
            {

                MessageBox.Show("the time is not good, change it");
                return;
            }
            try
            {
                blObject.ReleaseDroneFromeCharge(int.Parse(txtId.Text), time);
            }
            catch (IBL.NotExistIDException ex)
            {
                //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                MessageBox.Show(ex.Message);
                return;
            }
            catch (IBL.DroneCantReleaseFromChargeException ex) { MessageBox.Show(ex.Message); return; }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            MessageBox.Show("the drone released successfully");
            lblTimeInCharge.Visibility = Visibility.Hidden;
            txtTimeInCharge.Visibility = Visibility.Hidden;
            OKrelease.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Send Drone To Delivery- Assign Parcel To the Drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneToDelivery(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);
            if (txtStatus.Text != DroneStatus.Available.ToString())
            {
                MessageBox.Show("the drone is not available");
                return;
            }
            else
            {
                int id;
                int.TryParse(txtId.Text, out id);
                try
                {
                    // blObject.SendDroneToCharge(id);
                    blObject.AssignParcelToDrone(id);
                }
                catch (IBL.NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                }
                catch (IBL.DroneCantTakeParcelException)
                {
                    MessageBox.Show("drone cant be accociated");
                    return;
                }
                catch (IBL.ThereNotGoodParcelToTake)
                {
                    MessageBox.Show("drone cant take a parcel because its battery not enugh. \nTry to send the drone to charge");
                    return;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
                MessageBox.Show("the drone has send to delivary");
            }
        }
        /// <summary>
        /// Pick Up Parcel by the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickUpParcel(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            int id;
            DroneStatus.TryParse(txtStatus.Text, out status);
            int.TryParse(txtParcel.Text, out id);
            if (txtStatus.Text != DroneStatus.Associated.ToString() && blObject.DisplayParcel(id).PickUpTime == null)//#צריך איכשהו לבדוק אם החבילה לאנאספה
            {
                MessageBox.Show("the drone is not Associated");
                return;
            }
            else
            {
                int.TryParse(txtId.Text, out id);
                try
                {
                    blObject.PickParcelByDrone(id);
                }
                catch (IBL.NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                }
                catch (IBL.DroneCantTakeParcelException) { MessageBox.Show("drone cant deliver the parcel because its battery is not enugh. try to send the drone to charge"); return; }
                catch (IBL.TransferException) { MessageBox.Show("there is a problem with the statuses of the parcel or the drone. please check the data and try again"); return; }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            }
            MessageBox.Show("the drone piked up the parcel");
        }
        /// <summary>
        /// deliver the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeliverParcel(object sender, RoutedEventArgs e)
        {
            int id, parcelId;
            int.TryParse(txtParcel.Text, out parcelId);
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);
            if (txtStatus.Text != DroneStatus.Delivery.ToString()
                && blObject.DisplayParcel(parcelId).PickUpTime != null
                && blObject.DisplayParcel(parcelId).DeliverTime == null)//#צריך איכשהו לבדוק אם החבילה לאנאספה
            {
                MessageBox.Show("the drone is not in delivery");
                return;
            }
            else
            {
                int.TryParse(txtId.Text, out id);
                try
                {
                    blObject.DeliverParcelByDrone(id);
                }
                catch (IBL.NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (IBL.DroneCantTakeParcelException) { MessageBox.Show("drone cant deliver the parcel because its battery is not enugh. try to send the drone to charge"); return; }
                catch (IBL.TransferException) { MessageBox.Show("there is a problem with the statuses of the parcel or the drone. please check the data and try again"); return; }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            }
            MessageBox.Show("drone deliver the parcel successfully");
        }
        /// <summary>
        /// determinate the color of the id text box, according to wich is valid or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdColor(object sender, TextChangedEventArgs e)
        {
            int id;
            if (int.TryParse(txtId.Text, out id) && id >= 100000 && id <= 999999)
            {
                txtId.Foreground = Brushes.Green;
            }
            else
            {
                if (txtId.Text == "6 digits") txtId.Foreground = Brushes.Black;
                else txtId.Foreground = Brushes.Red;
            }
        }
        /// <summary>
        /// go to the fuction that do what the user choosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectOption(object sender, SelectionChangedEventArgs e)
        {
            switch (updateOptions.SelectedItem)
            {
                case (Options.DeliverParcel):
                    DeliverParcel(sender, e);
                    break;
                case (Options.PickUpParcel):
                    PickUpParcel(sender, e);
                    break;
                case (Options.ReleaseDroneFromCharge):
                    ReleaseDroneFromCharge(sender, e);
                    break;
                case (Options.SendDroneToCharge):
                    SendDroneToCharge(sender, e);
                    break;
                case (Options.SendDroneToDelivery):
                    SendDroneToDelivery(sender, e);
                    break;
                case (Options.UpdateTheModel):
                    UpdateDroneModel(sender, e);
                    break;

            }
        }
        /// <summary>
        /// in actions window- paint the model green when the user change it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtModel_TextChanged(object sender, TextChangedEventArgs e) //color for model
        {
            if (isInActionsState && txtModel.Text == blObject.DisplayDrone(int.Parse(txtId.Text)).Model)
                txtModel.Foreground = Brushes.Black;
            else
                txtModel.Foreground = Brushes.Green;
        }
        /// <summary>
        /// close the window after clicking the close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            canClose = true;
            MessageBoxResult mb;
            if (isInActionsState == false)
                mb = MessageBox.Show("do you want to close the window? \n the drone will not be added", "cancel adding of drone", MessageBoxButton.YesNo);
            else
                mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);

            if (mb == MessageBoxResult.Yes)
                this.Close();
        }
        /// <summary>
        /// prevenet closing the window with the X button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (canClose == false)
            {
                MessageBox.Show("don't close with the x button, close with the close window button");
                e.Cancel = true;
            }
        }
    }
}

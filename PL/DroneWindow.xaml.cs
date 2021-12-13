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

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private Ibl blObject;
        bool isInActionsState;
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
            close.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Hidden;
            charge.Visibility = Visibility.Hidden;
            sendDeliver.Visibility = Visibility.Hidden;
            pickParcel.Visibility = Visibility.Hidden;
            deliver.Visibility = Visibility.Hidden;
            releaseFromCharge.Visibility = Visibility.Hidden;
            lblTimeInCharge.Visibility = Visibility.Hidden;
            txtTimeInCharge.Visibility = Visibility.Hidden;
            
            txtStationId.ItemsSource = blObject.DisplayListOfStations().Select(x => x.Id);
            txtStatus.Text = DroneStatus.Maintenance.ToString();
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
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
            txtWeight.Text = drone.MaxWeight.ToString();
            if (drone.ParcelInT == null)
                txtParcel.Text = " -";
            else
                txtParcel.Text = drone.ParcelInT.Id.ToString();

            txtId.IsReadOnly = true;
            txtLatti.IsReadOnly = true;
            txtModel.IsReadOnly = false;
            txtLongi.IsReadOnly = true;
            txtBattery.IsReadOnly = true;
            txtStatus.IsReadOnly = true;
            txtWeight.IsReadOnly = true;
            txtId.SelectionTextBrush = Brushes.Red;
            txtLatti.SelectionTextBrush = Brushes.Gray;
            txtLongi.SelectionTextBrush = Brushes.Gray;
            txtBattery.SelectionTextBrush = Brushes.Gray;
            txtParcel.SelectionTextBrush = Brushes.Gray;
            
            rowStation.Height = new GridLength(0);
            txtStationId.Visibility = Visibility.Hidden;
            lblStation.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Hidden;
        }

        private void txtStationId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Location loc = blObject.DisplayStation((int)txtStationId.SelectedItem).Location;
            txtLatti.Text = loc.Latti.ToString();
            txtLongi.Text = loc.Longi.ToString();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)//להוסיף בדיקות תקינות וכו
        {
            int id;
            //if (!checkId()) MessageBox.Show("the id is not correct, please try again\n");
            if (txtModel.Text == null) MessageBox.Show("please enter an id\n");
            if (int.TryParse(txtId.Text, out id)==false) MessageBox.Show("the id is not correct, please try again\n");
            if(id<=0) MessageBox.Show("the id suppose to be possible, please try again\n");
            if (txtModel.Text == null) MessageBox.Show("please enter a model\n");
            if (txtWeight == null) MessageBox.Show("please enter maximum weight\n");
            else
            {
                int stationId;
                int.TryParse((string)txtStationId.SelectedItem, out stationId);
                try
                {
                    blObject.AddDrone(id, txtModel.Text, (WeightCategories)txtWeight.SelectedItem, stationId);
                    MessageBox.Show("The drone added successfully");
                    //עדכון רשימת הרחפנים
                }
                catch (IBL.ExistIdException)
                {
                    MessageBox.Show("this id already exist, please choose another one and try again\n");
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (isInActionsState == false)
            {
                MessageBoxResult mb = MessageBox.Show("do you want to close the window? \n the drone will not be added", "cancel adding of drone", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                    this.Close();
            }
            else
            {
                MessageBoxResult mb = MessageBox.Show("do you want to close the window?","close", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                    this.Close();
            }
        }

        private void UpdateDroneModel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtId.Text, out id);
            try
            {
                blObject.UpdateDroneModel(id, txtModel.Text);
            }
            catch (IBL.NotExistIDException)
            {
                MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again");
            }
            MessageBox.Show("the model updated successfully");
        }

        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);

            if (status != DroneStatus.Available)
            {
                MessageBox.Show("the drone is not available, it cant go to charge");
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
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (IBL.DroneCantGoToChargeException)
                {
                    MessageBox.Show("Drone can't go to charge, apperantly there is not station that the drone can arrive to it");
                }

            }
            MessageBox.Show("drone sent successfully");//#צריך לשלוח את זה רק אם הוא לא זרק כלום
        }

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
                int id;
                int.TryParse(txtId.Text, out id);
                TimeSpan time;
                if (TimeSpan.TryParse(txtTimeInCharge.Text.ToString(), out time) == false)
                {

                    MessageBox.Show("the time is not good, change it");
                    return;
                }
                try
                {
                    blObject.ReleaseDroneFromeCharge(id, time);
                }
                catch (IBL.NotExistIDException ex)
                {
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    MessageBox.Show(ex.Message);
                }
                catch (IBL.DroneCantReleaseFromChargeException ex) { MessageBox.Show(ex.Message); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

            }
            MessageBox.Show("the drone realesed successfully");//#לזרוק רק אם הוא לא זרק כלום
        }

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
                    blObject.SendDroneToCharge(id);
                }
                catch (IBL.NotExistIDException)
                {
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                }
                catch (IBL.DroneCantTakeParcelException) { MessageBox.Show("drone cant be accociated");
                    return;
                }
                MessageBox.Show("the drone has send to delivary");
            }
        }

        private void PickUpParcel(object sender, RoutedEventArgs e)
        {
            DroneStatus status;
            int id;
            DroneStatus.TryParse(txtStatus.Text, out status);
            int.TryParse(txtParcel.Text, out id);
            if (txtStatus.Text != DroneStatus.Associated.ToString()&&blObject.DisplayParcel(id).PickUpTime==null)//#צריך איכשהו לבדוק אם החבילה לאנאספה
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
                catch (IBL.DroneCantTakeParcelException) { MessageBox.Show("Drone cant pick up the parcel"); return; }

            }
            MessageBox.Show("the drone piked up the parcel");
        }

        private void DeliverParcel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtParcel.Text, out id);
            DroneStatus status;
            DroneStatus.TryParse(txtStatus.Text, out status);
            if (txtStatus.Text != DroneStatus.Delivery.ToString()&& blObject.DisplayParcel(id).PickUpTime != null&& blObject.DisplayParcel(id).DeliverTime == null)//#צריך איכשהו לבדוק אם החבילה לאנאספה
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
                catch (IBL.DroneCantTakeParcelException) { MessageBox.Show("drone cant deliver the parcel"); return; }
            }
            MessageBox.Show("drone deliver the parcel successfully");
        }

        private void IdColor(object sender, TextChangedEventArgs e)
        {
            int id;
            if (txtId.Text==null || (int.TryParse(txtId.Text, out id) && id > 0))
            {
                txtId.BorderBrush = Brushes.White;
                txtId.Background = Brushes.White;
            }
            else
            {
                txtId.BorderBrush = Brushes.Red;
                txtId.Background = Brushes.Red;
            }
        }
        //private bool checkId()
        //{
        //    int id;
        //    if (txtId.Text == null) return false;
        //    if (int.TryParse(txtId.Text, out id)==false) return false;
        //    return id > 0;
        //}
    }
}

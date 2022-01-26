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
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        
        private IBL blObject;
        bool isInActionsState;
        bool canClose = false;
        BackgroundWorker worker;
        Visibility myVisibility = Visibility.Visible;

        #region constructors
        /// <summary>
        /// constructor for making window for add drone
        /// </summary>
        /// <param name="obj"></param>
        public DroneWindow(IBL obj) //add
        {
            InitializeComponent();
            isInActionsState = false;
            blObject = obj;

            lblBattery.Visibility= Visibility.Collapsed;
            //txtBattery.Visibility= Visibility.Collapsed;
            //prgrsBattery.Visibility= Visibility.Collapsed;
            lblStatus.Visibility = Visibility.Collapsed;
            //txtStatus.Visibility = Visibility.Collapsed;
            lblParcel.Visibility = Visibility.Collapsed;
            //txtParcel.Visibility = Visibility.Collapsed;

            //txtId.Text = "6 digits";
            options.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Hidden;
            charge.Visibility = Visibility.Hidden;
            
            txtStationId.ItemsSource = blObject.GetStationsList().Select(x => x.Id);
            txtStatus.Text = DroneStatus.Maintenance.ToString();
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        /// <summary>
        /// constructor for open window for actions with specific drone
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="droneId"></param>
        public DroneWindow(IBL obj, int droneId) //actions
        {
            InitializeComponent();
            isInActionsState = true;
            blObject = obj;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            //txtBattery.Text = string.Format($"{drone.Battery:0.000}");
            //txtStatus.Text = drone.Status.ToString();
            //txtWeight.Text = drone.MaxWeight.ToString();
            //if (drone.ParcelInT == null)
            //    txtParcel.Text = " -";
            //else
            //    txtParcel.Text = drone.ParcelInT.Id.ToString();

            txtId.IsEnabled = false;
            txtWeight.IsEnabled = false;

            txtStationId.Visibility = Visibility.Collapsed;
            lblStation.Visibility = Visibility.Collapsed;
            add.Visibility = Visibility.Hidden;

            txtId.Text = droneId.ToString();
            refresh();
        }
        #endregion

        #region simulation
        private void simulator(object sender, RoutedEventArgs e)
        {
            btnSimulator.Content = "manual state";
            btnSimulator.Click -= simulator;
            btnSimulator.Click += manual;
            myVisibility = Visibility.Hidden;
            worker = new()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(int.Parse(txtId.Text));
        }
        private void manual(object sender, RoutedEventArgs e)
        {
            btnSimulator.Content = "automatic state";
            btnSimulator.Click -= manual;
            btnSimulator.Click += simulator;
            myVisibility = Visibility.Visible;
            worker.CancelAsync();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            blObject.RunsTheSimulator((int)e.Argument, ()=>worker.ReportProgress(0), () => worker.CancellationPending);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            refresh();
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker = null; //if the window need to be closed - boolean variable, that is true if the user want to close the window in the middle of auto mode
        }
        #endregion

        #region actions etc
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
                canClose = true;
                this.Close();
            }
            catch (ExistIdException)
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
            Location loc = blObject.GetStation((int)txtStationId.SelectedItem).Location;
            txtLatti.Content = loc.Latti.ToString();
            txtLongi.Content = loc.Longi.ToString();
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
            if (txtModel.Text == blObject.GetDrone(id).Model)
            {
                MessageBox.Show("there nothing to update");
                return;
            }
            try
            {
                blObject.UpdateDroneModel(id, txtModel.Text);
            }
            catch (NotExistIDException)
            {
                MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            MessageBox.Show("the model updated successfully");
            txtModel.Foreground = Brushes.Black;
        }
        /// <summary>
        /// send the drone to charge in the closest station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            int id;
            if (txtStatus.Text != DroneStatus.Available.ToString())
            {
                MessageBox.Show("the drone is not available, it cant go to charge");
                return;
            }
            else
            {
                int.TryParse(txtId.Text, out id);
                try
                {
                    blObject.SendDroneToCharge(id);
                }
                catch (NotExistIDException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                    return;
                    //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                }
                catch (DroneCantGoToChargeException)
                {
                    MessageBox.Show("Drone can't go to charge, apperantly there is not station that the drone can arrive to it");
                    return;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            }
            MessageBox.Show("drone sent successfully");
            //InitialiseData(id);
            //DataContext = blObject.GetDrone(id);//?צריך
            //charge.Visibility = Visibility.Hidden;
            //sendDeliver.Visibility = Visibility.Hidden;
            //releaseFromCharge.Visibility = Visibility.Visible;
            //options.Content = "release drone\nfrom charge";
            //options.Click -= SendDroneToDelivery;
            //options.Click += ReleaseDroneFromCharge;
            refresh();
        }
        /// <summary>
        /// ask the user how long the drone has been charging, realese it from charge
        /// after the user entered time- release the drone from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseDroneFromCharge(object sender, RoutedEventArgs e)
        {
            if (txtStatus.Text != DroneStatus.Maintenance.ToString())
            {
                MessageBox.Show("the drone is not in maintenance, it cant realese from charge");
                return;
            }
            int id = int.Parse(txtId.Text);
            try
            {
                blObject.ReleaseDroneFromeCharge(id);
            }
            catch (NotExistIDException ex)
            {
                //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                MessageBox.Show(ex.Message);
                return;
            }
            catch (DroneCantReleaseFromChargeException ex) { MessageBox.Show(ex.Message); return; }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            MessageBox.Show("the drone released successfully");
            //InitialiseData(id);
            DataContext = blObject.GetDrone(id);//?צריך
            //releaseFromCharge.Visibility = Visibility.Hidden;
            //sendDeliver.Visibility = Visibility.Visible;
            charge.Visibility = Visibility.Visible;
            //options.Content = "send drone\nto delivery";
            //options.Click -= ReleaseDroneFromCharge;
            //options.Click += SendDroneToDelivery;
            refresh();
        }
        /// <summary>
        /// Send Drone To Delivery- Assign Parcel To the Drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneToDelivery(object sender, RoutedEventArgs e)
        {
            if (txtStatus.Text != DroneStatus.Available.ToString())
            {
                MessageBox.Show("the drone is not available");
                return;
            }
            int id;
            int.TryParse(txtId.Text, out id);
            try
            {
                // blObject.SendDroneToCharge(id);
                blObject.AssignParcelToDrone(id);
            }
            catch (NotExistIDException)
            {
                MessageBox.Show("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                return;
            }
            catch (DroneCantTakeParcelException)
            {
                MessageBox.Show("drone cant be associated");
                return;
            }
            catch (ThereNotGoodParcelToTakeException)
            {
                MessageBox.Show("drone cant take a parcel because its battery not enugh. \nTry to send the drone to charge");
                return;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            MessageBox.Show("the drone has send to delivary");
            charge.Visibility = Visibility.Hidden;
            //sendDeliver.Visibility = Visibility.Hidden;
            //pickParcel.Visibility = Visibility.Visible;
            //options.Content = "pick up\nparcel";
            //options.Click -= SendDroneToDelivery;
            //options.Click += PickUpParcel;
            refresh();
        }
        /// <summary>
        /// Pick Up Parcel by the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickUpParcel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtParcel.Text, out id);
            if (txtStatus.Text != DroneStatus.Associated.ToString()) //DroneStatus.Associated= didnt took the parcel.
            {
                MessageBox.Show("the drone is not Associated or the parcel already has been picked up");
                return;
            }
            else //רחפן במשלוח, החבילה לא נאספה
            {
                int.TryParse(txtId.Text, out id);
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
            //InitialiseData(id);
            DataContext = blObject.GetDrone(id);//?צריך
            //pickParcel.Visibility = Visibility.Hidden;
            //deliver.Visibility = Visibility.Visible;
            //options.Content = "deliver\nparcel";
            //options.Click -= PickUpParcel;
            //options.Click += DeliverParcel;
            refresh();
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
            if (txtStatus.Text != DroneStatus.Delivery.ToString()) //DroneStatus.Delivery= already took the parcel.
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
            //InitialiseData(id);
            //deliver.Visibility = Visibility.Hidden;
            //sendDeliver.Visibility = Visibility.Visible;
            charge.Visibility = Visibility.Visible;
            //options.Content = "send drone\nto delivery";
            //options.Click -= DeliverParcel;
            //options.Click += SendDroneToDelivery;
            refresh();
        }
        
        bool somethingHasChanged()
        {
            if (isInActionsState)
            {
                return (txtModel.Foreground == Brushes.Green); //its green if its has changed (and it is the only thing that can be changed).
            }
            else
            {
                return (txtId.Text != "" || txtModel.Text != "" || txtWeight.SelectedItem != null || txtStationId != null);
            }
        }

        private void ViewParcelInTransfer(object sender, MouseButtonEventArgs e)
        {
            if (txtParcel.Text != "")
                new ParcelWindow(blObject, int.Parse(txtParcel.Text)).ShowDialog();
        }

        private void refresh()//just for action state
        {
            Drone drone;
            lock (blObject)
            {
                drone = blObject.GetDrone(int.Parse(txtId.Text));
                DataContext = drone;
            }

            if (drone.ParcelInT == null) lblParcel.Visibility = Visibility.Collapsed;
            else lblParcel.Visibility = Visibility.Visible;
            options.Visibility = myVisibility;
            update.Visibility = myVisibility;
            charge.Visibility = Visibility.Hidden;
            if (myVisibility == Visibility.Hidden) return;
            options.Click -= SendDroneToDelivery;
            options.Click -= ReleaseDroneFromCharge;
            options.Click -= PickUpParcel;
            options.Click -= DeliverParcel;
            switch (drone.Status) //show to thw user the right option according to the status of the drone
            {
                case DroneStatus.Available:
                    charge.Visibility = myVisibility; //Visibility.Visible;
                    options.Content = "send drone\nto delivery";
                    options.Click += SendDroneToDelivery;
                    break;
                case DroneStatus.Maintenance:
                    options.Content = "release drone\nfrom charge";
                    options.Click += ReleaseDroneFromCharge;
                    break;
                case DroneStatus.Associated:
                    options.Content = "pick up\nparcel";
                    options.Click += PickUpParcel;
                    break;
                case DroneStatus.Delivery:
                    options.Content = "deliver\nparcel";
                    options.Click += DeliverParcel;
                    break;
            }
        }

        #endregion

        #region close
        /// <summary>
        /// close the window after clicking the close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            canClose = true;
            MessageBoxResult mb;
            if (btnSimulator.Content.ToString() == "manual state")
            {
                mb=MessageBox.Show("you can't close the window whent the simulation is open. Do you want to stop the simulation and close the window?", "close", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.No) return;
                worker.CancelAsync();
            }
            else
            {
                if (somethingHasChanged())
                {
                    if (isInActionsState == false)
                        mb = MessageBox.Show("do you want to close the window? \n the drone will not be added", "cancel adding of drone", MessageBoxButton.YesNo);
                    else
                        mb = MessageBox.Show("do you want to close the window? \nchanges will not happen", "close", MessageBoxButton.YesNo);
                    if (mb == MessageBoxResult.No) return;
                }
            }
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
        #endregion

        #region colors
        /// <summary>
        /// in actions window- paint the model green when the user change it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelColor(object sender, TextChangedEventArgs e) //color for model
        {
            if (isInActionsState && txtModel.Text == blObject.GetDrone(int.Parse(txtId.Text)).Model)
                txtModel.Foreground = Brushes.Black;
            else
                txtModel.Foreground = Brushes.Green;
        }
        /// <summary>
        /// determinate the color of the id text box, according to wich is valid or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdColor(object sender, TextChangedEventArgs e)
        {
            int id;
            if (isInActionsState == true) return;
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
        #endregion
    }
    
}

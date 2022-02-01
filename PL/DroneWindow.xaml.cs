using BLApi;
using BO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        ObservableCollection<DroneToList> obDrones;
        int index;
        Visibility myVisibility = Visibility.Visible; //for hidde the buttons when the simulator is run.

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

            lblBattery.Visibility = Visibility.Collapsed;
            lblStatus.Visibility = Visibility.Collapsed;
            lblParcel.Visibility = Visibility.Collapsed;

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
        public DroneWindow(IBL obj, int droneId, ObservableCollection<DroneToList> obD = null) //actions
        {
            blObject = obj;
            if (obD == null) obDrones = new ObservableCollection<DroneToList>(blObject.GetDronesList());
            else obDrones = obD;
            obDrones.CollectionChanged += updateIndex;
            try { index = obDrones.IndexOf(obDrones.Where(x => x.Id == droneId).SingleOrDefault()); }
            catch (InvalidOperationException) { return; }; //more than one drone with this id, not suppose to happen.
            InitializeComponent();
            isInActionsState = true;
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            txtId.IsEnabled = false;
            txtWeight.IsEnabled = false;

            txtStationId.Visibility = Visibility.Collapsed;
            lblStation.Visibility = Visibility.Collapsed;
            add.Visibility = Visibility.Hidden;

            txtId.Text = droneId.ToString();
            refresh();
        }

        /// <summary>
        /// when the collection obDrones changed- check where is the new place of the drone in it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateIndex(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (!obDrones.Any(x => x.Id == int.Parse(txtId.Text))) index = -1;
                else index = obDrones.IndexOf(obDrones.Where(x => x.Id == int.Parse(txtId.Text)).Single());
            }
            catch (InvalidOperationException) { return; }; //more than one drone with this id, not suppose to happen.
        }
        #endregion

        #region simulation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
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
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            blObject.RunsTheSimulator((int)e.Argument, () => worker.ReportProgress(0), () => worker.CancellationPending);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            refresh();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
        private void addDrone_Click(object sender, RoutedEventArgs e)
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
        private void txtStationIdSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            locGrid.DataContext = blObject.GetStation((int)txtStationId.SelectedItem).Location;
        }
        /// <summary>
        /// update the model of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateDroneModel(object sender, RoutedEventArgs e)
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
        private void sendDroneToCharge(object sender, RoutedEventArgs e)
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
                }
                catch (DroneCantGoToChargeException)
                {
                    MessageBox.Show("Drone can't go to charge, apperantly there is not station that the drone can arrive to it");
                    return;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; }//***
            }
            MessageBox.Show("drone sent successfully");
           
            refresh();
        }
        /// <summary>
        /// ask the user how long the drone has been charging, realese it from charge
        /// after the user entered time- release the drone from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void releaseDroneFromCharge(object sender, RoutedEventArgs e)
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
                MessageBox.Show(ex.Message);
                return;
            }
            catch (DroneCantReleaseFromChargeException ex) { MessageBox.Show(ex.Message); return; }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }//***
            MessageBox.Show("the drone released successfully");
            DataContext = blObject.GetDrone(id);
            charge.Visibility = Visibility.Visible;   
            refresh();
        }
        /// <summary>
        /// Send Drone To Delivery- Assign Parcel To the Drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendDroneToDelivery(object sender, RoutedEventArgs e)
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
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }//***
            MessageBox.Show("the drone has send to delivary");
            charge.Visibility = Visibility.Hidden;
            refresh();
        }
        /// <summary>
        /// Pick Up Parcel by the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pickUpParcel(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(txtParcel.Text, out id);
            if (txtStatus.Text != DroneStatus.Associated.ToString()) //DroneStatus.Associated= didnt took the parcel.
            {
                MessageBox.Show("the drone is not Associated or the parcel already has been picked up");
                return;
            }
            else 
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
            DataContext = blObject.GetDrone(id);
            refresh();
        }
        /// <summary>
        /// deliver the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deliverParcel(object sender, RoutedEventArgs e)
        {
            int id, parcelId;
            int.TryParse(txtParcel.Text, out parcelId);
            if (txtStatus.Text != DroneStatus.Delivery.ToString()) //DroneStatus.Delivery= already took the parcel.
            {
                MessageBox.Show("the drone is not in delivery");
                return;
            }
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
            catch (Exception ex)//***
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("drone deliver the parcel successfully");
            charge.Visibility = Visibility.Visible;
            refresh();
        }

        /// <summary>
        /// return true if the user changed something from the data of the window
        /// </summary>
        /// <returns></returns>
        private bool somethingHasChanged()
        {
            if (isInActionsState)
            {
                return (txtModel.Foreground == Brushes.Green); //its green if its has changed (and it is the only thing that can be changed in action state).
            }
            else
            {
                return (txtId.Text != "" || txtModel.Text != "" || txtWeight.SelectedItem != null || txtStationId != null);
            }
        }

        /// <summary>
        /// open the window of the paercel the drone take.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcelInTransfer(object sender, MouseButtonEventArgs e)
        {
            int parcelId;
            if (int.TryParse(txtParcel.Text, out parcelId) == true)
                new ParcelWindow(blObject, parcelId).ShowDialog();
        }
        #endregion

        #region close
        /// <summary>
        /// close the window after clicking the close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeClick(object sender, RoutedEventArgs e)
        {
            canClose = true;
            MessageBoxResult mb;
            if (btnSimulator.Content.ToString() == "manual state")
            {
                mb = MessageBox.Show("you can't close the window whent the simulation is open. Do you want to stop the simulation and close the window?", "close", MessageBoxButton.YesNo);
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
        void dataWindowClosing(object sender, CancelEventArgs e)
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
        private void modelColor(object sender, TextChangedEventArgs e) //color for model
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
        private void idColor(object sender, TextChangedEventArgs e)
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

        /// <summary>
        /// refresh the wiew according to the data and the status of the drone.
        /// </summary>
        private void refresh()//just for action state
        {
            Drone drone;
            noParcelsNote.Visibility = Visibility.Collapsed;
            lock (blObject)
            {
                drone = blObject.GetDrone(int.Parse(txtId.Text));
                DataContext = drone;
            }
            if (index != -1)
            {
                obDrones[index] = new DroneToList
                {
                    Id = drone.Id,
                    Battery = drone.Battery,
                    Model = drone.Model,
                    MaxWeight = drone.MaxWeight,
                    Status = drone.Status,
                    ParcelId = drone.ParcelInT != null ? drone.ParcelInT.Id : 0,
                    CurrentLocation = drone.CurrentLocation
                };
            }

            if (drone.ParcelInT == null) lblParcel.Visibility = Visibility.Collapsed;
            else lblParcel.Visibility = Visibility.Visible;
            options.Visibility = myVisibility;
            update.Visibility = myVisibility;
            charge.Visibility = Visibility.Hidden;
            if (myVisibility == Visibility.Hidden)
            {
                if (drone.Status == DroneStatus.Available) noParcelsNote.Visibility = Visibility.Visible;
                return;
            }
            options.Click -= sendDroneToDelivery;
            options.Click -= releaseDroneFromCharge;
            options.Click -= pickUpParcel;
            options.Click -= deliverParcel;
            switch (drone.Status) //show to the user the right option according to the status of the drone
            {
                case DroneStatus.Available:
                    charge.Visibility = myVisibility; 
                    options.Content = "send drone\nto delivery";
                    options.Click += sendDroneToDelivery;
                    break;
                case DroneStatus.Maintenance:
                    options.Content = "release drone\nfrom charge";
                    options.Click += releaseDroneFromCharge;
                    break;
                case DroneStatus.Associated:
                    options.Content = "pick up\nparcel";
                    options.Click += pickUpParcel;
                    break;
                case DroneStatus.Delivery:
                    options.Content = "deliver\nparcel";
                    options.Click += deliverParcel;
                    break;
            }
        }

    }

}

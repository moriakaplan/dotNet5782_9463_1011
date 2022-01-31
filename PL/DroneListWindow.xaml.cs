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
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL blObject;
        bool canClose = false;
        ObservableCollection<DroneToList> obDrones;

        /// <summary>
        /// constructor- initialise the list to show all the drones
        /// </summary>
        /// <param name="obj"></param>
        public DroneListWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            //IEnumerable<DroneToList> drones = blObject.GetDronesList();
            obDrones = new ObservableCollection<DroneToList>(blObject.GetDronesList());
            DroneListView.DataContext = obDrones;
            //DroneListView.DataContext = blObject.GetDronesList();
            StatusFilter.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightFilter.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        #region filters
        /// <summary>
        /// filter by status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusFilter(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilter.SelectedIndex == -1) return;
            if (WeightFilter.SelectedItem != null) statusAndWeightFilter(sender, e);
            else
            {
                DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
                obDrones=obDrones.Where(x => x.Status == status) as ObservableCollection<DroneToList>;
                //DroneListView.DataContext = obDrones.Where(x => x.Status == status);
            }
        }
        /// <summary>
        /// filter by weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weightFilter(object sender, SelectionChangedEventArgs e)
        {
            if (WeightFilter.SelectedIndex == -1) return;
            if (StatusFilter.SelectedItem != null) statusAndWeightFilter(sender, e);
            else
            {
                WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
                ObservableCollection<DroneToList> DronesInSpecWeight = obDrones.Where(x => x.MaxWeight == weight) as ObservableCollection<DroneToList>;
                //DroneListView.DataContext = obDrones.Where(x => x.MaxWeight == weight);
            }
        }
        /// <summary>
        /// filter by status and weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusAndWeightFilter(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            obDrones = obDrones.Where(x => x.Status == status && x.MaxWeight == weight) as ObservableCollection<DroneToList>;
            //DroneListView.DataContext = obDrones.Where(x => x.Status == status && x.MaxWeight == weight);
        }
        /// <summary>
        /// delete the filtering by status and weight and show the whole drones list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restart(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedIndex = -1;
            WeightFilter.SelectedIndex = -1;
            obDrones = blObject.GetDronesList() as ObservableCollection<DroneToList>; //to do that this will updated
        }
        #endregion

        /// <summary>
        /// open the drone window in adding state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject).ShowDialog();
            obDrones = blObject.GetDronesList() as ObservableCollection<DroneToList>; //update the drones list

            if (StatusFilter.SelectedItem != null) statusFilter(StatusFilter, null);
            else
            {
                if (WeightFilter.SelectedItem != null) weightFilter(WeightFilter, null);
                else restart(Restart, null);
            }
        }
        /// <summary>
        /// open drone window in action mode (the drones list updated automaticly)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDrone(object sender, MouseButtonEventArgs e)
        {
            //Drone drone=blObject.GetDrone(((DroneToList)DroneListView.SelectedItem).Id);
            new DroneWindow(blObject, ((DroneToList)DroneListView.SelectedItem).Id, obDrones).Show();
        }
        /// <summary>
        /// close the window after clicking the close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeClick(object sender, RoutedEventArgs e)
        {
            canClose = true;
            this.Close();
        }
        /// <summary>
        /// prevenet closing the window with the X button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataWindowClosing(object sender, CancelEventArgs e)
        {
            if (canClose == false)
            {
                MessageBox.Show("don't close with the x button, close with the close window button");
                e.Cancel = true;
            }
        }
    }
}

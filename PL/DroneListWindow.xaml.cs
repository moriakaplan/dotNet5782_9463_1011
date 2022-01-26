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
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public DroneListWindow(IBL obj)
        {
            InitializeComponent();
            blObject = BLFactory.GetBl();
            DroneListView.DataContext = blObject.GetDronesList()
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
            StatusFilter.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightFilter.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
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
                DroneListView.DataContext = blObject.GetDronesList(x => x.Status == status)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
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
                DroneListView.ItemsSource = blObject.GetDronesList(x => x.MaxWeight == weight)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
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
            DroneListView.ItemsSource = blObject.GetDronesList(x => x.Status == status && x.MaxWeight == weight)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
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
            DroneListView.ItemsSource = blObject.GetDronesList();
        }
        /// <summary>
        /// open the drone window in adding state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject).ShowDialog();
            //update the drones list
            if (StatusFilter.SelectedItem != null) statusFilter(StatusFilter, null);
            else
            {
                if (WeightFilter.SelectedItem != null) weightFilter(WeightFilter, null);
                else DroneListView.ItemsSource = blObject.GetDronesList();
            }
        }
        /// <summary>
        /// open drone window for action mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewDrone(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(blObject, ((BO.DroneToList)DroneListView.SelectedItem).Id).Show();
            //update the drones list
            if (StatusFilter.SelectedItem != null) statusFilter(StatusFilter, null);
            else
            {
                if (WeightFilter.SelectedItem != null) weightFilter(WeightFilter, null);
                else DroneListView.ItemsSource = blObject.GetDronesList();
            }
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
        private void dataWindow(object sender, CancelEventArgs e)
        {
            if (canClose == false)
            {
                MessageBox.Show("don't close with the x button, close with the close window button");
                e.Cancel = true;
            }
        }
    }
}

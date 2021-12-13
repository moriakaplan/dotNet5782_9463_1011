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
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private Ibl blObject;
        bool canClose = false;
        public DroneListWindow(Ibl obj)
        {
            InitializeComponent();
            blObject = obj;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones();
            StatusFilter.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightFilter.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        /// <summary>
        /// filter by status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilter.SelectedIndex == -1) return;
            if (WeightFilter.SelectedItem != null) StatusAndWeightFilter(sender, e);
            else
            {
                DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
                DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.Status == status);
            }
        }
        /// <summary>
        /// filter by weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightFilter.SelectedIndex == -1) return;
            if (StatusFilter.SelectedItem != null) StatusAndWeightFilter(sender, e);
            else
            {
                WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
                DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.MaxWeight == weight);
            }
        }
        /// <summary>
        /// filter by status and weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusAndWeightFilter(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.Status == status && x.MaxWeight == weight);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedIndex = -1;
            WeightFilter.SelectedIndex = -1;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones();
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject).ShowDialog();
            //update the drones list
            if (StatusFilter.SelectedItem != null) StatusFilter_SelectionChanged(StatusFilter, null);
            else
            {
                if (WeightFilter.SelectedItem != null) WeightFilter_SelectionChanged(WeightFilter, null);
                else DroneListView.ItemsSource = blObject.DisplayListOfDrones();
            }
        }
        /// <summary>
        /// open drone window for action mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDrone(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(blObject, ((IBL.BO.DroneToList)DroneListView.SelectedItem).Id).ShowDialog();
            //update the drones list
            if (StatusFilter.SelectedItem != null) StatusFilter_SelectionChanged(StatusFilter, null);
            else
            {
                if (WeightFilter.SelectedItem != null) WeightFilter_SelectionChanged(WeightFilter, null);
                else DroneListView.ItemsSource = blObject.DisplayListOfDrones();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            canClose = true;
            this.Close();
        }

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

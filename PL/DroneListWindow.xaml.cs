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

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightFilter.SelectedItem != null) StatusAndWeightFilter(sender, e);
            else
            {
                DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
                DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.Status == status);
            }
        }

        private void WeightFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusFilter.SelectedItem != null) StatusAndWeightFilter(sender, e);
            else
            {
                WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
                DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.MaxWeight == weight);
            }
        }

        private void StatusAndWeightFilter(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.Status == status && x.MaxWeight == weight);
        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject).Show();
        }

        private void ViewDrone(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(blObject, ((IBL.BO.DroneToList)DroneListView.SelectedItem).Id).Show();
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

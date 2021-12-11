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
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private Ibl blObject;
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
            DroneStatus status = (DroneStatus)StatusFilter.SelectedItem;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.Status == status);
        }

        private void WeightFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            DroneListView.ItemsSource = blObject.DisplayListOfDrones(x => x.MaxWeight == weight);
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObject).Show();
        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       
    }
}

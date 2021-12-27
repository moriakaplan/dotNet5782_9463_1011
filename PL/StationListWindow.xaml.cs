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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        IBL blObject;
        public StationListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            stationDataGrid.ItemsSource = blObject.DisplayListOfStations();
            //new StationWindow(blObject, 1111).ShowDialog();
        }

        private void ViewStation(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(blObject, ((BO.StationToList)stationDataGrid.SelectedItem).Id).ShowDialog();
        }

        private void AddStation(object sender, RoutedEventArgs e)
        {
            new StationWindow(blObject).ShowDialog();
        }
    }
}

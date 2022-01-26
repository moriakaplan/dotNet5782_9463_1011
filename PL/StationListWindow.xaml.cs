using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        IBL blObject;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public StationListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            nonGroup();
            List<string> a= new List<string>();
            a.Add("regular");
            a.Add("group by number of charge slots");
            groupButton.ItemsSource = a;
            groupButton.SelectedItem="regular";
        }

        /// <summary>
        /// By double-clicking on a row in the table of station a specific station window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewStation(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(blObject, ((sender as DataGrid).SelectedItem as BO.StationToList).Id).ShowDialog();
            nonGroup();
        }
        /// <summary>
        /// open station window in add mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addStation(object sender, RoutedEventArgs e)
        {

            new StationWindow(blObject).ShowDialog();
            nonGroup();
        }

        /// <summary>
        /// if we want no grouping
        /// </summary>
        private void nonGroup()
        {
            stationDataGrid.Visibility = Visibility.Visible;
            ListViewStations.Visibility = Visibility.Collapsed;
            stationDataGrid.DataContext = blObject.GetStationsList();
        }
        /// <summary>
        /// regresh the station list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh(object sender, RoutedEventArgs e)
        {
            if (groupButton.SelectedItem.ToString() == "regular")
                nonGroup();
            else
                group();
        }
        /// <summary>
        /// grouping
        /// </summary>
        private void group()
        {
            stationDataGrid.Visibility = Visibility.Collapsed;
            ListViewStations.Visibility = Visibility.Visible;
            IEnumerable<IGrouping<int, StationToList>> result = from st in blObject.GetStationsList()
                                                                group st by st.AvailableChargeSlots into gs
                                                                select gs;
            var datagrids =
                result.Select(g =>
                {
                    DataGrid d = new DataGrid();
                    d.ItemsSource = g.ToList();
                    d.PreviewMouseDoubleClick += viewStation;
                    return d;
                });
           
            ListViewStations.DataContext = datagrids;
        }
       

    }
}

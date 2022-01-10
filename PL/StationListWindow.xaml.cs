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

        private void ViewStation(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(blObject, ((sender as DataGrid).SelectedItem as BO.StationToList).Id).ShowDialog();

            //DataGridCell cell = sender as DataGridCell;
            //StationToList s = cell.DataContext as StationToList;
            //new StationWindow(blObject, s.Id).ShowDialog();
            nonGroup();
        }

        private void AddStation(object sender, RoutedEventArgs e)
        {

            new StationWindow(blObject).ShowDialog();
            nonGroup();
        }

        private void nonGroup()
        {
            stationDataGrid.Visibility = Visibility.Visible;
            ListViewStations.Visibility = Visibility.Collapsed;
            stationDataGrid.DataContext = blObject.GetStationsList();
        }
        private void refresh(object sender, RoutedEventArgs e)
        {
            if (groupButton.SelectedItem.ToString() == "regular")
                nonGroup();
            else
                group();
        }

        private void group()
        {
            stationDataGrid.Visibility = Visibility.Collapsed;
            ListViewStations.Visibility = Visibility.Visible;
            IEnumerable<IGrouping<int, StationToList>> result = from st in blObject.GetStationsList()
                                                                group st by st.AvailableChargeSlots into gs
                                                                select gs;
            //DataGrid func(IGrouping<int, StationToList> g)
            //{ 
            //    DataGrid d = new DataGrid(); 
            //    d.ItemsSource = g.ToList(); 
            //    return d; 
            //};
            var datagrids =
                result.Select(g =>
                {
                    DataGrid d = new DataGrid();
                    d.ItemsSource = g.ToList();
                    d.PreviewMouseDoubleClick += ViewStation;
                    return d /*new { num=g.Key, dataGrid = d }*/;
                });
            //IEnumerable<DataGrid> datagrids =
            //    result.Select(g =>
            //    {
            //        DataGrid d = new DataGrid();
            //        d.DataContext = g.ToList();
            //        List<DataGridTextColumn> l = new List<DataGridTextColumn>();
            //        //l.Add( { Binding = , });
            //        //d.Columns = new List<DataGridTextColumn>();
            //        //= new DataGridTextColumn x: Name = "idColumn" Binding = "{Binding Id}" Header = "Id" />

            //        //  //< DataGridTextColumn x: Name = "nameColumn" Binding = "{Binding Name}" Header = "Name" />

            //        //  //      < DataGridTextColumn x: Name = "notAvailableChargeSlotsColumn" Binding = "{Binding NotAvailableChargeSlots}" Header = "Not Available Charge Slots" />

            //        //  //            < DataGridTextColumn x: Name = "availableChargeSlotsColumn" Binding = "{Binding AvailableChargeSlots}" Header = "Available Charge Slots" />

            //        d.PreviewMouseDoubleClick += ViewStation;
            //        return d;
            //    });
            ListViewStations.DataContext = datagrids;
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult mb;
            //mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);
            //if (mb == MessageBoxResult.No) e.Cancel=true;
            new ManagerWindow(blObject).Show();
        }

    }
}

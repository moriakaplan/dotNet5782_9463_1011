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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        IBL blObject;
        ObservableCollection<ParcelToList> parcelsOb;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public ParcelListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            parcelsOb = new ObservableCollection<ParcelToList>(blObject.GetParcelsList());
            parcelToListDataGrid.DataContext= blObject.GetParcelsList();
            StatusFilter.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            WeightFilter.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// By double-clicking on a row in the table of parcels a specific parcel window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcel(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((BO.ParcelToList)parcelToListDataGrid.SelectedItem).Id).ShowDialog();
            //if (StatusFilter.SelectedItem != null) statusFilter(null, null);
            //else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
            timeFilter(null, null);
        }

        /// <summary>
        /// open parcel window in add mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject).ShowDialog();
            //if (StatusFilter.SelectedItem != null) statusFilter(null, null);
            //else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
            timeFilter(null, null);
        }

        #region filters
        /// <summary>
        /// filter the list of displayed parcels to contain just parcels with specific weight and status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filter(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox && (sender as ComboBox).SelectedIndex == -1) return;
            Func<ParcelToList, bool> pre = x => true;
            if (WeightFilter.SelectedItem != null)
            {
                WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
                if (StatusFilter.SelectedItem != null)
                {
                    ParcelStatus status = (ParcelStatus)StatusFilter.SelectedItem;
                    pre = x => (x.Status == status) && (x.Weight == weight);
                }
                else
                {
                    pre = x => (x.Weight == weight);
                }
            }
            else
            {
                if (StatusFilter.SelectedItem != null)
                {
                    ParcelStatus status = (ParcelStatus)StatusFilter.SelectedItem;
                    pre = x => (x.Status == status);
                }
            }
            parcelToListDataGrid.DataContext = parcelsOb.Where(pre);
        }

        /// <summary>
        /// update the obsevable collection to contain fust parcels that created in specific time range (and update the display).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeFilter(object sender, SelectionChangedEventArgs e)
        {
            if (CreatedFrom.SelectedDate != null && CreatedTo.SelectedDate != null)
            {
                parcelsOb = new ObservableCollection<ParcelToList>(
                    from p in blObject.GetParcelsList()
                    let time = blObject.GetParcel(p.Id).CreateTime
                    where time >= CreatedFrom.SelectedDate && time <= CreatedTo.SelectedDate
                    select p);
            }
            else
            {
                if (CreatedFrom.SelectedDate != null)
                {
                    parcelsOb = new ObservableCollection<ParcelToList>(
                        from p in blObject.GetParcelsList()
                        where blObject.GetParcel(p.Id).CreateTime >= CreatedFrom.SelectedDate
                        select p);
                }
                else if (CreatedTo.SelectedDate != null)
                {
                    parcelsOb = new ObservableCollection<ParcelToList>(
                        from p in blObject.GetParcelsList()
                        where blObject.GetParcel(p.Id).CreateTime <= CreatedTo.SelectedDate
                        select p);
                }
            }
            filter(null, null);
        }

        /// <summary>
        /// restart all the selctions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedIndex = -1;
            WeightFilter.SelectedIndex = -1;
            parcelToListDataGrid.DataContext = parcelsOb; 
        }
        #endregion
    }
}

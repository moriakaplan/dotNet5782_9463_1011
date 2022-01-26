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

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        IBL blObject;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public ParcelListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
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
            if (StatusFilter.SelectedItem != null) statusFilter(null, null);
            else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
        }

        /// <summary>
        /// open parcel window in add mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject).ShowDialog();
            if (StatusFilter.SelectedItem != null) statusFilter(null, null);
            else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
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
                parcelToListDataGrid.ItemsSource = blObject.GetParcelsList().Where(x => x.Weight == weight)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
            }
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
                ParcelStatus status = (ParcelStatus)StatusFilter.SelectedItem;
                parcelToListDataGrid.DataContext = blObject.GetParcelsList().Where(x => x.Status == status)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
            }
        }
        /// <summary>
        /// status and weight filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusAndWeightFilter(object sender, SelectionChangedEventArgs e)
        {
            ParcelStatus status = (ParcelStatus)StatusFilter.SelectedItem;
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            parcelToListDataGrid.ItemsSource = blObject.GetParcelsList().Where(x => (x.Status == status) && (x.Weight == weight))
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedIndex = -1;
            WeightFilter.SelectedIndex = -1;
            parcelToListDataGrid.DataContext = blObject.GetParcelsList();
        }
    }
}

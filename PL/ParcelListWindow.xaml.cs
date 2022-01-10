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
        public ParcelListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            parcelToListDataGrid.DataContext= blObject.GetParcelsList();
            StatusFilter.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            WeightFilter.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void ViewParcel(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((BO.ParcelToList)parcelToListDataGrid.SelectedItem).Id).ShowDialog();
            if (StatusFilter.SelectedItem != null) statusFilter_SelectionChanged(null, null);
            else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
        }

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject).ShowDialog();
            if (StatusFilter.SelectedItem != null) statusFilter_SelectionChanged(null, null);
            else parcelToListDataGrid.DataContext = blObject.GetParcelsList();
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult mb;
            //mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);
            //if (mb == MessageBoxResult.No) e.Cancel=true;
            new ManagerWindow(blObject).Show();
        }

        private void weightFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void statusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        private void statusAndWeightFilter(object sender, SelectionChangedEventArgs e)
        {
            ParcelStatus status = (ParcelStatus)StatusFilter.SelectedItem;
            WeightCategories weight = (WeightCategories)WeightFilter.SelectedItem;
            parcelToListDataGrid.ItemsSource = blObject.GetParcelsList().Where(x => (x.Status == status) && (x.Weight == weight))
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
        }
    }
}

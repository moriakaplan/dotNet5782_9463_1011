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
        }

        private void ViewParcel(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((BO.ParcelToList)parcelToListDataGrid.SelectedItem).Id).ShowDialog();
        }

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject).ShowDialog();
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

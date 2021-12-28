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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        IBL blObject;
        public CustomerListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            customerToListDataGrid.DataContext = blObject.DisplayListOfCustomers();
        }

        private void ViewCustomer(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(blObject, ((BO.CustomerToList)customerToListDataGrid.SelectedItem).Id).ShowDialog();
        }

        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject).ShowDialog();
        }
    }
}

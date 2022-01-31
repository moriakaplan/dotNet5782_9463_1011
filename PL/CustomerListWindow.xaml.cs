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
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        IBL blObject;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public CustomerListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            try { customerToListDataGrid.DataContext = blObject.GetCustomersList(); }
            catch (NotExistIDException ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Double-clicking on a row in the table opens a specific customer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewCustomer(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(blObject, ((BO.CustomerToList)customerToListDataGrid.SelectedItem).Id).ShowDialog();
            customerToListDataGrid.DataContext = blObject.GetCustomersList();
        }

        /// <summary>
        /// add customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCustomer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject).ShowDialog();
            customerToListDataGrid.DataContext = blObject.GetCustomersList();
        }

    }
}

﻿using System;
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
        public CustomerListWindow(IBL obj)
        {
            blObject = obj;
            InitializeComponent();
            try { customerToListDataGrid.DataContext = blObject.GetCustomersList(); }
            catch (NotExistIDException ex) { MessageBox.Show(ex.Message); }
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult mb;
            //mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);
            //if (mb == MessageBoxResult.No) e.Cancel=true;
            new ManagerWindow(blObject).Show();
        }

        private void ViewCustomer(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(blObject, ((BO.CustomerToList)customerToListDataGrid.SelectedItem).Id).ShowDialog();
            customerToListDataGrid.DataContext = blObject.GetCustomersList();
        }

        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject).ShowDialog();
            customerToListDataGrid.DataContext = blObject.GetCustomersList();
        }

    }
}

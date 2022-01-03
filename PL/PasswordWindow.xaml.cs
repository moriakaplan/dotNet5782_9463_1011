﻿using System;
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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        IBL blObject;
        public PasswordWindow()
        {
            InitializeComponent();
            //blObject = obj;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            int id = blObject.DisplayListOfCustomers.Where(x => x.Name == txtUserName).Single().Id;
            new UserWindow(id).ShowDialog();
        }
    }
}

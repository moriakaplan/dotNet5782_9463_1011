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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        IBL blObject;
        bool isManager;
        public PasswordWindow(IBL obj, bool manager)
        {
            InitializeComponent();
            blObject = obj;
            isManager = manager;
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult mb;
            mb = MessageBox.Show("do you want to close the window? \n and go back to the main window?", "close", MessageBoxButton.YesNo);
            if (mb == MessageBoxResult.No) e.Cancel=true;
            else new MainWindow().Show();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (isManager == false)
            {
                try
                {
                    int id = blObject.GetUserId(txtUserName.Text, txtPassword.Text);
                    new UserWindow(blObject, id).ShowDialog();
                    this.Close();
                }
                catch (NotExistIDException ex) { MessageBox.Show(/*"the username or the password are not correct, please try again. maybe you are a manger?"*/ ex.Message); }
            }
            else
            {
                if (blObject.ExistManager(txtUserName.Text, txtPassword.Text))
                {
                    new ManagerWindow(blObject).Show();
                    this.Close();
                }
                else { MessageBox.Show("the username or the password are not correct, please try again. maybe you are a regular customer?"); }
            }
        }
        private void signUp(object sender, RoutedEventArgs e)
        {
            new SignWindow(blObject, isManager).ShowDialog();
            this.Close();
        }
    }
}

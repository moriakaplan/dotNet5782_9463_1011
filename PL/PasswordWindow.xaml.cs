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
        public PasswordWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
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
            if (blObject.ExistManager(txtUserName.Text, txtPassword.Text))
            {
                new ManagerWindow(blObject).Show();
                this.Close();
                return;
            }
            
            try
            {
                int id = blObject.GetUserId(txtUserName.Text, txtPassword.Text);
                new UserWindow(blObject, id).ShowDialog();
                this.Close();
            }
            catch (NotExistIDException ex) { MessageBox.Show(/*"the username or the password are not correct, please try again. maybe you are a manger?"*/ ex.Message); }
            MessageBox.Show("the username or the password are not correct, please try again");
        }

        private void signUp(object sender, RoutedEventArgs e)
        {
            new SignWindow(blObject).ShowDialog();
            this.Close();
        }
    }
}

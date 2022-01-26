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
        bool closeX = true;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public PasswordWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            txtUserName.Focus();
        }
        
        /// <summary>
        /// close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataWindowClosing(object sender, CancelEventArgs e)
        {
            
            if(closeX) new MainWindow().Show();
        }

        /// <summary>
        /// log in- as a user or manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginClick(object sender, RoutedEventArgs e)
        {
            if (blObject.ExistManager(txtUserName.Text, PPassword.Password))//login as a manager
            {
                new ManagerWindow(blObject).Show();
                closeX = false;
                this.Close();
                return;
            }
            
            try
            {
                int id = blObject.GetUserId(txtUserName.Text, PPassword.Password);
                new UserWindow(blObject, id).ShowDialog();//login as a user
                closeX = false;
                this.Close();
            }
            catch (NotExistIDException ex) { MessageBox.Show("the username or the password are not correct, please try again" /*ex.Message*/); }

        }

        /// <summary>
        /// open the sign op window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signUp(object sender, RoutedEventArgs e)
        {
            new SignWindow(blObject).Show();
            closeX = false;
            this.Close();
        }

        /// <summary>
        /// show the password- update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = PPassword.Password;
        }
    }
}

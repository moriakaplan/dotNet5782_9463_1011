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
    /// Interaction logic for SignWindow.xaml
    /// </summary>
    public partial class SignWindow : Window
    {
        IBL blObject;
        enum typesOfUsers { RegularUser, Manager };
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        public SignWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            typeOfUser.ItemsSource = Enum.GetValues(typeof(typesOfUsers));
            typeOfUser.SelectedItem = typesOfUsers.RegularUser;
        }

        /// <summary>
        /// sign up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sighn(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")///if there is no name
            {
                MessageBox.Show("Please choose a username");
                return;
            }
            if (txtPassword.Text == "")//if there is no password
            {
                MessageBox.Show("Please choose a password");
                return;
            }
            if (!passIsStrong(txtPassword.Text))//if the password is not strong
            {
                MessageBox.Show("this password is not strong enugh, please choose another one.\n in the password must be big letters, small letters and numbers and it must have at least 8 chars.");
                return;
            }
            try
            {
                if ((typesOfUsers)typeOfUser.SelectedItem == typesOfUsers.Manager)//if he is a manager-check if his password is manager password
                {
                    if (txtManager.Text == blObject.GetManagmentPassword())
                        blObject.AddManager(txtName.Text, txtPassword.Text);
                    else
                        MessageBox.Show("this is not the right password, \n please ask another manager what is the current password.");
                }
                else
                    blObject.AddUser(int.Parse(txtId.Text), txtName.Text, txtPassword.Text);
                MessageBox.Show("the user added successfully");
                this.Close();
            }
            catch (ExistIdException)
            {
                MessageBox.Show("the username already exist, try to choose another one");
            }
        }

        /// <summary>
        /// check if the password is strong
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        private bool passIsStrong(string pass)
        {
            if (pass.Length < 8 ||
                pass.Distinct().Count() < 6)
                return false;
            if (pass.Any(x => x >= 'a' && x <= 'z') &&
                pass.Any(x => x >= 'A' && x <= 'Z') &&
                pass.Any(x => x >= '0' && x <= '9'))
                return true;
            return false;
        }

        /// <summary>
        /// Changes the view by user type 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void typeOfUserChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((typesOfUsers)typeOfUser.SelectedItem == typesOfUsers.Manager)//if he is a manager
            {
                txtManager.Visibility = Visibility.Visible;
                lblManager.Visibility = Visibility.Visible;
                txtId.Visibility = Visibility.Collapsed;
                lblId.Visibility = Visibility.Collapsed;
                help.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtId.Visibility = Visibility.Visible;
                lblId.Visibility = Visibility.Visible;
                help.Visibility = Visibility.Visible;
                txtManager.Visibility = Visibility.Collapsed;
                lblManager.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// return to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataWindowClosing(object sender, CancelEventArgs e)
        {
            new MainWindow().Show();
        }

        /// <summary>
        /// show if the password is strong or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passChange(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text == "")
            {
                lblGoodOrWrong.Content = "";
                return;
            }
            if (passIsStrong(txtPassword.Text))//if the password is good
            {
                lblGoodOrWrong.Content = "strong password";
                lblGoodOrWrong.Foreground = Brushes.Green;
            }
            else//if the password is not good
            {
                lblGoodOrWrong.Content = $"not strong password\nThe password must be at least 8 chars (6 different) and with big letters, small letters and numbers.";
                lblGoodOrWrong.Foreground = Brushes.Red;
            }
        }
    }
}

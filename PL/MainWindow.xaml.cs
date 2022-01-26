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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly IBL blObject = BLFactory.GetBl(); 

        /// <summary>
        /// constractor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// open the password window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logIn(object sender, RoutedEventArgs e)
        {
            new PasswordWindow(blObject).Show();
            this.Close();
        }

        /// <summary>
        /// open the sign window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sign(object sender, RoutedEventArgs e)
        {
            new SignWindow(blObject).Show();
            this.Close();
        }
    }
}
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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL blObject;
        public ParcelWindow(IBL obj, int parcelId)
        {
            InitializeComponent();
            blObject = obj;
            InitializeComponent();

            Parcel parcel = blObject.DisplayParcel(parcelId);
            txtId.Text = parcel.Id.ToString();
            txtDrone.Text = parcel.Drone.Id.ToString();
            txtPriority.Text = parcel.Priority.ToString();
            txtSender.Text = parcel.Sender.ToString();
            txtTarget.Text = parcel.Target.ToString();
            txtWeight.Text = parcel.Weight.ToString();
            txtCreateTime.Text = parcel.CreateTime.ToString();
            txtAssociateTime.Text = parcel.AssociateTime.ToString();
            txtPickUpTime.Text = parcel.PickUpTime.ToString();
            txtDeliverTime.Text = parcel.DeliverTime.ToString();
        }
    }
}

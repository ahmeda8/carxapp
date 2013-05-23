using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using carXapp2.Models;

namespace carXapp2.Pages
{
    public partial class MainPivotPage : PhoneApplicationPage
    {
        private int carID;

        public MainPivotPage()
        {
            InitializeComponent();
            
        }

        private void menuAddMaint_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditMaint.xaml?carID=" + carID, UriKind.Relative));
        }

        private void menuAddFuel_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid=" + carID, UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string carIDStr;
            NavigationContext.QueryString.TryGetValue("id", out carIDStr);
            this.carID = int.Parse(carIDStr);

            FuelsModel fm = new FuelsModel(this.carID);
            listBoxFuel.ItemsSource = fm.GetRecords();
            fuelOverallMpg.Text = fm.OverallFuelConsumption().ToString("F2");

            CarsModel cm = new CarsModel(this.carID);
            carInfo car = cm.GetCar();
            tCarName.Text = car.CarYear + " " + car.CarMake + " " + car.CarModel;
        }

        private void EditFuelBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int FuelID = int.Parse(btn.Tag.ToString());
            NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid=" + this.carID + "&fid=" + FuelID, UriKind.Relative));
        }
    }
}
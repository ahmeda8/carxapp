using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using carXapp2.Models;
using System.Windows.Controls;

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

            //load settings for units
            string distance, volume, average, currency;
            bool avgmethod;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("distance", out distance);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("volume", out volume);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("average", out average);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("currency", out currency);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("avgmethod", out avgmethod);

            stMpgUnits.Text = distance + "/" + volume;

            FuelsModel fm = new FuelsModel(this.carID);
            listBoxFuel.ItemsSource = fm.GetRecords();
            fuelOverallMpg.Text = fm.OverallFuelConsumption().ToString("F2") + " "+distance + "/" + volume;
            /*
            tFuelCS.Text = fm.totalCost.ToString("F2");
            tFCPD.Text = (fm.totalCost / fm.GetTotalTimeSpan().Days).ToString();
            tAvgFuelCS.Text = (fm.totalCost/fm.totalFuel).ToString("F2");
            tFCPM.Text = (fm.totalCost / fm.GetTotalTimeSpan().Days * 30).ToString();
            tMilesPD.Text = (fm.totalMiles / fm.GetTotalTimeSpan().Days).ToString();
            tMilesPM.Text = (fm.totalMiles / fm.GetTotalTimeSpan().Days * 30).ToString();
            */
            //tFuelCS.Text = fm.totalCost.ToString("F2");
            stMpg.Text = fm.OverallFuelConsumption().ToString("F2");
            stCostpd.Text = currency+(fm.totalCost / fm.GetTotalTimeSpan().Days).ToString()+" /day";
            stCostpm.Text = currency+(fm.totalCost / fm.GetTotalTimeSpan().Days * 30).ToString()+" /month";
            stMilepd.Text = (fm.totalMiles / fm.GetTotalTimeSpan().Days).ToString()+ " "+distance+"/day";
            stMilepm.Text = (fm.totalMiles / fm.GetTotalTimeSpan().Days * 30).ToString() + " " + distance + "/month";
            tFuelC.Text = currency+(fm.totalCost).ToString("F2");

            MaintsModel mm = new MaintsModel(this.carID);
            listBoxMaint.ItemsSource = mm.GetRecords();
            tTotalCostMaint.Text = currency+" "+mm.GetTotalCost().ToString();
            tMaintCS.Text = currency+mm.GetTotalCost().ToString("F2");
            tMaintParts.Text = "Parts: "+currency+" " + mm.TotalPartsCost.ToString("F2");
            tMaintLabor.Text = "Labor: "+currency+" " + mm.TotalLaborCost.ToString("F2");

            CarsModel cm = new CarsModel(this.carID);
            carInfo car = cm.GetCar();
            tCarname.Text =  car.CarModel;
        }

        private void EditFuelBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int FuelID = int.Parse(btn.Tag.ToString());
            NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid=" + this.carID + "&fid=" + FuelID, UriKind.Relative));
        }

        private void EditMaintBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int maintID = int.Parse(btn.Tag.ToString());
            NavigationService.Navigate(new Uri("/Pages/AddEditMaint.xaml?carID=" + this.carID + "&maintID=" + maintID, UriKind.Relative));
        }
    }
}
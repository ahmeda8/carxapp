using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using carXapp2;
using Microsoft.Phone.Tasks;
using System.Windows.Controls;

namespace carXapp2
{
    public partial class Main : PhoneApplicationPage
    {
        
        private string carID;

        public Main()
        {
            InitializeComponent();
        }

                
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            NavigationContext.QueryString.TryGetValue("id", out carID);
            
            var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                           where cInfo.CarID == int.Parse(carID)
                           select cInfo;

            var selMake = carsInDb.FirstOrDefault().CarMake;
            var selModel = carsInDb.FirstOrDefault().CarModel;
            var selyear = carsInDb.FirstOrDefault().CarYear;
            string delmiter = " ";
            string selected = String.Concat(selMake, delmiter, selModel);
            PageTitle.Text = selected;
            PageTitle2.Text = selyear;
            listBox1.SelectedIndex = -1;
            ErrorLogging.Analytics(this.GetType().ToString(), "FuncMain", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedFrom(e);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/Pages/addEditCar.xaml?id="+carID, UriKind.Relative));
        }

      

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    
                    NavigationService.Navigate(new Uri("/Pages/CarStatsInfo.xaml?carID="+carID, UriKind.Relative));
                    break;
                case 1:
                    NavigationService.Navigate(new Uri("/Pages/FuelInfo.xaml?carID="+carID, UriKind.Relative));
                    break;
                case 2:
                    NavigationService.Navigate(new Uri("/Pages/MaintInfo.xaml?carID=" + carID, UriKind.Relative));
                    break;
                case 3:
                    NavigationService.Navigate(new Uri("/Pages/RemiderInfo.xaml?carID=" + carID, UriKind.Relative));
                    break;
            }
        }

        
    }
}
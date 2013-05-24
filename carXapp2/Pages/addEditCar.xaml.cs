using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows;
using carXapp2;

namespace carXapp2
{
    public partial class AddEditCar : PhoneApplicationPage
    {
        private int carID;
        private carInfo CurrentCar;

        public AddEditCar()
        {
            InitializeComponent();

        }

        private void saveClick(object sender, EventArgs e)
        {
            if (carID == 0)
            {
                addCar();
            }
            else
            {
                updateCar();
            }
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void addCar()
        {
            if (txtMake.Text.Length > 0)
            {
                App.ViewModel.Database.carInfo.InsertOnSubmit(CurrentCar);
                App.ViewModel.Database.SubmitChanges();
            }
        }

        private void updateCar()
        {
            if (txtMake.Text.Length > 0)
            {
                /*
                var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                               where cInfo.CarID == int.Parse(carID)
                               select cInfo;
                App.ViewModel.Database.carInfo.DeleteOnSubmit(carsInDb.FirstOrDefault());
                App.ViewModel.Database.carInfo//InsertOnSubmit(CurrentCar);
                 */
                App.ViewModel.Database.SubmitChanges();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string carIDStr;
            NavigationContext.QueryString.TryGetValue("id", out carIDStr);
            int.TryParse(carIDStr, out this.carID);

            if (this.carID != 0)
            {
                var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                               where cInfo.CarID == carID
                               select cInfo;
                CurrentCar = carsInDb.First();
            }
            else 
                CurrentCar = new carInfo {CarRegExpiry = DateTime.Now, CarInsExpiry = DateTime.Now };

            this.DataContext = CurrentCar;
            //ErrorLogging.Analytics(this.GetType().ToString(), "AddCar", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            ErrorLogging.Analytics(this.GetType().ToString(), "AddCar", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedFrom(e);
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            
            if(carID != 0)
            {
                MessageBoxButton cancel = MessageBoxButton.OKCancel;
                MessageBoxResult result = MessageBox.Show("delete this car?", "Confirm",cancel);
                if (result == MessageBoxResult.OK)
                {
                    var carInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                                   where cInfo.CarID == carID
                                   select cInfo;
                    var carfuelInDb = from fuelInfo fInfo in App.ViewModel.Database.fuelInfo
                                   where fInfo.CarID == carID
                                   select fInfo;
                    var carmaintInDb = from maintInfo mInfo in App.ViewModel.Database.maintInfo
                                      where mInfo.CarID == carID
                                      select mInfo;
                    App.ViewModel.Database.carInfo.DeleteAllOnSubmit(carInDb);
                    App.ViewModel.Database.fuelInfo.DeleteAllOnSubmit(carfuelInDb);
                    App.ViewModel.Database.maintInfo.DeleteAllOnSubmit(carmaintInDb);
                    App.ViewModel.Database.SubmitChanges();
                }
            }

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
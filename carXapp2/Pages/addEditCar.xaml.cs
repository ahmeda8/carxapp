using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows;
using carXapp2;

namespace carXapp2
{
    public partial class AddEditCar : PhoneApplicationPage
    {
        private string carID;
        private carInfo CurrentCar;

        public AddEditCar()
        {
            InitializeComponent();

        }

        private void saveClick(object sender, EventArgs e)
        {
            if (carID == null)
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
                var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                               where cInfo.CarID == int.Parse(carID)
                               select cInfo;
                App.ViewModel.Database.carInfo.DeleteOnSubmit(carsInDb.FirstOrDefault());
                App.ViewModel.Database.carInfo.InsertOnSubmit(CurrentCar);
                App.ViewModel.Database.SubmitChanges();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("id", out carID);
            var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                           where cInfo.CarID == int.Parse(carID)
                           select cInfo;
            if (carID != null && CurrentCar == null)
                CurrentCar = carsInDb.FirstOrDefault();
            else if(CurrentCar == null)
                CurrentCar = new carInfo {CarRegExpiry = DateTime.Now, CarInsExpiry = DateTime.Now };
            this.DataContext = CurrentCar;
            ErrorLogging.Analytics(this.GetType().ToString(), "AddCar", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            ErrorLogging.Analytics(this.GetType().ToString(), "AddCar", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedFrom(e);
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            if (carID == null)
            {
                MessageBox.Show("Car not saved, cannot delete");
            }
            else
            {
                MessageBoxButton cancel = MessageBoxButton.OKCancel;
                MessageBoxResult result = MessageBox.Show("Are you sure to delete the entire car record", "Confirm",cancel);
                if (result == MessageBoxResult.OK)
                {
                    var carInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                                   where cInfo.CarID == int.Parse(carID)
                                   select cInfo;
                    var carfuelInDb = from fuelInfo fInfo in App.ViewModel.Database.fuelInfo
                                   where fInfo.CarID == int.Parse(carID)
                                   select fInfo;
                    var carmaintInDb = from maintInfo mInfo in App.ViewModel.Database.maintInfo
                                      where mInfo.CarID == int.Parse(carID)
                                      select mInfo;
                    App.ViewModel.Database.carInfo.DeleteOnSubmit(carInDb.FirstOrDefault());
                    App.ViewModel.Database.fuelInfo.DeleteAllOnSubmit(carfuelInDb);
                    App.ViewModel.Database.maintInfo.DeleteAllOnSubmit(carmaintInDb);
                    App.ViewModel.Database.SubmitChanges();
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                
                }
            }
        }
    }
}
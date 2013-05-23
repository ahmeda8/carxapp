using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Advertising;
using Microsoft.Phone.Tasks;

namespace carXapp2
{
    public partial class MainPage : PhoneApplicationPage
    {

        public ObservableCollection<CarListItem> CurrentCarList;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //this.DataContext = App.ViewModel;
        }

       private void addClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/addEditCar.xaml",UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Save changes to the database.
            App.ViewModel.Database.SubmitChanges();
            ErrorLogging.Analytics(this.GetType().ToString(), "Mainpage", e.NavigationMode.ToString(), string.Empty);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            listBox1.SelectedIndex = -1;
            NavigationService.RemoveBackEntry();
            NavigationService.RemoveBackEntry();
            CurrentCarList = new ObservableCollection<CarListItem>();
            var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                           select cInfo;
            var carsindbarray = carsInDb.ToArray();
            CarListItem cl;
            foreach (carInfo c in carsindbarray)
            {
                cl = new CarListItem { 
                    Year = c.CarYear,
                    Make = c.CarMake,
                    Model = c.CarModel,
                    Lic = c.CarLic,
                    ID = c.CarID
                };
                CurrentCarList.Add(cl);
            }

            listBox1.ItemsSource = CurrentCarList;
            ErrorLogging.Analytics(this.GetType().ToString(), "Mainpage", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string id = CurrentCarList[listBox1.SelectedIndex].ID.ToString();
                NavigationService.Navigate(new Uri("/Pages/MainPivotPage.xaml?id="+id+"&index="+listBox1.SelectedIndex, UriKind.Relative));
            }
        }

        private void menuSettings_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void about_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/about.xaml", UriKind.RelativeOrAbsolute));
        }

        
        private void AdControl_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            ErrorLogging.Log(this.GetType().ToString(), e.Error.Message, string.Empty, string.Empty);
        }

        private void listEditBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int carID = int.Parse(btn.Tag.ToString());
            NavigationService.Navigate(new Uri("/Pages/addEditCar.xaml?id="+carID, UriKind.Relative));
        }

        
    }
}
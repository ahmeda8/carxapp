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
using System.IO.IsolatedStorage;

namespace carXapp2
{
    public partial class MainPage : PhoneApplicationPage
    {

        public ObservableCollection<CarListItem> CurrentCarList;
        private Heroku _heroku;
        private BackgroundWorker DataLoader;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //this.DataContext = App.ViewModel;
            DataLoader = new BackgroundWorker();
            DataLoader.DoWork += new DoWorkEventHandler(DataLoader_DoWork);
            DataLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DataLoader_RunWorkerCompleted);

            CurrentCarList = new ObservableCollection<CarListItem>();
        }

        void DataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => {
                listBox1.ItemsSource = CurrentCarList;
                ToggleProgressIndicator();
                });
            
        }

        void DataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ToggleProgressIndicator();
            });
            //CurrentCarList = new ObservableCollection<CarListItem>();
            CurrentCarList = (ObservableCollection<CarListItem>)e.Argument;
            var carsInDb = from carInfo cInfo in App.ViewModel.Database.carInfo
                           select cInfo;
            var carsindbarray = carsInDb.ToArray();
            CarListItem cl;
            foreach (carInfo c in carsindbarray)
            {
                cl = new CarListItem
                {
                    Year = c.CarYear,
                    Make = c.CarMake,
                    Model = c.CarModel,
                    Lic = c.CarLic,
                    ID = c.CarID
                };
                CurrentCarList.Add(cl);
            }

            //listBox1.ItemsSource = CurrentCarList;
            //ErrorLogging.Analytics(this.GetType().ToString(), "Mainpage", e.NavigationMode.ToString(), string.Empty);

            string userid, email;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("userid", out userid);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("email", out email);
            if (userid != null && email != null)
            {
                _heroku = new Heroku();
                _heroku.UpdateUser(email, userid);
            }
        }

        private void ToggleProgressIndicator()
        {
            progressIndicator.IsIndeterminate = !progressIndicator.IsIndeterminate;
            progressIndicator.IsVisible = !progressIndicator.IsVisible;
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
            
            CurrentCarList.Clear();
            DataLoader.RunWorkerAsync(new ObservableCollection<CarListItem>());
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

        private void BackupClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/backup.xaml", UriKind.RelativeOrAbsolute));
        }

        
    }
}
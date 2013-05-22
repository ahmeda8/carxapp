using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using carXapp2;
using System.Collections.ObjectModel;
using System.ComponentModel;
using carXapp2.classes;
using System.Threading;
using System.IO.IsolatedStorage;

namespace carXapp2
{
    public partial class FuelInfo : PhoneApplicationPage
    {
        private string carID;
        public ObservableCollection<FuelListItem> CurrentFuelList;
      
        public FuelInfo()
        {
            InitializeComponent();
            //loading screen code
            Loaded += new RoutedEventHandler(FuelInfo_Loaded);

        }

        void FuelInfo_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(int.Parse(carID));
            toggleProgressBar();
        }

        
        private void LoadData(int id)
        {
            string distance, volume, average, currency;
            bool avgmethod;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("distance", out distance);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("volume", out volume);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("average", out average);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("currency", out currency);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("avgmethod", out avgmethod);


           var fuelInfoWithCarID = from fuelInfo info in App.ViewModel.Database.fuelInfo
                                   where info.CarID==id
                                   orderby info.Date ascending
                                   select info;
           var temp = fuelInfoWithCarID.ToArray();
           CurrentFuelList = new ObservableCollection<FuelListItem>();
           FuelListItem F;
           for (int i = 0; i < temp.Count(); i++)
           {
               F = new FuelListItem();
               F.FuelID = temp[i].FuelID;
               F.Cost = currency+" "+temp[i].Cost.ToString();
               F.Date = temp[i].DateStr;
               F.Fill = temp[i].Filled + " "+volume;
               F.MPG = "0 "+average;
               F.MilesDriven = temp[i].Miles+" Base "+distance;
                if (i > 0)
                {
                    if(!avgmethod)
                        F.MPG = ((temp[i].Miles - temp[i - 1].Miles) / temp[i].Filled).ToString("F1") + " " + average;
                    else
                        F.MPG = ((temp[i].Miles - temp[i - 1].Miles) / temp[i - 1].Filled).ToString("F1") + " " + average;
                    F.MilesDriven = (temp[i].Miles - temp[i - 1].Miles).ToString() + " " + distance + " Driven";
                }
               
               CurrentFuelList.Insert(0, F);
           }
           listBox1.ItemsSource = CurrentFuelList;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("carID", out carID);
            ErrorLogging.Analytics(this.GetType().ToString(), "FuelInfo", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        private void menuADD_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid="+carID,UriKind.Relative));
        }

       
        private void menuEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int FuelID = CurrentFuelList[listBox1.SelectedIndex].FuelID;
                NavigationService.Navigate(new Uri("/Pages/AddEditFuel.xaml?cid=" + carID + "&fid=" + FuelID, UriKind.Relative));
            }
            else
            {
                MessageBox.Show("please select a record");
            }
        }

        private void menuDel_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var selectedRecord = from fuelInfo info in App.ViewModel.Database.fuelInfo
                                    where info.CarID == int.Parse(carID) && info.FuelID == CurrentFuelList[listBox1.SelectedIndex].FuelID
                                    select info;
                App.ViewModel.Database.fuelInfo.DeleteOnSubmit(selectedRecord.FirstOrDefault());
                App.ViewModel.Database.SubmitChanges();
                CurrentFuelList.RemoveAt(listBox1.SelectedIndex);
            }
            else
            {
                MessageBox.Show("please select a record");
            }
        }


       


        private void toggleProgressBar()
        {
            //customIndeterminateProgressBar.IsIndeterminate = !(customIndeterminateProgressBar.IsIndeterminate);
            //customIndeterminateProgressBar.Visibility = Visibility.Collapsed;
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            ErrorLogging.Log(this.GetType().ToString(), e.Error.Message, string.Empty, string.Empty);
        }
                
    }
}
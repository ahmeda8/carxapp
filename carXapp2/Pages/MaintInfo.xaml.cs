using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using carXapp2;
using System.Collections.ObjectModel;
using System.ComponentModel;
using carXapp2.classes;
using System.IO.IsolatedStorage;

namespace carXapp2
{
    public partial class MaintInfo : PhoneApplicationPage
    {
        private string carID;
        private string maintID;
        private ObservableCollection<MaintListItem> CurrentMaintList;
        public MaintInfo()
        {
            InitializeComponent();
        }

        private void LoadData(string carID)
        {
            string currency;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("currency", out currency);

            var maintInfoDb = from maintInfo data in App.ViewModel.Database.maintInfo
                              where data.CarID == int.Parse(carID)
                              orderby data.Date descending
                              select data;
            CurrentMaintList = new ObservableCollection<MaintListItem>();
            var maininfinarray = maintInfoDb.ToArray();
            MaintListItem ml;
            foreach (maintInfo mi in maininfinarray)
            {
                ml = new MaintListItem
                {
                    Cost = currency+" "+(mi.PartCost+mi.LaborCharges).ToString(),
                    Date = mi.Date.ToShortDateString(),
                    MaintID = mi.MaintID,
                    Mainttype = mi.MaintType,
                    Partname = mi.PartName,
                    Partnumber = mi.PartNumber
                };
                CurrentMaintList.Add(ml);
            }
            listbox1.ItemsSource = CurrentMaintList;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("carID", out carID);
            NavigationContext.QueryString.TryGetValue("maintID", out maintID);
            LoadData(carID);
            listbox1.SelectedIndex = -1;
            ErrorLogging.Analytics(this.GetType().ToString(), "MaintInfo", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        private void menuAdd_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AddEditMaint.xaml?carID="+carID, UriKind.Relative));
        }

        private void menuEdit_click(object sender, EventArgs e)
        {
            if (listbox1.SelectedIndex != -1)
            {
                maintID = CurrentMaintList[listbox1.SelectedIndex].MaintID.ToString();
                NavigationService.Navigate(new Uri("/Pages/AddEditMaint.xaml?carID=" + carID + "&maintID=" + maintID, UriKind.Relative));
            }
            else
            {
                MessageBox.Show("please select a record");
            }
            
        }

        private void menuDelete_click(object sender, EventArgs e)
        {
            if (listbox1.SelectedIndex != -1)
            {
                maintID = CurrentMaintList[listbox1.SelectedIndex].MaintID.ToString();
                var maintInfoDb = from maintInfo data in App.ViewModel.Database.maintInfo
                                  where data.CarID == int.Parse(carID) && data.MaintID == int.Parse(maintID)
                                  select data;
                App.ViewModel.Database.maintInfo.DeleteOnSubmit(maintInfoDb.FirstOrDefault());
                CurrentMaintList.RemoveAt(listbox1.SelectedIndex);
                App.ViewModel.Database.SubmitChanges();
            }
            else
            {
                MessageBox.Show("please select a record");
            }
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            ErrorLogging.Log(this.GetType().ToString(), e.Error.Message, string.Empty, string.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using carXapp2;
using carXapp2.classes;

namespace carXapp2
{
    public partial class AddEditFuel : PhoneApplicationPage
    {
        private string carID;
        private string fuelID;
        public FuelRecord CurrentRecord;
        
        public AddEditFuel()
        {
            InitializeComponent();
        }

        private void Menu_save(object sender, EventArgs e)
        {
            bool success = false;
            if (fuelID == null)
            {
                success = addFuelRecord();
            }
            else
            {
                success = UpdateFuelRecord();
 
            }
            if (NavigationService.CanGoBack && success)
            { NavigationService.GoBack(); }
        }

        private bool addFuelRecord()
        {
            float cost, fill;
            int miles;
            var rs1 = float.TryParse(txtCost.Text, out cost);
            var rs2 = float.TryParse(txtFill.Text, out fill);
            var rs3 = int.TryParse(txtMiles.Text, out miles);

            if (txtMiles.Text.Length > 0 && txtCost.Text.Length > 0 && txtFill.Text.Length > 0 && datePick.Value.ToString().Length > 0
                && rs1 && rs2 && rs3 )
            {
                fuelInfo finfo = new fuelInfo { 
                    Cost = cost,
                    DateStr = datePick.Value.Value.ToShortDateString(),
                    Date = datePick.Value.Value,
                    Filled = fill,
                    Miles = miles,
                    CarID = int.Parse(carID)
                };
                App.ViewModel.Database.fuelInfo.InsertOnSubmit(finfo);
                App.ViewModel.Database.SubmitChanges();
                return true;
                    
            }
            else
            {
                MessageBox.Show("Incomplete or Incorrect data");
                return false; 
            }
        }

        private bool UpdateFuelRecord()
        {
             float cost, fill;
            int miles;
            var rs1 = float.TryParse(txtCost.Text, out cost);
            var rs2 = float.TryParse(txtFill.Text, out fill);
            var rs3 = int.TryParse(txtMiles.Text, out miles);

            if (txtMiles.Text.Length > 0 && txtCost.Text.Length > 0 && txtFill.Text.Length > 0 && datePick.Value.ToString().Length > 0
                && rs1 && rs2 && rs3)
            {
                var record = from fuelInfo info in App.ViewModel.Database.fuelInfo
                             where info.CarID == int.Parse(carID) && info.FuelID == int.Parse(fuelID)
                             orderby info.Date ascending
                             select info;
                App.ViewModel.Database.fuelInfo.DeleteOnSubmit(record.FirstOrDefault());
                fuelInfo finfo = new fuelInfo
                {
                    Cost = cost,
                    DateStr = datePick.Value.Value.ToShortDateString(),
                    Date = datePick.Value.Value,
                    Filled = fill,
                    Miles = miles,
                    CarID = int.Parse(carID)
                };
                App.ViewModel.Database.fuelInfo.InsertOnSubmit(finfo);
                App.ViewModel.Database.SubmitChanges();
                
                return true;
            }
            else
            {
                MessageBox.Show("Incorrect or Incomplete data");
                return false;

            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("cid", out carID);
            NavigationContext.QueryString.TryGetValue("fid", out fuelID);
            if (fuelID != null && CurrentRecord == null)
            {
                var record = from fuelInfo info in App.ViewModel.Database.fuelInfo
                             where info.CarID == int.Parse(carID) && info.FuelID == int.Parse(fuelID)
                             orderby info.Date ascending
                             select info;
                CurrentRecord = new FuelRecord
                {
                    Cost = record.FirstOrDefault().Cost.ToString(),
                    Date = record.FirstOrDefault().DateStr,
                    Fill = record.FirstOrDefault().Filled.ToString(),
                    Odometer = record.FirstOrDefault().Miles.ToString()

                };
            }
            else if(CurrentRecord == null)
            {
                CurrentRecord = new FuelRecord { PreciseDateTime = DateTime.Now, Date = DateTime.Now.ToShortDateString() };
            }
            this.DataContext = CurrentRecord;
            ErrorLogging.Analytics(this.GetType().ToString(), "AddFuel", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            ErrorLogging.Log(this.GetType().ToString(), e.Error.Message, string.Empty, string.Empty);
        }

        private void Menu_delete(object sender, EventArgs e)
        {
            if (fuelID != null)
            {
                MessageBoxButton cancel = MessageBoxButton.OKCancel;
                MessageBoxResult result = MessageBox.Show("delete this fuel record?", "Confirm",cancel);
                if (result == MessageBoxResult.OK)
                {
                    var query = from fuelInfo f in App.ViewModel.Database.fuelInfo
                                where f.FuelID == int.Parse(fuelID)
                                select f;
                    App.ViewModel.Database.fuelInfo.DeleteAllOnSubmit(query);
                    App.ViewModel.Database.SubmitChanges();
                }
            }
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }


    }
}
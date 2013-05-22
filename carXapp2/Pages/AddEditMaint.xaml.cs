using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace carXapp2
{
    public partial class AddEditMaint : PhoneApplicationPage
    {
        private string carID;
        private string maintID;
        private maintInfo CurrentMaintInfo;

        public AddEditMaint()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if (maintID != null)
            {
                var maintRecord = from maintInfo mi in App.ViewModel.Database.maintInfo
                                  where mi.CarID == int.Parse(carID) && mi.MaintID == int.Parse(maintID)
                                  select mi;
                CurrentMaintInfo = maintRecord.FirstOrDefault();
                
            }
            else if(CurrentMaintInfo == null)
            {
                CurrentMaintInfo = new maintInfo { Date = DateTime.Now, DateStr = DateTime.Now.ToShortDateString(),CarID = int.Parse(carID)};
            }
            this.DataContext = CurrentMaintInfo;
            
        }

        private void menuSave_click(object sender, EventArgs e)
        {
            bool success = false;
            if (maintID == null)
            {
                success = addRecord();
            }
            else
            {
                success = UpdateRecord();
            }

            if (NavigationService.CanGoBack && success)
            {
                NavigationService.GoBack();
            }
        }

        private bool addRecord()
        {
            float lch, pco;
            int miles;
            var rs1 = float.TryParse(txtLaborCharges.Text, out lch);
            var rs2 = float.TryParse(txtPartCost.Text, out pco);
            //var rs3 = float.TryParse(txtTotalCost.Text, out tco);
            var rs4 = int.TryParse(txtMiles.Text, out miles);
            if (rs1 && rs2 &&  rs4)
            {
                CurrentMaintInfo.TotalCost = float.Parse(txtLaborCharges.Text) + float.Parse(txtPartCost.Text);
                App.ViewModel.Database.maintInfo.InsertOnSubmit(CurrentMaintInfo);
                App.ViewModel.Database.SubmitChanges();
                return true;

            }
            else
            {
                MessageBox.Show("Incorrect or Incomplete data");
                return false;
            }
            
        }

        private bool UpdateRecord()
        {
            float lch, pco;
            int miles;
            var rs1 = float.TryParse(txtLaborCharges.Text, out lch);
            var rs2 = float.TryParse(txtPartCost.Text, out pco);
            //var rs3 = float.TryParse(txtTotalCost.Text, out tco);
            var rs4 = int.TryParse(txtMiles.Text, out miles);
            if (rs1 && rs2 && rs4)
            {
                CurrentMaintInfo.TotalCost = float.Parse(txtLaborCharges.Text) + float.Parse(txtPartCost.Text);
                var maintRecord = from maintInfo mi in App.ViewModel.Database.maintInfo
                                  where mi.CarID == int.Parse(carID) && mi.MaintID == int.Parse(maintID)
                                  select mi;
                App.ViewModel.Database.maintInfo.DeleteOnSubmit(maintRecord.FirstOrDefault());
                App.ViewModel.Database.maintInfo.InsertOnSubmit(CurrentMaintInfo);
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
            NavigationContext.QueryString.TryGetValue("carID", out carID);
            NavigationContext.QueryString.TryGetValue("maintID", out maintID);
            LoadData();
            ErrorLogging.Analytics(this.GetType().ToString(), "AddMaint", e.NavigationMode.ToString(), string.Empty);
            base.OnNavigatedTo(e);
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            ErrorLogging.Log(this.GetType().ToString(), e.Error.Message, string.Empty, string.Empty);
        }
    }
}
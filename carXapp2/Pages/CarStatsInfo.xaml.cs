using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using carXapp2;
using carXapp2.classes;
using AmCharts.Windows.QuickCharts;
using System.Windows;

namespace carXapp2
{
    public partial class CarStatsInfo : PhoneApplicationPage
    {
        private int carID;
        bool fVisibility= false;
        bool MVisibility = false;
        private ObservableCollection<ChartData> _fuelChartDataSet = new ObservableCollection<ChartData>();
        public ObservableCollection<ChartData> FuelChartDataSet
        {
            get { return _fuelChartDataSet; }
            set { _fuelChartDataSet = value; }
        }

        private ObservableCollection<ChartData> _maintChartDataSet = new ObservableCollection<ChartData>();
        public ObservableCollection<ChartData> MaintChartDataSet
        {
            get { return _maintChartDataSet; }
            set { _maintChartDataSet = value; }
        }

        public CarStatsInfo()
        {
            FuelChartDataSet = new ObservableCollection<ChartData>();
            InitializeComponent();
            this.DataContext = this;

        }

        private void LoadData(int id)
        {
            var carInfo = from carInfo ci in App.ViewModel.Database.carInfo
                          where ci.CarID == id
                          select ci;
            grid1.DataContext = carInfo.FirstOrDefault();
            var fuelInfoWithCarID = from fuelInfo info in App.ViewModel.Database.fuelInfo
                                    where info.CarID == id
                                    orderby info.Date ascending
                                    select info;
            var fuel = new ObservableCollection<fuelInfo>(fuelInfoWithCarID);

            var MaintInfoWithCarID = from maintInfo info in App.ViewModel.Database.maintInfo
                                     where info.CarID == id
                                     orderby info.Date ascending
                                     select info;
            var maint = new ObservableCollection<maintInfo>(MaintInfoWithCarID);

            float distance = 0f;
            if (fuel.Count > 1)
                distance = fuel.Last().Miles - fuel.First().Miles;
            float volume = 0f;
            float total_cost = 0f;
            Dictionary<string, float> fuelmonthCost = new Dictionary<string, float>();
            Dictionary<string, float> fuelmonthMileage = new Dictionary<string, float>();
            Dictionary<string, float> mainttotalmonth = new Dictionary<string, float>();
            Dictionary<string, float> maintpartsmonth = new Dictionary<string, float>();
            Dictionary<string, float> maintlabormonth = new Dictionary<string, float>();

            int start_miles_for_month = 0;
            int end_miles_for_month = 0;
            float volume_for_month = 0f;
            float milage_for_month = 0f;
            for (int i = 0; i < fuel.Count;i++)
            {
                if(i < fuel.Count-1)
                    volume += fuel[i].Filled;
                total_cost += fuel[i].Cost;
                volume_for_month += fuel[i].Filled;
                end_miles_for_month = fuel[i].Miles;
                if (fuel[i].Date.Year == DateTime.Now.Year)
                {
                    if (!fuelmonthCost.ContainsKey(fuel[i].Date.ToString("MMM")))
                    {
                        milage_for_month = (end_miles_for_month - start_miles_for_month) / volume_for_month;
                        fuelmonthCost.Add(fuel[i].Date.ToString("MMM"), 0f);
                        start_miles_for_month = fuel[i].Miles;
                        volume_for_month = 0f;
                    }
                    fuelmonthCost[fuel[i].Date.ToString("MMM")] += fuel[i].Cost;
                    if (!fuelmonthMileage.ContainsKey(fuel[i].Date.ToString("MMM")))
                    {
                        fuelmonthMileage.Add(fuel[i].Date.ToString("MMM"), 0f);
                        if (i != 0)
                            fuelmonthMileage[fuel[i - 1].Date.ToString("MMM")] = milage_for_month;
                    }
                }
            }
            float mpg = distance / volume;
            txtmpg.Text = mpg.ToString("F2");
            txtMiles.Text = distance.ToString();
            txtFuelCost.Text = total_cost.ToString("F2");

            //FuelChartDataSet = new ObservableCollection<ChartData>();
            foreach (var item in fuelmonthCost)
            {
                FuelChartDataSet.Add(new ChartData { val1 = item.Key.ToString(), val2 = item.Value,val3=fuelmonthMileage[item.Key.ToString()]});
            }
            //chart1.DataContext = this;

            //timewise stats
            TimeSpan ts = new TimeSpan();
            
            if (fuel.Count > 1)
            {
                ts = fuel.Last().Date - fuel.First().Date;
            }
            txtFCoPd.Text = (total_cost / ts.TotalDays).ToString("F2");
            txtFCoPm.Text = (total_cost / ts.TotalDays * 30).ToString("F2");
            txtMiPd.Text = (distance / ts.TotalDays).ToString("F2");
            txtMiPm.Text = (distance / ts.TotalDays * 30).ToString("f2");
            
            
            //List<ChartData> fuelChartData = new List<ChartData>();

            //stats for maint

            float total_maint_parts = 0f;
            float total_maint_labor = 0f;
            foreach (maintInfo mi in maint)
            {
                total_maint_labor += mi.LaborCharges;
                total_maint_parts += mi.PartCost;
                if (mi.Date.Year == DateTime.Now.Year)
                {
                    if (!mainttotalmonth.ContainsKey(mi.Date.ToString("MMM")))
                    {
                        mainttotalmonth.Add(mi.Date.ToString("MMM"), 0f);
                        maintpartsmonth.Add(mi.Date.ToString("MMM"), 0f);
                        maintlabormonth.Add(mi.Date.ToString("MMM"), 0f);
                    }

                    mainttotalmonth[mi.Date.ToString("MMM")] += mi.PartCost + mi.LaborCharges;
                    maintpartsmonth[mi.Date.ToString("MMM")] += mi.PartCost;
                    maintlabormonth[mi.Date.ToString("MMM")] += mi.LaborCharges;
                }
            }
            txtMaPco.Text = total_maint_parts.ToString("F2");
            txtMaLco.Text = total_maint_labor.ToString("F2");
            txtToMaCo.Text = (total_maint_labor + total_maint_parts).ToString("F2");

            foreach (var mitem in mainttotalmonth)
            {
                MaintChartDataSet.Add(new ChartData {val1=mitem.Key,val2 = mainttotalmonth[mitem.Key],val3 = maintpartsmonth[mitem.Key],val4 = maintlabormonth[mitem.Key]});
            }
        }

        public class ChartData
        {
            public string val1 { get; set; }
            public float val2 { get; set; }
            public float val3 { get; set; }
            public float val4 { get; set; }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string cid;
            NavigationContext.QueryString.TryGetValue("carID", out cid);
            int.TryParse(cid,out carID);
            grid2.DataContext = this;
            LoadData(carID);
            ErrorLogging.Analytics(this.GetType().ToString(), "CarStatInfo", e.NavigationMode.ToString(), string.Empty);
        }
        
        private void fuelBtn_Click(object sender, RoutedEventArgs e)
        {
            fVisibility = !fVisibility;
            if (!fVisibility)
            {
                txtFCoPd.Visibility = System.Windows.Visibility.Collapsed;
                txtFCoPm.Visibility = System.Windows.Visibility.Collapsed;
                txtMiPd.Visibility = System.Windows.Visibility.Collapsed;
                txtMiPm.Visibility = System.Windows.Visibility.Collapsed;
                lblFcoPd.Visibility = System.Windows.Visibility.Collapsed;
                lblFcoPm.Visibility = System.Windows.Visibility.Collapsed;
                lblFmiPd.Visibility = System.Windows.Visibility.Collapsed;
                lblFMiPm.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                txtFCoPd.Visibility = System.Windows.Visibility.Visible;
                txtFCoPm.Visibility = System.Windows.Visibility.Visible;
                txtMiPd.Visibility = System.Windows.Visibility.Visible;
                txtMiPm.Visibility = System.Windows.Visibility.Visible;
                lblFcoPd.Visibility = System.Windows.Visibility.Visible;
                lblFcoPm.Visibility = System.Windows.Visibility.Visible;
                lblFmiPd.Visibility = System.Windows.Visibility.Visible;
                lblFMiPm.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void btnMaint_Click(object sender, RoutedEventArgs e)
        {
            MVisibility = !MVisibility;
            if (!MVisibility)
            {
                txtMaLco.Visibility = System.Windows.Visibility.Collapsed;
                txtMaPco.Visibility = System.Windows.Visibility.Collapsed;
                txtToMaCo.Visibility = System.Windows.Visibility.Collapsed;
                lblMaLco.Visibility = System.Windows.Visibility.Collapsed;
                lblMaPco.Visibility = System.Windows.Visibility.Collapsed;
                lblMaToCo.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                txtMaLco.Visibility = System.Windows.Visibility.Visible;
                txtMaPco.Visibility = System.Windows.Visibility.Visible;
                txtToMaCo.Visibility = System.Windows.Visibility.Visible;
                lblMaLco.Visibility = System.Windows.Visibility.Visible;
                lblMaPco.Visibility = System.Windows.Visibility.Visible;
                lblMaToCo.Visibility = System.Windows.Visibility.Visible;

 
            }
        }

    }
}
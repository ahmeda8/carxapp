using System;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Windows;

namespace carXapp2
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Settings_Loaded);
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            string distance, volume, average, currency;
            bool avgmethod;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("distance", out distance);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("volume", out volume);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("average", out average);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("currency", out currency);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("avgmethod", out avgmethod);

            txtDistance.Text = distance;
            txtVolume.Text = volume;
            //txtAverage.Text = average;
            txtCurrency.Text = currency;
            checkBox1.IsChecked = avgmethod;
            ErrorLogging.Analytics(this.GetType().ToString(), "Settings", "Loaded", string.Empty);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (txtDistance.Text.Length > 0 && txtCurrency.Text.Length > 0 && txtVolume.Text.Length > 0)
            {
                IsolatedStorageSettings.ApplicationSettings["distance"] = txtDistance.Text;
                IsolatedStorageSettings.ApplicationSettings["volume"] = txtVolume.Text;
                IsolatedStorageSettings.ApplicationSettings["currency"] = txtCurrency.Text;
                //IsolatedStorageSettings.ApplicationSettings["average"] = txtAverage.Text;
                IsolatedStorageSettings.ApplicationSettings["avgmethod"] = checkBox1.IsChecked;
                IsolatedStorageSettings.ApplicationSettings.Save();
                MessageBox.Show("Settings saved");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Some invalid input was provided");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Facebook;
using carXapp2;

namespace carXapp2.Pages
{
    public partial class backup : PhoneApplicationPage
    {
        private string id;
        private string accessToken;
        private Filepicker_io fioInstance;

        public backup()
        {
            InitializeComponent();
            fioInstance = Filepicker_io.GetInstance();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("fbID", out id);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("fbAccessToken",out accessToken);

            if (id != null)
            {
                GetUserProfilePicture(id, accessToken);
                GraphApiSample(accessToken);
                fbBtn.Content = "Logout";
            }
        }

        private void GetUserProfilePicture(string _userId,string _accessToken)
        {
            // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
            // for more info visit http://developers.facebook.com/docs/reference/api
            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", _userId, "square", _accessToken);

            picProfile.Source = new BitmapImage(new Uri(profilePictureUrl));
        }

        private void GraphApiSample(string _accessToken)
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    tFbName.Text = "Logged in as: " + (string)result["name"];
                    
                });
            };

            fb.GetAsync("me");
        }

        private void fblogin_click(object sender, RoutedEventArgs e)
        {
            if (id != null)
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("fbID");
                IsolatedStorageSettings.ApplicationSettings.Remove("fbAccessToken");
                IsolatedStorageSettings.ApplicationSettings.Save();
                fbBtn.Content = "Facebook Login";
                tFbName.Text =  "Not logged in";
                id = null;
                accessToken = null;
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
            }
        }

        private void btnBackup_click(object sender, RoutedEventArgs e)
        {
            fioInstance.Upload();

        }

        private void btnRestore_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
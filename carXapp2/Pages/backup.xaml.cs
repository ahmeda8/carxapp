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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace carXapp2.Pages
{
    public partial class backup : PhoneApplicationPage
    {
        private string id,profilePicUrl;
        private string accessToken;
        private ObservableCollection<BackupsListItem> BackupsList;
        private Filepicker_io fioInstance;
        private Heroku heroku;
        private BackgroundWorker BackupWorker;
        private BackgroundWorker DataLoader;

        public backup()
        {
            InitializeComponent();
            fioInstance = Filepicker_io.GetInstance();
            heroku = new Heroku();
            BackupWorker = new BackgroundWorker();
            BackupWorker.DoWork += new DoWorkEventHandler(BackupWorker_DoWork);
            //BackupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackupWorker_RunWorkerCompleted);

            DataLoader = new BackgroundWorker();
            DataLoader.DoWork += new DoWorkEventHandler(DataLoader_DoWork);
            DataLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DataLoader_RunWorkerCompleted);
        }

        void DataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //string backups;
            //IsolatedStorageSettings.ApplicationSettings.TryGetValue("backups", out backups);
            //JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(backups);
            Dispatcher.BeginInvoke(() =>
            {
                if (id != null)
                {
                    fbBtn.Content = "Logout";
                    picProfile.Source = new BitmapImage(new Uri(profilePicUrl));
                }
                BackupslistBox.ItemsSource = BackupsList;
            });
        }

        void DataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("fbid", out id);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("fbAccessToken", out accessToken);
            string userid;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("userid", out userid);
            if (id != null)
            {
                profilePicUrl = GetUserProfilePicture(id, accessToken);
                GraphApiSample(accessToken);
                heroku.GetBackups(userid);
            }
            string backups;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("backups",out backups);
            if (backups != null)
            {
                JsonArray jarr = (JsonArray)SimpleJson.DeserializeObject(backups);
                BackupsList = (ObservableCollection<BackupsListItem>)e.Argument;
                BackupsListItem item;
                foreach (JsonObject jobj in jarr)
                {
                    item = new BackupsListItem();
                    item.Created = DateTime.Parse(jobj["created"].ToString());
                    BackupsList.Add(item);
                }
            }
        }

        void BackupWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void BackupWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            fioInstance.Upload();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataLoader.RunWorkerAsync(new ObservableCollection<BackupsListItem>());
            base.OnNavigatedTo(e);
        }

        private string GetUserProfilePicture(string _userId,string _accessToken)
        {
            // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
            // for more info visit http://developers.facebook.com/docs/reference/api
            return string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", _userId, "square", _accessToken);

            //picProfile.Source = new BitmapImage(new Uri(profilePictureUrl));
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
                    IsolatedStorageSettings.ApplicationSettings["firstname"] = (string)result["first_name"];
                    IsolatedStorageSettings.ApplicationSettings["lastname"] = (string)result["last_name"];
                    IsolatedStorageSettings.ApplicationSettings["email"] = (string)result["email"];
                    IsolatedStorageSettings.ApplicationSettings["fbid"] = (string)result["id"];
                    IsolatedStorageSettings.ApplicationSettings.Save();
                    heroku.AddUser((string)result["id"], (string)result["email"], (string)result["first_name"], (string)result["last_name"]);
                });
            };

            fb.GetAsync("me");
        }

        private void fblogin_click(object sender, RoutedEventArgs e)
        {
            if (id != null)
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("fbid");
                IsolatedStorageSettings.ApplicationSettings.Remove("fbAccessToken");
                IsolatedStorageSettings.ApplicationSettings.Remove("userid");
                IsolatedStorageSettings.ApplicationSettings.Remove("firstname");
                IsolatedStorageSettings.ApplicationSettings.Remove("lastname");
                IsolatedStorageSettings.ApplicationSettings.Remove("email");
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

        private void btnBackup_click(object sender, EventArgs e)
        {
            string userid;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("userid", out userid);
            if (userid != null)
                BackupWorker.RunWorkerAsync();
            else
                MessageBox.Show("Please login first");
        }

        private void btnRefresh_click(object sender, EventArgs e)
        {
            DataLoader.RunWorkerAsync(new ObservableCollection<BackupsListItem>());
        }

        private void backupRestoreBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
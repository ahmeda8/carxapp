using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Facebook;
using System.Text.RegularExpressions;

namespace facebook_windows_phone_sample.Pages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = "130555453814753";

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,read_stream";

        private readonly FacebookOAuthClient _fbOAuth = new FacebookOAuthClient();
        //private readonly FacebookClient _fb = new FacebookClient();

        public FacebookLoginPage()
        {
            InitializeComponent();            
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }
            
            return _fbOAuth.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string htmlstr = webBrowser1.SaveToString();
            FacebookOAuthResult oauthResult;
             if (!_fbOAuth.TryParseResult(e.Uri, out oauthResult))
            {   
                return;
            }

            if (oauthResult.IsSuccess)
            {
                
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

           IsolatedStorageSettings.ApplicationSettings["fbAccessToken"] = accessToken;

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];
                IsolatedStorageSettings.ApplicationSettings["fbID"] = id;
                IsolatedStorageSettings.ApplicationSettings.Save();
                
                Dispatcher.BeginInvoke(() =>
                {
                    if (NavigationService.CanGoBack)
                    {
                        //webBrowser1.Navigate(new Uri("http://m.facebook.com/index.php?stype=lo"));
                        /*
                        using(IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            var dir = iso.GetDirectoryNames();
                            var files = iso.GetFileNames("Microsoft");
                            var files1 = iso.GetFileNames("Shared");
                            var files2 = iso.GetFileNames("AppCache");
                            iso.DeleteDirectory("AppCache");
                            iso.DeleteDirectory("Microsoft");

                        }
                         */
                        NavigationService.GoBack();
                    }
                });
            };

            fb.GetAsync("me?fields=id");
        }
    }
}
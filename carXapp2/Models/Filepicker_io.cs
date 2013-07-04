using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using carXapp2;
using System.IO.IsolatedStorage;
using Microsoft.Phone.BackgroundTransfer;
using System.Diagnostics;
using SimpleJson;
using System.IO;
using System.Net;

namespace carXapp2
{
    class Filepicker_io 
    {
        private static Filepicker_io instance = null;
        private BackgroundTransferRequest BTR;
        private BackgroundTransferRequest BTR_Download;
        private const string FILEPICKER_BASEURL = "http://www.filepicker.io";
        public static string FILEPICKER_APIKEY = "AQ4LQWd28TyS1wZtDX9Rjz";
        private const string TRANSFER_FOLDER = "/shared/transfers";
        private const string SAVE_RESPONSE_LOCATION = TRANSFER_FOLDER + "/RESP.JSON";
        private const string DOWNLOAD_LOCATION = TRANSFER_FOLDER + "/cars.sdf";
        private bool Uploading = false;
        private bool Downloading = false;
        private Heroku _heroku;
        private AsyncCallback _uploadCallback;

        public static Filepicker_io GetInstance()
        {
            if (instance == null)
            {
                instance = new Filepicker_io();
            }
            return instance;
        }

        public Filepicker_io()
        {
            //string upload_uri = FILEPICKER_BASEURL + "/api/store/S3?key=" + FILEPICKER_APIKEY;
            //BTR = new BackgroundTransferRequest(new Uri(upload_uri));
            //BTR.TransferStatusChanged += BTR_TransferStatusChanged;
            //BTR.TransferProgressChanged += BTR_TransferProgressChanged;
            //BTR.Method = "POST";
            _heroku = new Heroku();
        }


        void BTR_TransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            Debug.WriteLine("Sent"+e.Request.BytesSent);
            Debug.WriteLine("Received" + e.Request.BytesReceived);
        }

        void BTR_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            if (e.Request.TransferStatus == TransferStatus.Completed )
            {
                Uploading = false;
                if (e.Request.StatusCode == 200)
                {
                    using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (iso.FileExists(SAVE_RESPONSE_LOCATION))
                        {
                            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(SAVE_RESPONSE_LOCATION, System.IO.FileMode.Open, iso);
                            //byte[] buffer = new byte[fs.Length];
                            //fs.Read(buffer, 0, (int)fs.Length);
                            StreamReader str = new StreamReader(fs);
                            string resp = str.ReadToEnd();
                            fs.Close();
                            JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(resp);
                            string userid;
                            IsolatedStorageSettings.ApplicationSettings.TryGetValue("userid",out userid);
                            if(userid != null)
                                _heroku.AddBackup(jobj["key"].ToString(),jobj["url"].ToString(), userid);
                            iso.DeleteFile(SAVE_RESPONSE_LOCATION);
                            _uploadCallback.Invoke(new AsyncCallbackEvent("success"));
                        }
                    }
                }
                BackgroundTransferService.Remove(e.Request);
            }
            Debug.WriteLine(e.Request.TransferStatus);
        }

        public void Upload(AsyncCallback UploadCallback)
        {
            string upload_uri = FILEPICKER_BASEURL + "/api/store/S3?key=" + FILEPICKER_APIKEY;
            BTR = new BackgroundTransferRequest(new Uri(upload_uri));
            BTR.TransferStatusChanged += BTR_TransferStatusChanged;
            BTR.TransferProgressChanged += BTR_TransferProgressChanged;
            BTR.Method = "POST";

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(TRANSFER_FOLDER))
                {
                    iso.CreateDirectory(TRANSFER_FOLDER);
                }
                iso.CopyFile("/cars.sdf", TRANSFER_FOLDER + "/cars.sdf",true);
            }
            BTR.DownloadLocation = new Uri(SAVE_RESPONSE_LOCATION, UriKind.Relative);
            BTR.UploadLocation = new Uri(TRANSFER_FOLDER+"/cars.sdf",UriKind.Relative);
            if (!Uploading)
            {
                Uploading = true;
                if (BackgroundTransferService.Requests.Contains(BTR))
                    BackgroundTransferService.Remove(BTR);
                BackgroundTransferService.Add(BTR);
                _uploadCallback = UploadCallback;
            }
        }

        public void Delete(string url)
        {
            HttpWebRequest wr = HttpWebRequest.CreateHttp(url);
            wr.Method = "DELETE";
            wr.BeginGetResponse((res) => {
                HttpWebRequest wr1 = (HttpWebRequest)res.AsyncState;
                HttpWebResponse resp = (HttpWebResponse)wr1.EndGetResponse(res);
                StreamReader str = new StreamReader(resp.GetResponseStream());
                string rtxtResp = str.ReadToEnd();
                Debug.WriteLine(rtxtResp);

            }, wr);
        }

        public void Download(string url)
        {
            BTR_Download = new BackgroundTransferRequest(new Uri(url));
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(TRANSFER_FOLDER))
                {
                    iso.CreateDirectory(TRANSFER_FOLDER);
                }
            }
            BTR_Download.DownloadLocation = new Uri(DOWNLOAD_LOCATION,UriKind.Relative);
            BTR_Download.TransferStatusChanged += BTR_Download_TransferStatusChanged;
            if (!Downloading)
            {
                if (BackgroundTransferService.Requests.Contains(BTR_Download))
                    BackgroundTransferService.Remove(BTR_Download);
                BackgroundTransferService.Add(BTR_Download);
                Downloading = true;
            }
        }

        void BTR_Download_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            if (e.Request.TransferStatus == TransferStatus.Completed)
            {
                Downloading = false;
                if (e.Request.StatusCode == 200)
                {
                    using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if(iso.FileExists(DOWNLOAD_LOCATION))
                        {
                            iso.CopyFile(DOWNLOAD_LOCATION, "/cars.sdf",true);
                        }
                    }
                    string DBConnectionString = "Data Source=isostore:/cars.sdf";
                    /*
                    App.ViewModel.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues,App.ViewModel.Database.carInfo);
                    App.ViewModel.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, App.ViewModel.Database.fuelInfo);
                    App.ViewModel.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, App.ViewModel.Database.maintInfo);
                    App.ViewModel.Database.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, App.ViewModel.Database.settingsInfo);
                     */
                    App.ViewModel = null;
                    App.ViewModel = new Data(DBConnectionString);
                }
                BackgroundTransferService.Remove(e.Request);
            }
            Debug.WriteLine(e.Request.TransferStatus);
            Debug.WriteLine("downloaded-"+e.Request.BytesReceived);
        }
    }
}

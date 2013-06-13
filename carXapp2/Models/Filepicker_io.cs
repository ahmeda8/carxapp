using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using carXapp2;
using System.IO.IsolatedStorage;
using Microsoft.Phone.BackgroundTransfer;
using System.Diagnostics;

namespace carXapp2
{
    class Filepicker_io 
    {
        private static Filepicker_io instance = null;
        private BackgroundTransferRequest BTR;
        private const string FILEPICKER_BASEURL = "https://www.filepicker.io";
        private const string FILEPICKER_APIKEY = "AQ4LQWd28TyS1wZtDX9Rjz";
        private const string TRANSFER_FOLDER = "/shared/transfers";
        private bool Uploading = false;

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
            string upload_uri = FILEPICKER_BASEURL + "/api/store/S3?key=" + FILEPICKER_APIKEY;
            BTR = new BackgroundTransferRequest(new Uri(upload_uri));
            BTR.TransferStatusChanged += BTR_TransferStatusChanged;
            BTR.TransferProgressChanged += BTR_TransferProgressChanged;
            BTR.Method = "POST";
        }


        void BTR_TransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            Debug.WriteLine("Sent"+e.Request.BytesSent);
            Debug.WriteLine("Received" + e.Request.BytesReceived);
        }

        void BTR_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            if (e.Request.TransferStatus == TransferStatus.Completed)
            {
                Uploading = false;
            }
            Debug.WriteLine(e.Request.TransferStatus);
        }

        public void Upload()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(TRANSFER_FOLDER))
                {
                    iso.CreateDirectory(TRANSFER_FOLDER);
                }
                iso.CopyFile("/cars.sdf", TRANSFER_FOLDER + "/cars.sdf",true);
            }
            BTR.DownloadLocation = new Uri(TRANSFER_FOLDER + "/resp.json", UriKind.Relative);
            BTR.UploadLocation = new Uri(TRANSFER_FOLDER+"/cars.sdf",UriKind.Relative);
            if (!Uploading)
            {
                Uploading = true;
                BackgroundTransferService.Add(BTR);
            }
        }
    }
}

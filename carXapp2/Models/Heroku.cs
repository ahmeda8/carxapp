using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Net;
using System.IO;
using SimpleJson;
using System.Globalization;

namespace carXapp2
{
    class Heroku : WebMethod
    {
        private const string HEROKU_BASEURL = "http://driverdashserver.herokuapp.com";
        private AsyncCallback GetBackupCallBack;
        public override void GET_Method_CallBack(IAsyncResult res)
        {
            HttpWebRequest wr = (HttpWebRequest)res.AsyncState;
            HttpWebResponse wrsp = (HttpWebResponse)wr.EndGetResponse(res);
            StreamReader strm = new StreamReader(wrsp.GetResponseStream());
            string response = strm.ReadToEnd();
            
            GetBackupCallBack.Invoke(new AsyncCallbackEvent(response));
            //JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(response);
        }

        public override void PUT_Method_CallBack(IAsyncResult res)
        {
            HttpWebRequest wr = (HttpWebRequest)res.AsyncState;
            HttpWebResponse wrsp = (HttpWebResponse)wr.EndGetResponse(res);
            StreamReader strm = new StreamReader(wrsp.GetResponseStream());
            string response = strm.ReadToEnd();
            JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(response, typeof(JsonObject));
        }

        public override void POST_Method_CallBack(IAsyncResult res)
        {
            HttpWebRequest wr = (HttpWebRequest)res.AsyncState;
            HttpWebResponse wrsp =(HttpWebResponse)wr.EndGetResponse(res) ;
            StreamReader strm = new StreamReader(wrsp.GetResponseStream());
            string response = strm.ReadToEnd();
            JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(response,typeof(JsonObject));
            if (jobj != null && jobj.ContainsKey("id"))
            {
                string email, userid;
                userid = jobj["id"].ToString();
                IsolatedStorageSettings.ApplicationSettings["userid"] = userid;
                IsolatedStorageSettings.ApplicationSettings.Save();
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("email",out email);
            }
        }

        public void AddUser(string fbid, string email, string firstName, string lastName)
        {
            string created = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern);
            string lastLogin = created;
            JsonObject user = new JsonObject();
            user.Add("fbid", fbid);
            user.Add("email", email);
            user.Add("created", created);
            user.Add("last_login", lastLogin);
            user.Add("firstname", firstName);
            user.Add("lastname", lastName);
            string req_url = HEROKU_BASEURL + "/user";
            string msg = "user=" + SimpleJson.SerializeObject(user);
            base.POST(req_url, msg);
        }

        public void UpdateUser(string email, string id)
        {
            DateTime login = DateTime.Now;
            JsonObject user = new JsonObject();
            user.Add("id", id);
            user.Add("email", email);
            user.Add("logintime", login.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern)); 
            string req_url = HEROKU_BASEURL + "/user";
            string msg = "user=" + SimpleJson.SerializeObject(user);
            base.PUT(req_url, msg);
        }

        public void AddBackup(string filename, string download_url, string userid)
        {
            DateTime login = DateTime.Now;
            JsonObject user = new JsonObject();
            user.Add("filename", filename);
            user.Add("download_url", download_url);
            user.Add("iduser", userid);
            user.Add("created", login.ToString(CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern));
            string req_url = HEROKU_BASEURL + "/backup";
            string msg = "info=" + SimpleJson.SerializeObject(user);
            base.POST(req_url, msg);
        }

        public void GetBackups(string userid,AsyncCallback Callback)
        {
            string req_url = HEROKU_BASEURL + "/user/backups/" + userid;
            GetBackupCallBack = Callback;
            GET(req_url);
        }

        public void DeleteBackup(int id, string url)
        {
            /*JsonObject jobj = new JsonObject();
            jobj.Add("id",id);
            jobj.Add("url",url+"?key="+Filepicker_io.FILEPICKER_APIKEY);
            string msg = "info=" + SimpleJson.SerializeObject(jobj);*/
            string req_url = HEROKU_BASEURL + "/backup/"+id;
            DELETE(req_url,"");
        }

        public override void DELETE_Method_CallBack(IAsyncResult res)
        {
            try
            {
                HttpWebRequest wr = (HttpWebRequest)res.AsyncState;
                HttpWebResponse wrsp = (HttpWebResponse)wr.EndGetResponse(res);
                StreamReader strm = new StreamReader(wrsp.GetResponseStream());
                string response = strm.ReadToEnd();
                JsonObject jobj = (JsonObject)SimpleJson.DeserializeObject(response, typeof(JsonObject));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}

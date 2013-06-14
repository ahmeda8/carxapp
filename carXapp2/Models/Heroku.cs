using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.Net;
using System.IO;
using SimpleJson;

namespace carXapp2
{
    class Heroku : WebMethod
    {
        private const string HEROKU_BASEURL = "http://driverdashserver.herokuapp.com";

        public override void GET_Method_CallBack(IAsyncResult res)
        {
            throw new NotImplementedException();
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
            if (jobj != null)
            {
                string email, userid;
                //JsonArray jarr = (JsonArray)jobj["rows"];
                //jobj = (JsonObject)jarr[0];
                userid = jobj["id"].ToString();
                IsolatedStorageSettings.ApplicationSettings["userid"] = userid;
                IsolatedStorageSettings.ApplicationSettings.Save();
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("email",out email);
                UpdateUser(email, userid);
            }
        }

        public void AddUser(string fbid, string email, string firstName, string lastName)
        {
            string created = DateTime.Now.ToShortDateString();
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
            user.Add("logintime", login.ToShortDateString());
            string req_url = HEROKU_BASEURL + "/user";
            string msg = "user=" + SimpleJson.SerializeObject(user);
            base.PUT(req_url, msg);
        }
    }
}

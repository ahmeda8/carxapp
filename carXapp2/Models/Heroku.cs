using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using ServiceStack.Text;
using System.Net;
using System.IO;

namespace carXapp2
{
    class Heroku : WebMethod
    {
        private const string HEROKU_BASEURL = "http://driverdashserver.herokuapp.com";
        public override void GET_Method_CallBack(IAsyncResult res)
        {
            throw new NotImplementedException();
        }

        public override void POST_Method_CallBack(IAsyncResult res)
        {
            HttpWebRequest wr = (HttpWebRequest)res.AsyncState;
            HttpWebResponse wrsp =(HttpWebResponse)wr.EndGetResponse(res) ;
            StreamReader strm = new StreamReader(wrsp.GetResponseStream());
            string response = strm.ReadToEnd();
            JsonArrayObjects jobj = JsonArrayObjects.Parse(response);
            if (jobj != null)
            {
                JsonObject jo = jobj[0];
                JsonArrayObjects rows = jo["rows"];
                IsolatedStorageSettings.ApplicationSettings["userid"] = rows[0]["id"];
            }
        }

        public void AddUser(string fbid, string email, string firstName, string lastName)
        {
            string created = DateTime.Now.ToShortestXsdDateTimeString();
            string lastLogin = created;
            JsonObject user = new JsonObject();
            user.Add("fbid", fbid);
            user.Add("email", email);
            user.Add("created", created);
            user.Add("last_login", lastLogin);
            user.Add("firstname", firstName);
            user.Add("lastname", lastName);
            string req_url = HEROKU_BASEURL + "/user";
            string msg = "user=" + user.ToJson();
            base.POST(req_url, msg);
        }

        public void GetUser(string fbid)
        {

        }

    }
}

using System;
using System.Net;
using System.Text;
using System.IO;

namespace carXapp2
{   
    public abstract class WebMethod:IDisposable
    {
        public string Response { get; set; }
        public abstract void GET_Method_CallBack(IAsyncResult res);
        public abstract void POST_Method_CallBack(IAsyncResult res);
        public abstract void PUT_Method_CallBack(IAsyncResult res);
        private HttpWebRequest POST_REQUEST;
        private HttpWebRequest PUT_REQUEST;
        private HttpWebRequest GET_REQUEST;

        protected void POST(string URL, string post_message)
        {
            POST_REQUEST = (HttpWebRequest)HttpWebRequest.Create(URL);
            POST_REQUEST.Method = "POST";
            POST_REQUEST.ContentType = "application/x-www-form-urlencoded";
            POST_REQUEST.BeginGetRequestStream(result =>
            {
                Stream post_stream = POST_REQUEST.EndGetRequestStream(result);
                byte[] ba = Encoding.UTF8.GetBytes(post_message);
                post_stream.Write(ba, 0, ba.Length);
                post_stream.Close();
                POST_REQUEST.BeginGetResponse(new AsyncCallback(POST_Method_CallBack),POST_REQUEST);
            }, POST_REQUEST);
        }

        protected void PUT(string URL, string put_message_body)
        {
            PUT_REQUEST = (HttpWebRequest)HttpWebRequest.Create(URL);
            PUT_REQUEST.Method = "PUT";
            PUT_REQUEST.ContentType = "application/x-www-form-urlencoded";
            PUT_REQUEST.BeginGetRequestStream(result =>
            {
                Stream post_stream = PUT_REQUEST.EndGetRequestStream(result);
                byte[] ba = Encoding.UTF8.GetBytes(put_message_body);
                post_stream.Write(ba, 0, ba.Length);
                post_stream.Close();
                PUT_REQUEST.BeginGetResponse(new AsyncCallback(PUT_Method_CallBack), PUT_REQUEST);
            }, PUT_REQUEST);
        }

        protected void GET(string URL)
        {
            GET_REQUEST = (HttpWebRequest)HttpWebRequest.CreateHttp(URL);
            GET_REQUEST.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:14.0) Gecko/20100101 Firefox/14.0.1";
            GET_REQUEST.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            GET_REQUEST.Headers[HttpRequestHeader.AcceptLanguage] = "en-us,en;q=0.5";
            GET_REQUEST.Headers[HttpRequestHeader.Connection] = "keep-alive";
            GET_REQUEST.Headers["Accept-Location"] = "*";
            GET_REQUEST.Headers[HttpRequestHeader.Referer] = "http://www.bing.com/";
            if (this.GetType().ToString() == "Resources.youtube_org")
            {
               GET_REQUEST.Headers[HttpRequestHeader.Host] = "www.youtube-mp3.org";
               GET_REQUEST.Headers[HttpRequestHeader.Referer] = "http://www.youtube-mp3.org/";
            }
            GET_REQUEST.BeginGetResponse(new AsyncCallback(GET_Method_CallBack), GET_REQUEST);
        }


        public void Dispose()
        {
            if(POST_REQUEST != null)
                POST_REQUEST.Abort();
            if(GET_REQUEST !=null)
                GET_REQUEST.Abort();
            if (PUT_REQUEST != null)
                PUT_REQUEST.Abort();
        }
    }
}

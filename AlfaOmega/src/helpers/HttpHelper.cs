using System.IO;
using System.Net;
using StringBuilder = Java.Lang.StringBuilder;

namespace AlfaOmega.helpers
{
    internal class HttpHelper
    {
        public static string Get(string url)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Proxy = null;
            request.UseDefaultCredentials = true;
            request.Accept = "application/json";
            //request.Method = "GET";
            //request.Headers.Add(HttpRequestHeader.Accept, "json");

            //var ua = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";
            //request.Headers.Add(HttpRequestHeader.UserAgent, ua);

            var sb = new StringBuilder();
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                sb.Append(sr.ReadToEnd());
            }

            var res = sb.ToString();

            return res;
        }
    }
}
using System.IO;
using System.Net;
using StringBuilder = Java.Lang.StringBuilder;

namespace AlfaOmega.helpers
{
    internal class HttpHelper
    {
        public static string Get(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/json";

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
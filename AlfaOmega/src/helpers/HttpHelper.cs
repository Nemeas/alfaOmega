using System.IO;
using System.Net;
using AlfaOmega.enums;
using StringBuilder = Java.Lang.StringBuilder;

namespace AlfaOmega.helpers
{
    internal class HttpHelper
    {
        public static string Get(string url, Accept accept = Accept.Default)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            SetAcceptHeader(accept, request);

            request.Headers.Add("X-Client", "AlfaOmega");
            request.Headers.Add("X-Kontaktperson", "tordenskjold_89@hotmail.com");

            var sb = new StringBuilder();
            var response = (HttpWebResponse) request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                sb.Append(sr.ReadToEnd());
            }

            var res = sb.ToString();

            return res;
        }

        private static void SetAcceptHeader(Accept accept, HttpWebRequest request)
        {
            switch (accept)
            {
                case Accept.V1:
                    request.Accept = "application/vnd.vegvesen.nvdb-v1+json";
                    break;
                case Accept.V2:
                    request.Accept = "application/vnd.vegvesen.nvdb-v2+json";
                    break;
                case Accept.Default:
                    request.Accept = "application/json";
                    break;
            }
        }
    }
}
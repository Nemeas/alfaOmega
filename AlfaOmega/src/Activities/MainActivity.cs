using System.Collections.Generic;
using System.IO;
using System.Net;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Org.Json;

namespace AlfaOmega.Activities
{
    [Activity(Label = "AlfaOmega", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        private TextView _tvSl;
        private TextView _lon;
        private TextView _lat;
        private LocationManager _lm;
        private const int Secs = 3;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _tvSl = FindViewById<TextView>(Resource.Id.speed_limit);
            _lon = FindViewById<TextView>(Resource.Id.lon);
            _lat = FindViewById<TextView>(Resource.Id.lat);
            _lm = (LocationManager) GetSystemService(LocationService);
            _lm.RequestLocationUpdates(LocationManager.GpsProvider, Secs * 1000, 1, this); 

        }

        private string Get(string url)
        {
            var request = WebRequest.Create(url);
            //request.Headers.Set(HttpRequestHeader.Accept, "application/vnd.vegvesen.nvdb-v1+json");

            //request.Headers.Set(HttpRequestHeader.Accept, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");

            var sb = new StringBuilder();
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                sb.Append(sr.ReadToEnd());
            }
            return sb.ToString();
        }

        public void OnLocationChanged(Location location)
        {
            var lat = location.Latitude;
            var lon = location.Longitude;

            _lat.Text = "lat: " + lat;
            _lon.Text = "lon: " + lon;

            // get the vegReference based on the lat & lon
            var url = "https://www.vegvesen.no/nvdb/api/vegreferanse/koordinat?lon=" + lon.ToString("") + "&lat=" + lat.ToString("") + "&geometri=WGS84";
            var json = new JSONObject(Get(url));

            Log.Debug("url", url);
            Log.Debug("json", json.ToString());

            if (json.GetString("kommuneNr") == "0") return;

            var kommuneNr = json.GetString("kommuneNr");

            var vegReferanse = kommuneNr + json.GetString("visningsNavn");

            // get the object id 105 (speed limit) on set vegReference
            // API dok: https://www.vegvesen.no/nvdb/apidokumentasjon/#/get/vegobjekter
            var url2 = "https://www.vegvesen.no/nvdb/api/v2/vegobjekter/105?vegreferanse=" + vegReferanse.Replace(" ", "") + "&inkluder=lokasjon&segmentering=false";

            Log.Debug("url2", url2);

            var json2 = new JSONObject(Get(url2));

            Log.Debug("json2", json2.ToString());

            // get the speed limit with the heighest id (this should be the latest one (FYI: speedlimits have changed over the years, all speed limits are in the database for historical reasons(?)))

            var objs = json2.GetJSONArray("vegObjekter");

            var list = new List<JSONObject>();


            // getting a list of possible objects based on the kommuneNr (because for some f*ced up reason the result returns objects in other kommunes aswell...)
            for (int i = 0; i < objs.Length() - 1; i++)
            {
                if (objs.GetJSONObject(i).GetJSONObject("lokasjon").GetJSONArray("kommuner").GetString(0) == kommuneNr)
                    list.Add(objs.GetJSONObject(i));
            }

            if (list.Count == 0) return;

            var url3 = list[list.Count - 1].GetString("href");

            Log.Debug("url3", url3);

            var json3 = new JSONObject(Get(url3));

            Log.Debug("json3", json3.ToString());

            // set the speed-limit to the textview.

            var res = json3.GetJSONArray("egenskaper").GetJSONObject(0).GetString("verdi");
            _tvSl.Text = res + " km/t";

        }

        public void OnProviderDisabled(string provider)
        {
            Toast.MakeText(this, "No provider..", ToastLength.Long).Show();
        }

        public void OnProviderEnabled(string provider)
        {
            Toast.MakeText(this, "Good job!", ToastLength.Long).Show();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Toast.MakeText(this, "Status has changed..", ToastLength.Short).Show();
        }
    }
}


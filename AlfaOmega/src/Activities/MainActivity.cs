using System;
using System.Collections.Generic;
using AlfaOmega.enums;
using AlfaOmega.helpers;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;
using Org.Json;
using Void = Java.Lang.Void;

namespace AlfaOmega.Activities
{
    [Activity(Label = "AlfaOmega", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        private static TextView _tvSl;
        private TextView _lon;
        private TextView _lat;
        private LocationManager _lm;
        private static ImageView _iv;
        private const int Secs = 3;

        private static string _kommune;

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
            _iv = (ImageView)FindViewById(Resource.Id.speedLimit);

            _lm = (LocationManager) GetSystemService(LocationService);
            _lm.RequestLocationUpdates(LocationManager.GpsProvider, Secs * 1000, 1, this); 

        }

        public void OnLocationChanged(Location location)
        {
            var lat = location.Latitude;
            var lon = location.Longitude;

            _lat.Text = "lat: " + lat;
            _lon.Text = "lon: " + lon;

            // get the vegReference based on the lat & lon
            var url = "https://www.vegvesen.no/nvdb/api/vegreferanse/koordinat?lon=" + lon.ToString("") + "&lat=" + lat.ToString("") + "&geometri=WGS84";

            try
            {
                new AsyncTask1().Execute(url); // starting the show.
            }
            catch (Exception e)
            {
                Log.Debug("ERROR!", e.Message);
            }
        }

        private class AsyncTask1 : AsyncTask<string, Void, string>
        {
            protected override string RunInBackground(params string[] @params)
            {
                try
                {
                    Log.Debug("url1", @params[0]);
                    return HttpHelper.Get(@params[0], Accept.V1);
                }
                catch (Exception e)
                {
                    Log.Debug("ERROR1", e.Message);
                    return null;
                }
            }

            protected override void OnPostExecute(string result)
            {

                if (result == null) return; // TODO

                var json = new JSONObject(result);
                
                Log.Debug("res1", json.ToString());

                var kommuneNr = json.GetString("kommuneNr");

                if (kommuneNr == "0") return;

                _kommune = kommuneNr;

                var visningsNavn = json.GetString("visningsNavn");

                var vegReferanse = kommuneNr + visningsNavn;

                // get the object id 105 (speed limit) on set vegReference
                // API dok: https://www.vegvesen.no/nvdb/apidokumentasjon/#/get/vegobjekter

                var url2 = "https://www.vegvesen.no/nvdb/api/v2/vegobjekter/105?vegreferanse=" + vegReferanse.Replace(" ", "") + "&inkluder=lokasjon&segmentering=false";

                new AsyncTask2().Execute(url2); // continuing the show =D
            }
        }

        

        private class AsyncTask2 : AsyncTask<string, Void, string>
        {
            protected override string RunInBackground(params string[] @params)
            {
                try
                {
                    Log.Debug("url2", @params[0]);
                    return HttpHelper.Get(@params[0], Accept.V2);

                }
                catch (Exception e)
                {
                    Log.Debug("ERROR2", e.Message);
                    return null;
                }
            }

            protected override void OnPostExecute(string result)
            {
                if (result == null) return; // TODO

                var json2 = new JSONObject(result);

                var kommuneNr = _kommune; // TODO make better..

                Log.Debug("res2", json2.ToString());

                // get the speed limit with the heighest id (this should be the latest one (FYI: speedlimits have changed over the years, all speed limits are in the database for historical reasons(?)))

                var objs = json2.GetJSONArray("objekter");

                var list = new List<JSONObject>();

                // getting a list of possible objects based on the kommuneNr (because for some f*ced up reason the result returns objects in other kommunes aswell...)
                for (int i = 0; i < objs.Length() - 1; i++)
                {
                    if (objs.GetJSONObject(i).GetJSONObject("lokasjon").GetJSONArray("kommuner").GetString(0) == kommuneNr)
                        list.Add(objs.GetJSONObject(i));
                }

                if (list.Count == 0) return;

                var url3 = list[list.Count - 1].GetString("href");

                new AsyncTask3().Execute(url3);
            }
        }

        private class AsyncTask3 : AsyncTask<string, Void, string>
        {
            protected override string RunInBackground(params string[] @params)
            {
                try
                {
                    Log.Debug("url3", @params[0]);
                    return HttpHelper.Get(@params[0]);
                }
                catch (Exception e)
                {
                    Log.Debug("Error3", e.Message);
                    return null;
                }
            }

            protected override void OnPostExecute(string result)
            {
                if (result == null) return; // TODO

                var json3 = new JSONObject(result);

                Log.Debug("json3", json3.ToString());

                // set the speed-limit to the textview.

                var res = json3.GetJSONArray("egenskaper").GetJSONObject(0).GetString("verdi");
                _tvSl.Text = res + " km/t";

                Log.Debug("res3", res);

                SetImage(res);
            }
        }

        public static void SetImage(string limit)
        {
            switch (limit)
            {
                case "30":
                    _iv.SetImageResource(Resource.Drawable.f30);
                    break;
                case "40":
                    _iv.SetImageResource(Resource.Drawable.f40);
                    break;
                case "50":
                    _iv.SetImageResource(Resource.Drawable.f50);
                    break;
                case "60":
                    _iv.SetImageResource(Resource.Drawable.f60);
                    break;
                case "70":
                    _iv.SetImageResource(Resource.Drawable.f70);
                    break;
                case "80":
                    _iv.SetImageResource(Resource.Drawable.f80);
                    break;
                case "90":
                    _iv.SetImageResource(Resource.Drawable.f90);
                    break;
                case "100":
                    _iv.SetImageResource(Resource.Drawable.f100);
                    break;
                case "110":
                    _iv.SetImageResource(Resource.Drawable.f110);
                    break;
            }
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
            //Toast.MakeText(this, "Status has changed..", ToastLength.Short).Show();
        }
    }
}


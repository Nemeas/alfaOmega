using System;
using Org.Json;
using Math = System.Math;

namespace AlfaOmega.models
{
    class RoadObject
    {
        public JSONObject SpeedLimitObject { get; set; }

        public bool IsCoordinateInside(double lat, double lon)
        {

            string lineString = SpeedLimitObject.GetJSONObject("geometri").GetString("wkt");

            string[] lineStringArray = lineString.Split(Convert.ToChar(","));

            lineStringArray[0] = lineStringArray[0].Substring(12);
            lineStringArray[lineStringArray.Length - 1] = lineStringArray[lineStringArray.Length - 1].Substring(0,
                lineStringArray[lineStringArray.Length - 1].Length - 2);

            LatLon current = null;

            for (int i = 0; i < lineStringArray.Length - 1; i++)
            {
                var coordinateArray = lineStringArray[i].Trim().Split(Convert.ToChar(" "));

                var newLat = coordinateArray[0].Trim();
                var newLon = coordinateArray[1].Trim();

                LatLon newCurrent = new LatLon(Convert.ToDouble(newLat), Convert.ToDouble(newLon));

                if(current != null)
                    if (PointIsOnLine(lat, lon, current, newCurrent))
                        return true;

                current = newCurrent;
            }

            return false;
        }

        private const double Tolerance = 0.1; // TODO

        private bool PointIsOnLine(double lat, double lon, LatLon current, LatLon newCurrent)
        {

            var dxc = lat - current.Latitude;
            var dyc = lon - current.Longitude;

            var dxl = newCurrent.Latitude - current.Latitude;
            var dyl = newCurrent.Longitude - current.Longitude;

            var cross = dxc * dyl - dyc * dxl;

            if (Math.Abs(cross) > Tolerance)
                return false;

            if (Math.Abs(dxl) >= Math.Abs(dyl))
                return dxl > 0 ?
                  current.Latitude <= lat && lat <= newCurrent.Latitude :
                  newCurrent.Latitude <= lat && lat <= current.Latitude;
            return dyl > 0 ?
                current.Longitude <= lon && lon <= newCurrent.Longitude :
                newCurrent.Longitude <= lon && lon <= current.Longitude;
        }


        internal class LatLon
        {
            public LatLon(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

            public double Latitude { get; set; }
            public double Longitude { get; set; }

        }

        public string SpeedLimit()
        {
            return SpeedLimitObject.GetJSONArray("egenskaper").GetJSONObject(0).GetString("verdi");
        }

        public string RoadReference()
        {
            return SpeedLimitObject.GetJSONObject("lokasjon").GetJSONArray("vegreferanser").GetJSONObject(0).GetString("kortform").Replace(" ", "");
        }

        public bool HasObject()
        {
            return SpeedLimitObject != null;
        }
    }
}
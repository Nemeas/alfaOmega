using AlfaOmega.helpers;
using Org.Json;

namespace AlfaOmega.models
{
    class RoadObject
    {

        private const double Tolerance = 0.1; // TODO

        public JSONObject SpeedLimitObject { get; set; }

        public bool IsCoordinateInside(double lat, double lon)
        {
            string lineString = SpeedLimitObject.GetJSONObject("geometri").GetString("wkt");

            return Geometry.IsCoordinateInsideGeometry(lineString, lat, lon, Tolerance);
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
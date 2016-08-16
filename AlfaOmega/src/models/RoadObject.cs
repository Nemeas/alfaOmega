using System;
using Org.Json;

namespace AlfaOmega.models
{
    class RoadObject
    {
        public JSONObject SpeedLimitObject { get; set; }

        public bool IsCoordinateInside(double lat, double lon)
        {
            // TODO - implement
            return true;
        }

        public string SpeedLimit()
        {
            // TODO - implement
            return "30";
        }

        public string RoadReference()
        {
            // TODO - implement
            return "1601EV1337HP1m0-m666";
        }

        public bool HasObject()
        {
            return SpeedLimitObject != null;
        }
    }
}
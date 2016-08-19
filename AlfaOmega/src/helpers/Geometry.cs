using System;
using AlfaOmega.models;

namespace AlfaOmega.helpers
{
    class Geometry
    {
        private static bool PointIsOnLine(double lat, double lon, LatLon current, LatLon newCurrent, double tolerance)
        {

            var dxc = lat - current.Latitude;
            var dyc = lon - current.Longitude;

            var dxl = newCurrent.Latitude - current.Latitude;
            var dyl = newCurrent.Longitude - current.Longitude;

            var cross = dxc * dyl - dyc * dxl;

            if (Math.Abs(cross) > tolerance)
                return false;

            if (Math.Abs(dxl) >= Math.Abs(dyl))
                return dxl > 0 ?
                  current.Latitude <= lat && lat <= newCurrent.Latitude :
                  newCurrent.Latitude <= lat && lat <= current.Latitude;
            return dyl > 0 ?
                current.Longitude <= lon && lon <= newCurrent.Longitude :
                newCurrent.Longitude <= lon && lon <= current.Longitude;
        }

        public static bool IsCoordinateInsideGeometry(string lineString, double lat, double lon, double tolerance)
        {

            LatLon[] latLons = CoordinateExtractor.Get(lineString);

            LatLon current = null;

            for (int i = 0; i < latLons.Length - 1; i++)
            {

                LatLon newCurrent = latLons[i];

                if (current != null)
                    if (PointIsOnLine(lat, lon, current, newCurrent, tolerance))
                        return true;

                current = newCurrent;
            }

            return false;
        }
    }
}
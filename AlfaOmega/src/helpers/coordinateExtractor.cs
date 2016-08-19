using System;
using AlfaOmega.models;

namespace AlfaOmega.helpers
{
    class CoordinateExtractor
    {
        /// <summary>
        /// Refines the input to a list of LatLon-objects
        /// </summary>
        /// <param name="wkt">string in the form of: LINESTRING (64.123 10.24 105, 64.125 10.15 150)</param>
        /// <returns></returns>
        public static LatLon[] Get(string wkt)
        {
            
            string[] lineStringArray = wkt.Split(Convert.ToChar(","));

            lineStringArray[0] = lineStringArray[0].Substring(12);
            lineStringArray[lineStringArray.Length - 1] = lineStringArray[lineStringArray.Length - 1].Substring(0,
                lineStringArray[lineStringArray.Length - 1].Length - 2);

            LatLon[] res = new LatLon[lineStringArray.Length];

            for (int i = 0; i < lineStringArray.Length - 1; i++)
            {

                var coordinateArray = lineStringArray[i].Trim().Split(Convert.ToChar(" "));

                var newLat = coordinateArray[0].Trim();
                var newLon = coordinateArray[1].Trim();

                res[i] = new LatLon(Convert.ToDouble(newLat), Convert.ToDouble(newLon));
            }

            return res;
        }
    }
}
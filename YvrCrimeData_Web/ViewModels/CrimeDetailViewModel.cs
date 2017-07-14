using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YvrCrimeData_Web.Models;
using YvrCrimeData_Web.Utilities;

namespace YvrCrimeData_Web.ViewModels
{
    public class CrimeDetailViewModel : ViewModelBase
    {
        private const int UTMZone = 10;
        private const string UTMLatZone = "U";

        public Crime Crime { get; set; }

        public double Latitude
        {
            get
            {
                // Calculate Latitude based on XCoordinates and YCoordinates (UTM)
                var converter = new CoordinateConversion();
                string utm = UTMZone + " " + UTMLatZone + " " + Crime.XCoordinate.ToString() + " " + Crime.YCoordinate.ToString();
                return converter.UTM2LatLon(utm)[0];
            }
        }

        public double Longitude
        {
            get
            {
                // Calculate Longitude based on XCoordinates and YCoordinates (UTM)
                var converter = new CoordinateConversion();
                string utm = UTMZone + " " + UTMLatZone + " " + Crime.XCoordinate.ToString() + " " + Crime.YCoordinate.ToString();
                return converter.UTM2LatLon(utm)[1];
            }
        }

        public CrimeDetailViewModel() : base()
        {
        }
    }
}
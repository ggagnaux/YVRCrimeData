using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.Models
{
    public class CrimeListViewModel
    {
        // Make hidden
        public int ID { get; set; }

        [Display(Name = "Offence Date/Time")]
        public DateTime OffenceDate { get; set; }

        // Make Hidden
        public int? CrimeTypeID { get; set; }

        [Display(Name = "Type of Crime")]
        public string CrimeTypeName { get; set; }

        [Display(Name = "Hundred Block")]
        public string HundredBlock { get; set; }

        // Make Hidden
        public int? NeighbourhoodID { get; set; }

        [Display(Name = "Neighbourhood")]
        public string NeighbourhoodName { get; set; }

        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.Models
{
    public class OffenceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

    public class NeighbourhoodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    
    public class CrimeListViewModel
    {
        public CrimeListViewModel()
        {
        }

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

        // Used to build checkbox group for Crime Types (Offences)
        public static IList<OffenceViewModel> Offences { get; set; }
        public static string SelectedOffenceIdString()
        {
            var arr = Offences.Where(x => x.Checked == true)
                           .Select(x => x.Id.ToString()).ToArray();
            return string.Join(",", arr);
        }
        public static int[] SelectedOffenceIdArray()
        {
            var arr = Offences.Where(x => x.Checked == true)
                           .Select(x => x.Id).ToArray();
            return arr;
        }

        // Used to build checkbox group for Neighbourhoods
        public static IList<NeighbourhoodViewModel> Neighbourhoods { get; set; }
        public static string SelectedNeighbourhoodIdString()
        {
            var arr = Neighbourhoods.Where(x => x.Checked == true)
                                 .Select(x => x.Id.ToString()).ToArray();
            return string.Join(",", arr);
        }
        public static int[] SelectedNeighbourhoodIdArray()
        {
            var arr = Neighbourhoods.Where(x => x.Checked == true)
                                 .Select(x => x.Id).ToArray();
            return arr;
        }
    }
}
namespace YvrCrimeData_Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    [Table("Crime")]
    public partial class Crime
    {
        public int ID { get; set; }

        public int? CrimeTypeID { get; set; }

        [Display(Name = "Offence Date/Time")]
        public DateTime OffenceDate { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public int? Hour { get; set; }

        public int? Minute { get; set; }

        [Display(Name="Hundred Block")]
        [StringLength(80)]
        public string HundredBlock { get; set; }

        public int? NeighbourhoodID { get; set; }

        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }

        public virtual CrimeType CrimeType { get; set; }

        public virtual Neighbourhood Neighbourhood { get; set; }
    }
}

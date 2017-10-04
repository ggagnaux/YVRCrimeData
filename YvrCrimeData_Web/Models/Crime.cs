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
        public virtual int ID { get; set; }

        public virtual int? CrimeTypeID { get; set; }

        [Display(Name = "Offence Date/Time")]
        public virtual DateTime OffenceDate { get; set; }

        public virtual int? Year { get; set; }

        public virtual int? Month { get; set; }

        public virtual int? Day { get; set; }

        public virtual int? Hour { get; set; }

        public virtual int? Minute { get; set; }

        [Display(Name="Hundred Block")]
        [StringLength(80)]
        public virtual string HundredBlock { get; set; }

        public virtual int? NeighbourhoodID { get; set; }

        public virtual decimal? XCoordinate { get; set; }

        public virtual decimal? YCoordinate { get; set; }

        public virtual CrimeType CrimeType { get; set; }

        public virtual Neighbourhood Neighbourhood { get; set; }
    }
}

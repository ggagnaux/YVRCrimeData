namespace YvrCrimeData_Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class YvrCrimeDataContext : DbContext
    {
        public YvrCrimeDataContext()
            : base("name=YvrCrimeDataContext")
        {
        }

        public virtual DbSet<Crime> Crimes { get; set; }
        public virtual DbSet<CrimeType> CrimeTypes { get; set; }
        public virtual DbSet<Neighbourhood> Neighbourhoods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

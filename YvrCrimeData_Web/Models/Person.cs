using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.Models
{
    [MetadataType(typeof(PersonMetadata))]
    public class Person
    {
        public int PersonID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
    }

    public class PersonMetadata
    {
        [Required()]
        public int PersonID { get; set; }

        [Required()]
        public string Firstname { private get; set; }

        [Required()]
        public string Lastname { private get; set; }

        public string Name
        {
            get
            {
                return $"{this.Firstname} {this.Lastname}";
            }
        }

        [Required()]
        public int Age { get; set; }
    }
}
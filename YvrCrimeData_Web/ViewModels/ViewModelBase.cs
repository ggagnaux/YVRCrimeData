using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.ViewModels
{
    public class ViewModelBase
    {
        public string PageHeading { get; set; }

        // Dictionary to hold other labels used in page.
        public IDictionary<string, string> LabelDictionary { get; set; }

        public ViewModelBase()
        {
            PageHeading = string.Empty;
            LabelDictionary = new Dictionary<string, string>();
        }
    }
}
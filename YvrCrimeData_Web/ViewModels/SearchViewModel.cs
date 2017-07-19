using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PagedList;
using YvrCrimeData_Web.Models;

namespace YvrCrimeData_Web.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        public enum SortByEnum 
        {
            Date = 0,
            Offence,
            Neighbourhood
        }

        public enum SortOrderEnum
        {
            Ascending = 0,
            Descending
        }

        private DateTime DefaultStartDate = new DateTime(2003, 1, 1, 0, 0, 0);
        private DateTime DefaultEndDate = new DateTime(2003, 1, 31, 0, 0, 0);

        public bool FormVisibility { get; set; }


        [Display(Name = "Start Phrase")]
        public string SearchPhrase { get; set; }


        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public SortOrderEnum   SortOrder { get; set; }
        public SortByEnum      SortBy { get; set; }

        public IPagedList<Crime> Items { get; set; }

        public IList<OffenceViewModel> Offences { get; set; }

        public int SelectedOffenceCount
        {
            get
            {
                return SelectedOffences.Count;
            }
        }

        public IList<int> SelectedOffences
        {
            get
            {
                return Offences.Where(x => x.Selected == true).Select(x => x.Id).ToList();
            }
        }

        public string SelectedOffenceIdString()
        {
            var arr = Offences.Where(x => x.Selected == true)
                           .Select(x => x.Id.ToString()).ToArray();
            return string.Join(",", arr);
        }

        public int[] SelectedOffenceIdArray()
        {
            var arr = Offences.Where(x => x.Selected == true)
                           .Select(x => x.Id).ToArray();
            return arr;
        }

        public IList<NeighbourhoodViewModel> Neighbourhoods { get; set; }

        public int SelectedNeighbourhoodCount
        {
            get
            {
                return SelectedNeighbourhoods.Count;
            }
        }

        public IList<int> SelectedNeighbourhoods
        {
            get
            {
                return Neighbourhoods.Where(x => x.Selected == true)
                                     .Select(x => x.Id).ToList();
            }
        }

        public string SelectedNeighbourhoodIdString()
        {
            var arr = Neighbourhoods.Where(x => x.Selected == true)
                                    .Select(x => x.Id.ToString()).ToArray();
            return string.Join(",", arr);
        }

        public int[] SelectedNeighbourhoodIdArray()
        {
            var arr = Neighbourhoods.Where(x => x.Selected == true)
                                 .Select(x => x.Id).ToArray();
            return arr;
        }


        public SearchViewModel() : base()
        {
            this.FormVisibility = true;
            this.Offences = new List<OffenceViewModel>();
            this.Neighbourhoods = new List<NeighbourhoodViewModel>();
            this.StartDate = DefaultStartDate;
            this.EndDate = DefaultEndDate;
            this.SortBy = SortByEnum.Date;
            this.SortOrder = SortOrderEnum.Ascending;
        }
    }

    public class OffenceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class NeighbourhoodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
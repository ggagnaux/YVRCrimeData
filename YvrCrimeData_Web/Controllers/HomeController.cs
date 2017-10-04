using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using YvrCrimeData_Web.DAL.Repositories;
using YvrCrimeData_Web.Models;
using YvrCrimeData_Web.Services;
using YvrCrimeData_Web.ViewModels;

namespace YvrCrimeData_Web.Controllers
{
    public class HomeController : Controller
    {
        private CrimeServices _crimeServices = null;

        private CrimeRepository _repository = null;
        private CrimeTypeRepository _crimeTypeRepository = null;
        private NeighbourhoodRepository _neighbourhoodRepository = null;

        private const int FirstYear = 2013;
        private const int PageSize = 10;

        public HomeController()
        {
            YvrCrimeDataContext context = new YvrCrimeDataContext();



            this._repository = new CrimeRepository(context);
            this._crimeTypeRepository = new CrimeTypeRepository(context);
            this._neighbourhoodRepository = new NeighbourhoodRepository(context);
        }

        public HomeController(CrimeRepository repo, 
                              CrimeTypeRepository crimeTypeRepo, 
                              NeighbourhoodRepository neighbourhoodRepo)
        {
            this._repository = repo;
            this._crimeTypeRepository = crimeTypeRepo;
            this._neighbourhoodRepository = neighbourhoodRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(int? page, SearchViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    viewModel = new SearchViewModel();
                }

                ViewBag.Title = "YVR Crime Data";

                viewModel.PageHeading = "Search Crimes";

                // Get the Date Range
                DateTime dateStart, dateEnd;
                dateStart = viewModel.StartDate;
                dateEnd = viewModel.EndDate;

                // Run the query
                var crimes = _repository.GetAll();

                // Filter by OffenceDate.Start
                if (viewModel.StartDate != null)
                {
                    crimes = crimes.Where(c => c.OffenceDate >= viewModel.StartDate);
                }

                // Filter by OffenceDate.End
                if (viewModel.EndDate != null)
                {
                    crimes = crimes.Where(c => c.OffenceDate <= viewModel.EndDate);
                }

                // Filter by CrimeType
                if (viewModel.SelectedOffenceCount > 0)
                {
                    // Now filter the crime list
                    crimes = crimes.Where(c => viewModel.SelectedOffences.Contains(c.CrimeTypeID.Value));
                }

                // Filter by Neighbourhood
                if (viewModel.SelectedNeighbourhoodCount > 0)
                {
                    // Now filter the crime list
                    crimes = crimes.Where(c => viewModel.SelectedNeighbourhoods.Contains(c.NeighbourhoodID.Value));
                }

                // Set sort order
                crimes = SetSortOrder(crimes, viewModel.SortBy, viewModel.SortOrder);

                //
                // Add the Offence and Neighbourhood lists (with selections) to ViewModel
                //
                viewModel.Neighbourhoods = _neighbourhoodRepository.GetAll().Select(x => new NeighbourhoodViewModel()
                {
                    Id = x.ID,
                    Name = x.Name,
                    Selected = viewModel.SelectedNeighbourhoods.Contains(x.ID)
                }).ToList();

                viewModel.Offences = _crimeTypeRepository.GetAll().Select(x => new OffenceViewModel()
                {
                    Id = x.ID,
                    Name = x.Type,
                    Selected = viewModel.SelectedOffences.Contains(x.ID)
                }).ToList();


                int totalRecordCount = crimes.Count();

                int pageNumber = (page ?? 1);
                int skip = 0;
                if (totalRecordCount <= PageSize)
                {
                    skip = 0;
                }
                else
                {
                    skip = pageNumber * PageSize;
                }


                int take = PageSize;

                if (totalRecordCount < skip)
                {
                    skip = 0;
                }
                if (totalRecordCount < take)
                {
                    take = totalRecordCount;
                }

                //
                // Get the actual data
                //
                var subsetOfAllItems = crimes.Skip(skip).Take(take).ToList();
                var staticPagedList = new StaticPagedList<Crime>(
                                                        subsetOfAllItems, pageNumber, PageSize, totalRecordCount);

                viewModel.Items = staticPagedList;

                //
                // Set some other view content
                //
                viewModel.LabelDictionary.Add("OffenceTypes", "Offence Types");
                viewModel.LabelDictionary.Add("Neighbourhoods", "Neighbourhoods");
                viewModel.LabelDictionary.Add("DateRange", "Date Range");
                viewModel.LabelDictionary.Add("OutputSectionTitle", "Search Results");
                viewModel.LabelDictionary.Add("RecordsFound", MakeRecordsFoundLabel(totalRecordCount));
                viewModel.LabelDictionary.Add("SearchAndFilterOptions", "Search & Filter Options");

                var currentPage = viewModel.Items.PageCount < viewModel.Items.PageNumber ? 0 : viewModel.Items.PageNumber;
                viewModel.LabelDictionary.Add("PageXOfY", $"Page {currentPage} of {viewModel.Items.PageCount}");


                return View("Search", viewModel);
            }
            catch (Exception e)
            {

            }

            return View();
        }
        
        private string MakeRecordsFoundLabel(int recordCount = 0)
        {
            string output = string.Empty;
            if (recordCount == 0)
            {
                output = "0 records found";
            }
            else if (recordCount == 1)
            {
                output = "1 record found";
            }
            else if (recordCount > 1)
            {
                output = $"{recordCount} records found";
            }
            return output;
        }

        public ActionResult Details(int id)
        {
            CrimeDetailViewModel viewModel = new CrimeDetailViewModel();
            var crime = _repository.GetByID(id);

            viewModel.PageHeading = "Offence";
            viewModel.LabelDictionary.Add("OutputSectionTitle", "Details");

            viewModel.Crime = crime;

            return View(viewModel);
        }

        private void SetDateRange(ref string startAsString, 
                                  ref string endAsString,
                                  out DateTime dateStart,
                                  out DateTime dateEnd)
        {
            string DateFormat = "MM/dd/yyyy";

            int startYear, startMonth, startDay;
            int endYear, endMonth, endDay;

            if (startAsString?.Length > 0)
            {
                startMonth = Convert.ToInt32(startAsString.Substring(0, 2));
                startDay = Convert.ToInt32(startAsString.Substring(3, 2));
                startYear = Convert.ToInt32(startAsString.Substring(6, 4));
            }
            else
            {
                // Just default to the current year and current month
                startMonth = 1;
                startDay = 1;
                startYear = FirstYear;
                startAsString = new DateTime(startYear, startMonth, startDay).ToString(DateFormat);
            }
            dateStart = new DateTime(startYear, startMonth, startDay);

            if (endAsString?.Length > 0)
            {
                endMonth = Convert.ToInt32(endAsString.Substring(0, 2));
                endDay = Convert.ToInt32(endAsString.Substring(3, 2));
                endYear = Convert.ToInt32(endAsString.Substring(6, 4));
            }
            else
            {
                // Just default to the first year and month
                endYear = FirstYear;
                endMonth = 1;
                endDay = 31;

                endAsString = new DateTime(endYear, endMonth, endDay).ToString(DateFormat);
            }
            dateEnd = new DateTime(endYear, endMonth, endDay);
        }

        private IQueryable<Crime> SetSortOrder(IQueryable<Crime> crimes, 
                                               SearchViewModel.SortByEnum sortBy, 
                                               SearchViewModel.SortOrderEnum sortOrder)
        {
            switch (sortBy)
            {
                case SearchViewModel.SortByEnum.Date:
                    switch (sortOrder)
                    {
                        default:
                        case SearchViewModel.SortOrderEnum.Ascending:
                            crimes = crimes.OrderBy(c => c.OffenceDate);
                            break;
                        case SearchViewModel.SortOrderEnum.Descending:
                            crimes = crimes.OrderByDescending(c => c.OffenceDate);
                            break;
                    }
                    break;
                case SearchViewModel.SortByEnum.Offence:
                    switch (sortOrder)
                    {
                        default:
                        case SearchViewModel.SortOrderEnum.Ascending:
                            crimes = crimes.OrderBy(c => c.CrimeType.Type);
                            break;
                        case SearchViewModel.SortOrderEnum.Descending:
                            crimes = crimes.OrderByDescending(c => c.CrimeType.Type);
                            break;
                    }
                    break;
                case SearchViewModel.SortByEnum.Neighbourhood:
                    switch (sortOrder)
                    {
                        default:
                        case SearchViewModel.SortOrderEnum.Ascending:
                            crimes = crimes.OrderBy(c => c.Neighbourhood.Name);
                            break;
                        case SearchViewModel.SortOrderEnum.Descending:
                            crimes = crimes.OrderByDescending(c => c.Neighbourhood.Name);
                            break;
                    }
                    break;

                default:
                    // Date - Ascending (Oldest to Newest)
                    crimes = crimes.OrderBy(c => c.OffenceDate);
                    break;
            }

            return crimes;
        }

        // Move to library
        private string MakeDateTimeString(int year, 
                                          int month, 
                                          int day, 
                                          int hour, 
                                          int minute,
                                          int second = 0)
        {
            var theDate = new DateTime(year, month, day, hour, minute, second);
            return theDate.ToLongDateString();
        }


        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
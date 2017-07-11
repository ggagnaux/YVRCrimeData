using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YvrCrimeData_Web.Models;
using PagedList;
using YvrCrimeData_Web.DAL.Repositories;
using YvrCrimeData_Web.DAL.Interfaces;

namespace YvrCrimeData_Web.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult CrimeList(string sortOrder, 
                                       string currentFilter, 
                                       string searchString, 
                                       int? page,
                                       string dateStartAsString,
                                       string dateEndAsString)
        {
            try
            {
                ViewBag.Message = "View Crimes";

                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; 
                ViewBag.CrimeTypeSortParm = sortOrder == "crimetype_desc" ? "crimetype" : "crimetype_desc"; 
                ViewBag.NeighbourhoodSortParam = sortOrder == "neighbourhood_desc" ? "neighbourhood" : "neighbourhood_desc";

                string selectedCrimeTypeList = Request.QueryString["CrimeTypeList"];
                string selectedNeighbourhoodList = Request.QueryString["NeighbourhoodList"];

                BuildCrimeTypeList(selectedCrimeTypeList);
                BuildNeighbourhoodList(selectedNeighbourhoodList);


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                // Get the Date Range
                DateTime dateStart, dateEnd;
                SetDateRange(ref dateStartAsString, ref dateEndAsString, out dateStart, out dateEnd);
                ViewBag.DateStart = dateStartAsString;
                ViewBag.DateEnd = dateEndAsString;

                // Run the query
                var crimes = _repository.GetAll();

                // Filter by OffenceDate
                crimes = crimes.Where(c => c.OffenceDate >= dateStart && c.OffenceDate <= dateEnd);

                // Filter by CrimeType
                if (selectedCrimeTypeList?.Length > 0)
                {
                    crimes = crimes.Where(c => selectedCrimeTypeList.Contains(c.CrimeTypeID.ToString()));
                }

                // Filter by Neighbourhood
                if (selectedNeighbourhoodList?.Length > 0)
                {
                    crimes = crimes.Where(c => selectedNeighbourhoodList.Contains(c.NeighbourhoodID.ToString()));
                }

                // Set sort order
                crimes = SetSortOrder(crimes, sortOrder);



                int pageNumber = (page ?? 1);
                int skip = pageNumber * PageSize;
                int take = PageSize;

                int totalRecordCount = crimes.Count();

                // Transform Data

                var items = crimes.ToList().Select(x => new CrimeListViewModel()
                {
                    ID = x.ID,
                    CrimeTypeID = x.CrimeTypeID,
                    CrimeTypeName = x.CrimeType.Type,
                    HundredBlock = x.HundredBlock,
                    NeighbourhoodID = x.NeighbourhoodID,
                    NeighbourhoodName = x.Neighbourhood.Name,
                    OffenceDate = x.OffenceDate,
                    XCoordinate = x.XCoordinate,
                    YCoordinate = x.YCoordinate
                });

                if (totalRecordCount < skip)
                {
                    skip = 0;
                }
                if (totalRecordCount < take)
                {
                    take = totalRecordCount;
                }

                var subset = items.ToList().Skip(skip).Take(take);
                var staticPagedList = new StaticPagedList<CrimeListViewModel>(
                                                        subset, pageNumber, PageSize, totalRecordCount);

                return View(staticPagedList);
            }
            catch (Exception e)
            {

            }

            return View();
        }

        public ActionResult Details(int id)
        {
            ViewBag.Message = "Crime Details";
            var crime = _repository.GetByID(id);
            return View(crime);
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

        private IQueryable<Crime> SetSortOrder(IQueryable<Crime> crimes, string sortOrder)
        {
            switch (sortOrder)
            {
                case "date_desc":
                    crimes = crimes.OrderByDescending(c => c.OffenceDate);
                    break;

                case "crimetype":
                    crimes = crimes.OrderBy(c => c.CrimeType.Type);
                    break;

                case "crimetype_desc":
                    crimes = crimes.OrderByDescending(c => c.CrimeType.Type);
                    break;

                case "neighbourhood":
                    crimes = crimes.OrderBy(c => c.Neighbourhood.Name);
                    break;

                case "neighbourhood_desc":
                    crimes = crimes.OrderByDescending(c => c.Neighbourhood.Name);
                    break;

                default:
                    // Date - Ascending (Oldest to Newest)
                    crimes = crimes.OrderBy(c => c.OffenceDate);
                    break;
            }

            return crimes;
        }


        private void BuildCrimeTypeList(string selectedIds)
        {
            var list = _crimeTypeRepository.GetAll().ToList().Select(x => new SelectListItem()
            {
                Selected = !string.IsNullOrEmpty(selectedIds) ? 
                                selectedIds.Split(',').Contains(x.ID.ToString()) : 
                                false,
                Value = x.ID.ToString(),
                Text = x.Type
            }).ToList();

            ViewData.Add("CrimeTypeList", list);
        }

        private void BuildNeighbourhoodList(string selectedIds)
        {
            var list = _neighbourhoodRepository.GetAll().ToList().Select(x => new SelectListItem()
            {
                Selected = !string.IsNullOrEmpty(selectedIds) ?
                                selectedIds.Split(',').Contains(x.ID.ToString()) :
                                false,
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();

            ViewData.Add("NeighbourhoodList", list);
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
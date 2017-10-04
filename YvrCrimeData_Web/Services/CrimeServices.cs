using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YvrCrimeData_Web.DAL.Repositories;
using YvrCrimeData_Web.DAL.Interfaces;
using YvrCrimeData_Web.Models;

namespace YvrCrimeData_Web.Services
{
    public class CrimeServices
    {
        private IRepository<Crime> _repo;

        public CrimeServices(IRepository<Crime> repo)
        {
            this._repo = repo;
        }

        public IEnumerable<Crime> GetAllCrimes()
        {
            return _repo.GetAll();
        }

        public Crime GetCrimeById(int id)
        {
            return _repo.GetByID(id);
        }
    }
}
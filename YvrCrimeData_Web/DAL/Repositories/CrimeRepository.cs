using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YvrCrimeData_Web.DAL.Interfaces;
using YvrCrimeData_Web.Models;

namespace YvrCrimeData_Web.DAL.Repositories
{
    public class CrimeRepository : IRepository<Crime>, IDisposable
    {
        private YvrCrimeDataContext _dbContext = null;

        public CrimeRepository()
        {
            this._dbContext = new YvrCrimeDataContext();
        }

        public CrimeRepository(YvrCrimeDataContext context)
        {
            this._dbContext = context;
        }

        public void Delete(int id)
        {
            var crime = _dbContext.Crimes.Find(id);
            _dbContext.Crimes.Remove(crime);
        }

        public IQueryable<Crime> GetAll()
        {
            return _dbContext.Crimes;
        }

        public Crime GetByID(int id)
        {
            return _dbContext.Crimes.Find(id);
        }

        public void Insert(Crime entity)
        {
            _dbContext.Crimes.Add(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Crime entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CrimeRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YvrCrimeData_Web.DAL.Interfaces;
using YvrCrimeData_Web.Models;

namespace YvrCrimeData_Web.DAL.Repositories
{
    public class CrimeTypeRepository : IRepository<CrimeType>, IDisposable
    {
        private YvrCrimeDataContext _dbContext = null;

        public CrimeTypeRepository()
        {
            this._dbContext = new YvrCrimeDataContext();
        }

        public CrimeTypeRepository(YvrCrimeDataContext context)
        {
            this._dbContext = context;
        }

        public void Delete(int id)
        {
            var crimeType = _dbContext.CrimeTypes.Find(id);
            _dbContext.CrimeTypes.Remove(crimeType);
        }

        public IQueryable<CrimeType> GetAll()
        {
            return _dbContext.CrimeTypes;
        }

        public CrimeType GetByID(int id)
        {
            return _dbContext.CrimeTypes.Find(id);
        }

        public void Insert(CrimeType entity)
        {
            _dbContext.CrimeTypes.Add(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(CrimeType entity)
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
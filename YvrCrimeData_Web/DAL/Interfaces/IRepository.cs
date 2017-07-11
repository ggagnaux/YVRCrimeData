using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YvrCrimeData_Web.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetByID(int Id);
        void Insert(T entity);
        void Delete(int entity);
        void Update(T entity);
        void Save();
    }
}

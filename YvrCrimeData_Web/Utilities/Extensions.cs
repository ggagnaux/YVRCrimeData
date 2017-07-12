using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.Utilities
{
    public static class Extensions
    {
        public static string ToTraceString<T>(this IQueryable<T> t)
        {
            string sql = "";
            ObjectQuery<T> oqt = t as ObjectQuery<T>;
            if (oqt != null)
                sql = oqt.ToTraceString();
            return sql;
        }
    }
}
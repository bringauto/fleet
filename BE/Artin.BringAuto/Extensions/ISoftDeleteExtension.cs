using Artin.BringAuto.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Extensions
{
    public static class ISoftDeleteExtension
    {
        public static IQueryable<T> SoftDeleteFilter<T>(this IQueryable<T> source)
            where T : class
        {
            if (typeof(T).IsAssignableTo(typeof(ISoftDelete)))
                return source.Where(x => !((ISoftDelete)x).Deleted);
            return source;
        }
    }
}

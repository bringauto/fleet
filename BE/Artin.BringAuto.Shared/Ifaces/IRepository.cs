using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared
{
    public interface IRepository<TDto, TNew, TUpdate, TId>
        where TDto : IId<TId>
        where TUpdate : IId<TId>
    {
        Task<TDto> GetByIdAsync(TId id);

        Task<TDto> UpdateAsync(TUpdate dto);

        Task<TDto> AddAsync(TNew dto);

        Task<TDto> DeleteAsync(TId id);


        IQueryable<TDto> Load();
    }
}

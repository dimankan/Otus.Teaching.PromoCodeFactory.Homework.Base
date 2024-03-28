using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> GetByIdAsync(Guid id);

        Task<string> DeleteAsync(Guid id);
        Task<T> AddAsync(T value);
        Task<T> UpdateAsync(Guid employeeId, T value);

        Task<bool> IsExistByIdAsync(Guid id);
    }
}
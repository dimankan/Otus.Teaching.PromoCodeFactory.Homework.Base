using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected Dictionary<Guid, T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data.ToDictionary(item => item.Id, item => item);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.Select(item => item.Value));
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data[id]);
        }

        public Task<string> DeleteAsync(Guid id)
        {
            string result = string.Empty;

            if (!Data.ContainsKey(id))
            {
                result = $"Пользователя с идентификатором {id} не существует";
            }
            else
            {
                Data.Remove(id);
                result = $"Пользоваетль с идентификатором {id} удалён";
            }

            return Task.FromResult(result);
        }

        public Task<T> AddAsync(T employee)
        {
            employee.Id = Guid.NewGuid();

            Data.Add(employee.Id, employee);

            return Task.FromResult(employee);
        }

        public async Task<T> UpdateAsync(Guid employeeId, T value)
        {
            string updateResult = string.Empty;
            
            value.Id = employeeId;
            Data[employeeId] = value;

            return await Task.FromResult(Data[employeeId]);
        }

        public async Task<bool> IsExistByIdAsync(Guid id)
        {
            bool isExist = Data.ContainsKey(id);

            return await Task.FromResult<bool>(isExist);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Customer.Repository.Sql
{
    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        protected CustomersDbContext _context;
        public RepositoryBase(CustomersDbContext context)
        {
            _context = context;
        }


        public async Task<T> GetById(object id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _context.AddAsync<T>(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Remove<T>(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Update<T>(entity);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}

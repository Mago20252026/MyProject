using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tenant> GetByIdAsync(Guid tenantId)
        {
            throw new NotImplementedException();

        }

        public async  Task AddAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();

        }


    }
}

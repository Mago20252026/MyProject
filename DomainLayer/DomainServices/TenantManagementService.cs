﻿using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class TenantManagementService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantManagementService(ITenantRepository tenantRepository)
        {
            _tenantRepository=tenantRepository;
        }

        public async Task CreateTenantAsync(Tenant tenant)
        {
            await _tenantRepository.AddAsync(tenant);
        }
    }
}

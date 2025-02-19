using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices
{
    public class UserApplicationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;

        public UserApplicationService(IUserRepository userRepository, ITenantRepository tenantRepository)
        {
            _userRepository=userRepository;
            _tenantRepository=tenantRepository;
        }

        public async Task RegisterUser(RegisterUserRequest request)
        {
            var user = new User(request.Username, new EmailAddress(request.Emai));
            await _userRepository.AddAsync(user);

            foreach (var tenantId in request.TenantIds)
            {
                var tenat = await _tenantRepository.GetByIdAsync(tenantId);
               // user.AddTenant(tenat, request.Role);
            }
            await _userRepository.UpdateAsync(user);
        }
        public async Task AddUserToTenant(AddUserToTenantRequest request) 
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
           // user.AddTenant(tenant, request.Role);

            await _userRepository.UpdateAsync(user);
        }
        public async Task RemoveUserFromTenant(RemoveUserFromTenantRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
           // user.RemoveTenant(tenant);

            await _userRepository.UpdateAsync(user);
        }

    }
}

﻿using Domain.ValueObjects;
using SharedDomain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; private set; }
        public List<UserTenant> UserTenants { get; set; }

        public Tenant(string name) {
            Name = name;
            UserTenants = new List<UserTenant>();
        }

        public void AddUser(User user, TenantRole role) {
            var userTenant = new UserTenant(user,this,role);
            UserTenants.Add(userTenant);
        }
        public void UpdateName(string name)
        {
          Name = name;
        }
    }
}

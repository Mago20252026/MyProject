using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserTenant
    {
        
        public User User { get;private set; }
        public Tenant Tenant { get;private set; }
        public TenantRole Role { get;private set; }

        public UserTenant(User user,Tenant tenant,TenantRole role) {
            User = user;
            Tenant = tenant;
            Role = role;
        }
    }
}

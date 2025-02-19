using Domain.DomainEvents;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SharedDomain.Common;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public EmailAddress Email { get; private set; }
        public List<UserTenant> UserTenants { get; private set; }

        public User(string username, EmailAddress email)
        {
            Username = username;
            Email = email;
            UserTenants = new List<UserTenant>();
        }

    }
}


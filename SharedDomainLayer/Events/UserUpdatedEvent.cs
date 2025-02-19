using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Events
{
    public class UserUpdatedEvent
    {
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string Email {  get; private set; }
        public UserUpdatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username =username;
            Email=email;
        }
    }
}

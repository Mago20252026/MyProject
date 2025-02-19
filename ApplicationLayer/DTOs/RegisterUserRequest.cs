using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Emai {  get; set; }
        public List<Guid> TenantIds {  get; set; }
        public TenantRole Role { get; set; }
    }
}

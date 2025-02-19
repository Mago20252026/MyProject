In our architecture, we divide the system into four primary layers:
1.	Domain Layer (Core Business Logic)
2.	Shared Domain Layer (Reusable Business Concepts)
3.	Application Layer (Service Orchestration)
4.	Infrastructure Layer (Persistence & Communication)
5.	Presentation Layer (API/UI Interaction)
 
1. Domain Layer (Core Business Logic)
The Domain Layer is the heart of the application. It encapsulates the core business rules, logic, and domain models, ensuring that business processes remain purely focused on business concepts rather than technical concerns.
Responsibilities
•	Represents the core business logic of a microservice.
•	Defines Entities, Value Objects, Aggregates, and Domain Events to model business operations.
•	Implements Domain Services for logic that spans multiple entities.
•	Uses Repositories to abstract database operations, keeping persistence concerns outside the domain logic.
•	Ignores external systems like databases, APIs, or frameworks (ensuring domain purity).
Key Components
1.	Entities
o	Objects with a unique identity that persist over time.
o	Example: Order, Customer, Invoice.
2.	Value Objects
o	Immutable, identity-less objects that define domain attributes.
o	Example: Money, Address, DateRange.
3.	Aggregates
o	Groups of domain objects that ensure data consistency within a bounded context.
o	Example: Order as an aggregate that includes OrderItems.
4.	Domain Events
o	Events triggered when a significant business operation occurs.
o	Example: OrderPlaced, PaymentProcessed.
5.	Domain Services
o	Encapsulates domain logic that does not belong to a single entity.
o	Example: A pricing calculation service that applies discounts.
6.	Repositories
o	Interface for retrieving and persisting domain objects.
o	Example: IOrderRepository abstracts database queries.
2. Shared Domain Layer (Reusable Business Concepts)
In a microservices ecosystem, some business concepts are shared across multiple services. Instead of duplicating logic, we introduce a Shared Domain Layer to store common domain models, rules, and events.
Why a Shared Domain Layer?
•	Prevents code duplication across multiple services.
•	Standardizes common business logic (e.g., shared validation rules).
•	Ensures consistency in cross-cutting concerns like auditing and identity.
•	Promotes reusability without tightly coupling microservices.
Key Components
1.	Common Value Objects (e.g., Currency, EmailAddress).
2.	Shared Aggregates (e.g., User, Tenant, Product).
3.	Domain Events used across services (UserRegistered, OrderConfirmed).
4.	Base Domain Entities & Interfaces (BaseEntity, IAggregateRoot).
5.	Cross-cutting Business Rules (e.g., standard validation policies).
Implementation Strategy
•	The Shared Domain Layer can be implemented as a shared library (NuGet, ).
•	Each microservice imports and extends the shared models as needed.
•	Shared logic should be purely domain-focused, avoiding dependencies on specific databases or infrastructure.

3. Application Layer (Service Orchestration)
The Application Layer acts as the orchestration layer, coordinating domain logic, executing use cases, and exposing functionality to external systems.
Responsibilities
•	Orchestrates business use cases by calling domain services.
•	Defines Application Services that act as the entry point for operations.
•	Handles transaction management and integrates with external services.
•	Implements security concerns such as access control and validation.
•	Exposes application functionality via REST, gRPC, or Messaging APIs.
Key Components
1.	Application Services
o	Serves as an interface between the API layer and domain layer.
o	Example: OrderApplicationService that processes new orders.
2.	Use Cases (Interactors)
o	Defines a clear business process within the system.
o	Example: A ProcessPayment use case that verifies and processes payments.
3.	DTOs (Data Transfer Objects)
o	Structures data passed between the application and presentation layers.
4.	Security & Transaction Management
o	Implements role-based access control (RBAC) and validation logic.
4. Infrastructure Layer (Persistence & Communication)
The Infrastructure Layer provides technical implementations for external systems such as databases, messaging brokers, and third-party integrations.
Responsibilities
•	Implement repositories for database persistence.
•	Provides messaging/event bus integration for event-driven communication.
•	Handles caching mechanisms to optimize performance.
•	Manages external API integration (e.g., payment gateways).
Key Components
1.	Persistence Mechanisms
o	Uses ORM frameworks (e.g., EF Core) for database management.
2.	Messaging/Event Bus
o	Implements RabbitMQ, or Azure Service Bus for asynchronous communication.
3.	Caching & Distributed Storage
o	Implements Redis, Memcached for optimized performance.
4.	API Adapters & External Services
o	Integrates with external APIs, ensuring loose coupling.

5. Presentation Layer (API Interaction)
The Presentation Layer serves as the entry point for external consumers, including clients, frontend applications, and other microservices. It acts as the interface through which users and external systems interact with the application by exposing REST, gRPC, or GraphQL APIs. This layer ensures that requests are validated, authenticated, and authorized before executing business logic.
Responsibilities
•	Handles external interactions via APIs (REST, gRPC, GraphQL).
•	Expose business functionalities through well-defined endpoints.
•	Implements security mechanisms such as authentication and authorization.
•	Delegates business logic execution to the Application Layer.
•	Integrates with API Gateways for centralized security, routing, and request aggregation.
Key Components
•	Exposes RESTful APIs for external applications and clients.
•	Provides gRPC services for high-performance, inter-microservice communication.
 

Full Project Implimentaion tell now 
// File: Class1.cs

namespace Application
{
    public class Class1
    {

    }
}


--------------------------------------------------------------------------------

// File: TenantApplicationService.cs

using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationServices
{
    public class TenantApplicationService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantApplicationService(ITenantRepository tenantRepository)
        {
            _tenantRepository=tenantRepository;
        }

        public async Task CreateTenant(CreateTenantRequest request) 
        {
            var tenant = new Tenant(request.Name);
            await _tenantRepository.AddAsync(tenant);
        }

        public async Task UpdateTenant(UpdateTenantRequest request)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            tenant.UpdateName(request.Name);
            await _tenantRepository.UpdateAsync(tenant);
        }

        public async Task DeleteTenant(DeleteTenantRequest request) 
        {
            await _tenantRepository.DeleteAsync(request.TenantId);
        }
    }
}


--------------------------------------------------------------------------------

// File: UserApplicationService.cs

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


--------------------------------------------------------------------------------

// File: AddUserToTenantRequest.cs

using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AddUserToTenantRequest
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public TenantRole Role { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: CreateTenantRequest.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateTenantRequest
    {
        public string Name { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: DeleteTenantRequest.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeleteTenantRequest
    {
        public Guid TenantId { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: RegisterUserRequest.cs

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


--------------------------------------------------------------------------------

// File: RemoveUserFromTenantRequest.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RemoveUserFromTenantRequest
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: UpdateTenantRequest.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateTenantRequest
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: RoleBasedAccessControl.cs

using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Security
{
    public class RoleBasedAccessControl
    {
         private readonly Dictionary<TenantRole, List<string>> _rolePermissions;

        public RoleBasedAccessControl()
        {
            _rolePermissions = new Dictionary<TenantRole, List<string>>
            {
                { TenantRole.SuperAdmin, new List<string> { "ManageTenants", "ManageUsers", "ViewReports" } },
                { TenantRole.TenantAdmin, new List<string> { "ManageUsers", "AssignRoles" } },
                { TenantRole.User, new List<string> { "ViewContent" } }
            };
        }

        public bool HasPermission(TenantRole role, string permission)
        {
            if (_rolePermissions.TryGetValue(role, out var permissions))
            {
                return permissions.Contains(permission);
            }

            return false;
        }
    }
}


--------------------------------------------------------------------------------

// File: TenantAggregate.cs

using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates
{
    public class TenantAggregate
    {
        public Tenant Tenant {  get; private set; }
        public List<User> Users { get; private set; }

        public TenantAggregate(Tenant tenant) 
        {
            Tenant = tenant;
            Users = new List<User>();
        }

        public void AddUser(User user, TenantRole role)
        {
           Tenant.AddUser(user, role);
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }
        }
        public void RemoveUser(User user) { 
            Tenant.UserTenants.RemoveAll(ut => ut.User.Id == user.Id);
            Users.Remove(user);
        }
    }
}


--------------------------------------------------------------------------------

// File: UserAggregate.cs

using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates
{
    public class UserAggregate
    {
        public User User { get; private set; }
        public List<UserTenant> UserTenants => User.UserTenants;

        public UserAggregate(User user)
        {
            User = user;
        }

        public void UpdateEmailAddress(EmailAddress newEmail)
        {
            // Update Email Address
            //
        }

        public void UpdateUsername(string newUsername)
        {
            // Update Username
            //
        }

    }
}


--------------------------------------------------------------------------------

// File: TenantCreatedEvent.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class TenantCreatedEvent
    {
        public Guid TenantId { get; private set;}
        public string TenantName {  get; private set;}

        public TenantCreatedEvent(Guid tenantId, string tenantName)
        {
            TenantId=tenantId;
            TenantName=tenantName;
        }
    }
}


--------------------------------------------------------------------------------

// File: UserRegisteredEvent .cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class UserRegisteredEvent 
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }

        public UserRegisteredEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            UserName = username;
            Email = email;
        }
    }
}


--------------------------------------------------------------------------------

// File: TenantManagementService.cs

using Domain.Entities;
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


--------------------------------------------------------------------------------

// File: UserManagementService.cs

using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class UserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;
        
        public UserManagementService(IUserRepository userRepository, ITenantRepository tenantRepository)
        {
            _userRepository=userRepository;
            _tenantRepository=tenantRepository;
        }
        public async Task RegisterUserAsync(User user) 
        {
            await _userRepository.AddAsync(user);
        }
        public async Task AddUserToTenantAsync(User user, Tenant tenant, TenantRole role)
        {
            // Add User to Tenant
            await _userRepository.UpdateAsync(user);
        }
    }
}


--------------------------------------------------------------------------------

// File: Tenant.cs

using Domain.ValueObjects;
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


--------------------------------------------------------------------------------

// File: User.cs

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



--------------------------------------------------------------------------------

// File: UserTenant.cs

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


--------------------------------------------------------------------------------

// File: ITenantRepository.cs

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITenantRepository
    {
        Task<Tenant> GetByIdAsync(Guid id);
        Task AddAsync(Tenant tenant);
        Task UpdateAsync(Tenant tenant);
        Task DeleteAsync(Guid id);
    }
}


--------------------------------------------------------------------------------

// File: IUserRepository.cs

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}


--------------------------------------------------------------------------------

// File: EmailAddress.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record EmailAddress
    {
        public String Value { get; private set;}
        public EmailAddress(String value) {
            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                throw new  Exception("Invalid email format.");

            Value = value;
        }
    }
}


--------------------------------------------------------------------------------

// File: TenantRole.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public enum TenantRole
    {
        SuperAdmin,
        TenantAdmin,
        User
    }
}


--------------------------------------------------------------------------------

// File: Class1.cs

namespace Infrastructure
{
    public class Class1
    {

    }
}


--------------------------------------------------------------------------------

// File: AppDbContext.cs

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserTenant> UserTenants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //configurations
        }
    }
}


--------------------------------------------------------------------------------

// File: TenantRepository.cs

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


--------------------------------------------------------------------------------

// File: UserRepository.cs

using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}


--------------------------------------------------------------------------------

// File: Program.cs

using Application.ApplicationServices;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Authentication;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<UserApplicationService>();
builder.Services.AddScoped<TenantApplicationService>();


builder.Services.AddMemoryCache();
builder.Services.AddSingleton(new JwtTokenService(builder.Configuration["Jwt:Key"]));
/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
*/
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();



--------------------------------------------------------------------------------

// File: JwtTokenService.cs

using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Authentication
{
    public class JwtTokenService
    {
        private readonly string _secret;

        public JwtTokenService(string secret)
        {
            _secret = secret;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


--------------------------------------------------------------------------------

// File: TenantController.cs

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.ApplicationServices;
using Application.DTOs;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly TenantApplicationService _tenantApplicationService;

        public TenantController(TenantApplicationService tenantApplicationService)
        {
            _tenantApplicationService = tenantApplicationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTenantRequest request)
        {
            await _tenantApplicationService.CreateTenant(request);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTenantRequest request)
        {
            await _tenantApplicationService.UpdateTenant(request);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteTenantRequest request)
        {
            await _tenantApplicationService.DeleteTenant(request);
            return Ok();
        }
    }
}


--------------------------------------------------------------------------------

// File: UserController.cs

using Application.ApplicationServices;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserApplicationService _userApplicationService;

        public UserController(UserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _userApplicationService.RegisterUser(request);
            return Ok();
        }

        [HttpPost("add-to-tenant")]
        public async Task<IActionResult> AddUserToTenant([FromBody] AddUserToTenantRequest request)
        {
            await _userApplicationService.AddUserToTenant(request);
            return Ok();
        }

        [HttpPost("remove-from-tenant")]
        public async Task<IActionResult> RemoveUserFromTenant([FromBody] RemoveUserFromTenantRequest request)
        {
            await _userApplicationService.RemoveUserFromTenant(request);
            return Ok();
        }
    }
}


--------------------------------------------------------------------------------

// File: CreateTenantResponse.cs

namespace Presentation.DTOs
{
    public class CreateTenantResponse
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: RegisterUserResponse.cs

namespace Presentation.DTOs
{
    public class RegisterUserResponse
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: TenantModel.cs

namespace Presentation.Models
{
    public class TenantModel
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public List<UserModel> Users { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: UserModel.cs

namespace Presentation.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<TenantModel> Tenants { get; set; }
    }
}


--------------------------------------------------------------------------------

// File: Class1.cs

namespace SharedDomain
{
    public class Class1
    {

    }
}


--------------------------------------------------------------------------------

// File: BaseEntity.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get;protected set; }

        protected BaseEntity() { 
            Id= Guid.NewGuid();
        }
    }
}


--------------------------------------------------------------------------------

// File: IAggregateRoot.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDomain.Common
{
    public interface IAggregateRoot
    {
    }
}


--------------------------------------------------------------------------------

// File: UserUpdatedEvent.cs

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


--------------------------------------------------------------------------------



I need you to implement  with details focus on every single line and  and notice the architecture layers we need to handle I need from you to give me Implementation of all project and the most important thing is File structure give me what I must create or do in details after opening visual studio 
based on wafa project architecture  complete Implementation for  requirement I Will  provided to you  : 


#User Story Description :

As a Super Admin, I want to manage all tenants and users across the system without being assigned to any specific tenant.
As an External User, I want to register independently and have the ability to join one or multiple tenants.
As a Tenant Admin, I want to manage users within my assigned tenant, controlling their roles and permissions.
As a System, I want to allow users to belong to multiple tenants while maintaining separate permissions for each.
As a Super Admin, I want to define and manage roles and permissions for all tenants to maintain system-wide control.
As a Tenant Admin, I want to create and assign roles to users within my tenant, ensuring that permissions do not exceed my own access level.
As the System, I want to restrict admins from creating roles with permissions higher than their own but allow them to assign those roles to themselves afterward.
As a Super Admin, I want to securely authenticate and manage authorization for all users to ensure proper access control.
As a Tenant Admin, I want to authenticate securely and access system functionalities based on my assigned role and permissions.
As an External User, I want to authenticate and be authorized using policies that determine my access level dynamically.
As a Developer, I want to implement an authorization attribute to enforce role-based permissions for each API endpoint.
As a System, I want to enforce a policy-based authorization mechanism for external users to allow flexible access control.

#Functionality : 

1. Super Admin Role
Can create, update, and delete tenants.
Can manage all users across tenants.
Not assigned to any specific tenant.
Can view system-wide reports.
2. Tenant Admin Role
Can manage users within their tenant.
Can assign roles within their tenant.
Cannot access users from other tenants.
3. External User Registration
Users can register independently.
Users can request to join one or multiple tenants.
Tenant Admins must approve external users before they gain access.
4. User-Tenant Relationship
Users can belong to multiple tenants with different roles in each.
A user’s permissions are scoped to the tenant they are operating under.
Users can request to leave a tenant.
User Type Enum (Super Admin, Tenant Admin, External User)
1. Super Admin Role
Can define global roles and permissions.
Can create, update, and delete roles across all tenants.
Has unrestricted access to all system modules.
Can override role restrictions.
2. Tenant Admin Role
Can create and manage roles within their assigned tenant.
Cannot create roles with permissions higher than their own.
Can assign created roles to other users within the tenant.
Can assign a newly created role to themselves after creation.
3. Permission Restriction Mechanism
When creating a role, admins cannot assign permissions beyond their own.
After creating a role, admins can assign it to themselves.
A role’s permissions are validated before assignment.
The system ensures hierarchical enforcement of permissions.
4. Role Assignment Rules
Users can have multiple roles within a tenant.
Each role is associated with a set of permissions.
Roles can be modified but must comply with hierarchical restrictions.
1. Authentication Mechanism
Admins (Super Admin & Tenant Admin)
Role-based authentication using JWT (JSON Web Tokens).
Multi-factor authentication (MFA) for added security.
Session timeout and re-authentication mechanisms.
External Users
Authentication via username/password or external providers (OAuth, SSO).
Policy-based authentication to determine access dynamically.
Temporary session tokens for limited actions.
2. Authorization Mechanism
Role-Based Permissions for Admins

Each role is assigned multiple permissions.
Access is granted or restricted based on assigned permissions.
Permissions and sensitive operations.
Policy-Based Authorization for External Users

Policies define user access dynamically based on attributes (e.g., tenant affiliation, subscription status).
Policies are stored in a policy engine (e.g., Policy-Based Access Control (PBAC)).
Policies can be updated without modifying the application code.
Authorization Attribute for API Protection

Custom [Authorize] attribute applied to each API endpoint.
Attribute enforces access control based on roles and permissions.
API endpoints validate JWT tokens and check user roles before granting access.


#Acceptance criteria : 

Super Admin can manage all tenants and users without being assigned to a specific tenant.
Tenant Admin can only manage users within their tenant.
External Users can register independently and request to join tenants.
Users can belong to multiple tenants with distinct roles in each.
Tenant Admins must approve external users before they gain access to a tenant.
The system should enforce role-based access control based on user type and assigned tenants.
The UI should clearly display tenant affiliations and roles for each user.
Users can request to leave a tenant, which requires admin approval.
Super Admins can create, modify, and delete roles across all tenants.
Tenant Admins can only create roles with permissions equal to or lower than their own.
Tenant Admins can assign newly created roles to themselves after creation.
The system validates role permissions before allowing assignment.
Role permissions are enforced hierarchically, ensuring no user gains access beyond their scope.
A user can hold multiple roles, with permissions merged accordingly.
Permissions updates take effect immediately upon role assignment.
The UI should provide a clear and user-friendly role creation interface with validation messages.
Audit logs track all role and permission changes.
Admins (Super Admin & Tenant Admins) authenticate using secure login with JWT and optional MFA.
Role-based permissions define access control for system modules and actions.
External users authenticate using policies that dynamically define their access level.
Super Admins manage role permissions, ensuring proper access control enforcement.
Tenant Admins manage user roles within their tenant, following role-based restrictions.
API endpoints are protected using an authorization attribute, enforcing role-based access.
The system validates user tokens before granting API access.
Session expiration is enforced, requiring re-authentication after a defined timeout.
Audit logs track authentication attempts, role changes, and permission modifications.
UI displays clear error messages for unauthorized access attempts.

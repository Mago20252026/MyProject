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

# SafeRoad Backend

SafeRoad is a community-driven safe road platform for reporting, verifying, and managing traffic incidents. The backend is built on **.NET 8** using **Clean Architecture** and **CQRS (MediatR)** patterns.

---

## Tech Stack

| Technology | Description |
|---|---|
| **.NET 8** | Runtime & SDK |
| **PostgreSQL + PostGIS** | Database (spatial queries) |
| **Entity Framework Core 8** | ORM (Npgsql + NetTopologySuite) |
| **MediatR** | CQRS mediator pattern |
| **FluentValidation** | Request validation pipeline |
| **AutoMapper** | DTO ↔ Entity mapping |
| **BCrypt.Net** | Password hashing |
| **JWT Bearer** | Authentication |
| **Serilog** | Structured logging |
| **Swagger / Swashbuckle** | API documentation |
| **OSRM** | Route calculation (Open Source Routing Machine) |
| **Supabase Storage** | File/photo storage |

---

## Project Structure

```
SafeRoad-backend/
├── SafeRoad.sln
├── global.json
└── SafeRoad/
    ├── SafeRoad.Application/         # Core / Application layer
    │   ├── Behaviours/               # MediatR pipeline behaviours
    │   ├── DTOs/                     # Data Transfer Objects
    │   ├── Entities/                 # Domain entities
    │   ├── Enums/                    # Enum definitions
    │   ├── Exceptions/               # Custom exception classes
    │   ├── Features/                 # CQRS Commands & Queries
    │   ├── Interfaces/               # Repository & Service interfaces
    │   ├── Settings/                 # Configuration classes
    │   └── Wrappers/                 # ApiResponse<T> wrapper
    │
    ├── SafeRoad.Infrastructure/      # Infrastructure layer
    │   ├── Contexts/                 # EF Core DbContext
    │   ├── Migrations/               # Database migrations
    │   ├── Repositories/             # Repository implementations
    │   ├── Seeds/                    # Seed data
    │   └── Services/                 # Token, Routing, Storage services
    │
    └── SafeRoad.WebApi/              # Presentation layer
        ├── Controllers/              # API Controllers
        ├── Extensions/               # JWT, Swagger extension methods
        ├── Middlewares/              # Exception & Logging middleware
        ├── Services/                 # CurrentUserService
        ├── Swagger/                  # Swagger filters
        └── Program.cs               # Application entry point
```

---

## Setup & Running

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (with PostGIS extension)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### 1. Database Configuration

Configure the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=saferoad;Username=postgres;Password=YOUR_PASSWORD"
  },
  "JwtSettings": {
    "Secret": "A_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG"
  }
}
```

### 2. Apply Migrations

```bash
cd SafeRoad-backend
dotnet ef database update --project SafeRoad/SafeRoad.Infrastructure --startup-project SafeRoad/SafeRoad.WebApi
```

### 3. Run

```bash
cd SafeRoad-backend/SafeRoad/SafeRoad.WebApi
dotnet run
```

Or from the root directory:
```bash
cd SafeRoad-backend
dotnet run --project SafeRoad/SafeRoad.WebApi/SafeRoad.WebApi.csproj
```

The application listens on the following addresses by default:
- **HTTPS:** `https://localhost:9001`
- **HTTP:** `http://localhost:9000`
- **Swagger UI:** `https://localhost:9001/swagger`

---

## Entities (Domain Models)

| Entity | Description | Spatial Type |
|---|---|---|
| **User** | User account (email, password hash, trust score, avatar) | — |
| **Role** | Role definition (User, Moderator, Admin, Municipality) | — |
| **UserRole** | User-Role join table (multi-role support) | — |
| **Incident** | Traffic incident report (title, description, location, status) | `Point` |
| **IncidentCategory** | Incident category (Pothole, Accident, etc.) | — |
| **IncidentPhoto** | Incident photo (Supabase blob URL) | — |
| **Municipality** | Municipality (name, boundary polygon) | `Polygon` |
| **Comment** | Comment on an incident | — |
| **Verification** | Verification/dispute vote on an incident | — |
| **UserJourney** | User journey (route, status) | `LineString` |
| **JourneyIncident** | Journey-Incident relationship | — |
| **WatchedArea** | Watched area (center point, radius) | `Point` |
| **DeviceToken** | Push notification device token | — |

---

## Enums

| Enum | Values |
|---|---|
| `IncidentStatus` | `Pending`, `Verified`, `Disputed`, `Resolved` |
| `JourneyStatus` | `Active`, `Completed` |
| `UserStatus` | `Active`, `Banned` |

> All enums are stored as **strings** in the database.

---

## Roles & Authorization

| Id | Role | Permissions |
|---|---|---|
| 1 | **User** | Report incidents, comment, verify, journey, watched areas |
| 2 | **Moderator** | User + update incident status |
| 3 | **Admin** | All permissions (user management, ban/unban, incident CRUD) |
| 4 | **Municipality** | User + view/update incidents assigned to their municipality |

Users can have **multiple roles** (`UserRole` join table).

---

## API Endpoints

### Health — `api/health`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/health` | Health check | — |

### Auth — `api/auth`
| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Register a new user | — |
| POST | `/api/auth/login` | Login (email/password) | — |
| POST | `/api/auth/logout` | Logout (clears device tokens) | `Authorize` |

### Incidents — `api/incidents`
| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/incidents` | Report a new incident | `Authorize` |
| GET | `/api/incidents` | Get all incidents (paginated) | — |
| GET | `/api/incidents/nearby` | Get nearby incidents (lat, lng, radius) | — |
| GET | `/api/incidents/{id}` | Get incident details | — |
| GET | `/api/incidents/my` | Get my incidents | `Authorize` |
| GET | `/api/incidents/by-municipality/{id}` | Get incidents by municipality | `Admin, Moderator, Municipality` |
| PATCH | `/api/incidents/{id}/status` | Update incident status | `Admin, Moderator, Municipality` |
| DELETE | `/api/incidents/{id}` | Delete incident (owner only) | `Authorize` |

### Comments — `api/incidents/{incidentId}/comments`
| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/incidents/{id}/comments` | Add a comment | `Authorize` |
| GET | `/api/incidents/{id}/comments` | Get comments (paginated) | — |
| DELETE | `/api/incidents/{id}/comments/{commentId}` | Delete own comment | `Authorize` |

### Verifications — `api/incidents/{incidentId}/...`
| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/incidents/{id}/verify` | Verify incident (upvote) | `Authorize` |
| POST | `/api/incidents/{id}/dispute` | Dispute incident (downvote) | `Authorize` |
| DELETE | `/api/incidents/{id}/verify` | Remove vote | `Authorize` |
| GET | `/api/incidents/{id}/verifications` | List verifications | — |

### Users — `api/users`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/users/me` | Get my profile | `Authorize` |
| PUT | `/api/users/me` | Update profile (name, avatar) | `Authorize` |
| PUT | `/api/users/me/password` | Change password | `Authorize` |
| GET | `/api/users/me/stats` | Get personal stats | `Authorize` |
| GET | `/api/users/{id}` | Get public profile | — |
| GET | `/api/users/{id}/trust-score` | Get trust score | — |

### Journeys — `api/journeys`
| Method | Route | Description | Auth |
|---|---|---|---|
| POST | `/api/journeys/start` | Start journey (route + incident detection) | `Authorize` |
| POST | `/api/journeys/end` | End active journey | `Authorize` |
| GET | `/api/journeys/active` | Get active journey | `Authorize` |
| GET | `/api/journeys` | Get past journeys (paginated) | `Authorize` |

### Watched Areas — `api/watched-areas`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/watched-areas` | Get my watched areas | `Authorize` |
| POST | `/api/watched-areas` | Create a watched area | `Authorize` |
| PUT | `/api/watched-areas/{id}` | Update a watched area | `Authorize` |
| DELETE | `/api/watched-areas/{id}` | Delete a watched area | `Authorize` |

### Analytics — `api/analytics`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/analytics/overview` | Platform-wide overview stats | — |
| GET | `/api/analytics/categories` | Incident count per category | — |
| GET | `/api/analytics/trends` | Incident trends (last N days) | — |

### Admin — `api/admin`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/admin/users` | Get all users (paginated, filterable) | `Admin` |
| PATCH | `/api/admin/users/{id}/ban` | Ban a user | `Admin` |
| PATCH | `/api/admin/users/{id}/unban` | Unban a user | `Admin` |
| PUT | `/api/admin/users/{id}` | Update user (name, roles) | `Admin` |
| GET | `/api/admin/incidents` | Get all incidents (admin view) | `Admin` |
| PUT | `/api/admin/incidents/{id}` | Update an incident | `Admin` |
| DELETE | `/api/admin/incidents/{id}` | Delete an incident | `Admin` |

### Incident Categories — `api/incident-categories`
| Method | Route | Description | Auth |
|---|---|---|---|
| GET | `/api/incident-categories` | Get all categories | — |

> **Total: 38 endpoints** across 10 Controllers

---

## CQRS Structure (Features)

Each feature resides in its own directory containing Command/Query + Handler + (optional) Validator.

| Domain | Commands | Queries | Total |
|---|---|---|---|
| **Analytics** | — | 3 | 3 |
| **Auth** | 3 | — | 3 |
| **Comments** | 2 | 1 | 3 |
| **IncidentCategories** | — | 1 | 1 |
| **Incidents** | 5 | 5 | 10 |
| **UserJourneys** | 2 | 2 | 4 |
| **Users** | 5 | 4 | 9 |
| **Verifications** | 3 | 1 | 4 |
| **WatchedAreas** | 3 | 1 | 4 |
| **Total** | **18** | **15** | **33** |

---

## Architecture Decisions

### API Response Wrapper
All endpoints return an `ApiResponse<T>` wrapper:
```json
{
  "succeeded": true,
  "message": "Success",
  "data": { ... },
  "errors": []
}
```

### Validation Pipeline
`FluentValidation` validators run automatically via the `ValidationBehaviour<TRequest, TResponse>` MediatR pipeline behaviour. Invalid requests throw a `BadRequestException` before reaching the handler.

### Exception Handling
`GlobalExceptionMiddleware` catches all exceptions and maps them to appropriate HTTP status codes:

| Exception | Status Code |
|---|---|
| `BadRequestException` | 400 |
| `UnauthorizedException` | 401 |
| `ForbiddenException` | 403 |
| `NotFoundException` | 404 |
| Other | 500 |

### Spatial Queries
Powered by PostGIS + NetTopologySuite:
- **Nearby incident search** → `ST_DWithin` (radius-based)
- **Incidents along route** → `ST_Buffer` + `ST_Intersects`
- **Watched area check** → Point/radius-based search

### Route Calculation
When a journey is started, a real road route is calculated via the [OSRM](http://project-osrm.org/) API and stored as a `LineString`. Incidents along the route are automatically detected.

---

## Seed Data (Test Data)

| Seed | Content |
|---|---|
| **Roles** | User, Moderator, Admin, Municipality |
| **Municipalities** | 11 municipalities (Antalya, Kepez, Hatay, etc.) |
| **Incident Categories** | Pothole, Accident, Flood, etc. |
| **Users** | Admin, Moderator, 2 Users, 2 Municipality Officers |
| **Incidents** | Sample incidents (location, category, photos) |
| **Comments** | Sample comments |
| **Verifications** | Sample vote data |

### Test Accounts

| Account | Email | Password | Role |
|---|---|---|---|
| Admin | `admin@saferoad.com` | `Admin@123!` | Admin |
| Moderator | `moderator@antalya.bel.tr` | `Mod@123!` | Moderator |
| User 1 | `john.doe@gmail.com` | `User@123!` | User |
| User 2 | `jane.smith@gmail.com` | `User@123!` | User |
| Municipality | `officer@kepez.bel.tr` | `Mun@123!` | Municipality |
| Hatay Municipality | `hatay@belediye.gov.tr` | `123456` | Municipality |

---

## Configuration

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "PostgreSQL connection string"
  },
  "JwtSettings": {
    "Secret": "min 32 character secret key",
    "Issuer": "SafeRoad",
    "Audience": "SafeRoad",
    "ExpirationInMinutes": 60
  },
  "SupabaseStorage": {
    "SupabaseUrl": "https://xxx.supabase.co",
    "SupabaseServiceKey": "service key",
    "BucketName": "incident-photos"
  }
}
```

### CORS
CORS is enabled for the frontend origin (`http://localhost:4200`) with credentials support.

---

## Useful Commands

```bash
# Build
dotnet build SafeRoad.sln

# Run
cd SafeRoad/SafeRoad.WebApi && dotnet run

# Create a new migration
dotnet ef migrations add MigrationName \
  --project SafeRoad/SafeRoad.Infrastructure \
  --startup-project SafeRoad/SafeRoad.WebApi

# Apply migrations
dotnet ef database update \
  --project SafeRoad/SafeRoad.Infrastructure \
  --startup-project SafeRoad/SafeRoad.WebApi

# Resolve port conflict
lsof -ti :9001 | xargs kill -9
```

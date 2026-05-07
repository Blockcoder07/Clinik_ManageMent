# Clinic Management System

A multi-tenant Clinic Management System built with **ASP.NET Core 9 Web API**, **Angular**, and **SQL Server**. Each clinic (tenant) can manage its patients, appointments, invoices, and view revenue summaries with strict data isolation enforced at the data-access layer.

## Tech Stack

- **Backend:** ASP.NET Core 9 Web API, EF Core 9 (Code First), SQL Server
- **Frontend:** Angular (TypeScript, strict mode)
- **Auth:** JWT Bearer
- **Validation:** FluentValidation
- **Concurrency:** EF Core `RowVersion` + DB-level unique indexes
- **Reporting:** SQL Stored Procedure (`usp_GetRevenueSummary`)

## Architecture

```
Clini_Management_System.Server/
├── Controllers/        API endpoints
├── Services/           Business logic (Interfaces + Implementations)
├── Repositories/       Data access (Interfaces + Implementations)
├── Data/               AppDbContext + SQL scripts + Migrations
├── Models/             Entities, DTOs, Enums
├── Common/             ApiResponse, Exceptions, TenantContext, PagedResult
├── Helpers/            JwtTokenGenerator, PasswordHasher
├── Middleware/         GlobalExceptionMiddleware
└── Validators/         FluentValidation rules

clini_management_system.client/
└── src/app/
    ├── core/           Services, guards, interceptors, types
    └── features/       Login, Appointments, Dashboard
```

## Key Features

- JWT-based authentication with role + `clinicId` claim
- Multi-tenant isolation via EF Core global query filters
- Optimistic concurrency on `Appointment` (`RowVersion` → 409 Conflict)
- Race-safe invoice creation (unique index on `AppointmentId`)
- Revenue dashboard with **DB-level aggregation**
- Pagination, filtering, searching
- Global exception handling with consistent `ApiResponse<T>` envelope
- Centralized Angular API client + global 401 handler

## Business Rules

1. Appointments cannot be created in the past.
2. Completed appointments cannot be cancelled.
3. Invoices can only be created for completed appointments.
4. Only one invoice per appointment.

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js 18+
- SQL Server (local or LocalDB)

### Backend
```bash
cd Clini_Management_System.Server
dotnet restore
dotnet ef database update
dotnet run
```
Backend runs at `https://localhost:7019`. Swagger UI: `https://localhost:7019/swagger`.

Apply the stored procedure once:
```bash
sqlcmd -S "(localdb)\\MSSQLLocalDB" -d ClinicManagementDb -i Data/Sql/usp_GetRevenueSummary.sql
```

### Frontend
```bash
cd clini_management_system.client
npm install
npm start
```
Frontend runs at `http://localhost:4200` and proxies `/api` → backend.

## API Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/auth/register` | Register clinic + admin user |
| POST | `/api/auth/login` | Login, returns JWT |
| GET  | `/api/patients` | List patients (paged, search) |
| POST | `/api/patients` | Create patient |
| GET  | `/api/appointments` | List appointments (date filter, paged) |
| POST | `/api/appointments` | Create appointment |
| PATCH| `/api/appointments/{id}/status` | Update status (concurrency-checked) |
| POST | `/api/invoices` | Create invoice (one per appointment) |
| GET  | `/api/dashboard/revenue-summary?from=&to=` | Revenue summary (DB-aggregated) |

## License

MIT

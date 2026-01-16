# 🎓 RMP-Core: University & Professor Rating

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Status](https://img.shields.io/badge/Status-Active%20Development-blue?style=flat-square)](README.md)
[![API](https://img.shields.io/badge/API-RESTful%20%26%20gRPC-informational?style=flat-square)](README.md)

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [Configuration](#configuration)
- [Database](#database)
- [API Documentation](#api-documentation)
- [Usage Examples](#usage-examples)
- [Contributing](#contributing)
- [Authors](#authors)
- [License](#license)

---

## 🎯 Overview

**RMP-Core** is a comprehensive backend system for managing university ratings and monitoring platforms. It provides a robust API for handling universities, departments, professors, courses, and student ratings. Built with modern .NET technologies, the platform implements industry-standard patterns like CQRS, MediatR, and Domain-Driven Design principles.

**Use Case:** Educational institutions can leverage this platform to:
- ✅ Manage organizational hierarchies (Universities → Departments → Professors)
- ✅ Handle student ratings and reviews for universities and professors
- ✅ Track academic course information and management
- ✅ Monitor platform metrics and analytics through admin dashboards
- ✅ Predict rating trends using AI/ML integration

---

## ✨ Features

### Core Features

| Feature | Description |
|---------|-------------|
| 👥 **User Management** | JWT-based authentication, role-based access control (RBAC), identity management |
| 🏫 **University Management** | CRUD operations for universities, departments, and organizational units |
| 👨‍🏫 **Professor Management** | Professor profiles, course associations, rating tracking |
| 📚 **Course Management** | Course creation, updates, and department-wise organization |
| ⭐ **Rating System** | Multi-strategy rating system for professors and universities |
| 📊 **Admin Dashboard** | Analytics, statistics, and monitoring capabilities |
| 📰 **News Management** | News publishing and content management |
| 🤖 **AI Integration** | gRPC-based prediction service for rating trend analysis |

### Advanced Features

- **CQRS Pattern**: Command Query Responsibility Segregation for clean architecture
- **Validation Pipeline**: FluentValidation with automatic pipeline behaviors
- **Health Checks**: Comprehensive health check endpoints with UI dashboard
- **Error Handling**: Centralized global exception handling with custom error types
- **File Upload**: Integrated file upload management system
- **API Documentation**: Swagger/OpenAPI integration with security definitions
- **Logging**: Structured logging using MediatR behaviors
- **CORS Support**: Configurable cross-origin resource sharing

---

## 🛠️ Technology Stack

### Backend Framework & ORM

```
┌─────────────────────────────────────┐
│   .NET 8.0 (C#)                     │
│   Entity Framework Core 8.0         │
│   SQLite / SQL Server               │
└─────────────────────────────────────┘
```

| Layer | Technology | Purpose |
|-------|-----------|---------|
| **Framework** | ASP.NET Core 8.0 | Web API framework |
| **Architecture** | Carter, MediatR | Minimal APIs & CQRS |
| **Database** | Entity Framework Core 8.0 | ORM & Data Access |
| **Validation** | FluentValidation 12.1.1 | Business logic validation |
| **Mapping** | Riok.Mapperly 4.1.1 | Object mapping (source generators) |
| **API Docs** | Swagger/Swashbuckle 6.4.0 | OpenAPI documentation |
| **gRPC** | gRPC.AspNetCore 2.76.0 | Service-to-service communication |
| **Health Checks** | AspNetCore.HealthChecks | Monitoring & diagnostics |
| **Authentication** | ASP.NET Core Identity | User management & authentication |
| **Containerization** | Docker | Deployment & environments |

---

## 🏗️ Architecture

### Design Patterns

```
┌────────────────────────────────────────────────────────┐
│                    API Layer (Minimal APIs)             │
├────────────────────────────────────────────────────────┤
│                   Command/Query Handlers                │
│                      (MediatR)                          │
├────────────────────────────────────────────────────────┤
│            Domain Services & Business Logic             │
├────────────────────────────────────────────────────────┤
│              Repository Pattern (EF Core)               │
├────────────────────────────────────────────────────────┤
│              Application DbContext (EF)                 │
├────────────────────────────────────────────────────────┤
│        Database (SQLite / SQL Server)                   │
└────────────────────────────────────────────────────────┘
```

### CQRS Implementation

- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations returning data transfer objects
- **Handlers**: Implements business logic for each command/query
- **Behaviors**: Cross-cutting concerns (validation, logging)

### Error Handling

- Custom `Error` domain objects for type-safe error handling
- Global exception middleware for consistent error responses
- Feature-specific error definitions (e.g., `CourseErrors.cs`)

---

## 📁 Project Structure

### Directory Organization

```
RMP.Core.Host/
├── 📄 Program.cs                    # Application entry point & DI setup
├── 🔧 appsettings.json              # Configuration & secrets
├── 📦 RMP.Core.Host.csproj          # Project file & dependencies
│
├── Abstractions/                    # Domain abstractions & interfaces
│   ├── CQRS/                       # Command/Query interfaces
│   ├── Errors/                     # Error domain models
│   └── ResultResponse/             # Generic result wrappers
│
├── Behaviors/                       # MediatR pipeline behaviors
│   ├── ValidationBehavior.cs       # Fluent validation integration
│   └── LoggingBehavior.cs          # Structured logging
│
├── Database/                        # EF Core configuration
│   └── ApplicationDbContext.cs      # DbContext definition
│
├── Entities/                        # Domain entities
│   ├── BaseEntity.cs               # Base entity (Id, timestamps)
│   ├── CourseEntity.cs             # Course aggregate
│   ├── ProfessorEntity.cs          # Professor aggregate
│   ├── UniversityEntity.cs         # University aggregate
│   ├── DepartmentEntity.cs         # Department aggregate
│   ├── NewsEntity.cs               # News content
│   ├── RateProfessorEntity.cs      # Rating for professors
│   ├── RateUniversityEntity.cs     # Rating for universities
│   └── Identity/                   # ASP.NET Core Identity tables
│
├── Exceptions/                      # Exception handling
│   └── GlobalExceptionHandler.cs   # Centralized error handling
│
├── Extensions/                      # Extension methods
│   ├── DatabaseExtensions.cs       # EF Core setup
│   ├── MediatRExtensions.cs        # CQRS pipeline configuration
│   ├── HealthChecksExtensions.cs   # Health check setup
│   ├── FileUploadHelper.cs         # File upload utilities
│   └── ResultExtensions.cs         # Result wrapper helpers
│
├── Features/                        # Feature modules (CQRS)
│   ├── AdminDashboard/             # Dashboard queries & handlers
│   ├── Course/                     # Course CRUD + errors
│   ├── Department/                 # Department operations
│   ├── Professor/                  # Professor management
│   ├── University/                 # University operations
│   ├── News/                       # News management
│   ├── Rating/                     # Rating system (multi-strategy)
│   └── User/                       # User management & auth
│
├── Mapper/                          # Auto-mapping configurations
│   ├── CourseMapper.cs
│   ├── ProfessorMapper.cs
│   └── ... (other entity mappers)
│
├── Migrations/                      # EF Core migrations
│   ├── 20260113220044_Initial.cs
│   └── ApplicationDbContextModelSnapshot.cs
│
├── Properties/
│   └── launchSettings.json         # Debug profiles
│
└── wwwroot/                         # Static files & uploads
    └── uploads/                    # User-uploaded files
```

### Feature Module Structure Example (Course)

```
Features/Course/
├── CourseErrors.cs                 # Domain-specific errors
├── CreateCourse/
│   ├── CreateCourseCommand.cs      # Request DTO
│   ├── CreateCourseHandler.cs      # Command handler
│   ├── CreateCourseValidator.cs    # FluentValidation rules
│   └── CreateCourseEndpoint.cs     # API endpoint
├── GetCourses/
│   ├── GetCoursesQuery.cs
│   ├── GetCoursesHandler.cs
│   └── GetCoursesEndpoint.cs
├── UpdateCourse/
│   └── ...
├── DeleteCourse/
│   └── ...
└── GetCourseById/
    └── ...
```

---

## 💻 Installation & Setup

### Prerequisites

- **.NET SDK 8.0** or higher ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **SQL Server** or **SQLite** (SQLite included with .NET)
- **Docker** (optional, for containerization)
- **Visual Studio 2022** or **Visual Studio Code**

### Step 1: Clone Repository

```bash
git clone https://github.com/yourusername/RMP-Core.git
cd RMP-Core
```

### Step 2: Navigate to Project

```bash
cd RMP.Core.Host
```

### Step 3: Restore Dependencies

```bash
dotnet restore
```

### Step 4: Configure Database

#### Using SQLite (Development)

The project is pre-configured for SQLite. The database file will be created automatically:

```bash
# Apply migrations
dotnet ef database update

# Or, if you prefer to create from scratch
dotnet ef migrations add Initial
dotnet ef database update
```

#### Using SQL Server (Production)

Modify `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "SQLConnection": "Server=YOUR_SERVER;Database=RMP_Core;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=true;"
  }
}
```

Then apply migrations:

```bash
dotnet ef database update
```

### Step 5: Run Application

```bash
dotnet run
```

The API will start at: `https://localhost:7000` (HTTPS) or `http://localhost:5000` (HTTP)

### Step 6: Access Swagger UI

Navigate to: **`https://localhost:7000/swagger/index.html`**

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "Jwt": {
    "Key": "your-base64-encoded-secret-key-min-32-chars"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "SQLConnection": "Data Source=rmpu2026.db"
  },
  "frontend_url": "http://localhost:3000"
}
```

### Environment Variables

| Variable | Purpose | Default |
|----------|---------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment mode | `Development` |
| `ASPNETCORE_URLS` | Server URLs | `http://localhost:5000` |
| `ConnectionStrings__SQLConnection` | Database connection | See appsettings |
| `Jwt__Key` | JWT signing key | From appsettings |

### Identity Configuration

Password policy (from `Program.cs`):

```csharp
options.Password.RequiredLength = 6;
options.Password.RequireDigit = true;
options.Lockout.MaxFailedAccessAttempts = 7;
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
```

---

## 🗄️ Database

### Entity Relationships

```
┌─────────────────────────────────────────┐
│         University                      │
│    (Rates: RateUniversityEntity)       │
└──────────────┬──────────────────────────┘
               │
               ├─→ Department
               │       ├─→ DepartmentProfessor
               │       └─→ Course
               │
               └─→ Professor
                       ├─→ ProfessorCourse
                       └─→ Rates: RateProfessorEntity
```

### Key Entities

| Entity | Purpose |
|--------|---------|
| `UserEntity` | User accounts with Identity integration |
| `UniversityEntity` | University information |
| `DepartmentEntity` | Academic departments |
| `ProfessorEntity` | Professor profiles |
| `CourseEntity` | Academic courses |
| `RateProfessorEntity` | Student ratings for professors |
| `RateUniversityEntity` | Student ratings for universities |
| `NewsEntity` | News and announcements |

### Migrations

View existing migrations:

```bash
dotnet ef migrations list
```

Create new migration:

```bash
dotnet ef migrations add YourMigrationName
```

---

## 📡 API Documentation

### Authentication

All protected endpoints require a JWT token in the Authorization header:

```http
Authorization: Bearer {your-jwt-token}
```

### Base URL

```
https://localhost:7000/api
```

### Health Check Endpoint

```http
GET /health
```

Response:
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy"
  }
}
```

### Example Endpoints

#### Get All Universities

```http
GET /api/universities
```

#### Create Course

```http
POST /api/courses
Content-Type: application/json

{
  "name": "Advanced C# Programming",
  "description": "Learn modern C# patterns",
  "departmentId": "uuid-here"
}
```

#### Rate Professor

```http
POST /api/ratings/professor
Content-Type: application/json

{
  "professorId": "uuid-here",
  "rating": 4.5,
  "comment": "Excellent teacher",
  "userId": "uuid-here"
}
```

---

## 🚀 Usage Examples

### Running via Docker

#### Build Image

```bash
docker build -t rmp-core:latest .
```

#### Run Container

```bash
docker run -p 5000:8080 \
  -e ConnectionStrings__SQLConnection="Data Source=/app/data/rmp.db" \
  -e Jwt__Key="your-secret-key" \
  -v rmp-data:/app/data \
  rmp-core:latest
```

### Accessing Health Checks UI

```
https://localhost:7000/healthchecks-ui
```

### Database Seeding (if implemented)

```bash
dotnet run --seed
```

---

## 🤝 Contributing

We welcome contributions! Here's how to get started:

### Development Workflow

1. **Fork** the repository
2. **Create** a feature branch: `git checkout -b feature/your-feature`
3. **Follow** the coding standards:
   - Use Pascal case for class names
   - Use CQRS pattern for new features
   - Add FluentValidation validators
   - Write meaningful commit messages
4. **Test** your changes locally
5. **Push** to your fork: `git push origin feature/your-feature`
6. **Create** a Pull Request with a clear description

### Code Style Guidelines

- Follow [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use nullable reference types (`#nullable enable`)
- Implement feature-specific error classes
- Add XML documentation comments for public APIs

### Reporting Issues

Please use the GitHub Issues tracker with:
- Clear title and description
- Reproduction steps (if bug)
- Expected vs. actual behavior
- Environment details (.NET version, OS)

---

## 👥 Authors

This project was developed as part of an academic seminar on multidisciplinary applications:

| Name |
|------|
| **Rigon Pira** | 
| **Gentrit Halimi** |
| **Argjend Azizi** | 
| **Euron Ramadani** |
| **Ardit Shabani** |

**Institution**: [UBT College](https://www.ubt-uni.net/) - Prishtina, Kosovo

**Seminar**: Seminar & LAB ne aplikimet multidisplinare (Multidisciplinary Applications Seminar & Lab)
**Semester**: 3 (Academic Year 2025-2026)

---


## 📚 Additional Resources

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Carter Documentation](https://github.com/CarterCommunity/carter)

---


### 🌟 If you found this project helpful, please give it a ⭐ Star!


</div>

# 🛒 ECommerce API

[![.NET](https://img.shields.io/badge/.NET-10-blue?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-9.0-blue?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

A **scalable E-Commerce RESTful API** built with **ASP.NET Core (.NET 10)** following **Clean Architecture** principles.
Implements **CQRS**, **MediatR**, **Result Pattern**, **Repository Pattern**, **JWT Authentication**, and **FluentValidation**.

---

## 📖 Table of Contents

* [Features](#-features)
* [Tech Stack](#-tech-stack)
* [Project Structure](#-project-structure)
* [Getting Started](#-getting-started)
* [API Endpoints](#-api-endpoints)
* [Architecture Overview](#-architecture-overview)
* [Future Improvements](#-future-improvements)
* [Author](#-author)

---

## ✨ Features

* **Authentication & Authorization**

  * JWT Bearer Token
  * Email confirmation
* **Clean Architecture**

  * Separation of concerns
  * Domain-driven design
* **CQRS + MediatR**

  * Commands & Queries pattern
  * ValidationBehavior pipeline
* **Repository Pattern**

  * Optional explicit repositories
* **Result Pattern**

  * Standardized API responses & error handling
* **FluentValidation**

  * Centralized request validation
* **EF Core**

  * SQL Server database
  * Entity configurations
* **Swagger / OpenAPI**

  * API documentation

---

## 🛠️ Tech Stack

| Layer          | Technologies                                          |
| -------------- | ----------------------------------------------------- |
| Framework      | ASP.NET Core (.NET 10)                                |
| Language       | C#                                                    |
| Database       | SQL Server, EF Core                                   |
| Architecture   | Clean Architecture, CQRS, Repository Pattern, MediatR |
| Validation     | FluentValidation                                      |
| Mapping        | AutoMapper                                            |
| Authentication | JWT Bearer                                            |
| Logging        | Serilog                                               |
| Documentation  | Swagger / OpenAPI                                     |

---

## 📂 Project Structure

```text
ECommerce.Domain
├── Entities
│   ├── Product.cs
│   ├── Category.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── ApplicationUser.cs
├── Errors
│   ├── ProductErrors.cs
│   ├── CategoryErrors.cs
│   └── OrderErrors.cs
└── Common
    ├── Error.cs
    └── Result.cs

ECommerce.Application
├── Common
│   ├── Behaviors/ValidationBehavior.cs
│   ├── Interfaces/IApplicationDbContext.cs
│   ├── Interfaces/ICurrentUserService.cs
│   └── Errors/ValidationErrors.cs
├── Features/...
└── DependencyInjection.cs

ECommerce.Infrastructure
├── Persistence/ApplicationDbContext.cs
├── Persistence/Configurations/...
├── Services/CurrentUserService.cs
└── DependencyInjection.cs

ECommerce.Api
├── Controllers/...
├── Middleware/GlobalExceptionHandler.cs
├── Extensions/ResultExtensions.cs
└── DependencyInjection.cs
```

---

## ⚡ Getting Started

### 1️⃣ Clone the repository

```bash
git clone https://github.com/meshmuhammadasaadg/ECommerce.git
cd ECommerce
```

### 2️⃣ Configure Database

Update `appsettings.json` connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;"
}
```

### 3️⃣ Apply Migrations

```bash
dotnet ef database update
```

### 4️⃣ Run the API

```bash
dotnet run --project ECommerce.Api
```

Swagger will be available at:
`https://localhost:5001/swagger`

---

## 📌 Example API Requests

### Get All Products

```http
GET /api/products
Authorization: Bearer {JWT_TOKEN}
```

### Create Product

```http
POST /api/products
Authorization: Bearer {JWT_TOKEN}
Content-Type: application/json

{
  "name": "Laptop",
  "categoryId": 1,
  "price": 1200.50,
  "stock": 10
}
```

### Response

```json
{
  "isSuccess": true,
  "data": {
    "id": 1,
    "name": "Laptop",
    "categoryId": 1,
    "price": 1200.5,
    "stock": 10
  },
  "errors": null
}
```

---

## 🧱 Architecture Overview

* **Domain** → Entities, Errors, Result pattern
* **Application** → CQRS (Commands & Queries), MediatR pipeline, ValidationBehavior
* **Infrastructure** → EF Core, Repositories, Services
* **API** → Controllers, Middleware, Extensions

---

## 🔮 Future Improvements

* Unit & Integration Tests
* Role-based Authorization
* Refresh Token mechanism
* Docker Support
* CI/CD pipeline

---

## 👨‍💻 Author

**Muhammad Asaad**

* GitHub: [meshmuhammadasaadg](https://github.com/meshmuhammadasaadg)
* LinkedIn: [meshmuhammad-asaad](https://linkedin.com/in/meshmuhammad-asaad)
* Email: [meshmuhammadasaad@gmail.com](mailto:meshmuhammadasaad@gmail.com)

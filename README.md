# ECommerce

ECommerce.Domain
├── Entities
│   ├── Product.cs
│   ├── Category.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── ApplicationUser.cs
│
├── Errors
│   ├── ProductErrors.cs
│   ├── CategoryErrors.cs
│   └── OrderErrors.cs
│
└── Common
    ├── Error.cs
    └── Result.cs

ECommerce.Application
├── Common
│   ├── Behaviors
│   │   └── ValidationBehavior.cs       ← MediatR pipeline behavior
│   ├── Interfaces
│   │   ├── IApplicationDbContext.cs
│   │   └── ICurrentUserService.cs
│   └── Errors
│       └── ValidationErrors.cs
├── Features
│   ├── Products
│   │   ├── Commands
│   │   │   ├── CreateProduct
│   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   └── CreateProductCommandValidator.cs
│   │   │   └── UpdateProduct
│   │   │       ├── UpdateProductCommand.cs
│   │   │       ├── UpdateProductCommandHandler.cs
│   │   │       └── UpdateProductCommandValidator.cs
│   │   └── Queries
│   │       └── GetAllProducts
│   │           ├── GetAllProductsQuery.cs
│   │           └── GetAllProductsQueryHandler.cs
│   ├── Categories
│   │   └── Commands/ ...
│   ├── Orders
│   │   └── Commands/ ...
│   └── Auth
│       └── Commands/ ...
└── DependencyInjection.cs

ECommerce.Infrastructure
├── Persistence
│   ├── ApplicationDbContext.cs
│   └── Configurations
│       ├── ProductConfiguration.cs
│       └── OrderConfiguration.cs
├── Repositories/   ← Optional (explicit repositories)
├── Services
│   └── CurrentUserService.cs
└── DependencyInjection.cs

ECommerce.Api
├── Controllers
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── OrdersController.cs
│   └── AuthController.cs
├── Middleware
│   └── GlobalExceptionHandler.cs
├── Extensions
│   └── ResultExtensions.cs
└── DependencyInjection.cs

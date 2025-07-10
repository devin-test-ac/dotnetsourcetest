# CityInfo API - Class Diagram

```mermaid
classDiagram
    %% Entities
    class City {
        +int Id
        +string Name
        +string? Description
        +ICollection~PointOfInterest~ PointsOfInterest
        +City(string name)
    }

    class PointOfInterest {
        +int Id
        +string Name
        +string Description
        +City? City
        +int CityId
        +PointOfInterest(string name)
    }

    %% DTOs
    class CityDto {
        +int Id
        +string Name
        +string? Description
        +int NumberOfPointsOfInterest
        +ICollection~PointOfInterestDto~ PointsOfInterest
    }

    class CityWithoutPointsOfInterestDto {
        +int Id
        +string Name
        +string? Description
    }

    class PointOfInterestDto {
        +int Id
        +string Name
        +string? Description
    }

    class PointOfInterestForCreationDto {
        +string Name
        +string? Description
    }

    class PointOfInterestForUpdateDto {
        +string Name
        +string? Description
    }

    %% Controllers
    class CitiesController {
        -ICityInfoRepository _cityInfoRepository
        -IMapper _mapper
        -int maxCitiesPageSize
        +CitiesController(ICityInfoRepository, IMapper)
        +GetCities(string?, string?, int, int) ActionResult~IEnumerable~CityWithoutPointsOfInterestDto~~
        +GetCity(int, bool) IActionResult
    }

    class PointsOfInterestController {
        -ILogger~PointsOfInterestController~ _logger
        -IMailService _mailService
        -ICityInfoRepository _cityInfoRepository
        -IMapper _mapper
        +PointsOfInterestController(ILogger, IMailService, ICityInfoRepository, IMapper)
        +GetPointsOfInterest(int) ActionResult~IEnumerable~PointOfInterestDto~~
        +GetPointOfInterest(int, int) ActionResult~PointOfInterestDto~
        +CreatePointOfInterest(int, PointOfInterestForCreationDto) ActionResult~PointOfInterestDto~
        +UpdatePointOfInterest(int, int, PointOfInterestForUpdateDto) ActionResult
        +PartiallyUpdatePointOfInterest(int, int, JsonPatchDocument) ActionResult
        +DeletePointOfInterest(int, int) ActionResult
    }

    class AuthenticationController {
        +AuthenticationController(IConfiguration)
        +Authenticate(AuthenticationRequestBody) ActionResult
    }

    %% Services and Repositories
    class ICityInfoRepository {
        <<interface>>
        +GetCitiesAsync() Task~IEnumerable~City~~
        +GetCitiesAsync(string?, string?, int, int) Task~(IEnumerable~City~, PaginationMetadata)~
        +GetCityAsync(int, bool) Task~City?~
        +CityExistsAsync(int) Task~bool~
        +GetPointsOfInterestForCityAsync(int) Task~IEnumerable~PointOfInterest~~
        +GetPointOfInterestForCityAsync(int, int) Task~PointOfInterest?~
        +AddPointOfInterestForCityAsync(int, PointOfInterest) Task
        +DeletePointOfInterest(PointOfInterest) void
        +CityNameMatchesCityId(string?, int) Task~bool~
        +SaveChangesAsync() Task~bool~
    }

    class CityInfoRepository {
        -CityInfoContext _context
        +CityInfoRepository(CityInfoContext)
        +GetCitiesAsync() Task~IEnumerable~City~~
        +GetCitiesAsync(string?, string?, int, int) Task~(IEnumerable~City~, PaginationMetadata)~
        +GetCityAsync(int, bool) Task~City?~
        +CityExistsAsync(int) Task~bool~
        +GetPointsOfInterestForCityAsync(int) Task~IEnumerable~PointOfInterest~~
        +GetPointOfInterestForCityAsync(int, int) Task~PointOfInterest?~
        +AddPointOfInterestForCityAsync(int, PointOfInterest) Task
        +DeletePointOfInterest(PointOfInterest) void
        +CityNameMatchesCityId(string?, int) Task~bool~
        +SaveChangesAsync() Task~bool~
    }

    class IMailService {
        <<interface>>
        +Send(string, string) void
    }

    class LocalMailService {
        -string _mailTo
        -string _mailFrom
        +LocalMailService(IConfiguration)
        +Send(string, string) void
    }

    class CloudMailService {
        -string _mailTo
        -string _mailFrom
        +CloudMailService(IConfiguration)
        +Send(string, string) void
    }

    %% DbContext
    class CityInfoContext {
        +DbSet~City~ Cities
        +DbSet~PointOfInterest~ PointsOfInterest
        +CityInfoContext(DbContextOptions~CityInfoContext~)
        #OnModelCreating(ModelBuilder) void
    }

    %% Utility Classes
    class PaginationMetadata {
        +int TotalItemCount
        +int TotalPageCount
        +int PageSize
        +int CurrentPage
        +PaginationMetadata(int, int, int)
    }

    %% AutoMapper Profiles
    class CityProfile {
        +CityProfile()
    }

    class PointOfInterestProfile {
        +PointOfInterestProfile()
    }

    %% Relationships
    City ||--o{ PointOfInterest : "has many"
    CityDto ||--o{ PointOfInterestDto : "contains"
    
    CitiesController --> ICityInfoRepository : "uses"
    CitiesController --> IMapper : "uses"
    
    PointsOfInterestController --> ICityInfoRepository : "uses"
    PointsOfInterestController --> IMailService : "uses"
    PointsOfInterestController --> IMapper : "uses"
    
    CityInfoRepository ..|> ICityInfoRepository : "implements"
    CityInfoRepository --> CityInfoContext : "uses"
    
    LocalMailService ..|> IMailService : "implements"
    CloudMailService ..|> IMailService : "implements"
    
    CityInfoContext --> City : "manages"
    CityInfoContext --> PointOfInterest : "manages"
    
    %% Inheritance
    CitiesController --|> ControllerBase : "extends"
    PointsOfInterestController --|> ControllerBase : "extends"
    AuthenticationController --|> ControllerBase : "extends"
    CityInfoContext --|> DbContext : "extends"
```

## Architecture Overview

### **Layers:**

1. **Controllers Layer**
   - `CitiesController` - Handles city-related HTTP requests
   - `PointsOfInterestController` - Handles points of interest HTTP requests
   - `AuthenticationController` - Handles authentication

2. **Services Layer**
   - `ICityInfoRepository` / `CityInfoRepository` - Data access abstraction
   - `IMailService` / `LocalMailService` / `CloudMailService` - Email services

3. **Data Layer**
   - `CityInfoContext` - Entity Framework DbContext
   - `City` / `PointOfInterest` - Domain entities

4. **DTOs (Data Transfer Objects)**
   - Various DTOs for API communication
   - Separate DTOs for creation, update, and read operations

5. **AutoMapper Profiles**
   - `CityProfile` / `PointOfInterestProfile` - Object mapping configuration

### **Key Design Patterns:**

- **Repository Pattern**: `ICityInfoRepository` abstracts data access
- **Dependency Injection**: Controllers depend on abstractions
- **DTO Pattern**: Separate models for API and domain
- **Service Layer**: Business logic separated from controllers
- **Entity Framework Code First**: Database schema from entities

### **Key Features:**

- RESTful API design
- Entity relationships (City has many PointsOfInterest)
- Pagination support
- Authentication and authorization
- Email notifications
- AutoMapper for object mapping
- Comprehensive CRUD operations
# Equipment Rental System

Console application for managing university equipment rentals

## How to run

```
dotnet run
```

## Project structure

```
Models/          — domain classes (Equipment, User, Rental and their subtypes)
Services/        — business logic (RentalService, EquipmentService, UserService, ReportService)
Enums/           — EquipmentStatus, UserType
Program.cs       — console interface
```

## Design decisions

### Class responsibilities

Each service handles one area: `RentalService` manages renting and returning, `EquipmentService` manages the equipment list and availability, `UserService` manages users, `ReportService` generates summaries. `Program.cs` only handles input/output and delegates to services.

### Inheritance

`Equipment` is abstract because all equipment shares an ID, name, and status, but each type (Laptop, Projector, Camera) has its own specific fields. `User` is abstract with `Student` and `Employee` subclasses, where the difference is the rental limit defined via `MaxActiveRentals`.

### Cohesion and coupling

Services depend on models but not on each other, except for `ReportService` which reads from other services. Penalty calculation is behind an `IPenaltyCalculator` interface so the rule can be changed without modifying `RentalService` or `Rental`. Rental limits are defined in user subclasses and enforced in one place (`RentalService.RentEquipment`).

### Error handling

Operations that can fail will throw exceptions with descriptive messages. Next the console layer displays them.
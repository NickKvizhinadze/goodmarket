# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build entire solution
dotnet build GoodMarket.sln

# Run a service
dotnet run --project Identity/src/GoodMarket.Identity.Api/GoodMarket.Identity.Api.csproj

# Run all tests
dotnet test GoodMarket.sln

# Run tests for a specific service
dotnet test Identity/tests/GoodMarket.Identity.Integration/GoodMarket.Identity.Integration.csproj

# Scaffold a new microservice (PowerShell)
.\Create-Microservice.ps1 -MicroserviceName <Name>
```

## Architecture

See `docs/` for full planning documentation. Key files: `docs/architecture.md`, `docs/decisions.md`, `docs/microservices.md`.

### Solution Structure

```
GoodMarket.sln
├── Shared/src/
│   ├── GoodMarket.Shared.Mediator   # Custom CQRS mediator
│   └── GoodMarket.Shared.Result     # Result/Error types
├── Identity/                        # Only service built so far
│   ├── src/GoodMarket.Identity.Api
│   └── tests/GoodMarket.Identity.Integration
└── docs/                            # Architecture decisions and planning
```

Each microservice follows: `Api` → `Application` → `Domain` → `Infrastructure` → `SharedKernel` + `Integration` tests. Use the scaffold script to create new services — do not create them manually.

### Custom Mediator (`GoodMarket.Shared.Mediator`)

No MediatR — this project uses its own mediator. Handlers are auto-registered by scanning the assembly passed to `AddMediator()`.

```csharp
// Command — use when the operation mutates state
public class MyCommand : ICommand<Result<MyDto>> { }
public class MyCommandHandler : ICommandHandler<MyCommand, Result<MyDto>>
{
    public Task<Result<MyDto>> HandleAsync(MyCommand command, CancellationToken ct) { }
}

// Query — use when the operation is read-only
public class MyQuery : IQuery<Result<MyDto>> { }
public class MyQueryHandler : IQueryHandler<MyQuery, Result<MyDto>>
{
    public Task<Result<MyDto>> HandleAsync(MyQuery query, CancellationToken ct) { }
}

// Dispatching from an endpoint
var result = await mediator.SendCommandAsync<MyCommand, Result<MyDto>>(command, ct);
var result = await mediator.SendQueryAsync<MyQuery, Result<MyDto>>(query, ct);
```

Pipeline behaviors (`IPipelineBehavior<TInput, TOutput>`) wrap handlers in registration order. `ValidationBehavior` is already registered globally.

### Result Type (`GoodMarket.Shared.Result`)

All handlers return `Result` or `Result<T>`. Never throw exceptions for business logic — use the result type.

```csharp
// Returning from a handler
return Result.SuccessResult();                          // 204
return Result<MyDto>.SuccessResult().WithData(dto);     // 200
return Result.BadRequestResult().WithErrors(errors);    // 400
return Result.EntityNotFoundResult();                   // 404
return Result.Unauthorized();                           // 401

// T can be implicitly converted
Result<MyDto> result = myDto;   // wraps as Ok

// Convert to IResult in an endpoint
return result.CustomResult();   // maps ResultCode → HTTP status automatically
```

### Validation

Add a `FluentValidation` validator for any command/query — it is picked up automatically and runs before the handler via `ValidationBehavior`. Validation failures return `Result.BadRequestResult()` with errors; the handler is never called.

```csharp
public class MyCommandValidator : AbstractValidator<MyCommand>
{
    public MyCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```

### Endpoints (Minimal API)

Each endpoint is a static class with a `Map*` extension method on `IEndpointRouteBuilder`. All endpoints for a feature are grouped in an `*EndpointsExtensions` file and registered via the top-level `EndpointsExtensions`. Use `IApiMarker` as the assembly anchor.

```csharp
public static class MyEndpoint
{
    public static IEndpointRouteBuilder MapMyEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Something.Create, async (IMediator mediator, MyRequest req, CancellationToken ct) =>
        {
            var command = new MyCommand(req.Field);
            var result = await mediator.SendCommandAsync<MyCommand, Result<MyDto>>(command, ct);
            return result.CustomResult();
        })
        .WithName("EndpointName")
        .Produces<MyDto>(StatusCodes.Status201Created)
        .Produces<IEnumerable<Error>>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        return app;
    }
}
```

### Service Registration Pattern

All DI setup lives in `Shared/Extensions/ServiceCollectionExtensions.cs` and `WebApplicationExtensions.cs` as extension methods, chained in `Program.cs`. Add new services there — do not register things directly in `Program.cs`.

### API Versioning

Version is read from query string or `X-Api-Version` header. Default version is `v2`. Route groups must declare supported versions via `.HasApiVersion(...)`.

### OpenAPI

Scalar UI is used (not Swagger UI). Available at the Scalar endpoint in development. Bearer security scheme is pre-configured via `BearerSecuritySchemeTransformer`.

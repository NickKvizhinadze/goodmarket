# Sprint 4 — Media Service + Catalog Domain Model

**Roadmap phase:** Phase 2 + Phase 3 (start)
**Depends on:** Sprint 3 complete (Identity fully done, RabbitMQ wired)
**Blocker:** ADR-004 — file storage provider must be decided before Sprint 4 begins (Azure Blob / AWS S3 / MinIO)

## Goal

A working Media service that can upload and delete files and return public URLs, plus a fully defined Catalog domain model with EF Core entities and migrations ready for use case implementation in Sprint 5.

## Tasks

- [ ] Resolve ADR-004: decide file storage provider (Azure Blob with Azurite, AWS S3 with MinIO, or self-hosted MinIO) and document the decision in `docs/decisions.md`
- [ ] `.\Create-Microservice.ps1 -MicroserviceName Media`
- [ ] Add chosen storage SDK NuGet package to `GoodMarket.Media.Api` (e.g. `Azure.Storage.Blobs` for Azurite, or `AWSSDK.S3` for MinIO)
- [ ] Add storage emulator resource to `GoodMarket.AppHost` (Azurite or MinIO container) per ADR-004 decision
- [ ] Implement `IFileStorageService` interface and concrete implementation for the chosen provider
- [ ] Add `MediaFile` domain entity: `Id`, `FileName`, `PublicUrl`, `ContentType`, `SizeBytes`, `UploadedAt`
- [ ] Add EF Core configuration for `MediaFile`, create `goodmarket_media` database, generate migration
- [ ] Implement `UploadFileCommand` and `UploadFileCommandHandler` — store binary to file storage, persist metadata to DB, return public URL
- [ ] Implement `DeleteFileCommand` and `DeleteFileCommandHandler` — delete from storage and DB
- [ ] Implement `POST /api/v1/media/upload` endpoint (multipart/form-data, requires admin role)
- [ ] Implement `DELETE /api/v1/media/{id}` endpoint (requires admin role)
- [ ] Add `GoodMarket.Media.Api` to AppHost, wire to `postgres` and storage resource
- [ ] Add Gateway route: `/api/media/{**catch-all}` → Media service
- [ ] Scaffold Catalog service: `.\Create-Microservice.ps1 -MicroserviceName Catalog`
- [ ] Add all Catalog domain entities to `GoodMarket.Catalog.Api/Domain/`:
  - `Category`, `CategoryLanguage`
  - `Brand`, `BrandLanguage`, `BrandCategory`
  - `SpecificationType`, `SpecificationTypeLanguage`
  - `Specification`, `SpecificationLanguage`, `SpecificationCategory`
  - `Product`, `ProductLanguage`, `ProductSpecification`
  - `Inventory`
  - `City`, `CityLanguage`
- [ ] Add EF Core configurations for all Catalog entities
- [ ] Create `goodmarket_catalog` DB and generate initial migration
- [ ] Add `GoodMarket.Catalog.Api` to AppHost (no endpoints yet — to be wired in Sprint 5)

## Definition of Done

- [ ] `POST /api/media/upload` accepts an image file and returns a public URL
- [ ] `DELETE /api/media/{id}` removes the file from storage and DB
- [ ] Media service reachable through Gateway at `/api/media/...`
- [ ] Catalog domain entities compile with correct EF Core configurations
- [ ] Catalog migration applies cleanly to `goodmarket_catalog` database
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)

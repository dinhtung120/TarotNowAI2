# Cấu Trúc Thư Mục Hệ Thống TarotNow

Dưới đây là cây thư mục (tham chiếu nhanh) đại diện cho kiến trúc Clean Architecture của Backend, App Router của Frontend Next.js và Mobile App.

```text
.
|-- Backend
|   |-- src
|   |   |-- TarotNow.Api
|   |   |   |-- Controllers
|   |   |   |-- Extensions
|   |   |   |-- Filters
|   |   |   |-- Middlewares
|   |   |-- TarotNow.Application
|   |   |   |-- Behaviors
|   |   |   |-- Contracts
|   |   |   |-- DTOs
|   |   |   |-- Exceptions
|   |   |   |-- Features
|   |   |   |-- Mappings
|   |   |   |-- Messaging
|   |   |   |-- Validators
|   |   |-- TarotNow.Domain
|   |   |   |-- Entities
|   |   |   |-- Enums
|   |   |   |-- Events
|   |   |   |-- Exceptions
|   |   |   |-- Interfaces
|   |   |   |-- ValueObjects
|   |   |-- TarotNow.Infrastructure
|   |   |   |-- Authentication
|   |   |   |-- BackgroundJobs
|   |   |   |-- Caching
|   |   |   |-- Persistence
|   |   |   |   |-- Configurations
|   |   |   |   |-- Repositories
|   |   |   |-- Services
|   |-- tests
|   |   |-- TarotNow.Api.IntegrationTests
|   |   |-- TarotNow.Application.UnitTests
|   |   |-- TarotNow.Domain.UnitTests
|   |   |-- TarotNow.Infrastructure.IntegrationTests
|-- Frontend
|   |-- public
|   |-- src
|   |   |-- app
|   |   |   |-- (auth)
|   |   |   |-- (dashboard)
|   |   |   |-- api
|   |   |-- components
|   |   |   |-- layouts
|   |   |   |-- shared
|   |   |   |-- ui
|   |   |-- hooks
|   |   |-- lib
|   |   |-- services
|   |   |-- store
|   |   |-- types
|   |   |-- utils
|-- Mobile
|   |-- src
|   |   |-- assets
|   |   |-- components
|   |   |   |-- shared
|   |   |   |-- ui
|   |   |-- constants
|   |   |-- hooks
|   |   |-- navigation
|   |   |-- screens
|   |   |-- services
|   |   |   |-- api
|   |   |-- store
|   |   |-- theme
|   |   |-- utils
```

---
trigger: always_on
---

Global Antigravity Rules – TarotNow Project

1. Strictly follow Clean Architecture: Domain → Application → Infrastructure → Presentation.  
   Outer layers may depend on inner layers; inner layers never depend on outer ones.

2. Domain layer: contains only entities, value objects, enums, domain events, and business exceptions.  
   No references to any external libraries or frameworks.

3. Application layer: use MediatR CQRS pattern (Commands for writes, Queries for reads),  
   FluentValidation for input validation, AutoMapper for object mapping.

4. Infrastructure layer: EF Core + PostgreSQL for write model, MongoDB.Driver for document storage,  
   Redis for caching and rate limiting, adapter pattern for external services.

5. API layer: keep controllers thin, return ProblemDetails for errors,  
   use Serilog structured logging + OpenTelemetry for distributed tracing.

6. Finance / escrow / wallet / quota logic: use ACID transactions, enforce invariants (balance ≥ 0, no double-spend),  
   require idempotency keys, consume entitlements atomically (earliest-expiry-first rule).

7. AI calls: atomically reserve quota before calling the provider,  
   implement controlled retry + timeout + auto-refund exactly as specified.
s
8. Authentication & Security: short-lived JWT + refresh token rotation,  
   HttpOnly + Secure + SameSite cookies (web only), policy-based authorization + ownership checks,  
   rate limiting on sensitive endpoints.

9. Frontend (Next.js App Router): use Server Components for SEO/static content,  
   Client Components for interactivity, TanStack Query for server state,  
   Zustand only for local UI state, React Hook Form + Zod for forms.

10. General code style:
    - Methods < 50 lines, classes focused on single responsibility
    - Prefer guard clauses over deep nesting
    - Add XML comments (///) to all public members
    - No magic strings/numbers → use constants or configuration

11. i18n: support VI/EN/ZH with fallback chain (locale → en)

These rules have the highest priority. Always prioritize clean, testable, and maintainable code.
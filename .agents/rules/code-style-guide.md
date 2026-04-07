---
trigger: always_on
---

Global Antigravity Rules – TarotNow Project

1. Strictly follow Clean Architecture
   Domain → Application → Infrastructure → Presentation.
   Outer layers may depend on inner layers; inner layers never depend on outer ones.

2. Domain Layer
   Contains only entities, value objects, enums, domain events, and business exceptions.
   No references to any external libraries or frameworks.

3. Application Layer
   Use MediatR CQRS pattern (Commands for writes, Queries for reads).
   Use FluentValidation for input validation and AutoMapper for object mapping.

4. Infrastructure Layer
   EF Core + PostgreSQL for write model, MongoDB.Driver for document storage,
   Redis for caching and rate limiting, adapter pattern for external services.

5. API Layer
   Keep controllers thin. Return ProblemDetails for errors.
   Use Serilog structured logging + OpenTelemetry for distributed tracing.

6. Finance / Escrow / Wallet / Quota Logic
   Use ACID transactions, enforce invariants (balance ≥ 0, no double-spend),
   require idempotency keys, consume entitlements atomically (earliest-expiry-first rule).

7. AI Calls
   Atomically reserve quota before calling the provider.
   Implement controlled retry + timeout + auto-refund exactly as specified.

8. Authentication & Security
   Short-lived JWT + refresh token rotation.
   HttpOnly + Secure + SameSite cookies (web only).
   Policy-based authorization + ownership checks.
   Rate limiting on sensitive endpoints.

9. Frontend (Next.js App Router)
   Use Server Components for SEO/static content.
   Use Client Components for interactivity.
   TanStack Query for server state, Zustand only for local UI state.
   React Hook Form + Zod for forms.

10. General Code Style
    - Methods must be < 50 lines.
    - Classes must follow single responsibility principle.
    - Prefer guard clauses over deep nesting.
    - Add XML comments (///) to all public members.
    - No magic strings/numbers → use constants or configuration.

11. Internationalization (i18n)
    Support VI/EN/ZH with fallback chain (locale → vi).

12. Clearly Separate Custom Hooks
    All complex logic must be extracted into dedicated custom hooks
    (e.g. useAuth, useFormValidation, useDebounce, useInfiniteScroll, etc.).
    Never put state, effects, or data-fetching logic directly inside components.

13. Reusable / Shared Hooks
    All reusable hooks must be placed in the shared/ folder and properly exported
    so they can be used across multiple components.
    Do not duplicate logic between components.

14. Component Separation (Single Responsibility Principle)
    Each component must have only one responsibility.
    Large components (> 60 lines) must be broken down into smaller sub-components.
    Components should only handle UI rendering; all business logic must stay in custom hooks.

15. Component File Size Limit
    No component file may exceed 120 lines of code (ideally keep under 100 lines).
    If a file grows larger, it must be immediately split into smaller components.

16. Mandatory Best Practices
    - Use TypeScript 100% with clear types and interfaces.
    - Apply React.memo, useCallback, and useMemo wherever necessary to prevent unnecessary re-renders.
    - Avoid deep prop drilling — use Context or state management (Zustand / TanStack Query) instead.
    - All components must be responsive (Tailwind).
    - Accessibility: include proper aria-* attributes and button types where needed.
    - Naming conventions: Components = PascalCase, Hooks = use* prefix, file names must match component names.

17. Code Style & Clean Code Rules
    - Each component file must have only one default export.
    - No any type allowed. All ESLint warnings must be fixed.
    - Use early returns and avoid deeply nested conditionals.
    - Keep comments short and only for complex logic.

18. TAILWIND BEST PRACTICES (Anti-Duplication Rules)
    - Always use the cn() helper:
      Every className must be written using the cn() utility function (clsx + tailwind-merge).
      Create file src/lib/utils.ts with the following content and always import from there:

      import { type ClassValue, clsx } from "clsx";
      import { twMerge } from "tailwind-merge";

      export function cn(...inputs: ClassValue[]) {
        return twMerge(clsx(inputs));
      }

    - Never write long class strings directly in components.
    - Any repeated or long class strings must be extracted into reusable UI components in src/ui/
      (e.g. Button, Card, Input, Badge) or variant-based components.
    - Prefer Component Composition over repeating classes.
    - Components with multiple styles must use variant + size props.
    - Always install and use prettier-plugin-tailwindcss for automatic class sorting.
    - Avoid arbitrary values [ ] whenever possible. Prefer extending the theme in tailwind.config.js.
    - @apply is only allowed in CSS files (never in JSX).
    - Responsive and state variants (md:, lg:, hover:, focus:, dark:) must live only in UI components.
    - Before committing, scan and refactor any class string that appears 3 or more times.

These rules have the highest priority.
Always prioritize clean, testable, and maintainable code.
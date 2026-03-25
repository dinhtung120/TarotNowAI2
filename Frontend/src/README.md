# Frontend Source Architecture

This directory follows a feature-first Clean Architecture layout for Next.js App Router.

## Folder Intent

- `app/`: Thin route wrappers only. Compose feature hooks/components, no business logic.
- `features/*/domain`: Pure business rules/types/schemas. No React, no side effects.
- `features/*/application`: Use-cases and hooks that orchestrate behavior.
- `features/*/presentation`: UI components/pages for one feature.
- `features/*/public.ts`: Feature public API surface for imports from outside the feature.
- `shared/`: Cross-feature utilities, primitives, infrastructure adapters.
- `i18n/`: Locale routing and message loading/merge.

## Dependency Rules

- `domain` must not import from `application`, `presentation`, `app`, or browser/server APIs.
- `application` can import `domain` + `shared`, but should not import from `app`.
- `presentation` can import `application` and `domain`, but never call `fetch` directly.
- `app/**/page.tsx` can import from `features/*/public` and `shared/**` only.
- `infrastructure` is the only layer that should know HTTP/cookie/storage details.

## Import Guidelines

- Prefer `@/features/<feature>/public` as the entry point between features.
- Keep intra-feature imports local (avoid jumping between unrelated feature folders).
- Add/maintain `public.ts` when exposing new stable APIs from a feature.
- Avoid deep imports into another feature's internal files.


# Global Antigravity Rules – TarotNow Project

These rules guide all work in TarotNowAI2. Apply them with the current codebase, architecture tests, and enforcement scripts as the source of truth.

## Rule precedence

1. Explicit user instruction for the current task.
2. Architecture tests, lint rules, build gates, and enforcement scripts.
3. Rule 0 event-driven architecture intent.
4. The remaining rules in this file.
5. Existing nearby code patterns.

If a rule conflicts with an enforcement guard, follow the guard and report that this file needs updating. If no guard exists and the rule conflicts with nearby code, ask before introducing a new pattern. Do not perform broad refactors just to satisfy a rule while fixing a small, unrelated issue; report unrelated violations instead.

## 0. Mandatory Event-Driven Architecture

This repository uses a two-stage event-driven command model.

### 0.1 Command entry handlers

- `IRequestHandler<Command, TResult>` command entry handlers in Application must stay thin.
- Command entry handlers must dispatch a requested domain event through `IInlineDomainEventDispatcher` and return the event result.
- Command entry handlers must not inject repositories, infrastructure services, notification services, wallet services, email services, realtime broadcasters, leaderboard/gacha/achievement/task services, or other side-effect components.
- This matches `EventDrivenArchitectureRulesTests.CommandHandlers_ShouldOnlyDependOnInlineDomainEventDispatcher`.

### 0.2 Requested domain event handlers

- `*RequestedDomainEventHandler` classes in Application own use-case orchestration.
- Requested domain events and handlers must follow the existing pattern used by nearby commands. Deviations require an architecture-test allowlist or explicit user approval.
- They may call Application-layer abstractions such as repository interfaces, provider interfaces, cache interfaces, transaction coordinators, pricing/domain services, and domain event publishers.
- Application-facing contracts must live in Application namespaces such as `TarotNow.Application.Interfaces` or `TarotNow.Application.Common`; do not define new Application-facing contracts in Infrastructure.
- They must not depend on concrete Infrastructure implementations or framework-specific infrastructure namespaces.
- Primary state changes required to complete the command may happen inside the requested domain event handler transaction.
- External provider calls that are the primary purpose of the command, such as AI streaming, may happen here after required reservation/freeze steps.
- Secondary side effects must be published as domain events/outbox messages and handled by event handlers or Infrastructure workers.

### 0.3 Side effects

- Notifications, emails, realtime broadcasts, leaderboard updates, gacha results, achievements, title/exp card updates, tasks, telemetry fan-out, and other secondary effects must live in event handlers or Infrastructure workers.
- Controllers and command entry handlers must never perform these side effects directly.
- Any direct controller/API realtime broadcast that bypasses the event/bridge path is a critical issue unless an architecture test or explicit allowlist permits it.
- Exceptions must be documented in the relevant architecture test allowlist with rationale; undocumented exceptions are non-compliant.

### 0.4 Chat scope

- Chat messaging/realtime transport may keep bounded exceptions only where the current architecture tests and established patterns allow them.
- If unsure whether a chat exception is allowed, check architecture tests first; do not add new direct SignalR/controller/hub broadcast paths.
- Finance, escrow, wallet, quota, and entitlement behavior inside chat is not exempt: it must still use transactions, idempotency, and event/outbox-based side effects.

## 1. Clean Architecture Boundaries

- Layer ownership from core to edge is Domain → Application → Infrastructure → Presentation/API.
- Compile-time dependencies point inward: Presentation/API may depend on Application/Infrastructure composition, Infrastructure may depend on Application/Domain, Application may depend on Domain, and Domain depends on no outer layer.
- Domain must not depend on Application, Infrastructure, API, persistence frameworks, web frameworks, Redis, MongoDB, EF Core, or provider SDKs.
- Application may depend on Domain and Application-owned interfaces/contracts only.
- Application must not reference concrete Infrastructure namespaces or framework-specific infrastructure packages such as EF Core, MongoDB.Driver, Npgsql, StackExchange.Redis, or ASP.NET Core.
- Infrastructure implements Application interfaces for persistence, cache, external providers, outbox, event dispatch, Redis Pub/Sub, and background workers.
- API/Presentation must call Application entry points such as MediatR handlers or approved Application services; it must not call repositories, DbContexts, provider concretes, or realtime broadcasters directly unless allowlisted by architecture tests.

## 2. Domain Layer

- Domain contains entities, value objects, enums, domain event contracts, and business invariants.
- Domain events must be framework-agnostic domain contracts; MediatR is delivery/orchestration plumbing outside Domain.
- Do not add persistence annotations or infrastructure concerns to Domain models.
- Domain should expose business invariant failures through the project’s current error model; do not reintroduce legacy exception models forbidden by architecture tests.

## 3. Application Layer

- Use MediatR CQRS for entry points: Commands for writes, Queries for reads.
- Command entry handlers must follow Rule 0.1.
- Requested domain event handlers may orchestrate use cases with Application abstractions as described in Rule 0.2.
- Queries may read through repository/query abstractions but must not trigger side effects.
- Use FluentValidation for input validation where the project already uses validators.
- Use the existing mapping style. Prefer AutoMapper for repeated or complex mappings; simple one-off projections may be explicit when clearer.

## 4. Infrastructure Layer

- EF Core + PostgreSQL are used for write-model persistence where applicable.
- MongoDB.Driver is used for document storage where applicable.
- Redis is used for caching, rate limiting, Pub/Sub, and coordination where applicable.
- External services must be wrapped behind Application-owned interfaces/adapters.
- Outbox Pattern, Redis Pub/Sub bridges, idempotency services, event dispatch, and background workers belong in Infrastructure.

## 5. Finance / Escrow / Wallet / Quota Logic

- Use ACID transactions for money, escrow, quota, and entitlement state changes.
- Enforce invariants such as balance ≥ 0, no double-spend, and no duplicate settlement.
- Any command that mutates money/quota/entitlement or calls AI providers must carry a deterministic idempotency key at command and requested-event levels unless an architecture-test allowlist explicitly permits otherwise.
- Consume entitlements atomically using the earliest-expiry-first rule where applicable.
- Wallet/money state changes must publish exactly one canonical post-commit money event: `MoneyChangedDomainEvent` unless existing code in the same bounded context already uses an approved more-specific money event.
- Do not introduce a new equivalent money event without checking existing domain events first.
- Never call notification/realtime/gamification/task side effects directly from finance logic.
- For new or modified finance flows, verify and be able to state the idempotency key path, transaction boundary, and settlement/refund decision points.

## 6. AI Calls

- Reserve or freeze quota/wallet cost atomically before calling the provider.
- Use idempotency keys for reserve/freeze, consume/settle, and refund/compensation operations.
- Provider calls must use controlled timeout and retry boundaries from existing configuration or runtime policy; do not invent new retry behavior without checking current settings.
- If no existing timeout/retry policy exists, stop and ask before adding one.
- Final status must decide settlement vs refund/compensation.
- After AI state changes, publish the relevant domain event/outbox message; do not call notification, quota side effects, telemetry fan-out, or realtime services directly.

## 7. Authentication & Security

- Use short-lived JWT and refresh token rotation where applicable.
- Web cookies must be HttpOnly, Secure, and SameSite according to the project’s auth flow.
- Use policy-based authorization and ownership checks.
- Apply rate limiting on sensitive endpoints.
- Do not introduce command injection, XSS, SQL/NoSQL injection, SSRF, unsafe deserialization, token leakage, or secret logging.

## 8. Frontend App Router

- `Frontend/src/app/**/page.tsx` and `layout.tsx` files must stay thin composition wrappers.
- App route files should import feature entry points through established public exports such as `@/features/*/public` when available.
- Keep business/data orchestration outside app routes; move complex UI behavior into feature components/hooks.
- Use Server Components for SEO/static content when the route does not require browser-only APIs, client state, realtime subscriptions, or client-only libraries.
- Use Client Components for interactivity, browser APIs, realtime UI, local state, and client-only libraries.
- Use TanStack Query for server state and Zustand only for local UI state.
- Use React Hook Form + Zod for forms with multi-step flows, async validation, or roughly 5+ fields; simpler forms may use existing local patterns.

## 9. Internationalization

- Support VI/EN/ZH with fallback chain `locale → vi`.
- New user-facing frontend text must use the existing localization approach instead of hard-coded strings, except temporary diagnostics/tests.
- Do not migrate unrelated existing hard-coded copy during small fixes.

## 10. Custom Hooks

- Extract complex or reused state, effects, data fetching, subscriptions, debounce/throttle, infinite scroll, form orchestration, and browser integration into dedicated custom hooks.
- Trivial local UI state may stay inside a component.
- Do not extract or relocate hooks during small-scope fixes unless the logic is reused in 2+ current call sites or a guard/test failure requires it.
- Reusable hooks belong in the existing shared area and must follow existing export/barrel patterns.

## 11. Component Separation and Size

- Components should have one primary responsibility and primarily render UI.
- Business/data orchestration belongs in hooks or application layers.
- TSX size policy is enforced by `Frontend/scripts/check-component-size.mjs`: hard fail above 120 lines; above 100 lines is disallowed unless present in `Frontend/scripts/component-size-baseline.json`.
- Split components by responsibility, not just to satisfy line counts. Avoid artificial wrapper churn; prefer cohesive extraction of real subcomponents/hooks.
- If a touched file already exceeds a size/line budget, first attempt a minimal compliant delta. Do not split unrelated existing oversized components unless the guard fails on your change or the user asked for refactoring.

## 12. Frontend TypeScript and React Practices

- Use TypeScript with clear types and interfaces.
- Avoid `any`; fix ESLint warnings introduced by your change.
- Use `React.memo`, `useMemo`, and `useCallback` only when one of these is true: prop identity stability for memoized children, expensive computation, dependency stability for effects/queries, or profiler/measured rerender cost.
- Do not add memoization preemptively.
- Avoid deep prop drilling; use composition, Context, Zustand, or TanStack Query where appropriate.
- Minimum accessibility for touched interactive UI: explicit `button type`, accessible name for icon-only controls, `aria-invalid` plus error association for validated fields, and visible keyboard focus.
- Naming conventions: Components = PascalCase, Hooks = `use*`, and component file names should match the component when practical.
- Each component file should have at most one default export.

## 13. Tailwind Best Practices

- Use the project’s canonical `cn` utility from `Frontend/src/lib/utils.ts` through the configured alias, currently `@/lib/utils`.
- Use `cn()` for conditional, merged, long, repeated, or multi-class `className` values. Simple static one-token className may stay inline if consistent with nearby code.
- Long, repeated, or variant-heavy class sets should be extracted into reusable UI components or variant-based components.
- One-off layout/responsive classes are allowed in page/components when extracting them would add unnecessary indirection.
- Prefer component composition over repeated class blocks.
- Components with multiple visual styles should use variant and size props.
- Use prettier-plugin-tailwindcss if it is already configured in the project.
- Avoid arbitrary Tailwind values (`[]`) when a theme token or existing utility is appropriate.
- `@apply` is only allowed in CSS files, never in JSX/TSX.
- Before committing significant UI work, scan touched files for identical long class strings repeated 3 or more times. Refactor only when extraction improves readability within the current scope. Significant UI work means touching 3+ components or introducing a reusable component family.

## 14. Backend Code Style

- Backend methods must stay under the architecture test method line budget enforced by `Backend/tests/TarotNow.ArchitectureTests/CodeQualityRulesTests.cs` (< 50 logical lines at the time of writing).
- Backend source files must stay within the architecture test file line budget enforced by `CodeQualityRulesTests.cs`.
- Prefer guard clauses over deep nesting.
- For code changes, add or update XML comments only for new or modified public APIs and non-obvious contracts in the touched scope. Public API surface means controllers, externally consumed contracts, shared libraries, and non-obvious public members — not every public DTO property.
- Comments should explain non-obvious invariants or why a surprising choice exists; do not add line-by-line narration that restates code flow.
- Avoid magic strings/numbers for repeated business values, policy values, event names, and configuration; localized copy, test literals, one-off UI labels, and route templates may remain inline when clearer.

## 15. Verification Matrix

Use the smallest relevant verification first, then broaden if needed.

- Backend command/event/finance/AI/realtime boundary changes: run `TarotNow.ArchitectureTests`, affected unit/integration tests, and verify idempotency + outbox/event path.
- Backend business logic changes outside boundaries: run targeted unit tests and architecture tests if dependencies or file/method size changed.
- Frontend TSX/UI changes: run relevant lint/typecheck/test/build gate available in `Frontend/package.json`; run component-size guard if TSX files changed.
- Frontend route/import-boundary changes: run the clean-architecture frontend guard if available.
- Documentation/rule-only edits: inspect diff and run no production test sweep unless requested or the edit changes executable examples/scripts.
- UI behavior changes: test the affected browser flow when feasible; if not feasible, say so explicitly.

## 16. Execution Discipline

- Prefer minimal, surgical changes that directly serve the request.
- When an enforcement guard exists, follow the guard; when no guard exists, apply judgement and avoid broad refactors.
- If you discover an unrelated rule violation, mention it instead of refactoring it unless the user asked for cleanup.
- `where applicable` means the change directly modifies that concern, such as money/quota/auth/i18n/security/realtime.
- Do not commit, push, deploy, or modify shared infrastructure unless the user explicitly requests it.
- Before committing, inspect git status/diff and avoid staging secrets or local-only config.
- Documentation/rule-only edits should not include production code changes.

---

Rule 0 is the highest-priority architecture intent, but architecture tests and enforcement guards are authoritative when they conflict with this document.

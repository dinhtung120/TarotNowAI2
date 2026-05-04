# Compact Rules – TarotNowAI2

Use this as the short operating guide for Claude Code in this repository. The current codebase, tests, lint rules, build gates, and enforcement scripts remain the source of truth.

## 1. Rule precedence

1. Explicit user instruction for the current task.
2. Architecture tests, lint rules, build gates, and enforcement scripts.
3. Event-driven architecture intent in this file.
4. Other compact rules in this file.
5. Existing nearby code patterns.

If this file conflicts with an enforced guard, follow the guard and report that the rule needs updating. If there is no guard and nearby code conflicts with this file, ask before introducing a new pattern.

## 2. Backend architecture core

- Keep Application command entry handlers thin: `IRequestHandler<Command, TResult>` handlers dispatch the requested domain event through `IInlineDomainEventDispatcher` and return its result.
- Put use-case orchestration in `*RequestedDomainEventHandler` classes using Application-owned abstractions only.
- Do not inject repositories, infrastructure services, notification services, wallet services, realtime broadcasters, gamification/task services, or other side-effect components into command entry handlers.
- Secondary side effects such as notifications, emails, realtime broadcasts, leaderboard/gacha/achievement/task updates, telemetry fan-out, and similar work must flow through domain events, outbox messages, event handlers, or Infrastructure workers.
- Direct controller/API/hub realtime bypasses are critical issues unless an architecture test or explicit allowlist permits them.
- Chat transport exceptions stay bounded to the current tested/allowlisted patterns; finance, wallet, escrow, quota, and entitlement logic inside chat is not exempt.

## 3. Clean Architecture boundaries

- Dependency direction is Domain → Application → Infrastructure → API/Presentation.
- Domain contains business models, invariants, and framework-agnostic domain events; it must not reference Application, Infrastructure, API, persistence frameworks, web frameworks, Redis, MongoDB, EF Core, or provider SDKs.
- Application may depend on Domain and Application-owned contracts only; do not reference concrete Infrastructure namespaces or framework-specific packages from Application.
- Infrastructure implements Application contracts for persistence, cache, external providers, outbox, event dispatch, pub/sub, and workers.
- API/Presentation calls Application entry points such as MediatR handlers or approved Application services; it must not call repositories, DbContexts, provider concretes, or realtime broadcasters directly unless allowlisted.
- Queries read data only and must not trigger side effects.

## 4. Finance, wallet, quota, and AI invariants

- Use ACID transactions for money, escrow, wallet, quota, entitlement, and related settlement state.
- Enforce no negative balance, no double-spend, no duplicate settlement, and atomic earliest-expiry-first entitlement consumption where applicable.
- Commands that mutate money/quota/entitlement or call AI providers need deterministic idempotency keys at command and requested-event levels unless an architecture-test allowlist permits otherwise.
- Reserve or freeze quota/wallet cost atomically before AI provider calls.
- Provider calls must use existing timeout/retry configuration or runtime policy; stop and ask before inventing new retry behavior.
- Final AI/finance status must decide settle/consume versus refund/compensate.
- Wallet/money state changes publish exactly one canonical post-commit money event, usually `MoneyChangedDomainEvent`, unless the same bounded context already has an approved more-specific event.
- Finance/AI logic must not directly call notification, realtime, gamification, task, quota fan-out, or telemetry side effects.

## 5. Authentication and security

- Use the existing auth flow: short-lived JWT, refresh-token rotation, secure HttpOnly cookies, policy authorization, ownership checks, and rate limiting where the project already applies them.
- Do not introduce command injection, XSS, SQL/NoSQL injection, SSRF, unsafe deserialization, token leakage, or secret logging.
- Sensitive auth behavior must fail closed when guarded by existing tests/scripts.

## 6. Frontend architecture and UI rules

- `Frontend/src/app/**/page.tsx` and `layout.tsx` stay thin composition wrappers.
- App routes should import feature entry points through established public exports such as `@/features/*/public` when available.
- Keep business/data orchestration outside routes and presentational components; use feature components/hooks for complex UI behavior.
- Respect the frontend clean-architecture guard, including layer direction, domain purity, client/runtime boundaries, and sensitive EventSource/API path rules.
- Use TanStack Query for server state and Zustand for local UI state when state management is needed.
- New user-facing text must use the existing VI/EN/ZH localization approach with `locale → vi` fallback; do not migrate unrelated old hard-coded copy during small fixes.
- Extract custom hooks for reused or complex effects, subscriptions, data fetching, debounce/throttle, infinite scroll, forms, or browser integration; keep trivial local UI state inline.
- Respect component and hook/action size guards; split by responsibility, not artificial wrappers.
- Use TypeScript without introduced `any`; fix lint warnings introduced by the change.
- Add memoization only for stable memoized-child props, expensive computation, dependency stability, or measured rerender issues.
- Touched interactive UI needs baseline accessibility: explicit `button type`, accessible names for icon-only controls, validation state/error association, and visible keyboard focus.
- Use the canonical `cn` utility from `@/lib/utils` for conditional, merged, long, repeated, or variant-heavy Tailwind class sets.
- Avoid arbitrary Tailwind values when theme tokens or existing utilities fit; never use `@apply` in JSX/TSX.
- Next Image `unoptimized` usage requires the existing allowlist/justification path.

## 7. Backend style rules

- Keep backend methods and files within the architecture-test budgets; prefer guard clauses over deep nesting.
- Add or update XML comments only for new or modified public APIs or non-obvious public contracts in the touched scope.
- Comments should explain non-obvious invariants or surprising constraints, not restate code flow.
- Avoid repeated magic strings/numbers for business values, policy values, event names, and configuration.

## 8. Verification matrix

Use the smallest relevant check first, then broaden only when needed.

- Backend command/event/finance/AI/realtime boundary changes: run `Backend/tests/TarotNow.ArchitectureTests` plus affected unit/integration tests; verify idempotency, transaction boundary, and event/outbox path.
- Backend business logic changes outside boundaries: run targeted tests and architecture tests if dependencies, method size, or file size changed.
- Frontend TSX/UI/structure changes: run the relevant `Frontend/package.json` gate; `npm run lint` includes key architecture, auth, image, hook-size, and component-size guards.
- Frontend event-evidence or protected backend path changes: run `npm run verify:event-evidence`.
- Risk-tier frontend logic changes: run `npm run test:coverage:risk`.
- UI behavior changes: test the affected browser flow when feasible; if not feasible, say so explicitly.
- Documentation/rule-only edits: inspect the diff; do not run a production test sweep unless executable examples/scripts changed.

## 9. Execution discipline

- Make minimal, surgical changes directly tied to the request.
- Do not refactor unrelated code just to satisfy advisory rules; report unrelated violations instead.
- Match nearby style unless an enforced guard requires otherwise.
- Remove only imports, variables, functions, or files made unused by your own change unless cleanup was requested.
- Do not commit, push, deploy, modify shared infrastructure, or take destructive actions unless the user explicitly asks.
- Before committing when requested, inspect status/diff and avoid staging secrets or local-only config.

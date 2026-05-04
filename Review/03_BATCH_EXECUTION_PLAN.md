# Batch execution plan review kiến trúc

## Batch 0: Chuẩn hóa format

- Input: CLAUDE.md, architecture tests, Frontend scripts, cấu trúc module hiện tại.
- Focus area: mục lục, template, risk scoring, output format.
- Output: 7 file tổng quan tại `Review/`.

## Batch 1: Cross-cutting backend architecture

- Input: `ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `CodeQualityRulesTests.cs`.
- Focus area: clean architecture, thin command handler, requested domain event, outbox, realtime bridge, money/idempotency guard.
- Output: cross-cutting files cho boundary và event-driven architecture.

## Batch 2: Backend feature modules

- Input: `Backend/src/TarotNow.Application/Features`, `Backend/src/TarotNow.Domain/Events`, API controllers, tests.
- Focus area: entry point, dependency map, transaction boundary, idempotency, event/outbox path, test gap.
- Output: 23 file `Review/backend-features/BE_*.md`.

## Batch 3: Frontend feature modules

- Input: `Frontend/src/app/[locale]`, `Frontend/src/features`, `Frontend/src/shared`, `Frontend/src/i18n`, `Frontend/messages`, `Frontend/scripts`.
- Focus area: thin route, public export boundary, prefetch/hydration consistency, i18n VI/EN/ZH, component/hook size guard.
- Output: 16 file `Review/frontend-features/FE_*.md` và frontend cross-cutting file.

## Batch 4: Data/Ops/Security/Test gates

- Input: `database`, `deploy`, `.github/workflows`, auth/security architecture tests, frontend guards.
- Focus area: PostgreSQL/MongoDB/Redis ownership, migration/rollback, smoke test, backup/restore, auth fail-closed, rate limit.
- Output: cross-cutting files data, security, observability/test gates.

## Batch 5: Fit-gap tổng hợp

- Input: toàn bộ file review đã tạo.
- Focus area: gap P0/P1/P2, dependency chéo, module rủi ro cao, thứ tự remediation.
- Output: cập nhật dependency map, batch plan và checklist đầu ra.

## Output format cho mỗi batch

- Danh sách module đã review.
- Evidence path.
- Findings theo P0/P1/P2.
- Quyết định: pass, pass có điều kiện, hoặc cần remediation trước khi merge.
- Follow-up owner/module nếu có.

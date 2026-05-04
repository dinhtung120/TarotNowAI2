# FE Wallet

## Source đã đọc thủ công

- Feature: `Frontend/src/features/wallet`
- Public export: `Frontend/src/features/wallet/public.ts`
- Routes: `Frontend/src/app/[locale]/(user)/wallet/page.tsx`, `wallet/deposit/page.tsx`, `wallet/deposit/history/page.tsx`, `wallet/withdraw/page.tsx`
- API proxy đã đọc: `Frontend/src/app/api/wallet/balance/route.ts`, `Frontend/src/app/api/wallet/ledger/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/wallet.ts`
- Messages: `Frontend/messages/{vi,en,zh}` wallet/deposit/withdraw namespaces cần đối chiếu khi sửa copy.

## Entry points & luồng chính

Các route wallet đều là composition wrappers:

- `wallet/page.tsx` hydrate `prefetchWalletOverviewPage` và render `WalletOverviewPage`.
- `wallet/deposit/page.tsx` hydrate `prefetchDepositPage` và render `WalletDepositPage`.
- `wallet/deposit/history/page.tsx` hydrate `prefetchDepositHistoryPage` và render `WalletDepositHistoryPage`.
- `wallet/withdraw/page.tsx` gọi `requireSessionWithHandshake`, redirect non-`tarot_reader` về `/{locale}/wallet`, hydrate `prefetchWithdrawPage`, rồi render `WalletWithdrawPage`.

`features/wallet/public.ts` export:

- admin withdrawal actions: `listWithdrawalQueue`, `getWithdrawalDetail`, `processWithdrawal`.
- `WalletStoreBridge`.
- `WalletOverviewPage`, `WalletDepositPage`, `WalletDepositHistoryPage`, `WalletWithdrawPage`.
- withdrawal result types.

## Dependency và dữ liệu

Prefetch runners:

- `prefetchWalletOverviewPage`: query key `userStateQueryKeys.wallet.ledger(1)`, action `getLedger(1, 10)`.
- `prefetchDepositPage`: query key `userStateQueryKeys.wallet.depositPackages()`, action `listDepositPackages()`.
- `prefetchDepositHistoryPage`: query key `userStateQueryKeys.wallet.depositOrderHistory(1, 10, 'all')`, action `listMyDepositOrders(1, 10, null)` với empty-page fallback.
- `prefetchWithdrawPage`: query key `userStateQueryKeys.wallet.withdrawalsMine()`, action `listMyWithdrawals()`.

App API proxy đã đọc:

- `balance/route.ts`: fail-closed nếu thiếu token, gọi backend `/Wallet/balance`.
- `ledger/route.ts`: fail-closed nếu thiếu token, normalize `page >= 1`, clamp `limit <= 50`, gọi `/Wallet/ledger`.

Wallet route còn phụ thuộc deposit/withdrawal actions trong feature; finance mutation chi tiết cần đọc action/API proxy tương ứng khi sửa deposit hoặc withdraw flow.

## Boundary / guard

- Wallet là protected user finance route; API proxy phải fail-closed khi thiếu token.
- Withdraw route có role gate frontend `tarot_reader`; backend vẫn phải là source-of-truth cho authorization.
- Deposit/withdraw/ledger query keys phải khớp hooks để tránh hydration/stale-money bug.
- Finance mutations phải giữ idempotency, không retry duplicate và không log bank/payout/payment data.
- App route import qua `@/features/wallet/public`, đúng public API boundary.

## Rủi ro

- P0: wallet/deposit/withdraw mutation duplicate hoặc thiếu idempotency; withdraw route bypass role gate; ledger/balance leak qua proxy fail-open; bank/payout/payment data bị log/expose.
- P1: prefetch query key mismatch làm hiển thị số dư/ledger stale; deposit history key dùng `'all'` nhưng action gửi `null` filter cần kiểm tra khi sửa filter; admin withdrawal actions export từ wallet public surface bị dùng sai boundary.
- P2: docs chỉ review balance/ledger mà bỏ qua deposit, deposit history và withdraw route.

## Kết luận

FE Wallet là protected finance module gồm overview, deposit, deposit history và role-gated withdraw route. Review đúng phải đọc route, public export, prefetch runner và action/proxy của từng flow trước khi thay đổi UI hoặc mutation tiền.

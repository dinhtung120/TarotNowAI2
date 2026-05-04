# Quy tắc đánh giá và điểm rủi ro

## P0 - Chặn merge hoặc cần xử lý ngay

- Vi phạm clean architecture boundary đã có guard.
- Command handler backend inject repository, provider, notification, wallet, realtime hoặc service side-effect trực tiếp.
- Controller/API broadcast realtime trực tiếp ngoài allowlist.
- Flow finance/quota/AI thiếu transaction, idempotency hoặc settlement/refund path.
- Wallet mutation không phát canonical money event theo quy tắc hiện hành.
- Auth/security fail-open, thiếu ownership check, rate limit hoặc token/cookie không an toàn.

## P1 - Cần remediation có kế hoạch

- Coupling chéo module tăng rủi ro nhưng chưa phá guard.
- Prefetch/hydration frontend không nhất quán hoặc duplicate fetch đáng kể.
- i18n thiếu VI/EN/ZH ở user-facing copy mới.
- Test coverage thiếu cho use case quan trọng.
- Deploy/smoke/rollback path chưa rõ với thay đổi vận hành.

## P2 - Theo dõi hoặc cải thiện sau

- Tài liệu thiếu evidence chi tiết.
- Naming hoặc cấu trúc file chưa đồng nhất nhưng không ảnh hưởng guard.
- Component/hook gần vượt budget nhưng chưa fail.
- Minor duplication chưa gây rủi ro kiến trúc.

## Kết luận chuẩn

- `Pass`: không có P0/P1 đáng kể, evidence rõ.
- `Pass có điều kiện`: không có P0, có P1 đã có owner hoặc follow-up.
- `Cần remediation`: có P0 hoặc P1 chưa có phương án xử lý.

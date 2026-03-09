# TarotWeb – Thiết kế Database

Nguồn: Tổng hợp từ tài liệu thiết kế (`tai-lieu-thiet-ke/`) và tham khảo v1, v2.

## 1. Tổng quan

- **PostgreSQL**: Nguồn sự thật cho dữ liệu tài chính, ví, escrow, subscription, auth. Mọi thao tác tiền phải nằm trong transaction.
- **MongoDB**: Dữ liệu linh hoạt, volume cao: reading sessions, chat messages, gamification, logs.
- **Tham chiếu chéo**: PostgreSQL ↔ MongoDB dùng `TEXT` (UUID string hoặc ObjectId string).

## 2. Cấu trúc thư mục

```
database/
├── README.md                 # Tài liệu này
├── DATABASE_OVERVIEW.md      # Tổng quan bảng/collection + ERD
├── DESIGN_DECISIONS.md       # Quyết định thiết kế, invariants, double-entry, retention
├── postgresql/
│   └── schema.sql            # DDL PostgreSQL + stored procedures + reconciliation view
└── mongodb/
    ├── schema.md             # Schema MongoDB (markdown)
    └── init.js               # Script tạo collection + index + validator
```

**Cách chạy:**
- PostgreSQL: `psql -f postgresql/schema.sql` (hoặc dùng migration tool)
- MongoDB: `mongosh <connection_string> < mongodb/init.js`

## 3. Nguyên tắc thiết kế

- **PostgreSQL**: UUID cho ID, CHECK constraint cho balance >= 0, idempotency_key cho lệnh tài chính.
- **MongoDB**: ObjectId cho `_id` (trừ `cards_catalog._id` Int32 1–78), soft delete cho nghiệp vụ, TTL cho logs.
- **Ledger**: Ghi sổ kép logic (debit/credit), mọi biến động ví phải có dòng `wallet_transactions`.
- **Escrow**: Mỗi question item (main + add-question) có timer riêng; settlement aggregate vào 1 finance session.

## 4. Tham chiếu tài liệu

- `01-business-rules.md` – Quy tắc kinh doanh, mô hình escrow, entitlement, FX.
- `03-tech-architecture.md` – Mô hình lưu trữ, máy trạng thái AI/escrow, RNG audit.
- `04-ops-security-compliance.md` – Retention, KYC, geo gating.
- `DESIGN_DECISIONS.md` – Double-entry, reconciliation, ON DELETE, partitioning, retention.

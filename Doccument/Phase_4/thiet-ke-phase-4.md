# Phase 4 – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 4 (Community + Voice/Video).  
**Nguồn gốc:** Trích từ `01-business-rules.md`, `02-product-ux-specs.md`, `03-tech-architecture.md`

---

## 1. Quy tắc kinh doanh Phase 4 (từ BR)

### 4.1 Community (BR Phase 4.1)
- Forum/feed/chủ đề theo tài liệu
- Kiểm duyệt nội dung cộng đồng
- Reactions (like, love, etc.) — idempotent per user per post
- Report nội dung + admin moderation queue

### 4.2 Voice/Video Call (BR Phase 4.2)
- Metadata cho cuộc gọi (caller, receiver, duration, status)
- Luồng xử lý: requested → accepted → rejected → ended
- Lưu call session records (không lưu audio/video content)

---

## 2. Đặc tả UX Phase 4 (từ UX)

### 2.1 Community UX
- Feed UI: posts list + post composer
- Reactions: optimistic updates
- Report UI: reason codes
- Post visibility: public / friends-only / private

### 2.2 Call UX
- Call UI: join/leave + status indicators
- Call history/logs view
- In-conversation call initiation

---

## 3. Database Schema liên quan Phase 4

### MongoDB - Collections
- `community_posts` – posts (author_id, content, visibility, reactions_count, is_deleted)
  - Indexes: created_at, author_id, visibility, is_deleted
- `community_reactions` – reactions per post
  - Unique index: (post_id, user_id, type)
- `call_sessions` – call metadata
  - Validator: status enum (requested/accepted/rejected/ended)
  - Indexes: conversation_id, status

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [01-business-rules.md](../All_Phase/tai-lieu-thiet-ke/01-business-rules.md) | Phase 4 (4.1–4.2) |
| [02-product-ux-specs.md](../All_Phase/tai-lieu-thiet-ke/02-product-ux-specs.md) | Community, Moderation (4.6.5) |
| [database/mongodb/schema.md](../../database/mongodb/schema.md) | community_posts, community_reactions, call_sessions |

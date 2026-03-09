# Phase 4 – Test / Verification Checklist

**Mục đích:** Kiểm tra tất cả các bước trong Phase 4 (Community + Voice/Video) đã hoàn thành đúng chưa.  
**Cách dùng:** Chạy từng test case, đánh dấu `[x]` khi PASS, ghi note nếu FAIL.

---

## 1. Community – Posts (P4-COMM-BE-1.1)

### Test 1.1 – Create post
```bash
curl -s -X POST http://localhost:5000/api/v1/community/posts \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"content":"Hôm nay rút được The Fool!","visibility":"public"}' | python3 -m json.tool
```
- [ ] Tạo `community_posts` document trong MongoDB
- [ ] Có: author_id, content, visibility, created_at
- [ ] `reactions_count = 0`, `is_deleted = false`

### Test 1.2 – List posts (feed)
```bash
curl -s "http://localhost:5000/api/v1/community/posts?page=1&limit=20" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Paging hoạt động
- [ ] Chỉ hiện posts `is_deleted = false`
- [ ] Sort by created_at DESC
- [ ] Visibility filter hoạt động

### Test 1.3 – Delete post (soft-delete)
```bash
curl -s -X DELETE http://localhost:5000/api/v1/community/posts/<id> \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] `is_deleted = true` (soft-delete)
- [ ] Chỉ author hoặc admin được xóa
- [ ] Post biến mất khỏi feed

---

## 2. Community – Reactions (P4-COMM-BE-1.2)

### Test 2.1 – Add reaction
```bash
curl -s -X POST http://localhost:5000/api/v1/community/posts/<id>/reactions \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"type":"love"}' | python3 -m json.tool
```
- [ ] Tạo `community_reactions` document
- [ ] `reactions_count` trên post tăng

### Test 2.2 – Idempotent reaction (same user, same type)
```bash
# React lần 2 cùng type
```
- [ ] Không tạo duplicate (unique: post_id + user_id + type)
- [ ] `reactions_count` không tăng sai

### Test 2.3 – Remove reaction
```bash
curl -s -X DELETE http://localhost:5000/api/v1/community/posts/<id>/reactions/love \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] Reaction xóa thành công
- [ ] `reactions_count` giảm

---

## 3. Community – Moderation (P4-COMM-BE-1.3)

### Test 3.1 – Report post
```bash
curl -s -X POST http://localhost:5000/api/v1/community/posts/<id>/report \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"reason":"spam","details":"Quảng cáo"}' | python3 -m json.tool
```
- [ ] Report tạo trong `reports` collection
- [ ] Admin moderation queue nhận report

### Test 3.2 – Admin moderation
```bash
curl -s -X POST http://localhost:5000/api/v1/admin/reports/<id>/resolve \
  -H "Authorization: Bearer <admin_token>" -H "Content-Type: application/json" \
  -d '{"action":"remove_post","reason":"Vi phạm quy tắc"}' | python3 -m json.tool
```
- [ ] Post bị soft-delete
- [ ] Report status = resolved
- [ ] Audit trail ghi đầy đủ

---

## 4. Voice/Video Call Metadata (P4-CALL-BE)

### Test 4.1 – Create call session
```bash
curl -s -X POST http://localhost:5000/api/v1/conversations/<id>/call \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"type":"voice"}' | python3 -m json.tool
```
- [ ] Tạo `call_sessions` document trong MongoDB
- [ ] Status = `requested`
- [ ] Có: caller_id, receiver_id, conversation_id, type

### Test 4.2 – Call status transitions
```javascript
db.call_sessions.find({conversation_id: "<conv_id>"}).sort({created_at: -1}).limit(1);
```
- [ ] requested → accepted → ended (happy path)
- [ ] requested → rejected (reject path)
- [ ] Validator: chỉ cho phép status enum values

### Test 4.3 – Call history
```bash
curl -s "http://localhost:5000/api/v1/conversations/<id>/calls" \
  -H "Authorization: Bearer <token>" | python3 -m json.tool
```
- [ ] List call sessions cho conversation
- [ ] Có: duration, status, created_at
- [ ] Chỉ participants xem được

---

## 5. FE – Community UI

- [ ] Feed UI: posts list + infinite scroll / paging
- [ ] Post composer: text input + submit
- [ ] Reactions: tap to react + optimistic update
- [ ] Report button + reason selection modal
- [ ] Delete post (author's own posts)

---

## 6. FE – Call UI

- [ ] Call initiation button trong chat
- [ ] Incoming call UI: accept/reject
- [ ] Active call: status indicator + end button
- [ ] Call history list view

---

## Tổng kết Phase 4 Test

| # | Nhóm | Số test case | Kết quả |
|---|---|---:|---|
| 1 | Community Posts | 3 | |
| 2 | Reactions | 3 | |
| 3 | Moderation | 2 | |
| 4 | Call Metadata | 3 | |
| 5 | FE Community | 5 | |
| 6 | FE Call | 4 | |
| **Tổng** | | **20** | |

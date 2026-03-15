# Phase 1.6 – Legal + Profile + Deposit + Admin Test Checklist

---

## 1. Consent
```sql
SELECT * FROM user_consents WHERE user_id = '<user_id>';
```
- [x] Consent cho TOS, Privacy, AI disclaimer với version + consented_at
- [x] Version change → yêu cầu re-consent
- [x] Không cho truy cập features cho đến khi consent lại

## 2. Legal Pages
- [x] TOS page truy cập được (public route)
- [x] Privacy page truy cập được
- [x] SEO: title, meta description có

## 3. Profile
```bash
curl -s -X PATCH http://localhost:5000/api/v1/profile \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"displayName":"Tên Mới","dateOfBirth":"1995-07-15"}'
```
- [x] Display name + avatar cập nhật
- [x] DOB → zodiac đúng (Cancer cho July 15)
- [x] Numerology number tính đúng

## 4. Deposit
- [x] Tạo order (status=pending) + trả payment URL
- [x] Webhook signature sai → reject, không side effects
- [x] Webhook hợp lệ → credit Diamond via proc, ledger entry
- [x] Double-send → chỉ credit 1 lần
- [x] FX snapshot lưu nếu non-VND

## 5. Promotions
- [x] Deposit >= min_amount → auto-apply bonus
- [x] Diamond = base + bonus, ledger ghi rõ

## 6. Admin RBAC
- [x] Non-admin → 403
- [x] Admin → list users, lock/unlock
- [x] Lock → user không login được, unlock → login lại
- [x] Audit trail ghi admin_actions
- [x] Deposit orders: filters + paging hoạt động

---

## Tổng kết: **19/19 test cases PASSED** (Integrated Verified)

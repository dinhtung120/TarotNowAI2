# Phase 1.6 – Legal + Profile + Deposit + Admin Test Checklist

---

## 1. Consent
```sql
SELECT * FROM user_consents WHERE user_id = '<user_id>';
```
- [ ] Consent cho TOS, Privacy, AI disclaimer với version + consented_at
- [ ] Version change → yêu cầu re-consent
- [ ] Không cho truy cập features cho đến khi consent lại

## 2. Legal Pages
- [ ] TOS page truy cập được (public route)
- [ ] Privacy page truy cập được
- [ ] SEO: title, meta description có

## 3. Profile
```bash
curl -s -X PATCH http://localhost:5000/api/v1/profile \
  -H "Authorization: Bearer <token>" -H "Content-Type: application/json" \
  -d '{"displayName":"Tên Mới","dateOfBirth":"1995-07-15"}'
```
- [ ] Display name + avatar cập nhật
- [ ] DOB → zodiac đúng (Cancer cho July 15)
- [ ] Numerology number tính đúng

## 4. Deposit
- [ ] Tạo order (status=pending) + trả payment URL
- [ ] Webhook signature sai → reject, không side effects
- [ ] Webhook hợp lệ → credit Diamond via proc, ledger entry
- [ ] Double-send → chỉ credit 1 lần
- [ ] FX snapshot lưu nếu non-VND

## 5. Promotions
- [ ] Deposit >= min_amount → auto-apply bonus
- [ ] Diamond = base + bonus, ledger ghi rõ

## 6. Admin RBAC
- [ ] Non-admin → 403
- [ ] Admin → list users, lock/unlock
- [ ] Lock → user không login được, unlock → login lại
- [ ] Audit trail ghi admin_actions
- [ ] Deposit orders: filters + paging hoạt động

---

## Tổng kết: **19 test cases**

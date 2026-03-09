# Phase 5.2 – Subscription + Entitlement Test

---

- [ ] Multiple subscriptions active, each with own buckets
- [ ] Consume: earliest-expiry-first, tie-break subscription_id ASC
- [ ] Concurrent requests → no double-consume (row locking)
- [ ] Cross-key mapping OFF (default) → no auto deduction
- [ ] Daily reset đúng UTC midnight
- [ ] Expired subscriptions → entitlements not available

## Tổng kết: **6 test cases**

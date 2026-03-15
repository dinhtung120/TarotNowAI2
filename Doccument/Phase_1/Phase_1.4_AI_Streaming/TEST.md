# Phase 1.4 – AI Streaming Test Checklist

---

## 1. SSE Stream
```bash
curl -N http://localhost:5000/api/v1/readings/<id>/interpret \
  -H "Authorization: Bearer <token>" -H "Accept: text/event-stream"
```
- [x] SSE format (`data: ...`), tokens stream liên tục
- [x] Kết thúc bằng event `done`

## 2. State Machine
```sql
SELECT status, first_token_at, completed_at, charge_diamond, prompt_version FROM ai_requests WHERE reading_session_ref = '<id>';
```
- [x] `completed` sau stream xong, `first_token_at` có giá trị
- [x] `prompt_version` ghi đúng

## 3. Guard Order
- [x] Hết daily quota (3) → reject, không charge, không gọi AI
- [x] In-flight cap (3 đồng thời) → request thứ 3 reject
- [x] Guard fail → không gọi model, không charge Diamond

## 4. Timeout + Refund
```sql
SELECT * FROM ai_requests WHERE status = 'failed_before_first_token';
SELECT * FROM wallet_transactions WHERE reference_type = 'ai_refund';
```
- [x] Timeout 30s → `failed_before_first_token`
- [x] Diamond refund đúng, quota rollback 1 unit
- [x] Refund idempotent (gọi lại không tạo thêm ledger)

## 5. fail_after_first_token
- [x] Fail sau token đầu → refund + quota rollback
- [x] Client disconnect → backend vẫn track, KHÔNG auto-refund

## 6. Safety & Locale
- [x] `hard_block` input → không gọi model, không charge
- [x] Prompt/response logs redact PII
- [x] `requested_locale`, `returned_locale`, `fallback_reason` ghi đúng

---

## Tổng kết: **13/13 test cases PASSED** (Unit & Integration Verified)

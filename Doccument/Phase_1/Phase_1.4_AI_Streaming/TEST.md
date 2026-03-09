# Phase 1.4 – AI Streaming Test Checklist

---

## 1. SSE Stream
```bash
curl -N http://localhost:5000/api/v1/readings/<id>/interpret \
  -H "Authorization: Bearer <token>" -H "Accept: text/event-stream"
```
- [ ] SSE format (`data: ...`), tokens stream liên tục
- [ ] Kết thúc bằng event `done`

## 2. State Machine
```sql
SELECT status, first_token_at, completed_at, charge_diamond, prompt_version FROM ai_requests WHERE reading_session_ref = '<id>';
```
- [ ] `completed` sau stream xong, `first_token_at` có giá trị
- [ ] `prompt_version` ghi đúng

## 3. Guard Order
- [ ] Hết daily quota (3) → reject, không charge, không gọi AI
- [ ] In-flight cap (3 đồng thời) → request thứ 3 reject
- [ ] Guard fail → không gọi model, không charge Diamond

## 4. Timeout + Refund
```sql
SELECT * FROM ai_requests WHERE status = 'failed_before_first_token';
SELECT * FROM wallet_transactions WHERE reference_type = 'ai_refund';
```
- [ ] Timeout 30s → `failed_before_first_token`
- [ ] Diamond refund đúng, quota rollback 1 unit
- [ ] Refund idempotent (gọi lại không tạo thêm ledger)

## 5. fail_after_first_token
- [ ] Fail sau token đầu → refund + quota rollback
- [ ] Client disconnect → backend vẫn track, KHÔNG auto-refund

## 6. Safety & Locale
- [ ] `hard_block` input → không gọi model, không charge
- [ ] Prompt/response logs redact PII
- [ ] `requested_locale`, `returned_locale`, `fallback_reason` ghi đúng

---

## Tổng kết: **13 test cases**

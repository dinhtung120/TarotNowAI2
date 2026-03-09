# Phase 1.4 – AI Streaming + Refund

**Scope:** AI Provider, SSE, State Machine, Guards, Safety, Locale, Refund

---

## BE – AI Provider & SSE

- [ ] **P1-AI-BE-1.1** (1.0 PD) – Provider adapter interface + implementation
- [ ] **P1-AI-BE-1.2** (1.25 PD) – SSE endpoint + heartbeat
- [ ] **P1-AI-BE-1.3** (1.25 PD) – Prompt templates + `prompt_version`

## BE – AI State Machine

- [ ] **P1-AI-BE-2.1** (1.0 PD) – Create `ai_requests` row (`requested`) + idempotency
- [ ] **P1-AI-BE-2.2** (0.75 PD) – Persist `first_token_received`
- [ ] **P1-AI-BE-2.3** (1.5 PD) – Terminal transitions (completed/failed_*)
- [ ] **P1-AI-BE-2.4** (1.5 PD) – Refund & quota rollback idempotent

## BE – AI Guards

- [ ] **P1-AI-BE-3.1** (0.5 PD) – Guard 1: follow-up cap check
- [ ] **P1-AI-BE-3.2** (1.5 PD) – Guard 2: daily quota reserve atomic
- [ ] **P1-AI-BE-3.3** (0.75 PD) – Guard 3: rate limit enforcement
- [ ] **P1-AI-BE-3.4** (0.75 PD) – Guard 4: charge Diamond after pass 1-3

## BE – AI Safety & Locale

- [ ] **P1-AI-BE-4.1** (1.5 PD) – Moderation pre/post scaffolding
- [ ] **P1-AI-BE-4.2** (1.5 PD) – De-identification/redaction for prompt logs
- [ ] **P1-AI-BE-5.1** (1.0 PD) – Localization control fields

## DevOps

- [ ] **P1-AI-OPS-1.1** (0.75 PD) – Provider API keys secure + rotation
- [ ] **P1-AI-OPS-1.2** (0.75 PD) – Dashboards: timeout/refund/latency

## FE

- [ ] **P1-AI-FE-1.1** (1.25 PD) – SSE client: start/stop/reconnect
- [ ] **P1-AI-FE-1.2** (1.25 PD) – Streaming UI states + copy mapping

## QA

- [ ] **P1-AI-QA-1.1** (1.5 PD) – Integration: fail_before → refund + rollback
- [ ] **P1-AI-QA-1.2** (1.5 PD) – Integration: fail_after → refund + rollback

---

| Workstream | Tasks | PD |
|---|---:|---:|
| BE | 14 | ~16.25 |
| FE | 2 | ~2.5 |
| DevOps | 2 | ~1.5 |
| QA | 2 | ~3.0 |
| **Tổng** | **20** | **~23.25** |

# Phase 4 – Community + Voice/Video Call

**Nguồn:** `CODING_PLAN.md` Section 7  
**Mục tiêu:** Forum/feed cộng đồng, reactions, moderation, và metadata gọi thoại/video.

---

## Quy ước

- **PD** = person-days | Spec: `BR` = business-rules, `UX` = product-ux, `DB` = database/

---

## Task Checklist

### BE – Community

- [ ] **P4-COMM-BE-1.1** (2.0 PD) – Create/list posts API + pagination
  - Spec: BR(Phase-4.1)

- [ ] **P4-COMM-BE-1.2** (1.5 PD) – Reactions API + counters (idempotent)
  - Spec: BR(Phase-4.1)

- [ ] **P4-COMM-BE-1.3** (2.0 PD) – Moderation hooks + report integration
  - Spec: UX(4.6.5)

### FE – Community

- [ ] **P4-COMM-FE-1.1** (2.5 PD) – Feed UI + post composer
  - Spec: BR(Phase-4.1)

- [ ] **P4-COMM-FE-1.2** (1.5 PD) – Reactions UI + optimistic updates
  - Spec: BR(Phase-4.1)

- [ ] **P4-COMM-FE-1.3** (2.0 PD) – Report UI for posts
  - Spec: UX(4.6.5)

### BE – Voice/Video Call

- [ ] **P4-CALL-BE-1.1** (2.5 PD) – Call session metadata endpoints
  - Spec: BR(Phase-4.2)

- [ ] **P4-CALL-BE-1.2** (2.0 PD) – Store call session records in Mongo + indexes
  - Spec: DB(mongodb-schema)

### FE – Voice/Video Call

- [ ] **P4-CALL-FE-1.1** (2.0 PD) – Call UI: join/leave + status
  - Spec: BR(Phase-4.2)

- [ ] **P4-CALL-FE-1.2** (2.0 PD) – Call history/logs view
  - Spec: BR(Phase-4.2)

### QA

- [ ] **P4-QA-1.1** (2.0 PD) – E2E: community create/react/report
  - Spec: BR(Phase-4.1)

- [ ] **P4-QA-1.2** (2.0 PD) – E2E: call metadata lifecycle
  - Spec: BR(Phase-4.2)

---

## Tổng kết Phase 4

| Workstream | Số task | Tổng PD |
|---|---|---:|
| BE | 5 | 10.0 |
| FE | 5 | 10.0 |
| QA | 2 | 4.0 |
| **Tổng** | **12** | **24.0** |

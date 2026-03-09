# Phase 0.2 – Database Setup & Seed

**Scope:** PostgreSQL schema, MongoDB init, Seed data (cards, configs, EXP levels)

---

- [ ] **P0-DB-0.1** (0.5 PD) – Apply Postgres schema
  - Tạo đủ bảng/enum/proc/view; xác nhận chạy sạch
  - Spec: DB(schema.sql)

- [ ] **P0-DB-0.2** (0.5 PD) – Apply Mongo init
  - Tạo collections/index/validator; xác nhận không lỗi
  - Spec: DB(init.js)

- [ ] **P0-DB-0.3** (0.5 PD) – Seed `cards_catalog` (78 lá bài)
  - Spec: DB(mongodb-schema) | Phụ thuộc: P0-DB-0.2

- [ ] **P0-DB-0.4** (0.5 PD) – Seed `system_configs` tối thiểu
  - Spec: BR(Phase-1.5), ARCH(4.4.2) | Phụ thuộc: P0-DB-0.1

- [ ] **P0-DB-0.5** (0.5 PD) – Seed `user_exp_levels` + `card_exp_levels`
  - Spec: DB(schema.sql) | Phụ thuộc: P0-DB-0.1

---

| Tasks | PD |
|---:|---:|
| **5** | **2.5** |

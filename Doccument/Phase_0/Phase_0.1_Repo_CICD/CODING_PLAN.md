# Phase 0.1 – Repo & CI/CD

**Scope:** Cấu trúc thư mục, Config, Secrets, CI build/test/cache

---

- [ ] **P0-REPO-0.1** (0.5 PD) – Chuẩn hóa cấu trúc thư mục + naming conventions
  - Spec: ARCH(1.1)

- [ ] **P0-REPO-0.2** (0.5 PD) – Tạo mẫu config `.env.example` + appsettings skeleton
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-REPO-0.3** (0.5 PD) – Secrets strategy (không commit secret)
  - Spec: OPS(5.Security) | Phụ thuộc: P0-REPO-0.2

- [ ] **P0-CICD-0.1** (0.5 PD) – CI build API (.NET)
  - Pipeline build backend, fail-fast khi compile error
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-CICD-0.2** (0.5 PD) – CI build Web (Next.js)
  - Pipeline build frontend, verify typecheck/build pass
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-CICD-0.3** (0.5 PD) – CI run unit tests (skeleton)
  - Spec: ARCH(Testing-strategy) | Phụ thuộc: P0-CICD-0.1

- [ ] **P0-CICD-0.4** (0.5 PD) – CI cache deps + upload artifacts
  - Spec: ARCH(1.5) | Phụ thuộc: P0-CICD-0.1

---

| Tasks | PD |
|---:|---:|
| **7** | **3.5** |

# Phase 0.3 – API Scaffold + Auth Cookie

**Scope:** Base API, Swagger, ProblemDetails, Refresh Token Cookie

---

- [ ] **P0-API-0.1** (0.75 PD) – Base `/api/v1` + Swagger
  - Route versioning + Swagger để FE/QA có contract sớm
  - Spec: ARCH(4.14.4)

- [ ] **P0-API-0.2** (0.75 PD) – ProblemDetails + error code contract
  - Chuẩn hoá format lỗi RFC 7807
  - Spec: UX(4.15.3) | Phụ thuộc: P0-API-0.1

- [ ] **P0-AUTH-COOKIE-0.1** (1.0 PD) – Refresh token transport cookie + CSRF note
  - httpOnly cookie cho web, chuẩn bị CSRF
  - Spec: ARCH(4.1.5) | Phụ thuộc: P0-API-0.1

---

| Tasks | PD |
|---:|---:|
| **3** | **2.5** |

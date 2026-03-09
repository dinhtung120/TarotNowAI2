# Phase 0.4 – Web + Test Framework Test Checklist

---

## 1. Next.js Dev Server
```bash
cd src/web && npm run dev &
sleep 5
curl -s http://localhost:3000 | head -10
```
- [ ] Dev server khởi động không lỗi
- [ ] Trang chủ trả HTML
- [ ] Có layout chung + ít nhất 1 route ngoài `/`

## 2. i18n
```bash
ls src/web/locales/ # hoặc messages/ hoặc public/locales/
```
- [ ] Có file cho `vi`, `en`, `zh/zh-Hans`
- [ ] Fallback: thiếu key `vi` → hiện `en`
- [ ] Locale switcher / mechanism chuyển ngôn ngữ tồn tại

## 3. xUnit + Testcontainers
```bash
ls tests/
dotnet test tests/ --verbosity normal
```
- [ ] Project test tồn tại + chạy không lỗi framework
- [ ] Có NuGet: `Testcontainers.PostgreSql` / `Testcontainers.MongoDb`
- [ ] Có base class/fixture cho Postgres + MongoDB container

## 4. Playwright
```bash
ls tests/e2e/
npx playwright --version
npx playwright test --reporter=list
```
- [ ] Project Playwright tồn tại + dependencies cài được
- [ ] Có ít nhất 1 smoke test skeleton
- [ ] Test runner không crash

---

## Tổng kết: **10 test cases**

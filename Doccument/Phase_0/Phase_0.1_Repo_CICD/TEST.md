# Phase 0.1 – Repo & CI/CD Test Checklist

---

## 1. Cấu trúc thư mục
```bash
ls -d src/ docs/ database/ tests/
```
- [ ] Thư mục gốc có cấu trúc rõ ràng, tách biệt BE/FE/DB/docs
- [ ] Naming conventions nhất quán
- [ ] Không có thư mục/file rác

## 2. Config files
```bash
cat .env.example
cat src/api/appsettings.json
cat src/api/appsettings.Development.json
```
- [ ] `.env.example` tồn tại, có key: POSTGRES_*, MONGO_*, REDIS_*, AI_*, JWT_*
- [ ] Giá trị mẫu là placeholder, không phải secret thật
- [ ] `appsettings.json` có sections: ConnectionStrings, Jwt, Redis
- [ ] Development config trỏ localhost

## 3. Secrets strategy
```bash
grep -rn "password\s*=" --include="*.json" --include="*.yaml" . | grep -v "example" | grep -v "node_modules"
grep -rn "sk-" --include="*.ts" --include="*.cs" . | grep -v "node_modules"
```
- [ ] Không có secret thật trong source code
- [ ] Không có file `.env` (chỉ `.env.example`)
- [ ] Có tài liệu inject secrets

## 4. CI Build API
```bash
cd src/api && dotnet restore && dotnet build --no-restore
```
- [ ] `dotnet restore` + `dotnet build` thành công
- [ ] CI pipeline file tồn tại (`.github/workflows/`)
- [ ] Fail-fast: compile error → pipeline fail

## 5. CI Build Web
```bash
cd src/web && npm install && npm run build
```
- [ ] `npm install` + `npm run build` thành công
- [ ] Không có TypeScript errors
- [ ] CI có step build frontend + typecheck

## 6. CI Unit Tests
```bash
cd src/api && dotnet test --verbosity normal
```
- [ ] Tests chạy được (0 test OK = vẫn pass)
- [ ] CI có step chạy test

## 7. CI Cache
- [ ] Cache NuGet packages (`~/.nuget/packages`)
- [ ] Cache npm (`~/.npm` hoặc `node_modules`)

---

## Tổng kết: **14 test cases**

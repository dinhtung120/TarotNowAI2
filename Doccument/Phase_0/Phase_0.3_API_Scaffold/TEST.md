# Phase 0.3 – API Scaffold Test Checklist

---

## 1. API Health
```bash
cd src/api && dotnet run &
sleep 5
curl -s http://localhost:5000/api/v1/health
```
- [ ] Server khởi động không lỗi
- [ ] Health endpoint trả 200 OK

## 2. Swagger
```bash
curl -s http://localhost:5000/swagger/index.html | head -5
```
- [ ] Swagger UI truy cập được
- [ ] Hiển thị API versioning `/api/v1`

## 3. ProblemDetails
```bash
curl -s http://localhost:5000/api/v1/nonexistent | python3 -m json.tool
```
- [ ] Response format ProblemDetails: `type`, `title`, `status`, `detail`, `instance`
- [ ] `Content-Type: application/problem+json`

## 4. Refresh Token Cookie
- [ ] Có middleware/config cho httpOnly cookie
- [ ] Cookie flags: `HttpOnly=true`, `Secure=true`, `SameSite=Strict/Lax`
- [ ] Có tài liệu CSRF nếu cần

---

## Tổng kết: **6 test cases**

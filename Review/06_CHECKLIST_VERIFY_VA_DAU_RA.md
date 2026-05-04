# Checklist verify và đầu ra

## Verify cấu trúc

- [ ] `find Review -type f -name '*.md' | wc -l` trả về 52.
- [ ] 7 file tổng quan ở `Review/`.
- [ ] 6 file cross-cutting ở `Review/cross-cutting/`.
- [ ] 23 file backend feature khớp `Backend/src/TarotNow.Application/Features`.
- [ ] 16 file frontend feature khớp `Frontend/src/features`.

## Verify source-backed

- [ ] Mỗi file có evidence path cụ thể tới code/test/config đã đọc.
- [ ] Backend docs trỏ tới controller/Application feature/Infrastructure hoặc test phù hợp.
- [ ] Backend boundary/event-driven/method-size/API-config claims trỏ tới architecture tests.
- [ ] Frontend docs trỏ tới app route, public export, prefetch runner, app API proxy nếu có và messages namespace.
- [ ] Frontend route boundary/auth/i18n/size claims trỏ tới guard scripts hoặc route/proxy evidence.
- [ ] Data docs trỏ tới `ApplicationDbContext.cs`, `MongoDbContext.cs`, schema SQL/Mongo và Redis/outbox evidence.
- [ ] Ops docs trỏ tới compose files, deploy scripts and workflows.
- [ ] Không mô tả `database/*` và `deploy/*` như một thư mục lồng nhau.
- [ ] Không gọi runtime Mongo collection `conversation_reviews` là generic `reviews`.

## Verify bằng lệnh

```bash
find Review -type f -name '*.md' | wc -l
find Review/backend-features -type f -name 'BE_*.md' | wc -l
find Review/frontend-features -type f -name 'FE_*.md' | wc -l
find Backend/src/TarotNow.Application/Features -maxdepth 1 -mindepth 1 -type d | wc -l
find Frontend/src/features -maxdepth 1 -mindepth 1 -type d | wc -l
grep -R -n -E "<generic-template-phrases>|<invalid-combined-database-deploy-path>" Review || true
grep -R -n "conversation_reviews\|reviews" Review || true
git status --short
git diff --stat -- Review
```

## Đầu ra review chuẩn

- Kết luận tổng quan: số file đã sửa, phạm vi source đã đọc, còn gap nào không có evidence.
- Bảng findings P0/P1/P2 nếu phát hiện vấn đề kiến trúc cụ thể.
- Danh sách evidence path quan trọng theo backend/frontend/cross-cutting.
- Danh sách module cần đọc sâu khi thay đổi code tiếp theo.
- Git status/diff stat để người review biết phạm vi thay đổi chỉ nằm trong docs.

# Checklist verify và đầu ra

## Verify cấu trúc

- [ ] `find Review -type f -name '*.md' | wc -l` trả về 52.
- [ ] 7 file tổng quan ở `Review/`.
- [ ] 6 file cross-cutting ở `Review/cross-cutting/`.
- [ ] 23 file backend feature khớp `Backend/src/TarotNow.Application/Features`.
- [ ] 16 file frontend feature khớp `Frontend/src/features`.

## Verify source-backed

- [ ] Mỗi file có evidence path cụ thể.
- [ ] Không mô tả nhầm `database/*` và `deploy/*` như một thư mục lồng nhau; hai nhánh này phải được ghi riêng.
- [ ] Backend docs trỏ tới architecture tests khi nói về boundary/event-driven/code quality/API config.
- [ ] Frontend docs trỏ tới guard scripts khi nói về route boundary/component size/auth/i18n.
- [ ] Data docs trỏ tới `ApplicationDbContext.cs`, `MongoDbContext.cs`, schema SQL/Mongo, Redis DI.
- [ ] Ops docs trỏ tới compose, deploy scripts, workflows.

## Verify bằng lệnh

```bash
find Review -type f -name '*.md' | sort
find Backend/src/TarotNow.Application/Features -maxdepth 1 -mindepth 1 -type d | wc -l
find Frontend/src/features -maxdepth 1 -mindepth 1 -type d | wc -l
grep -R "database.*/deploy\|database deploy" -n Review || true
git status --short
```

## Đầu ra review chuẩn

- Kết luận tổng quan.
- Bảng findings P0/P1/P2.
- Danh sách evidence path.
- Gaps chưa có evidence.
- Follow-up theo module.

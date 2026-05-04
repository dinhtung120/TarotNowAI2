# Checklist verify và đầu ra

## Verify cấu trúc file

- [ ] Có 7 file tổng quan tại `Review/`.
- [ ] Có 6 file cross-cutting tại `Review/cross-cutting/`.
- [ ] Có 23 file backend feature tại `Review/backend-features/`.
- [ ] Có 16 file frontend feature tại `Review/frontend-features/`.
- [ ] Tổng cộng 52 file `.md` trong `Review/`.

## Verify nội dung từng file feature

- [ ] Có section phạm vi.
- [ ] Có entry points và luồng chính.
- [ ] Có dependency map upstream/downstream.
- [ ] Có ràng buộc kiến trúc.
- [ ] Có dữ liệu/trạng thái hoặc ghi rõ không áp dụng.
- [ ] Có test coverage hiện tại hoặc gap.
- [ ] Có rủi ro P0/P1/P2.
- [ ] Có output review chuẩn.

## Verify bám repo thực tế

- [ ] Backend đối chiếu `Backend/tests/TarotNow.ArchitectureTests`.
- [ ] Backend đối chiếu `Backend/src/TarotNow.Application/Features`.
- [ ] Frontend đối chiếu `Frontend/src/features/*/public.ts`.
- [ ] Frontend đối chiếu `Frontend/src/app/[locale]`.
- [ ] Data/Ops đối chiếu `database`, `deploy`, workflows.

## Lệnh kiểm tra tài liệu

```bash
find Review -type f -name '*.md' | sort
find Review -type f -name '*.md' | wc -l
git status --short
```

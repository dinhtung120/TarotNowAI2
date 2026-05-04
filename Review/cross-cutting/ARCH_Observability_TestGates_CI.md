# Observability, Test Gates, CI

## 1. Phạm vi

- Review concern cross-cutting: Architecture tests, unit/integration tests, frontend guards, smoke/rollback workflows.
- Áp dụng cho mọi module có dependency tới concern này.
- Không thay thế review chi tiết từng feature file.

## 2. Dependency map

- Upstream: feature modules, API routes/controllers, frontend app routes hoặc deploy workflows có sử dụng concern này.
- Downstream: source code, data store, infrastructure service, guard script hoặc test gate liên quan.
- Evidence gốc cần đối chiếu: `Backend/tests/TarotNow.ArchitectureTests`.

## 3. Focus area review

- Kiểm tra dependency có đi đúng boundary không.
- Kiểm tra side effect có nằm đúng layer hoặc đúng event/outbox path không.
- Kiểm tra test/guard hiện có có bao phủ rule quan trọng không.
- Kiểm tra rủi ro P0/P1/P2 theo `Review/05_QUY_TAC_DANH_GIA_VA_DIEM_RUI_RO.md`.

## 4. Output format chuẩn

- Kết luận: Pass / Pass có điều kiện / Cần remediation.
- Evidence: đường dẫn file, test, guard hoặc script liên quan.
- Findings: nhóm theo P0/P1/P2.
- Follow-up: module chịu trách nhiệm và batch nên xử lý.

## 5. Checklist

- [ ] Có evidence path cụ thể.
- [ ] Có dependency upstream/downstream.
- [ ] Có đánh giá rủi ro.
- [ ] Có đề xuất verify bằng guard hoặc script hiện có.

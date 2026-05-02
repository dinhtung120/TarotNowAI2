# Production Manual Checklist - Chat Realtime (2026-05-02)

## Scope
- URL test: `https://www.tarotnow.xyz/vi`
- Accounts:
  - `Lucifer / Sontung123!` (user side)
  - `Test / Sontung123!` (reader side)
- Browser sessions:
  - Google Chrome: Lucifer
  - Safari: Test

## Kết quả tổng quan
- `PASS` 1/4: User từ chối yêu cầu cộng tiền không lỗi.
- `PASS` 2/4: Luồng hoàn thành cuộc trò chuyện realtime (không cần F5).
- `PASS` 3/4: Popup đánh giá reader xuất hiện sau khi completed và submit thành công.
- `PASS` 4/4: Gửi tin nhắn liên tiếp bằng Enter không mất focus ô nhập.

---

## Case 1 - Reject add-money
### Steps
1. Reader (`Test`) mở action menu trong room ongoing -> chọn `Yêu cầu cộng tiền`.
2. Nhập amount `10` và note `them phi boi bai` -> gửi đề xuất.
3. User (`Lucifer`) nhận card payment offer realtime -> bấm `Từ chối`.

### Expected
- Không phát sinh lỗi/validation error.
- Card offer chuyển trạng thái `Đã bị từ chối`.
- Message system phản ánh nội dung reject (không crash).

### Actual
- Đúng kỳ vọng.
- User side hiển thị message: `Bạn đã từ chối đề xuất cộng tiền...`.
- Reader side nhận realtime message: `Đề xuất cộng tiền đã bị từ chối...`.

### Result
- `PASS`

---

## Case 2 - Complete conversation realtime
### Steps
1. Reader (`Test`) bấm action menu -> `Hoàn thành cuộc trò chuyện`.
2. User (`Lucifer`) nhận banner phản hồi complete ngay trong room.
3. User bấm `Đồng ý hoàn thành`.

### Expected
- Cả 2 phía nhận trạng thái complete gần tức thì.
- Không cần F5 để thấy state/message complete.

### Actual
- User nhận request complete realtime.
- Sau khi user accept, cả 2 phía thấy message complete và trạng thái completed ngay.
- Không cần F5.

### Result
- `PASS`

---

## Case 3 - Rating popup sau khi completed
### Steps
1. Sau khi user accept complete, theo dõi UI phía user (`Lucifer`).
2. Kiểm tra popup đánh giá.
3. Submit rating 5 sao.

### Expected
- Popup đánh giá xuất hiện cho user hợp lệ.
- Submit thành công, không submit trùng.

### Actual
- Popup `Đánh giá Reader` xuất hiện tự động.
- Submit thành công, có toast xác nhận: `Cảm ơn bạn! Đánh giá đã được ghi nhận.`
- UI hiển thị trạng thái đã đánh giá cho phiên này.

### Result
- `PASS`

---

## Case 4 - Composer focus sau khi gửi
### Steps
1. User (`Lucifer`) gửi liên tiếp `focus test 1`, `focus test 2` bằng Enter.
2. Không click lại vào input giữa các lần gửi.

### Expected
- Focus vẫn ở input sau mỗi lần gửi.
- Có thể gõ và gửi liên tiếp ngay.

### Actual
- Sau mỗi lần gửi, con trỏ vẫn trong ô nhập.
- Có thể gõ và gửi message kế tiếp ngay, không mất focus.

### Result
- `PASS`

---

## Ghi chú thêm trong phiên test
- Luồng `pending -> awaiting_acceptance -> ongoing` hoạt động đúng khi user gửi tin nhắn đầu tiên và reader phản hồi accept.
- Realtime event cho payment reject và complete response hoạt động không cần refresh trang.

## Kết luận
- 4 lỗi trọng điểm trong checklist hiện đã đạt `PASS` trên production tại thời điểm test `2026-05-02 15:08 (UTC+7)`.

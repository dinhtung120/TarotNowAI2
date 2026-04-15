# Tính Năng Trải Bài Hiện Tại - Luồng Xử Lý Chi Tiết (Ngôn Ngữ Người Dùng)

Tài liệu này mô tả **đúng hành vi hiện tại trong code** của hệ thống trải bài.
Mục tiêu là giúp bạn trả lời rõ:
- Luồng xử lý chạy như thế nào từ đầu đến cuối.
- Cơ chế random hiện tại là gì.
- Khi nào random hoàn thành.
- Khi nào gửi request đến AI.
- Khi nào lấy dữ liệu ảnh và khi nào ảnh thực sự được tải.

---

## 1) Luồng tổng quan từ góc nhìn người dùng

1. Người dùng vào trang trải bài, chọn:
   - Loại trải bài: `daily_1`, `spread_3`, `spread_5`, `spread_10`.
   - Loại tiền: `gold` hoặc `diamond`.
   - Câu hỏi (tùy chọn).
2. Bấm bắt đầu => hệ thống gọi API `POST /api/v1/reading/init`.
3. Nếu init thành công:
   - Tạo `sessionId`.
   - Chuyển sang trang `/reading/session/{sessionId}`.
4. Ở trang session:
   - Hiện intro “shuffle” (hiệu ứng mở đầu).
   - Người dùng chọn lá thủ công hoặc bấm chọn ngẫu nhiên.
5. Khi đủ số lá, bấm “Reveal” => gọi API `POST /api/v1/reading/reveal`.
6. Backend random lá thật, lưu kết quả vào session, trả danh sách `cards[]`.
7. Frontend lật lá theo animation.
8. Đồng thời, frontend tự mở stream AI để luận giải cho session.
9. AI stream xong => lưu trạng thái cuối, xử lý ví (nếu follow-up trả phí), cập nhật summary/follow-up vào session.

---

## 2) Chi tiết theo mốc thời gian

## 2.1 Bước Init Session

Frontend gửi:

```json
POST /reading/init
{
  "spreadType": "spread_3",
  "question": "Câu hỏi của tôi",
  "currency": "gold"
}
```

Backend xử lý:
- Xác thực user từ token.
- Validate input (`spreadType`, `currency`, độ dài question).
- Nếu là `daily_1`, kiểm tra đã rút daily trong ngày UTC chưa.
- Tính chi phí theo cấu hình hệ thống:
  - `spread_3`: mặc định 50 gold hoặc 5 diamond
  - `spread_5`: mặc định 100 gold hoặc 10 diamond
  - `spread_10`: mặc định 500 gold hoặc 50 diamond
- Tạo `ReadingSession` trạng thái ban đầu:
  - `IsCompleted = false`
  - chưa có `CardsDrawn`
- Orchestrator trừ ví (nếu có phí), rồi lưu session.
- Nếu lưu session lỗi sau khi đã trừ ví thì rollback hoàn tiền.

Frontend sau init thành công:
- Cập nhật số dư ví.
- Lưu tạm vào `sessionStorage`:
  - `question_{sessionId}`
  - `cardsToDraw_{sessionId}`
- Chuyển trang session.

---

## 2.2 Bước vào trang Session (trước khi reveal)

Ngay khi vào trang session:
- Frontend khởi động **query lấy catalog lá bài** (`/reading/cards-catalog`).
- Intro “shuffle” hiển thị trong **2200ms** (`SESSION_SHUFFLE_DURATION_MS = 2200`).

Lưu ý quan trọng:
- Intro shuffle chỉ là hiệu ứng, **không quyết định lá kết quả**.
- `shufflePaths` trong UI là pattern tạo sẵn theo index, không phải random bảo mật.

---

## 2.3 Bước chọn lá ở UI

Có 2 cách:

1. Chọn tay từng lá trong deck UI.
2. Bấm “chọn ngẫu nhiên”:
   - UI random bằng `Math.random()`.
   - Trộn danh sách lá khả dụng rồi lấy số lượng còn thiếu.
   - Thêm lần lượt theo nhịp `180ms`/lá (`RANDOM_PICK_DELAY_MS`).

Animation hỗ trợ:
- Mỗi lá bay vào stack khoảng `460ms` (`FLIGHT_DURATION_MS`).

Điểm rất quan trọng:
- Danh sách lá user chọn ở UI (`pickedCards`) chỉ dùng cho trải nghiệm giao diện.
- Khi gọi reveal, frontend **không gửi** danh sách lá đã chọn.
- API reveal chỉ gửi:

```json
POST /reading/reveal
{
  "sessionId": "..."
}
```

=> Nghĩa là kết quả thật không dựa trên lá user click ở UI.

---

## 2.4 Bước Reveal (random thật trên server)

Khi bấm reveal, backend chạy:

1. Tải session theo `sessionId`.
2. Kiểm tra session thuộc đúng user.
3. Chặn reveal lặp nếu session đã `IsCompleted = true`.
4. Random deck 78 lá bằng `RngService.ShuffleDeck(78)`:
   - Thuật toán Fisher-Yates.
   - Nguồn random: `RandomNumberGenerator.GetInt32(...)` (mức bảo mật cao).
5. Lấy `N` lá đầu theo spread:
   - daily_1 => 1 lá
   - spread_3 => 3 lá
   - spread_5 => 5 lá
   - spread_10 => 10 lá
6. Cập nhật collection + exp user.
7. Lưu `cardsDrawn` vào session và đánh dấu completed.
8. Trả về `cards[]`.

### Khi nào random hoàn thành?

Random backend được coi là hoàn thành khi:
- Đã lấy xong `drawnCards = shuffledDeck.Take(cardsToDraw).ToArray()`
- Và session đã `UpdateAsync` thành công với trạng thái completed.
- Sau đó response reveal mới trả về frontend.

---

## 2.5 Sau Reveal: lật lá và AI bắt đầu lúc nào?

Ngay sau khi frontend nhận `cards[]`:
- `setCards(...)` chạy ngay.
- Bắt đầu animation lật lá: mỗi lá trễ `800ms` (`FLIP_CARD_DELAY_MS`).

Trong lúc đang lật:
- Panel AI đã mount (vì `cards.length > 0`).
- Hook stream AI tự chạy sau khoảng `100ms`.

### Khi nào request AI được gửi?

Request AI bắt đầu khi `EventSource` mở tới:
- `/{locale}/api/reading/sessions/{sessionId}/stream?...`

Điều này xảy ra **ngay sau reveal thành công**, thường **trước khi tất cả lá lật xong**.

UI có trạng thái “đợi lật đủ lá”, nhưng stream thực tế đã khởi động nền.

---

## 3) Luồng AI stream chi tiết

## 3.1 Frontend gọi stream

- Mở SSE `EventSource`.
- Nhận chunk text liên tục.
- Khi nhận `[DONE]`:
  - Đóng stream.
  - Đánh dấu complete phía UI.

Follow-up:
- User gửi câu hỏi bổ sung => mở stream mới với `followupQuestion`.
- UI chặn cứng tối đa 5 follow-up.

## 3.2 Backend xử lý stream

Backend `GET /sessions/{sessionId}/stream` làm các bước:
- Check feature flag `AiStreamingEnabled`.
- Check quyền user/session.
- Bắt buộc session đã reveal (`IsCompleted = true`), nếu chưa thì từ chối.
- Check quota/rate limit:
  - quota ngày
  - số request AI đang chạy đồng thời
  - khoảng cách chống spam giữa các request
- Tính phí:
  - Lần luận giải đầu: 0
  - Follow-up: tính theo `FollowupPricingService`
- Tạo `AiRequest` status `requested`.
- Nếu có phí > 0: freeze diamond escrow trước khi stream.
- Build prompt từ:
  - question
  - spreadType
  - cards đã rút
  - follow-up question (nếu có)
- Gọi OpenAI stream (`chat/completions`, `stream = true`).
- Đẩy từng chunk về client.
- Kết thúc stream thì gọi command `CompleteAiStream` để chốt trạng thái.

## 3.3 Khi nào AI được coi là hoàn tất?

Ở mức giao diện:
- Khi frontend nhận event `[DONE]`.

Ở mức nghiệp vụ backend:
- Khi `CompleteAiStream` xử lý xong:
  - update trạng thái `AiRequest` cuối cùng,
  - consume/refund escrow theo tình huống,
  - cập nhật `AiSummary` hoặc append follow-up vào `ReadingSession` (nếu completed).

---

## 4) Cơ chế lấy hình ảnh lá bài

Có 2 lớp “lấy ảnh”:

1. **Lấy metadata ảnh** (URL ảnh):
   - Frontend gọi `GET /reading/cards-catalog`.
   - Backend đọc từ Mongo `cards_catalog`, chuẩn hóa `imageUrl` (gắn CDN base nếu cần).

2. **Tải file ảnh thật**:
   - Chỉ xảy ra khi danh sách lá đã reveal được render bằng `<Image src=...>`.
   - Trình duyệt mới tải các URL ảnh đó.

Fallback hiện tại:
- Nếu chưa có catalog hoặc card không có `imageUrl`, UI hiện fallback text/number thay ảnh.

---

## 5) Các mốc “hoàn thành” quan trọng (tóm tắt nhanh)

1. Shuffle intro UI hoàn thành:
   - Sau timeout `2200ms`, `isShuffling = false`.

2. Random chọn nhanh UI hoàn thành:
   - Khi toàn bộ timer chọn ngẫu nhiên đã chạy xong và `pickedCards.length === cardsToDraw`.

3. Random thật backend hoàn thành:
   - Khi reveal đã random xong + lưu session completed thành công.

4. AI request bắt đầu:
   - Sau reveal thành công, component AI mount, khoảng 100ms sau mở SSE.

5. AI hoàn thành:
   - Frontend nhận `[DONE]` và backend complete command chạy xong.

6. Ảnh bắt đầu tải:
   - Sau khi card đã reveal và component ảnh render với `imageUrl` hợp lệ.

---

## 6) Lưu ý hành vi hiện tại cần nắm rõ

1. Lá user click trong UI không được gửi vào backend reveal.
2. Kết quả lá thật do backend random độc lập bằng RNG bảo mật.
3. AI stream có thể bắt đầu trước khi animation lật lá kết thúc.
4. Follow-up free slot ở UI đang cố định `0`, trong khi backend có cơ chế free slot động theo bộ lá.
5. Rule daily đang chặn theo lần `init` trong ngày UTC (không cần đợi reveal mới tính).

---

## 7) API chính của tính năng trải bài

- `POST /api/v1/reading/init`
- `POST /api/v1/reading/reveal`
- `GET /api/v1/reading/cards-catalog`
- `GET /api/v1/sessions/{sessionId}/stream?language=...&followupQuestion=...`


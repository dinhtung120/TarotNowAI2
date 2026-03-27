# HỆ THỐNG NHẮN TIN (CHAT) CHO ỨNG DỤNG XEM BÀI TAROT

**Phiên bản:** v2.0 – Production  
**Mục tiêu:** Xây dựng hệ thống chat công bằng, tự động hóa cao, giảm tranh chấp, trải nghiệm giống Instagram DM, phù hợp cả mobile và desktop.

---

## 1. Trạng Thái Phòng Chat (Chat Room States)

Mỗi phòng chat đi qua các trạng thái sau, **không thể quay ngược**:

```
Pending → AwaitingAcceptance → Ongoing → Completed
                  ↓                 ↓
              Cancelled          Disputed → Completed
                  ↓
               Expired
```

| Trạng thái | Ý nghĩa | Ai gây ra |
|---|---|---|
| `Pending` | User mở phòng chat, chưa gửi tin nhắn nào | Hệ thống |
| `AwaitingAcceptance` | User đã gửi tin nhắn đầu tiên, tiền đã freeze. Đợi Reader accept/reject | User gửi tin nhắn |
| `Ongoing` | Reader đã accept, hai bên đang trao đổi | Reader accept |
| `Completed` | Chat kết thúc bình thường (cả hai đồng ý, hoặc auto-complete) | User/Reader/Hệ thống |
| `Cancelled` | Reader từ chối (reject) câu hỏi, Reader bị ban/xóa, hoặc User hủy trước khi Reader phản hồi → **tự động hoàn tiền 100%** | Reader reject / Admin / User |
| `Expired` | SLA timeout – Reader không phản hồi trong thời gian cam kết → **tự động hoàn tiền 100%** | Hệ thống (cron job) |
| `Disputed` | Một bên tố cáo, đang chờ Admin xử lý. Tiền giữ nguyên freeze | User/Reader |

**Quy tắc:**
- Mỗi cặp User–Reader chỉ có **TỐI ĐA 1 phòng active** (`Pending`, `AwaitingAcceptance`, hoặc `Ongoing`) tại một thời điểm.
- User đang có tối đa **5 phòng active** → chặn tạo thêm, cảnh báo "Bạn đang chat quá nhiều, vui lòng hoàn thành một số chat trước".

---

## 2. Danh Sách Tarot Reader & Bắt Đầu Chat

### 2.1. Danh sách Reader

- Hiển thị danh sách Tarot Reader với filter, search, sắp xếp theo:
  - Giá/câu hỏi (kim cương 💎)
  - Rating trung bình ⭐
  - Số lượt chat hoàn thành
  - Số lượt đánh giá
  - Trạng thái: **Online** / **Away (Tạm nghỉ)** / **Offline**

### 2.2. Profile Reader – Cài đặt

| Cài đặt | Mô tả |
|---|---|
| **Giá cố định** (vd: 10 💎) | Giá cho **1 câu hỏi chính**. Nếu User muốn hỏi thêm → Reader dùng tính năng "Yêu cầu cộng thêm tiền" |
| **Mức SLA cam kết** | 3 mức: **Nhanh (6h)** / **Bình thường (12)** / **Chậm (24)**. User thấy rõ mốc này trước khi tạo chat |
| **Trạng thái hoạt động** | Reader có thể đặt: Online / **Away (Tạm nghỉ)** / Offline. Hệ thống **tự động chuyển Offline sau 1 giờ** không hoạt động trên app |

### 2.3. Flow tạo phòng chat

1. **User click vào Reader:**
   - Nếu đã có phòng chat active (Pending/AwaitingAcceptance/Ongoing) → mở phòng đó.
   - Nếu không → tạo phòng chat mới ở trạng thái `Pending`.

2. **Cảnh báo Reader Away/Offline:**
   - Nếu Reader đang **Away** hoặc **Offline** → hiện banner cảnh báo trong phòng chat:  
     _"⚠️ Reader đang không hoạt động. Thời gian phản hồi có thể lâu hơn SLA cam kết."_
   - User vẫn được gửi tin nhắn nếu muốn.

3. **User gửi tin nhắn đầu tiên** → kích hoạt thanh toán:
   - Kiểm tra ví: không đủ 💎 → chặn gửi, thông báo nạp thêm.
   - Đủ → **Freeze** đúng số tiền giá Reader.
   - Chuyển trạng thái: `Pending` → `AwaitingAcceptance`.
   - Hiển thị system message: _"Đã đóng băng X 💎 cho cuộc chat này. Đang chờ Reader phản hồi."_
   - Cả User và Reader đều thấy: **"Đang đóng băng: X 💎"** ở header phòng chat.

4. **Reader phản hồi (trong AwaitingAcceptance):**
   - Reader có **6-12h** để **Accept** hoặc **Reject**.
   - **Accept** → trạng thái chuyển sang `Ongoing`. SLA timer bắt đầu tính.
   - **Reject** → trạng thái chuyển sang `Cancelled`. Hệ thống tự động **hoàn 100% 💎** cho User. Reader bắt buộc nhập lý do từ chối. System message: _"Reader đã từ chối câu hỏi. Lý do: [lý do]. Đã hoàn X 💎 về ví của bạn."_
   - **Timeout 6-12h không phản hồi** → trạng thái chuyển sang `Expired`. **Hoàn 100% 💎** cho User. System message: _"Reader không phản hồi trong thời gian quy định. Đã hoàn X 💎."_

### 2.4. Edge Cases

- User click Reader nhưng chưa gửi tin nhắn → phòng `Pending`, không trừ tiền.
- Reader bị ban hoặc xóa tài khoản khi đang có chat active → tự động hoàn tiền toàn bộ, chuyển sang `Cancelled`, system message: _"Reader không còn khả dụng. Đã hoàn X 💎."_
- User hủy phòng `Pending` (chưa gửi tin nhắn) → xóa phòng hoặc chuyển `Cancelled`.

---

## 3. Quy Tắc Thanh Toán & Giải Ngân

### 3.1. Nguyên tắc chung

- Tất cả khoản tiền (ban đầu + cộng thêm) gộp vào **1 khoản đóng băng duy nhất**.
- **Phí nền tảng: 10%** – Reader nhận 90% tổng kim cương sau khi giải ngân.
- Hệ thống sử dụng **cron job chạy mỗi 1 giờ** (UTC) để xử lý timeout và auto-complete.
- ⚠️ Do cron job 1h, các thời hạn thông báo cho User đã **tính cả độ trễ tối đa 1h**:
  - Thông báo "24h" thực tế = **tối đa 25 giờ**.
  - Thông báo "48h" thực tế = **tối đa 49 giờ**.

### 3.2. SLA Timeout – Reader chưa reply

- Tính từ lúc chat chuyển sang `Ongoing` (Reader accept).
- Nếu Reader **không gửi tin nhắn nào** trong thời gian SLA cam kết (6h/12h/24h):
  - Hệ thống tự động **hoàn 100% 💎** cho User.
  - Chuyển trạng thái → `Expired`.
  - Xử lý ở cron job gần nhất sau khi đáo hạn.

### 3.3. Flow "Hoàn thành cuộc trò chuyện"

Khi chat đang `Ongoing` và Reader đã reply (có trao đổi thực tế):

- Cả hai bên đều có nút **"Hoàn thành cuộc trò chuyện"**.
- Khi một bên bấm → hệ thống gửi push notification + request xác nhận cho bên kia.
- ⚠️ **Khi bấm "Hoàn thành", mọi Add Money request đang pending sẽ bị tự động hủy** + ghi system message: _"Request cộng thêm tiền đã bị hủy do một bên yêu cầu hoàn thành."_

**Bảng kết quả:**

| Tình huống | Kết quả | Thời gian |
|---|---|---|
| **Cả hai đồng ý** | Giải ngân ngay (Reader nhận 90%, platform 10%) | Ngay lập tức |
| **User bấm, Reader chưa phản hồi** | Giải ngân cho Reader | Tối đa **12 giờ** (6h + cron) |
| **Reader bấm, User đồng ý** | Giải ngân cho Reader | Ngay lập tức |
| **Reader bấm, User không bấm gì** | Giải ngân cho Reader | Tối đa **48 giờ** (47h + cron) |
| **Reader bấm, User từ chối** | Tiền giữ freeze. User có nút "Tố cáo" | Xem mục Dispute bên dưới |

**Khi Reader bấm "Hoàn thành":**
- Hệ thống gửi system message vào chat:  
  _"Reader đã đánh dấu hoàn thành. Nếu bạn không phản hồi, hệ thống sẽ tự động giải ngân cho Reader sau tối đa 48 giờ."_
- User có 3 lựa chọn: **Đồng ý** / **Từ chối** / **Không làm gì**.

**Khi User từ chối hoàn thành:**
- Tiền giữ nguyên đóng băng.
- User có nút **"Tố cáo / Không đồng ý kết thúc"** → chuyển chat sang `Disputed`.
- Nếu User **không tố cáo** trong 7 ngày → tự động giải ngân cho Reader (xử lý ở cron job gần nhất).

### 3.4. Giải ngân chi tiết

Khi giải ngân thành công:
- **Reader nhận:** Tổng 💎 × 90%
- **Platform fee:** Tổng 💎 × 10%
- Reader nhận thông báo + email: _"Đã nhận X 💎 từ cuộc chat với [User]. Phí nền tảng: Y 💎."_

---

## 4. Tính Năng "Cộng Thêm Tiền" (Add Money)

### 4.1. Flow chính

1. **Chỉ Reader** có nút **"Yêu cầu cộng thêm tiền"** trong phòng chat `Ongoing`.
2. Reader nhập: **Số tiền** + **Lý do** (bắt buộc, tối đa 100 ký tự).
   - Gợi ý lý do: _"Phí cho câu hỏi phụ/thêm"_.
3. Chỉ cho phép **1 request đang pending** tại một thời điểm.
4. User nhận push notification. Trong chat hiện 2 nút: **Đồng ý** / **Từ chối**.
   - **Đồng ý:** Kiểm tra ví User → đủ → trừ và **cộng vào khoản đóng băng hiện tại**.
   - **Từ chối:** User nhập lý do từ chối (tùy chọn) → request bị hủy.

### 4.2. Edge Cases

| Case | Xử lý |
|---|---|
| Request timeout **24h** không phản hồi | Tự động hủy request |
| Reader gửi request khi User đã bấm "Hoàn thành" | Tự động từ chối |
| Một bên bấm "Hoàn thành" khi có request pending | Tự động hủy request + system message |
| User đồng ý nhưng ví không đủ | Request pending đợi nạp, tự hủy khi hết 24h |
| Reader gửi request khi chat không phải `Ongoing` | Hệ thống chặn, không cho gửi |

---

## 5. Dispute & Bảng Quyết Định Admin

### 5.1. Mở Dispute

- Cả hai bên đều có nút **"Báo cáo / Tố cáo"** bất cứ lúc nào khi chat đang `Ongoing`.
- Khi tố cáo → chat chuyển sang `Disputed`:
  - Tiền giữ nguyên freeze.
  - Chat **bị khóa** – không gửi được tin nhắn mới.
  - Lịch sử auto-flag (từ khóa tiêu cực, đe dọa, yêu cầu tình dục) được đính kèm vào báo cáo.
  - Admin nhận notification ngay lập tức.

### 5.2. Bảng Quyết Định Admin (Decision Matrix)

Admin phải xử lý trong tối đa **48 giờ**:

| Phán quyết | Tiền | Rating | Ghi chú |
|---|---|---|---|
| **Reader đúng** | Giải ngân 100% cho Reader (sau trừ 10% phí) | Rating được tính bình thường | Reader không bị ảnh hưởng |
| **User đúng** | Hoàn 100% về ví User | Rating bị hủy (không tính vào trung bình) | Reader nhận cảnh cáo |
| **Không kết luận** (gray area) | Admin tùy chọn % split (vd: 50/50) | Rating bị hủy | Cả hai nhận thông báo giải thích |
| **Admin quá hạn 48h** | Tự động giải ngân 100% cho Reader | Rating được tính | Escalate lên Senior Admin |

**Quy tắc bổ sung:**
- Reader có **> 3 dispute trong 7 ngày** → Admin freeze tài khoản Reader để review.
- Mọi phán quyết đều được ghi log vĩnh viễn + lý do.

---

## 6. Kết Thúc Chat & Đánh Giá

### 6.1. Khi chat chuyển sang `Completed`

| Cách kết thúc | Rating popup |
|---|---|
| Cả hai bấm "Hoàn thành" | Hiện **ngay lập tức** popup đánh giá cho User |
| Auto-complete (timeout) | Khi User mở lại chat Completed → hiện nút "Đánh giá ngay" |
| Sau Dispute (Admin phán) | Tùy phán quyết – nếu rating được tính → popup đánh giá |

### 6.2. Đánh giá

- User đánh giá **1–5 sao** + bình luận (không bắt buộc).
- User có thể check **"Đánh giá ẩn danh"**.
- Rating cập nhật **ngay lập tức** vào trung bình Reader, ảnh hưởng thứ hạng danh sách.
- Rating hiển thị công khai sau khi 2 bên xác nhận hoặc sau 48h.
- User đánh giá 1 sao + bình luận tiêu cực → Reader nhận thông báo và có thể **phản hồi 1 lần** (không xóa được rating).
- **Nếu chat kết thúc diện Disputed:** Rating **KHÔNG** tính vào trung bình cho tới khi Admin phán quyết.

### 6.3. Bắt đầu phiên mới sau Completed

- Khi chat ở trạng thái `Completed` → màn hình Read-only, ẩn thanh chat.
- Dưới cùng hiện nút **"Bắt đầu phiên tư vấn mới"**.
- Click → tạo phòng `Pending` mới hoàn toàn độc lập, áp dụng **giá hiện tại** của Reader (không kế thừa khoản cộng thêm từ chat cũ).
- **Chỉ hiện nút này khi** không có phòng active nào khác với Reader đó.

---

## 7. Giao Diện Chat (Responsive)

### 7.1. Desktop / Tablet (> 768px)

- Chia **2 cột** giống Instagram DM:
  - **Cột trái:** Danh sách cuộc trò chuyện
    - Tab 1: **Đang chat** (Pending + AwaitingAcceptance + Ongoing)
    - Tab 2: **Đã hoàn thành** (Completed + Cancelled + Expired + Disputed đã xử lý)
  - **Cột phải:** Nội dung chat chi tiết

### 7.2. Mobile (≤ 768px)

- **Màn hình 1:** Danh sách cuộc hội thoại (full screen)
  - Mỗi item: Tên Reader/User, tin nhắn cuối, thời gian, số 💎 đang freeze (nếu có), trạng thái badge.
  - Tab **Đang chat** / **Đã hoàn thành** (segmented control).
- **Màn hình 2:** Phòng chat chi tiết (full screen) – khi nhấn vào cuộc hội thoại.
  - Nút **Back** ← góc trên trái để quay về danh sách.
  - **Floating Action Button** "Yêu cầu cộng thêm tiền" ở góc dưới phải (chỉ hiện cho Reader khi chat `Ongoing`).

### 7.3. Tính năng chung trong phòng chat

**Gửi tin nhắn:**

| Loại | Chi tiết |
|---|---|
| Text | Hỗ trợ Emoji |
| Hình ảnh | Tối đa 10MB, nén tự động |
| Voice message | Ghi âm trực tiếp, nén audio, hiển thị waveform + nút Play |

**Header phòng chat:**
- Luôn hiển thị: **"Đang đóng băng: X 💎"** (khi có freeze)
- Trạng thái Reader: Online/Away/Offline
- Nút xem lịch sử

**Trạng thái tin nhắn:**
- ✓ Đã gửi
- ✓✓ Đã nhận
- ✓✓ (xanh) Đã đọc
- **Typing indicator** – hiển thị khi đối phương đang gõ

**Quick Replies (chỉ Reader):**
- Reader lưu các template mẫu (vd: _"Tôi đang bốc bài cho bạn..."_, _"Đây là ý nghĩa lá bài này..."_).
- Bấm icon → chèn nhanh văn bản vào ô chat.

**Infinite scroll:** Load thêm tin nhắn khi kéo lên (lazy load).

**Auto Flag:** Hệ thống tự động quét và gắn cờ nếu chat chứa từ khóa tiêu cực, đe dọa, hoặc yêu cầu tình dục → đính kèm vào báo cáo nếu có Dispute.

### 7.4. Giao diện theo trạng thái

| Trạng thái | Thanh chat | Ghi chú |
|---|---|---|
| `Pending` | Hiển thị, nhưng gửi tin nhắn sẽ trigger thanh toán | Banner cảnh báo nếu Reader Away/Offline |
| `AwaitingAcceptance` | Ẩn cho User (đợi Reader) | Hiện cho Reader: nút Accept / Reject |
| `Ongoing` | Hiển thị bình thường | Đầy đủ tính năng |
| `Completed` | Ẩn, Read-only | Hiện nút "Bắt đầu phiên mới" |
| `Cancelled` / `Expired` | Ẩn, Read-only | Hiện thông báo lý do + hoàn tiền |
| `Disputed` | Ẩn, Read-only | Hiện trạng thái "Đang chờ Admin xử lý" |

---

## 8. Reader Online Status & Offline Handling

### 8.1. Trạng thái hoạt động

| Trạng thái | Ý nghĩa | Cách set |
|---|---|---|
| **Online** 🟢 | Reader đang hoạt động trên app | Tự động khi mở app |
| **Away (Tạm nghỉ)** 🟡 | Reader chủ động tạm nghỉ | Reader tự đặt trong Settings |
| **Offline** 🔴 | Reader không hoạt động | Tự động sau **1 giờ** không hoạt động, hoặc đóng app |

### 8.2. Ảnh hưởng đến chat

- Reader **Online:** User tạo chat bình thường, không cảnh báo.
- Reader **Away/Offline:** User vẫn tạo chat được, nhưng hệ thống hiện banner:  
  _"⚠️ Reader hiện không hoạt động. Thời gian phản hồi có thể lâu hơn cam kết."_
- SLA timer **vẫn tính** bất kể trạng thái Reader → đây là cam kết của Reader.

---

## 9. Push Notification Strategy

| Sự kiện | Ai nhận | Channel |
|---|---|---|
| Tin nhắn mới | Đối phương | Push + In-app |
| Reader Accept/Reject chat | User | Push + In-app |
| Add Money request | User | Push + In-app + Email |
| Reponse cho Add Money | Reader | Push + In-app |
| Yêu cầu hoàn thành | Đối phương | Push + In-app |
| Giải ngân thành công | Reader | Push + Email |
| Hoàn tiền (bất kỳ lý do) | User | Push + Email |
| Dispute mở | Cả hai + Admin | Push + Email |
| Dispute kết quả | Cả hai | Push + Email |
| SLA sắp timeout (cảnh báo sớm) | Reader | Push (trước 1h) |
| Rating mới | Reader | Push + In-app |

---

## 10. Yêu Cầu Chung Cho Production

- Toàn bộ lịch sử chat, transaction, request add money, nút hoàn thành, dispute… đều **lưu log vĩnh viễn** (không cho phép xóa).
- **Không hỗ trợ xóa tin nhắn** – tất cả tin nhắn đã gửi là permanent.
- Currency: **Kim cương 💎** (virtual currency).
- Tất cả thời gian tính theo **UTC** và ghi log rõ ràng.
- Admin dashboard:
  - Bộ lọc dispute theo trạng thái, thời gian, Reader.
  - Manual refund capability.
  - Freeze tài khoản Reader nếu > 3 dispute trong 7 ngày.
  - Transaction history và audit trail đầy đủ.

---

## Phụ Lục: Sơ Đồ Flow Tổng Quan

```
┌─────────────────────────────────────────────────────────────────────┐
│                        LIFECYCLE TỔNG QUAN                         │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  User mở chat ──→ [Pending]                                        │
│       │                                                             │
│       ├── User gửi tin nhắn đầu (freeze 💎)                        │
│       │       │                                                     │
│       │       ▼                                                     │
│       │   [AwaitingAcceptance] ──6-12h──→ [Expired] (hoàn 💎)   │
│       │       │                                                     │
│       │       ├── Reader Accept ──→ [Ongoing]                       │
│       │       │       │                                             │
│       │       │       ├── SLA timeout (Reader ko reply)             │
│       │       │       │       → [Expired] (hoàn 💎)                │
│       │       │       │                                             │
│       │       │       ├── Cả hai "Hoàn thành" → Giải ngân          │
│       │       │       │       → [Completed] (Reader 90%)            │
│       │       │       │                                             │
│       │       │       ├── Auto-complete (timeout)                   │
│       │       │       │       → [Completed] (Reader 90%)            │
│       │       │       │                                             │
│       │       │       └── Tố cáo → [Disputed]                      │
│       │       │               → Admin phán → [Completed]            │
│       │       │                                                     │
│       │       └── Reader Reject ──→ [Cancelled] (hoàn 💎)          │
│       │                                                             │
│       └── User hủy (chưa gửi tin) ──→ [Cancelled]                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

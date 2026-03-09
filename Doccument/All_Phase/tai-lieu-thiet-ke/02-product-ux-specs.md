# TarotWeb - Đặc tả sản phẩm và trải nghiệm người dùng (Product UX + Design Specs) v1.5

Nguồn: tách từ `FEATURE_REQUIREMENTS_BLUEPRINT_v1.5.md` (2026-03-06).
Mục tiêu: tập trung vào hành trình người dùng (user journeys), hành vi tính năng, trạng thái giao diện/trải nghiệm (UI/UX) và hướng dẫn thiết kế (design guideline).

Tiền tố ID tài liệu (Doc ID prefix): `UX-*`

---

## 2) Hành trình người dùng và nghiên cứu (User journeys & research)

### 2.6 Hành trình người dùng cốt lõi (Core user journeys) (phạm vi bàn giao UX (scope for UX delivery))
- Hành trình A (người dùng mới): Trang đích (Landing) -> Đăng ký (Register) -> xác minh email (verify email) -> rút 1 lá hằng ngày (Daily 1 card) -> kết quả AI (AI result) -> câu hỏi tiếp/bán thêm (Follow-up/upsell).
- Hành trình B (người dùng trả phí): Nạp tiền (Deposit) -> trải 3/5/10 lá -> câu hỏi tiếp (Follow-up) -> lịch sử (History) -> bán thêm thuê bao (Subscription upsell) (kích hoạt từ Giai đoạn (Phase) 1.5).
- Hành trình C (chợ reader): danh sách reader (Reader listing) -> mở hội thoại (open conversation) -> đóng băng ký quỹ (freeze) -> phản hồi (reply) -> xác nhận (confirm) -> giải phóng/hoàn tiền (release/refund).
- Hành trình D (giữ chân): thông báo (notification) -> rút bài hằng ngày (Daily draw) -> tiến độ chuỗi/nhiệm vụ (Streak/Quest progress) -> nhận thưởng (Reward claim).

Tiêu chí chấp nhận (Acceptance Criteria):
- Mỗi hành trình (journey) phải có luồng giao diện (UI flow) đầy đủ cho trạng thái `loading`, `empty`, `error`, `blocked`.
- Phía giao diện (FE) phải định nghĩa thông điệp lỗi thân thiện cho AI hết thời gian (timeout), số dư không đủ (insufficient balance), giới hạn tốc độ (rate-limit), và tranh chấp đang chờ (dispute pending).

### 2.7 Danh sách tương đương di động (Mobile parity checklist) (cổng Giai đoạn (Phase) 2)
- Bắt buộc tương đương (must parity): xác thực/phiên (auth/session), xem số dư ví/sổ cái (wallet balance/ledger view), xem bài cốt lõi (reading core), câu hỏi tiếp (follow-up), trò chuyện ký quỹ (chat escrow), thông báo cốt lõi (core notifications).
- Chỉ trên web tạm thời (web-only): bảng điều khiển quản trị (Admin dashboard), màn hình duyệt chi trả cho reader (reader payout approval console).
- Hạng mục ưu tiên di động (mobile-first) bổ sung: liên kết sâu (deep-link) từ thông báo đẩy (push notification), kết nối lại theo vòng đời ứng dụng (app lifecycle reconnect) cho SignalR/SSE.

Tiêu chí chấp nhận (Acceptance Criteria):
- Mọi API dùng ở di động (mobile) phải có hợp đồng phiên bản hóa (versioned contract) và kiểm thử tương thích ngược (backward compatibility test).

### 2.8 Cơ sở nghiên cứu người dùng và tạo mẫu (User research & prototyping baseline)
- Định nghĩa tối thiểu 3 chân dung người dùng (persona): người mới dùng Tarot (beginner tarot user), người dùng tâm linh quay lại (returning spiritual user), người dùng trả phí ở chợ reader (paying reader-marketplace user).
- Mỗi persona có nhiệm vụ cần hoàn thành (JTBD), điểm đau (pain points), và điểm kích hoạt giữ chân (retention trigger) chính.
- Trước khi chốt giao diện production (UI production), phải có bản mẫu (prototype) cho luồng cốt lõi (core flow) (register -> first reading -> follow-up/deposit).

Tiêu chí chấp nhận (Acceptance Criteria):
- Có kiểm tra khả dụng (usability check) tối thiểu 5 người dùng/nhóm persona cho luồng cốt lõi (core flow).
- phản hồi trọng yếu được ghi vào tồn đọng với mức ưu tiên rõ ràng.

---

## 4) Đặc tả UX tính năng (Feature UX specs)

## 4.1 Xác thực và tài khoản (Auth & account)

### 4.1.1 Đăng ký/Đăng nhập (Register/Login)
- Đăng ký (Register) với tên đăng nhập (username) + email + mật khẩu (password).
- Đăng nhập (Login) bằng email hoặc tên đăng nhập (username) + mật khẩu (password).
- Băm mật khẩu (password hash) bằng Argon2id (không lưu văn bản thô (plain text) ở bất kỳ nhật ký (log) nào).
- Bắt buộc xoay vòng refresh token (refresh token rotation) + ràng buộc thiết bị (device binding) theo dấu vân tay thiết bị/phiên (device/session fingerprint).

Tiêu chí chấp nhận (Acceptance Criteria):
- Email/tên đăng nhập (username) phải duy nhất (unique).
- Chính sách mật khẩu (password policy) phải được cưỡng chế (enforced).
- Luồng access token JWT + refresh token (JWT access + refresh token flow).
- Giới hạn tốc độ (rate limit) + khóa tạm thời sau nhiều lần sai.
- Có endpoint revoke phiên (session)/revoke-all-sessions.
- Nếu phát hiện dùng lại refresh token (refresh token reuse) -> thu hồi toàn bộ chuỗi refresh token (revoke chain) của người dùng (user) + buộc đăng nhập lại (force re-login).

### 4.1.2 MFA (Reader/Admin bắt buộc)
- Reader và Admin phải bật MFA trước khi dùng tính năng nhạy cảm (payout, admin actions).
- User thường có thể bật MFA tự nguyện.

Tiêu chí chấp nhận (Acceptance Criteria):
- Không cho thực hiện rút tiền/hành động quản trị quan trọng (withdrawal/admin critical actions) nếu tài khoản (account) bắt buộc MFA nhưng chưa xác minh (verify).
- Có audit nhật ký cho bật/tắt/reset MFA.

### 4.1.3 Xác minh email và quên mật khẩu (Email verify & forgot password)
- OTP xác minh email (verify email) (30 phút).
- Token/liên kết đặt lại mật khẩu (reset password token/link) (30 phút, dùng một lần).

Tiêu chí chấp nhận (Acceptance Criteria):
- OTP/token hết hạn không được dùng.
- Xác minh thành công (verify success) -> cộng `+5 Gold` qua giao dịch sổ cái (ledger transaction).

### 4.1.4 Vai trò (Roles)
- Vai trò (roles): Người dùng (User) / Tarot Reader / Quản trị viên (Admin).
- Nâng cấp lên Reader theo yêu cầu (request) + phê duyệt quản trị (admin approval).

Tiêu chí chấp nhận (Acceptance Criteria):
- Reader chỉ nhận hội thoại (conversation) mới khi đã được duyệt (approved) + đang nhận câu hỏi (`accepting_questions`).
- Mọi role change có audit nhật ký.
- Ràng buộc transport/phiên (session) security thuộc `ARCH-4.1.5` và được quản lý tại [03-tech-architecture.md](./03-tech-architecture.md).

---

## 4.2 Hồ sơ và thông tin reader (Profile & reader information)

### 4.2.1 Hồ sơ người dùng (User profile)
- Tên hiển thị (display name), ngày sinh (DOB), ảnh đại diện (avatar), danh hiệu đang dùng (active title).
- Tự động tính cung hoàng đạo + thần số học (auto-calc zodiac + numerology) từ DOB.

Tiêu chí chấp nhận (Acceptance Criteria):
- Cập nhật DOB -> tính lại dữ liệu liên quan (recalc).

### 4.2.2 Hồ sơ reader (Reader profile)
- Ảnh đại diện (avatar), danh hiệu (title), chuyên môn (specialties), mô tả (bio) (`vi`/`en`/`zh-Hans`), giá theo câu hỏi (pricing per question), trạng thái online/offline/accepting.
- Payout bank info được quản lý bảo mật.

Tiêu chí chấp nhận (Acceptance Criteria):
- Trạng thái reader cập nhật theo thời gian thực (realtime) cho danh sách (listing).
- Bank info đủ điều kiện mới tạo withdrawal yêu cầu (request).

---

## 4.4 Trải bài Tarot và AI (Tarot reading + AI)

### 4.4.1 Loại trải bài (Spread types)
- Daily 1 card (1/day).
- Trải 3/5/10 lá (cards) với câu hỏi tùy chọn (optional question).

Tiêu chí chấp nhận (Acceptance Criteria):
- Không duplicate card trong cùng phiên (session).
- Giới hạn hằng ngày (daily limit) dùng múi giờ nghiệp vụ (business timezone).
- Daily 1 card nếu có diễn giải AI (AI interpretation) vẫn tiêu tốn AI quota ngày; UI phải hiển thị thông điệp rõ (copy) như các luồng `free` khác.

### 4.4.4 An toàn AI và kiểm duyệt nội dung (AI safety & content moderation)
- Áp dụng lớp an toàn (safety layer) trước và sau khi gọi mô hình (model).
- Chặn/giảm mức nội dung mang tính kết luận y tế/pháp lý/tài chính.
- Bắt buộc hiển thị cảnh báo miễn trừ (disclaimer) khi prompt hoặc đầu ra (output) chạm nhóm nội dung nhạy cảm.
- Lưu prompt/response ở dạng khử định danh (de-identified) để QA và cải thiện prompt.
- Hành động kiểm duyệt (moderation actions):
 - `allow`: trả output chuẩn.
 - `soft_filter`: thay đoạn nhạy cảm bằng safe template + disclaimer.
 - `hard_block`: từ chối trả nội dung harmful/high-risk và trả hướng dẫn an toàn phù hợp.
- Nhóm rủi ro cao tối thiểu (high-risk categories):
 - khuyến khích tự hại/tự tử (self-harm/suicide encouragement).
 - bạo lực/thù ghét/đe dọa (violence/hate/threat).
 - hướng dẫn hoạt động bất hợp pháp (illegal activities instructions).
 - chỉ dẫn mang tính quyết định về y tế/pháp lý/tài chính (medical/legal/financial directives) thay thế chuyên gia.
 - nội dung bóc lột/lạm dụng (exploitation/abuse content).
- nhà cung cấp (provider) safety orchestration:
 - Nếu AI của nhà cung cấp (provider) đã chặn (block) theo chính sách (policy) của họ -> giữ trạng thái bị chặn (blocked) và áp chính sách nội bộ để hiển thị thông điệp an toàn.
 - Nếu nhà cung cấp (provider) không chặn (block) nhưng chính sách nội bộ xác định rủi ro cao (high-risk) -> nội bộ phải ghi đè (override) sang `soft_filter` hoặc `hard_block`.

Chính sách khử định danh (De-identification policy)
- Che dữ liệu (redaction) theo bộ phát hiện dựa trên luật (rule-based detector) cho: email, số điện thoại, tài khoản ngân hàng, mã định danh quốc gia, địa chỉ cụ thể, URL chứa token.
- Giả danh hóa (pseudonymization) mặc định là không đảo ngược (`hash + salt rotation`) cho phân tích (analytics)/QA thường.
- Ánh xạ có thể đảo ngược (reversible mapping) (nếu cần điều tra) phải nằm trong kho bảo mật tách biệt (secure vault), chỉ mở cho vai trò được ủy quyền và có vết kiểm toán (audit trail).
- Không lưu prompt/response dạng thô (raw) mặc định ở production nhật ký.
- Dữ liệu prompt/response chỉ lưu bản đã che (redacted) theo chính sách lưu giữ vận hành (retention policy) (mặc định 90 ngày, trừ trường hợp tranh chấp/tuân thủ).

Tiêu chí chấp nhận (Acceptance Criteria):
- Nội dung vi phạm chính sách (policy) được chuyển hướng (route) sang mẫu an toàn (safe template).
- Nội dung mức `hard_block` không được trả raw output từ mô hình.
- Nếu `hard_block` xảy ra ở bước kiểm tra trước (pre-check) trước khi gọi nhà cung cấp (provider call), yêu cầu (request) không được gọi mô hình (model) và không tiêu thụ (consume) quota/Diamond.
- Bộ kiểm thử kiểm duyệt (moderation test) phải bao phủ (cover) đầy đủ các nhóm rủi ro cao (high-risk categories) tối thiểu.
- Có gắn mã lý do (reason code) cho từng lần lọc/kiểm duyệt (filter/moderate) để kiểm toán (audit).
- Có bộ kiểm thử mẫu PII để xác nhận redaction không lọt dữ liệu nhạy cảm.

### 4.4.5 Quy tắc follow-up (Follow-up rules)
- Lượt miễn phí (free slots) theo cấp lá bài cao nhất (highest card level) trong phiên (session):
 - cấp (level) >= 6: miễn phí 1 lượt
 - cấp (level) >= 11: miễn phí 2 lượt
 - cấp (level) >= 16: miễn phí 3 lượt
- Bậc trả phí cơ bản (paid tiers base): [1, 2, 4, 8, 16] (các bậc sau nhân đôi, khớp với bảng bên dưới).
- Nếu lượt miễn phí (free slots) được dùng trước, phần trả phí (paid) bắt đầu ở bậc (tier) tiếp theo.
 - Ví dụ cấp 6: miễn phí, 2, 4.
 - Ví dụ cấp 11: miễn phí, miễn phí, 4.
- Câu hỏi tiếp (follow-up) chỉ được gọi AI nếu còn hạn mức ngày (daily quota); lượt miễn phí (free-slot) không bỏ qua hạn mức (bypass quota).
- Bảng tình huống:
| Cấp cao nhất (highest level) trong bộ bài | Lần 1 | Lần 2 | Lần 3 | Lần 4 | Lần 5 |
|---|---|---|---|---|---|
| < 6 | 1 | 2 | 4 | 8 | 16 |
| 6-10 | miễn phí | 2 | 4 | 8 | 16 |
| 11-15 | miễn phí | miễn phí | 4 | 8 | 16 |
| >= 16 | miễn phí | miễn phí | miễn phí | 8 | 16 |
- Giới hạn cứng (hard cap): tối đa 5 câu hỏi tiếp (follow-up)/phiên (session). Sau lần thứ 5, khóa hành động (action) tạo follow-up mới.

Tiêu chí chấp nhận (Acceptance Criteria):
- Tổng lượt gọi AI/ngày (AI calls/day) = lượt ban đầu (initial) + lượt follow-up đã qua chốt chặn (guard).
- Nếu giới hạn phiên (session cap) hoặc hạn mức ngày (quota/day) bị chặn, UI trả mã lý do rõ (reason code) (`FOLLOWUP_CAP_REACHED` hoặc `DAILY_QUOTA_EXCEEDED`).
- Khi hiển thị nhãn `free`, UI phải kèm copy rõ: `Miễn phí Diamond, vẫn tiêu tốn AI quota ngày`.

### 4.4.6 Bản địa hóa đầu ra AI (AI output localization)
- Mẫu prompt (prompt template) và chỉ dẫn hệ thống (system instruction) phải theo ngôn ngữ vùng (locale) của người dùng (`vi`, `en`, `zh-Hans`).
- Nếu mô hình (model) trả sai ngôn ngữ mục tiêu, chạy bước hậu xử lý dịch (post-process translate) về locale đang dùng.
- Giữ nguyên thuật ngữ Tarot chuẩn, chỉ dịch phần giải nghĩa tự nhiên.

Thứ tự dự phòng (Fallback order)
- Ưu tiên locale UI hiện tại (`vi` hoặc `en` hoặc `zh-Hans` ).
- Nếu mô hình fail locale mục tiêu:
 - Bước 1: tạo bản `en` ổn định.
 - Bước 2: thử dịch lại sang locale mục tiêu ban đầu tối đa 1 lần nếu translation service còn khả dụng.
- Nếu translate service fail: trả bản `en` + notice dự phòng.
- Từ điển thuật ngữ Tarot chuẩn hóa (`Major Arcana`, tên lá bài chuẩn) không bị dịch sai ngữ nghĩa.

Tiêu chí chấp nhận (Acceptance Criteria):
- 100% phiên đọc trả về đúng locale UI đang chọn, trừ khi đồng thời lỗi mô hình + dịch (fail model + translate) thì dự phòng (fallback) `en`.
- Không được lặp thử lại vô hạn (retry loop) ở bước dịch (translation step).
- Có ghi nhật ký (logging) `requested_locale`, `returned_locale`, `fallback_reason` để kiểm toán (audit).
- Có ma trận kiểm thử (test matrix) cho dấu tiếng Việt, tiếng Trung và thuật ngữ Tarot không dịch sai.

---

## 4.5 Tiến trình thẻ bài và hiệu ứng hình ảnh (Card progression & visual FX)

### 4.5.1 Bộ sưu tập và EXP (Collection & EXP)
- Kho bộ bài (card inventory): 78 lá (cards).
- EXP nhận được theo loại tiền tệ dùng để rút (EXP gain by draw currency).
- Cấp lá bài (card level) mở khóa ý nghĩa/lịch sử/hiệu ứng nâng cao (unlock meanings/history/advanced effects).

### 4.5.2 Cấp bậc hình ảnh (Visual tiers)
- L1-5: tĩnh (static).
- L6-10: ánh kim/hologram (holographic).
- L11-15: thị sai + hạt (parallax + particle).
- L16-20: hiệu ứng nguyên tố + âm thanh (elemental effects + sound).
- L21+: hiệu ứng thần thoại phá khung + hào quang + chữ ký (mythic breaking-frame + aura + signature).

Ngân sách hiệu năng (performance budget):
- Thiết bị di động (mobile) tầm trung phải duy trì >= 45 FPS ở màn hình lật/mở bài tiêu chuẩn (card reveal).
- Thời gian khung hình (frame time) mục tiêu: <= 22ms/frame cho cảnh chính (main scene).
- Ngân sách bộ nhớ (memory) cho tài nguyên hiệu ứng (FX assets) trên mobile: <= 150MB vùng làm việc (working set) cho màn hình xem bài (reading).
- Hồ sơ thấp (low profile): tắt particle dày và giảm lớp thị sai (parallax layer).
- Hồ sơ trung bình (medium profile): particle giới hạn mật độ + texture nén.
- Hồ sơ cao (high profile): đầy đủ FX theo bậc (tier).
- Server trả siêu dữ liệu (metadata) hồ sơ (profile) gợi ý, client tự hạ cấp khi phát hiện rớt khung hình liên tục (detect dropped frames).

Tiêu chí chấp nhận (Acceptance Criteria):
- Có báo cáo đo đạc hiệu năng (profiling report) theo 3 hồ sơ (profile) (low/medium/high) trước khi phát hành (release).
- Nếu vượt ngân sách (budget) thì tự hạ hồ sơ (downgrade profile), không làm ứng dụng bị văng (crash app).

### 4.5.3 Ascension (triển khai từ Giai đoạn (Phase) 1.5)
- Kích hoạt theo mốc cấp (trigger by level milestone).
- Câu chuyện do AI tạo (AI-generated stories) lưu theo người dùng/lá bài/điểm kích hoạt cấp (user/card/level trigger).

### 4.5.4 Cơ sở Gacha (Gacha baseline) (Giai đoạn (Phase) 1.5)
- Gacha là cơ chế mở gói (pack) trả phí bằng Diamond để nhận mảnh lá bài/phần thưởng thẩm mỹ (card shards/cosmetic rewards).
- Kho phần thưởng (reward pool) phải công bố tỷ lệ theo độ hiếm (rarity) và phiên bản (version).
- RNG của gacha dùng cùng khung công bằng (fairness framework) (seed có phiên bản + gói kiểm toán).
- Có luật bảo hiểm rơi thưởng (pity rule) cấu hình được (ví dụ sau N lượt không ra độ hiếm cao thì tăng xác suất).

Tiêu chí chấp nhận (Acceptance Criteria):
- Mỗi lượt mở gói (pack) có giao dịch (transaction) + tham chiếu sổ cái (ledger reference) + nhật ký thưởng chống lặp (idempotent reward log).
- Giao diện hiển thị tỷ lệ (odds) công khai trước khi người dùng (user) xác nhận mua.
- Tính năng (feature) phải tuân thủ chốt chặn tuân thủ theo địa lý (geo compliance gating) (khóa tại thị trường không cho phép).

---

## 4.6 Chợ reader và trò chuyện (chat) ký quỹ (Reader marketplace + chat escrow)

### 4.6.1 Danh bạ reader (Reader directory)
- Tìm kiếm/lọc theo giá, đánh giá, chuyên môn (search/filter by price, rating, specialty).
- Nút nhắn tin bị vô hiệu hóa (message button disabled) nếu reader không khả dụng (unavailable).

Reader moderation cơ sở (baseline):
- Luồng duyệt reader (reader approval flow) có danh sách kiểm tra (checklist) (độ đầy đủ hồ sơ, tuân thủ chính sách, trạng thái KYC).
- Hàng đợi kiểm duyệt đánh giá (rating moderation queue) cho đánh giá spam/độc hại/giả (spam/toxic/fake review).
- Tín hiệu phát hiện reader gian lận (fraud reader detection signals): tỷ lệ hoàn tiền cao, tỷ lệ tranh chấp cao, bất thường phản hồi (response anomaly).

Tiêu chí chấp nhận (Acceptance Criteria):
- Trường hợp bất thường (case) bị đưa vào hàng đợi rà soát thủ công (manual review queue) với mã lý do rõ ràng (reason code).
- Quản trị viên (admin) có thể tạm ẩn reader khỏi danh sách (listing) khi đang điều tra.

### 4.6.2 Trò chuyện thời gian thực (Chat realtime)
- SignalR cho nhắn tin (messaging).
- Phần đầu hội thoại (conversation header) + luồng tin nhắn (message stream) + trạng thái đã đọc (read state).

### 4.6.5 An toàn và kiểm duyệt trò chuyện (Chat safety & moderation) (user-reader)
- Áp bộ lọc chính sách (policy filter) cho spam, lạm dụng, quấy rối, nội dung cấm.
- Có nút báo cáo tin nhắn/hội thoại (report message/conversation) với mã lý do chuẩn hóa (reason codes).
- Tin nhắn bị gắn cờ (flagged message) có thể bị ẩn tạm (soft hide) trước khi admin rà soát (review).

Tiêu chí chấp nhận (Acceptance Criteria):
- Mọi hành động kiểm duyệt (moderation) (tự động/thủ công) đều có vết kiểm toán (audit trail).
- Có hàng đợi leo thang (escalation queue) cho trường hợp tái phạm hoặc mức độ nghiêm trọng cao (high severity).

---

## 4.9 Thông báo (Notifications)
- Thông báo trong ứng dụng (in-app notifications) cho nhiệm vụ, chuỗi, ascension, thanh toán, tranh chấp.

Tiêu chí chấp nhận (Acceptance Criteria):
- Theo dõi trạng thái đã đọc/chưa đọc (read/unread tracking).
- TTL cho thông báo cũ (stale notifications) (tùy loại).
- Mẫu email/push (email/push templates) phải vượt kiểm thử hiển thị Unicode (Unicode rendering test) cho `vi`, `en`, `zh-Hans` (không bị lỗi ký tự).

---

## 4.15 Phân loại lỗi UX (UX error taxonomy)

### 4.15.3 Cơ sở xử lý lỗi UX (UX error handling baseline)
- Phía FE phải ánh xạ ProblemDetails/mã lỗi (error code) thành thông điệp rõ ràng cho người dùng.
- Nhóm lỗi bắt buộc có UI hướng dẫn:
 - AI hết thời gian/đang chờ hoàn tiền (timeout/refund pending).
 - luồng miễn phí nhưng hết AI quota (`FREE_FLOW_QUOTA_BLOCKED`).
 - số dư không đủ (insufficient balance).
 - giới hạn tốc độ/thời gian chờ (rate-limit/cooldown).
 - bị chặn theo khu vực pháp lý (legal region blocked).
 - ký quỹ đang chờ tranh chấp (escrow dispute pending).

Tiêu chí chấp nhận (Acceptance Criteria):
- Không hiển thị lỗi kỹ thuật thô (raw technical error) cho người dùng cuối (end user).
- Có dữ liệu đo đạc (telemetry) cho mỗi nhóm lỗi UX để theo dõi điểm ma sát trải nghiệm (friction).

---

## 5) Chất lượng frontend (Frontend quality)

### Khả năng truy cập (Accessibility) (a11y)
- Mục tiêu tối thiểu WCAG 2.1 AA cho màn hình cốt lõi (core) (auth, reading, wallet, chat).
- Hỗ trợ điều hướng bàn phím (keyboard navigation), trạng thái tiêu điểm (focus states), tương phản (contrast) và co giãn chữ (text scaling).
- Có chế độ an toàn màu sắc (color-safe) cho người mù màu ở màn hình lá bài/độ hiếm (card/rarity).
- Kết quả xem bài (reading result) có đánh dấu thân thiện chuyển văn bản thành giọng nói (text-to-speech friendly markup).
- i18n a11y cơ sở (baseline):
 - aria-label/aria-description đầy đủ cho VI/EN/ZH.
 - kiểm tra trình đọc màn hình (screen reader) với văn bản tiếng Trung (zh-Hans) không bị đọc sai cấu trúc.
- Có danh sách kiểm tra a11y (a11y checklist) trong QA trước mỗi đợt phát hành (release).

### Tối ưu công cụ tìm kiếm (SEO)
- SSR/SSG cho trang đích/trang nội dung (landing/content pages).
- Siêu dữ liệu đầy đủ (rich metadata) cho bản xem trước mạng xã hội (social previews).
- Thiết lập `hreflang` đầy đủ cho `vi`, `en`, `zh-Hans` và canonical URL.
- Chuẩn hóa chiến lược locale (locale strategy) cho tiếng Trung (`zh-Hans`) theo thị trường mục tiêu (target market).

---

## 4.16 Điểm danh hằng ngày (Daily Check-in) (C4 fix – bổ sung UX spec thiếu)

### 4.16.1 Luồng UX điểm danh (Check-in UX flow)
- **Vị trí:** Nút điểm danh hiển thị nổi bật trên trang chủ (home) hoặc sidebar, kế bên widget Daily Card.
- **Trạng thái nút:**
  - **Chưa điểm danh hôm nay:** Nút sáng (highlighted), có animation nhẹ (pulse/glow) thu hút bấm.
  - **Đã điểm danh hôm nay:** Nút mờ (disabled), hiển thị dấu tích (✓) và text "Đã điểm danh".
- **Khi bấm nút:**
  1. Gọi API `POST /api/v1/checkin`.
  2. Hiển thị animation thưởng (reward popup): "+N Gold" với hiệu ứng coin rơi (coin drop).
  3. Cập nhật streak counter trên UI (streak +1 nếu liên tiếp).
  4. Nếu đã điểm danh (idempotent): hiển thị toast "Bạn đã điểm danh hôm nay rồi".
- **Widget Streak:** Hiển thị bên cạnh nút check-in hoặc trong profile:
  - Số ngày streak hiện tại (current_streak).
  - Ngày hoạt động gần nhất (last_streak_date).
  - Trạng thái: "đang duy trì" (active) hoặc "bị gãy" (broken).
  - Nếu streak vừa gãy: hiển thị CTA "Mua Streak Freeze" với giá `ceil(pre_break/10)` Diamond.
- **Error states:**
  - Lỗi mạng: toast "Không thể điểm danh, vui lòng thử lại".
  - Server error: map ProblemDetails error code → thông điệp thân thiện theo `UX(4.15.3)`.

### 4.16.2 Lịch điểm danh (Check-in calendar) (tùy chọn Phase 5+)
- Hiển thị lịch tháng với các ngày đã điểm danh được đánh dấu (highlight).
- Streak freeze đã dùng: hiển thị icon riêng trên ngày tương ứng.

---

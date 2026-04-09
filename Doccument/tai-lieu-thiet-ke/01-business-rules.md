# TarotWeb - Quy tắc kinh doanh (Business Rules) v1.5

Nguồn: tách từ `FEATURE_REQUIREMENTS_BLUEPRINT_v1.5.md` (2026-03-06).
Mục tiêu: tập trung vào quy tắc kinh doanh, kiếm tiền (monetization), lộ trình (roadmap) và chỉ số hiệu suất chính (KPI).

Tiền tố ID tài liệu (Doc ID prefix): `BR-*`

---

## 1) Phạm vi sản phẩm (Product scope) và mục tiêu

- Sản phẩm: ứng dụng web xem bài Tarot + diễn giải AI (AI interpretation) + trò chơi hóa (gamification) + chợ reader (reader marketplace).
- Ngôn ngữ UI hỗ trợ: `vi`, `en`, `zh-Hans`.
- Mục tiêu kinh doanh:
 - Tăng giữ chân (retention) (daily habit, quest, collection progression).
 - kiếm tiền (monetization) ổn định (Diamond, subscription, reader chat, event packs).

### 1.4 Định vị cạnh tranh (Competitive positioning) (cơ sở (baseline))
- Nhóm đối thủ tham chiếu: ứng dụng Tarot thiên giáo dục (education), ứng dụng Tarot thiên ghi chép trải nghiệm (journaling), và ứng dụng tâm linh có chợ reader (marketplace).
- Điểm khác biệt mục tiêu của TarotWeb:
 - Reader escrow + độ chính xác tài chính (financial correctness) làm lớp tin cậy cốt lõi (core trust layer).
 - diễn giải AI (AI interpretation) theo thời gian thực (realtime) với cơ chế an toàn hoàn tiền (refund safety).
 - trò chơi hóa (gamification) theo tiến trình (progression) + điều phối sự kiện/thuê bao (event/subscription orchestration).

---

## 2) Lộ trình theo giai đoạn (Phase rollout)

*Nguyên tắc: mỗi phase con (1.1, 1.2, ...) chỉ chứa 1–3 tính năng; ưu tiên đạt Phase 3 (Mobile) nhanh; phần tốn thời gian/không cốt lõi đẩy xuống sau Phase 3 hoặc Phase 4.*

---

### Phase 1: Trải bài Tarot + phát luồng AI (Tarot reading + AI streaming)

**1.1** Xác thực cơ bản (Auth baseline)
- Đăng ký / đăng nhập (email + mật khẩu).
- Xác minh email (verify email) + OTP.
- JWT + refresh token rotation.

**1.2** Ví + sổ cái tối thiểu (Wallet + ledger minimum)
- Ví Gold/Diamond + sổ cái giao dịch (ledger).
- Mô hình ghi sổ kép logic (double-entry).
- Thưởng đăng ký +5 Gold sau verify email.

**1.3** Trải 1 lá hằng ngày (Daily 1 card)
- Rút 1 lá/ngày (Daily spread).
- RNG công bằng (CSPRNG, Fisher-Yates, audit package).
- Không trùng lá trong phiên.

**1.4** Trải 3/5/10 lá (Spread 3/5/10 cards)
- Loại trải 3 lá, 5 lá, 10 lá.
- Câu hỏi tùy chọn (optional question).
- Định giá theo spread (Gold/Diamond).

**1.5** Phát luồng AI (AI streaming)
- SSE token streaming cho diễn giải.
- Tích hợp Grok/ChatGPT API.
- Timeout/retry + refund tự động khi lỗi provider.

**1.6** Follow-up + lịch sử (Follow-up + history)
- Quy tắc follow-up (free slots theo cấp lá, bậc trả phí [1,2,4,8,16]).
- Tối đa 5 follow-up/phiên.
- Lịch sử trải bài (Reading history) cơ bản.

**1.7** Pháp lý + profile cơ bản (Legal + profile baseline)
- Cổng tuổi (age gate) 18+, TOS, Privacy Policy, AI disclaimer.
- Hồ sơ người dùng cơ bản (display name, avatar, DOB).
- Nạp tiền (deposit) 1 phương thức tối thiểu (VietQR/Bank).

---

### Phase 2: Reader listing + trò chuyện + ký quỹ (Reader marketplace + chat escrow)

**2.1** Danh sách reader (Reader listing)
- Danh bạ reader (tìm kiếm, lọc giá/đánh giá/chuyên môn).
- Hồ sơ reader (avatar, bio, giá, trạng thái online).
- Luồng duyệt reader (reader approval) cơ bản.

**2.2** Trò chuyện 1-1 (1-1 chat)
- SignalR realtime messaging.
- Luồng tin nhắn + trạng thái đã đọc.
- Kiểm duyệt chat cơ bản (spam, báo cáo).

**2.3** Ký quỹ cốt lõi (Escrow core)
- Máy trạng thái ký quỹ (freeze → reply → release/refund).
- Timer phản hồi (24h), auto-release, auto-refund.
- Tranh chấp (dispute) cơ bản + admin xử lý.

**2.4** Hành chính tối thiểu (Minimal admin)
- Admin dashboard: quản lý user, nạp/rút, tranh chấp.
- Rút tiền reader thủ công (manual payout).

---

### Phase 3: Ứng dụng di động (Mobile App)

*Mục tiêu: ra mobile nhanh; chỉ parity cốt lõi, không mở rộng tính năng mới.*

**3.1** Khung mobile + Auth (Mobile scaffold + auth)
- React Native (Expo) + TypeScript.
- Xác thực (auth) tương đương web (JWT, refresh, secure storage).

**3.2** Ví + Reading parity (Wallet + reading parity)
- Xem số dư ví, sổ cái.
- Trải bài (1/3/5/10 lá) + AI streaming (SSE).

**3.3** Chat parity (Chat parity)
- Trò chuyện 1-1 với reader.
- Ký quỹ (escrow) trong chat.

**3.4** Thông báo + deep-link (Notifications + deep-link)
- Push notification cơ bản.
- Deep-link từ thông báo đẩy.

---

### Phase 4: Cộng đồng (Community)

**4.1** Cộng đồng (Community)
- Tính năng cộng đồng (forum/feed/chủ đề theo tài liệu).
- Kiểm duyệt nội dung cộng đồng.

---

### Phase 5+: Sau mobile – giữ chân, kiếm tiền, mở rộng

*Đẩy xuống sau Phase 3 để ưu tiên ra mobile nhanh.*

**5.1** Giữ chân (Retention)
- Điểm danh hằng ngày (daily check-in).
- Chuỗi hằng ngày (daily streak) + Streak Freeze.
- Thông báo (notifications) đầy đủ.
- Vòng lặp giữ chân (D0, Daily Habit, Streak Rescue, Session Completion, Collection Progress, Reader Relationship, Quest Cadence, Win-back).

**5.2** Kiếm tiền nâng cao (Monetization)
- Thuê bao theo tháng/năm (subscription).
- Chuẩn hóa entitlement (nhiều gói song song).
- Gói sự kiện (event packs), Ascension, Gacha.
- Điều phối kiếm tiền (First Purchase, Intent-based Offer, Streak Protection, Event Pack, Escrow Upsell, Refund Recovery, High-value Segment).

**5.3** Trò chơi hóa (Gamification)
- Bảng xếp hạng (leaderboards), thành tựu (achievements), nhiệm vụ (quests).
- Thưởng chia sẻ (share reward) + giới thiệu (referral) cốt lõi.

**5.4** Gia cố nền tảng (Platform hardening)
- Rate limiting đầy đủ (auth/chat/payment/webhook).
- OpenTelemetry (API, jobs, realtime).
- Background jobs (settlement/refund/release) gia cố.

**5.5** Mở rộng không cốt lõi (Non-core expansion)
- Thưởng chia sẻ nâng cao (proof-share, anti-abuse scoring).
- Đánh giá/báo cáo (reviews/reports) đầy đủ.
- KYC reader chính thức (Phase 1 dùng xác minh thủ công).

---

### Phân loại chức năng cốt lõi và không cốt lõi (Core vs Non-core)

- **Cốt lõi trước/sát mobile:** Auth, Ví, Reading AI (1/3/5/10 cards + streaming), Chat escrow, Nạp tiền cơ bản, Pháp lý.
- **Sau Phase 3 (mobile):** Thuê bao, Event pack, Ascension, Gacha, Leaderboard/Quest/Achievement, Retention loops, Monetization orchestration.
- **Không cốt lõi (Phase 4+):** Cộng đồng, Thưởng chia sẻ nâng cao.

---

## 3) Quyết định kinh doanh (Business decisions)

1. Thưởng đăng ký (Register bonus):
- `+5 Gold` chỉ cấp sau khi xác minh email (verify email) thành công.

2. Thứ tự ưu tiên định giá follow-up (Follow-up pricing precedence):
- Lượt hỏi tiếp miễn phí (free follow-up) theo cấp lá bài (level card) được áp dụng trước.
- Sau khi dùng hết lượt miễn phí (free slots), các mốc trả phí (paid) giữ nguyên theo thứ tự còn lại.
- Ví dụ cấp 6 (free 1): lần 1 miễn phí, lần 2 chi phí 2, lần 3 chi phí 4, lần 4 chi phí 8, lần 5 chi phí 16.
- Giới hạn cứng: tối đa 5 câu hỏi tiếp (follow-up) trong 1 phiên xem bài (reading session) (không cho hỏi thêm sau mốc này).

3. Mô hình ký quỹ (Escrow model) cho hội thoại (conversation):
- 1 hội thoại (conversation) có 1 phiên tài chính (finance session).
- Diamond bị đóng băng theo tổng số câu hỏi đã chấp nhận (main + add-question).
- Khi kết thúc hội thoại (conversation), hệ thống giải phóng/hoàn tiền (release/refund) theo một luồng quyết toán (settlement) cho reader/người dùng (user).

4. Xác minh thưởng chia sẻ (Share reward verification):
- Chấp nhận heuristic chống lạm dụng (anti-abuse heuristic).
- Không bắt người dùng (user) dán link chia sẻ trong cơ sở (baseline) MVP; mô-đun bằng chứng chia sẻ (proof-share module) được tách sang Giai đoạn (Phase) 5.5.

5. Điểm bảng xếp hạng (Leaderboard scores):
- Có 3 nhánh điểm (score track) chạy song song:
 - `daily_rank_score`
 - `monthly_rank_score`
 - `lifetime_score`

6. Mô hình thuê bao (Subscription model):
- 1 người dùng (user) có thể có nhiều thuê bao (subscription) đang hoạt động (active) đồng thời.
- Mỗi thuê bao (subscription) đóng góp quyền lợi (entitlement) riêng (ví dụ trải 3 lá miễn phí/ngày và trải 5 lá miễn phí/ngày).
- quyền lợi (entitlement) được tính theo khóa (key), đặt lại theo ngày nghiệp vụ UTC (business date), và hết hạn độc lập theo từng gói.

7. Mô hình chuỗi hằng ngày (Daily streak model):
- Mỗi ngày streak tăng thêm +1% EXP cho thẻ rút được.
- Công thức giá Streak Freeze: `ceil(pre_break_streak_days / 10)` Diamond.
- Mốc neo (anchor): dùng streak của ngày ngay trước khi chuỗi bị gãy (`pre_break_streak_days`).
- Khi `pre_break_streak_days = 0` (chưa từng có streak), không áp dụng Streak Freeze.
- Ví dụ: streak 7 -> 1 Diamond, streak 11 -> 2 Diamond.

8. Hạng mục gia cố nền tảng (Platform hardening track):
- giới hạn tốc độ (rate limiting), OpenTelemetry, tác vụ nền (Background jobs) (settlement/refund/release) triển khai theo Phase 5.4; có thể song song hoặc sau mobile để ưu tiên ra mobile nhanh.

9. Cập nhật lộ trình theo giai đoạn (phase roadmap update):
- Phase 1: Trải bài Tarot + phát luồng AI (1/3/5/10 cards + streaming).
- Phase 2: Reader listing + chat 1-1 + ký quỹ cốt lõi (escrow core).
- Phase 3: Ứng dụng di động (mobile app) — ưu tiên đạt nhanh.
- Phase 4: Cộng đồng (Community).
- Phase 5+: Vòng lặp giữ chân (retention loops), điều phối kiếm tiền (monetization orchestration), Ascension/Gacha/leaderboard, gia cố nền tảng.

10. Gói điều phối tăng trưởng (Growth orchestration pack):
- Triển khai đầy đủ giữ chân (retention) loops + kiếm tiền (monetization) orchestration sau Phase 3 (mobile).

11. Mô hình công bằng RNG (RNG fairness model):
- Phiên rút bài phải phát lại/tái hiện được (replay/reproduce) để xử lý tranh chấp (dispute).
- Không được trùng lá trong cùng một phiên xem bài (reading session).

12. Mô hình đồng thời tài chính (Finance concurrency model):
- Mọi chuyển trạng thái tài chính (finance transition) (freeze/add/release/refund) bắt buộc chống lặp (idempotent).
- Lệnh tài chính nhạy cảm phải chạy với mức cô lập (isolation) `SERIALIZABLE` hoặc mẫu `SELECT ... FOR UPDATE` + khóa phân tán (distributed lock) tương đương.
- Không cho đóng băng kép/giải phóng kép/hoàn tiền kép (double-freeze/double-release/double-refund).

13. Đối soát thanh toán và chargeback (Payment reconciliation & chargeback):
- Có tác vụ (job) đối soát định kỳ giữa giao dịch của nhà cung cấp (provider transactions) và sổ cái nội bộ (internal ledger).
- Sai lệch (mismatch) vượt ngưỡng cảnh báo phải vào hàng đợi rà soát (review queue).
- Có trạng thái chargeback/disputed và quy trình khóa quyền lợi (entitlement) tạm thời nếu cần.

14. Mô hình AI hết thời gian/hoàn tiền (AI timeout/refund model):
- Chỉ hoàn tiền (refund) tự động cho lỗi kết thúc (terminal fail) do nhà cung cấp (provider)/hệ thống (system) (`failed_before_first_token` hoặc `failed_after_first_token`) sau khi đã hết ngân sách thử lại (retry budget).
- Nếu phía khách (client) tự đóng tab/ngắt kết nối, phía máy chủ (backend) vẫn theo dõi trạng thái hoàn tất (completion) từ nhà cung cấp (provider) và không tự hoàn tiền (auto-refund) chỉ vì ngắt kết nối từ client.
- Mỗi yêu cầu (request) AI có số lần thử lại (retry) giới hạn (`max_retry_per_request = 1`) + cơ chế ngắt mạch (circuit breaker), mọi hoàn tiền (refund) đều ghi sổ cái (ledger) + thông báo người dùng (notify user).

15. Cơ sở pháp lý và tuân thủ (legal & compliance baseline):
- Người dùng (user) tối thiểu 18 tuổi (hoặc theo luật địa phương nếu cao hơn).
- Bắt buộc có trang Điều khoản sử dụng/Chính sách riêng tư/Tuyên bố miễn trừ AI (TOS/Privacy/AI disclaimer pages) trước khi chạy môi trường production (go-live).
- Reader nhận payout phải qua KYC theo ngưỡng rút/cộng dồn. Hệ thống KYC chính thức triển khai Giai đoạn (Phase) 2.1; Phase 1 dùng xác minh thủ công bởi admin.

16. Thuật toán quyền lợi thuê bao (subscription entitlement algorithm):
- quyền lợi (entitlement) được tính theo khóa (key), tiêu thụ theo quy tắc xác định (deterministic) ưu tiên hết hạn sớm (earliest-expiry-first).
- Không cho tính đếm kép (double-count) giữa nhiều thuê bao (subscription) cùng lúc.
- Có bảng ví dụ/tình huống chuẩn để QA và product kiểm thử thống nhất.

17. Thứ tự hạch toán hạn mức AI (AI quota accounting precedence) :
- Đơn vị hạn mức (quota unit) là `per-request`, tính cho lần diễn giải ban đầu (initial interpretation) và từng câu hỏi tiếp (follow-up).
- Mọi lượt rút miễn phí/câu hỏi tiếp (free draw/follow-up) có gọi AI vẫn tiêu tốn hạn mức AI theo ngày (daily AI quota) như yêu cầu (request) thường (không có bỏ qua mặc định).
- Thứ tự chốt chặn (guard order) bắt buộc trước mỗi lần gọi AI (AI call):
 1) phiên (session) cap (max 5 follow-up/session).
 2) Hạn mức AI theo ngày (daily AI quota) theo bậc (tier).
 3) giới hạn tốc độ (rate-limit) theo endpoint/user/phiên (session).
 4) Quy tắc free/paid follow-up và số dư Diamond.
- Yêu cầu (request) không qua chốt chặn (fail guard) thì không gọi mô hình (model).

18. Hoàn tác tác dụng phụ hoàn tiền AI (AI refund side-effect rollback) :
- `completed` thì không rollback quota.
- `failed_before_first_token` hoặc `failed_after_first_token` thì hoàn Diamond (nếu đã trừ) + rollback quota/quyền lợi (entitlement).
- `failed_after_first_token` chỉ áp dụng cho lỗi nhà cung cấp (provider)/hệ thống (system), không áp dụng cho ngắt kết nối phía khách (client disconnect).
- Rollback/refund phải idempotent theo `ai_request_id`.

Ma trận kết quả thử lại (Retry outcome matrix) (business view):
| Trạng thái cuối cùng | Điều kiện retry | Quota | Diamond |
|---|---|---|---|
| `completed` | không áp dụng | giữ consume | không refund |
| `failed_before_first_token` | sau khi hết retry budget (`max_retry_per_request = 1`) | rollback 1 unit | refund full yêu cầu (request) charge |
| `failed_after_first_token` | sau khi hết retry budget (`max_retry_per_request = 1`) | rollback 1 unit | refund full yêu cầu (request) charge |

19. Mốc thời gian ký quỹ (Escrow timer anchors) :
- **N3 fix:** `offer_expires_at = created_at + offer_timeout_hours` (mặc định 12h, configurable qua `system_configs`). Reader không chấp nhận (accept) trước hạn → tự động hủy (auto-cancel), hoàn tiền đã freeze (nếu đã freeze).
- `reader_response_due_at = accepted_at + 24h` (theo thời điểm câu hỏi được accept/freeze).
- `auto_release_at = replied_at + 24h` nếu không có confirm/tranh chấp (dispute).
- Với hội thoại (conversation) có nhiều câu hỏi được chấp nhận (accepted) (main + add-question), `accepted_at`/`replied_at`/bộ hẹn giờ (timers) phải theo từng mục câu hỏi (question item); quyết toán cuối (settlement) được gom (aggregate) vào 1 phiên tài chính (finance session).
- `dispute_window` dùng mốc neo (anchor) thống nhất theo nhánh quyết toán (settlement):
 - Nhánh người dùng xác nhận/giải phóng sớm (user confirm/release): `dispute_window_start = release_at`.
 - Nhánh tự động giải phóng (auto release): `dispute_window_start = auto_release_at`.
 - Nhánh không phản hồi, tự động hoàn tiền (no-response auto refund): `dispute_window_start = auto_refund_at`.
- `dispute_window_end = dispute_window_start + 24h` (configurable theo legal policy).
- Tất cả bộ hẹn giờ (timer) dùng UTC, hiển thị giao diện (UI) theo múi giờ người dùng (user timezone).

20. Chính sách xoay vòng secret RNG và phát lại (RNG secret rotation replay policy):
- Rotation không được làm mất khả năng replay phiên cũ.
- Secret cũ phải được giữ tối thiểu bằng thời hạn lưu giữ bằng chứng (evidence retention) tranh chấp (dispute) (>= 24 tháng) hoặc lâu hơn theo chính sách pháp lý (legal policy).
- Công cụ phát lại (replay tool) chỉ mở cho vai trò (role) được ủy quyền, bắt buộc MFA + phê duyệt kép (dual-approval) + vết kiểm toán (audit trail).
- Nếu secret bị lộ (compromise): khóa phiên bản (version) ngay, xoay khóa khẩn cấp (rotate), và chạy rà soát sự cố (incident review) cho các phiên bị ảnh hưởng.

21. Chính sách tỷ giá và làm tròn (FX & rounding policy) :
- Diamond định giá chuẩn theo VND: `1 Diamond = 1,000 VND`.
- Với nhà cung cấp (provider) không quyết toán VND (settle VND, ví dụ PayPal), dùng `fx_rate_snapshot` tại thời điểm ghi nhận (capture).
- Thuật toán quy đổi chuẩn:
 1) Chuẩn hóa `provider_amount` theo minor units của currency nhà cung cấp (provider).
 2) Quy đổi sang VND bằng `fx_rate_snapshot` cố định tại thời điểm capture.
 3) `diamond_decimal = normalized_vnd / 1000`.
 4) Credit Diamond dùng `floor` (round toward zero) để ra `diamond_credited` kiểu số nguyên.
- Nếu có chênh lệch giữa thời điểm ghi nhận (capture) và quyết toán (settle), hoặc hoàn tiền một phần (partial refund), dùng `fx_rate_snapshot` và `diamond_credited` gốc để tính hoàn tác theo tỷ lệ (reversal/pro-rata); không dùng tỷ giá thời gian thực (realtime) tại lúc hoàn tiền (refund).
- Ngưỡng sai lệch cho đối soát (reconciliation tolerance threshold):
 - lệch <= 0.1% hoặc <= 1 minor unit tiền tệ nhà cung cấp (provider) (lấy ngưỡng lớn hơn) -> auto-accept và nhật ký.
 - lệch vượt ngưỡng -> đưa vào rà soát hàng đợi với reason `FX_MISMATCH_OVER_THRESHOLD`.
- Ví dụ hoàn tiền một phần (partial refund): ghi có gốc `10 Diamond`, hoàn `30%` số tiền gốc (amount) -> hoàn tác `3 Diamond` (xác định được (deterministic) theo tỷ lệ số tiền đã capture/credit ban đầu).

22. Khóa tính năng theo tuân thủ địa lý (Geo compliance feature gating) :
- Các tính năng RNG kiếm tiền (monetization) (ví dụ Gacha/event RNG) phải có chốt chặn theo quốc gia/khu vực (country/region gate) dựa trên ma trận pháp lý (legal matrix).
- Nếu thị trường cấm/hạn chế, tính năng (feature) phải tự ẩn và chặn API mua (purchase API) theo cờ địa lý/chính sách (geo/policy flag).
- Server-side API enforcement là bắt buộc cho mọi endpoint nhạy cảm (`purchase`, `payment`, `payout`, `gacha_open`, `event_pack_claim`), không chỉ UI hide.
- Với vòng lặp thưởng (reward loops) có yếu tố ngẫu nhiên (chance), API nhận thưởng (claim reward) cũng phải đi qua cùng chốt chặn địa lý phía máy chủ (server-side geo gate).
- Geo decision phải dựa trên multi-signal (`account_jurisdiction`, KYC country, payment country, IP geolocation).
- Nếu tín hiệu mâu thuẫn hoặc phát hiện rủi ro VPN/proxy cao (VPN/proxy risk) -> vào trạng thái `restricted_review`.
- `restricted_review` phải có SLA xử lý:
 - tự động xử lý (auto-resolve) trong <= 24h nếu có tín hiệu mới đủ tin cậy.
 - quá SLA phải nâng cấp xử lý (escalate) sang hàng đợi rà soát thủ công (manual review queue).
- Ngưỡng auto-resolve mẫu (phải config được):
 - `kyc_verified = true`
 - `payment_country = account_jurisdiction`
 - `ip_geo_consistency_score >= 0.95`
 - không có cờ `vpn_proxy_risk`.

23. Gacha kiếm tiền (monetization) safeguards:
- Gacha phải công bố tỷ lệ (odds) và phiên bản (version) rõ ràng trước khi mua (purchase).
- Mọi thao tác mua (purchase) và gán thưởng (reward assignment) phải chống lặp (idempotent) + kiểm toán được (auditable).
- Luật bảo hiểm rơi thưởng (pity rule) (nếu bật) phải xác định được (deterministic) theo cấu hình có phiên bản (versioned).
- Bắt buộc có hợp đồng công bố công khai (public disclosure contract) (JSON schema có phiên bản) cho `odds_version`, `rarity_pool`, `probabilities`, `effective_from`, `effective_to`.
- Nhật ký thưởng (reward logs)/ánh xạ phiên bản tỷ lệ (odds version mapping) phải được giữ tối thiểu bằng thời hạn lưu giữ tranh chấp (dispute retention) để xử lý khiếu nại sai lệch tỷ lệ (mismatch odds).
- Khi có hoàn nguyên/lộ lọt (rollback/compromise) của phiên bản tỷ lệ (odds version), phải có luồng tranh chấp (dispute flow) + phê duyệt pháp lý (legal sign-off) theo khu vực pháp lý (jurisdiction).

Chi tiết triển khai kỹ thuật/compliance: xem [03-tech-architecture.md](./03-tech-architecture.md) và [04-ops-security-compliance.md](./04-ops-security-compliance.md).

---

## 4.3 Ví, thanh toán và thuê bao (Wallet, payment, subscription)

### 4.3.1 Mô hình ví (Wallet model)
- Gold: tiền tệ miễn phí (free currency).
- Diamond: tiền tệ trả phí (paid currency), tỷ giá cố định 1 Diamond = 1,000 VND.
- Mọi biến động số dư phải có một dòng sổ cái (ledger row).
  - **Ngoại lệ:** `frozen_diamond_balance` khi escrow release – `diamond_balance` payer không đổi nên không ghi ledger row cho payer. Frozen balance được audit qua view `v_user_frozen_ledger_balance` (xem `database/DESIGN_DECISIONS.md` mục 7.1).
- sổ cái (ledger) tài chính theo mô hình ghi sổ kép logic (double-entry logical model) (cặp ghi nợ/ghi có (debit/credit pair)) để hỗ trợ đối soát và điều tra kỹ thuật số (forensic).
- Độ chính xác tiền tệ (money precision):
 - Diamond lưu số nguyên (`BIGINT`).
 - Tiền của nhà cung cấp (provider) lưu `DECIMAL` theo đơn vị tiền tệ nhỏ nhất (currency minor units).

Tiêu chí chấp nhận (Acceptance Criteria):
- Không âm số dư.
- sổ cái (ledger) đối soát được theo reference.
- Mọi chuyển trạng thái tài chính (finance transition) phải truy vết được theo chuỗi: thay đổi số dư (balance change) -> bút toán sổ cái (ledger entries) -> tham chiếu ngoài (external reference).

### 4.3.2 Nạp tiền (Deposit)
- Phương thức (methods): ngân hàng/ATM, VietQR, PayPal.
- Khuyến mãi (promotion) theo mốc nạp + cửa sổ sự kiện (event windows).
- Với phương thức không phải VND, bắt buộc chụp `fx_rate_snapshot` tại thời điểm capture để quy đổi Diamond.
- Nếu nhà cung cấp (provider) xử lý theo mô hình hold->capture/settle, phải lưu cả `captured_at` và `settled_at` để đối soát chênh lệch.

Tiêu chí chấp nhận (Acceptance Criteria):
- Callback thanh toán phải chống lặp (idempotent payment callback).
- Không ghi có kép (double-credit).
- Bắt buộc xác thực chữ ký webhook (verify webhook signature) theo từng nhà cung cấp (provider).
- Có chính sách thử lại (retry policy) + hàng đợi lỗi (dead-letter queue) cho callback lỗi tạm thời.
- Có tác vụ đối soát định kỳ (reconciliation job): đối soát `provider_txn_id` với sổ cái nội bộ (internal ledger) và đẩy sai lệch (mismatch) vào hàng đợi rà soát (review queue).
- Có trạng thái `disputed/chargeback` và luồng xử lý hoàn/khóa quyền lợi (entitlement) tạm thời khi có chargeback.
- Lưu đầy đủ `provider_amount`, `provider_currency`, `fx_rate_snapshot`, `diamond_credited`.
- Hoàn tiền một phần (partial refund) phải tính hoàn tác Diamond (reversal) theo tỷ lệ số tiền gốc (amount) đã ghi có (credit) (pro-rata, deterministic) và ghi sổ cái (ledger) riêng.

### 4.3.3 Rút tiền (Withdrawal)
- Yêu cầu rút tiền của reader được xử lý thủ công bởi quản trị viên (manual by admin).
- Phase 1: xác minh thủ công bởi admin; hệ thống KYC chính thức triển khai Phase 2.1 (xem mục 15).
- Phí nền tảng (platform fee): 10%.
- Tối thiểu 50 Diamond/yêu cầu (request), tối đa 1 yêu cầu (request)/ngày.
- Ranh giới của giới hạn `1 request/ngày` được tính theo ngày nghiệp vụ (business date) UTC.
- Reader phải KYC đạt chuẩn trước khi nhận payout.
- Áp dụng KYC nâng cao (enhanced KYC) khi tổng rút theo kỳ vượt ngưỡng cấu hình.
- Nếu tài khoản (account) đang ở trạng thái `chargeback/disputed hold`, chặn chi trả (payout) cho đến khi rà soát (review) xong.

Tiêu chí chấp nhận (Acceptance Criteria):
- Quy tắc min/max được thực thi (enforce) ở DB + dịch vụ (service).
- Withdrawal yêu cầu (request) bị chặn nếu KYC chưa đủ.
- Có vết kiểm toán (audit trail) cho người duyệt, thời điểm duyệt và lý do từ chối.
- Có trạng thái tạm giữ/rà soát (hold/review) cho trường hợp (case) liên quan tranh chấp (dispute)/chargeback.
- SLA chi trả thủ công (manual payout): xử lý <= 24h cho trường hợp chuẩn; tồn đọng (backlog) > 24h phải tạo sự cố vận hành (incident).
- Điều kiện kích hoạt (trigger) đánh giá tự động hóa chi trả (payout): khi 3 tháng liên tiếp tồn đọng chi trả thủ công (manual payout backlog) > 5% tổng yêu cầu (request) hoặc vượt ngưỡng sản lượng (volume) cấu hình.

### 4.3.4 Thuê bao (Subscription)
- Cho phép nhiều thuê bao (subscription) hoạt động (active) cùng lúc cho 1 người dùng (user).
- Mỗi gói có quyền lợi (entitlement) riêng theo loại trải bài/quyền lợi (spread/benefit) (ví dụ rút 3 lá miễn phí/ngày, rút 5 lá miễn phí/ngày).
- Theo tháng/theo năm (monthly/yearly) là chu kỳ thời hạn của từng gói.

Thuật toán quyền lợi (entitlement) :
- quyền lợi (entitlement) lưu theo key (ví dụ: `free_spread_3_daily`, `free_spread_5_daily`).
- Khi người dùng (user) dùng quyền lợi (entitlement) cùng khóa (key) từ nhiều gói, hệ thống tiêu thụ theo quy tắc `earliest-expiry-first`.
- Nếu nhiều quyền lợi (entitlement) cùng khóa (key) có cùng thời điểm hết hạn (expiry), quy tắc phân xử (tie-breaker) bắt buộc theo thứ tự xác định (deterministic) (`subscription_id` tăng dần).
- Không tự động trừ chéo khóa (auto cross-key deduction) nếu không có quy tắc ánh xạ (mapping rule) được cấu hình sẵn.
- Mọi lần tiêu thụ (consume) phải ghi nhật ký sử dụng (usage log) để chống đếm kép (double-count) và hỗ trợ tranh chấp (dispute).
- Quy tắc ánh xạ (mapping rule) (nếu bật) phải tường minh (explicit) theo cấu hình:
 - Ví dụ `free_spread_5_daily -> free_spread_3_daily` với tỷ lệ quy đổi (conversion ratio) `1:1` hoặc quy tắc tùy chỉnh (custom rule).
 - Mapping mặc định `OFF` để tránh tiêu thụ ngoài ý định.

Bảng ví dụ chuẩn:
| Tình huống | quyền lợi (entitlement) active | Hành động | Kết quả |
|---|---|---|---|
| A | `free_spread_3_daily=1` (pack A), `free_spread_3_daily=1` (pack B) | Rút 3 lá lần 1 | Trừ quota của pack hết hạn sớm hơn |
| B | `free_spread_3_daily=0`, `free_spread_5_daily=1` | Rút 3 lá | Chặn khi mapping `OFF`; chỉ cho phép khi có mapping rule explicit và đang `ON` |
| C | `free_spread_5_daily=1` | Rút 5 lá | Trừ quota 5 lá bình thường |
| D | 2 pack cùng key, một pack hết hạn trước 2h | Rút đúng key | Luôn trừ pack sắp hết hạn trước |
| E | Mapping bật: `free_spread_5_daily -> free_spread_3_daily` | Rút 3 lá khi quota 3 lá = 0 | Áp mapping đúng rule, ghi usage nhật ký có `mapping_rule_id` |

Tiêu chí chấp nhận (Acceptance Criteria):
- Hỗ trợ nhiều thuê bao đang hoạt động (active subscription) trên mỗi người dùng (user).
- Khóa quyền lợi (entitlement key) được cộng/trừ đúng, không đếm kép (double-count).
- quyền lợi (entitlement) reset theo UTC business date.
- Mỗi gói hết hạn độc lập, không ảnh hưởng quyền lợi (entitlement) của gói khác còn hạn.
- Có kiểm thử đơn vị (unit test) cho các trường hợp (case) cộng dồn/gia hạn/hủy giữa kỳ (stacking/renew/cancel).
- Có kiểm thử riêng cho trường hợp cùng mốc hết hạn (expiry timestamp) để xác nhận quy tắc phân xử (tie-breaker) xác định (deterministic) và không đếm kép (double-count).
- Có kiểm thử đồng thời (concurrency test) đa phiên bản chạy (multi-instance) cho tiêu thụ (consume) đồng thời, xác nhận không tiêu thụ kép (double-consume) khi làm mới bộ nhớ đệm (cache invalidate) bị trễ.

Chiến lược làm mới bộ nhớ đệm (cache invalidation strategy):
- Khóa bộ nhớ đệm quyền lợi (entitlement cache key) theo `user_id` + ngày nghiệp vụ (business_date).
- Tiêu thụ quyền lợi (entitlement consume) phải nguyên tử (atomic) ở DB (transaction + khóa dòng (row lock) theo nhóm quyền lợi/người dùng) trước khi phát sự kiện (publish event) làm mới cache.
- Invalidate bắt buộc qua event:
 - `SubscriptionActivated`
 - `SubscriptionRenewed`
 - `SubscriptionExpired`
 - `EntitlementAdjusted`
- Khi phát sinh sự kiện (event), xóa ngay cache quyền lợi (entitlement cache) và phát tác vụ làm mới cache (cache-refresh task).
- Không dùng cache làm nguồn dữ liệu chuẩn (source of truth) cho quyết định tài chính.

### 4.3.5 Gói sự kiện (Event packs) (cơ sở (baseline) từ Giai đoạn (Phase) 1.5)
- Gói Trăng Tròn (Full Moon Pack) và các gói sự kiện theo cửa sổ thời gian.
- Các quyền lợi sự kiện (event benefits) có thời hạn (ví dụ thưởng thêm Diamond, hệ số EXP trong N ngày).

Tiêu chí chấp nhận (Acceptance Criteria):
- Chỉ mua được trong thời gian sự kiện (event) hợp lệ.
- Mỗi đơn sự kiện (event order) xử lý chống lặp (idempotent) như đơn nạp thường.
- Quyền lợi sự kiện (event entitlement) hết hạn đúng hạn và không ghi đè sai quyền lợi nền (base entitlement).

---

## 4.7 Chia sẻ xã hội, chuỗi và giới thiệu (Social sharing, chain, referral)

### 4.7.1 Thưởng chia sẻ kết quả (Share result reward)
- Cơ sở triển khai (baseline rollout): bật cơ chế heuristic thưởng ở MVP; phần bằng chứng chia sẻ/chấm điểm nâng cao (proof-share/scoring) để Giai đoạn (Phase) 5.5.
- Lần chia sẻ đầu tiên (first share) trên mỗi mạng/ngày -> +2 Gold.
- Tự động tạo ảnh chia sẻ (auto-generate share image).
- Payload chia sẻ (share payload) mặc định được ẩn danh (anonymized): không lộ câu hỏi riêng tư hoặc PII.
- Trang chia sẻ công khai (public share page) phải có siêu dữ liệu (metadata) OpenGraph/Twitter chuẩn.

 Chiến lược (Strategy):
- MVP dùng heuristic chống lạm dụng (anti-abuse heuristic), không yêu cầu dán link; proof-share là mô-đun tách riêng ở Giai đoạn (Phase) 5.5.

Heuristic đề xuất:
- Trần thưởng (reward cap) theo ngày/mạng/người dùng.
- Thêm trần liên mạng/ngày (cross-network cap) theo người dùng (user) để chặn lạm dụng đổi mạng liên tục.
- Dấu vân tay thiết bị mềm (soft device fingerprint) + tốc độ IP (IP velocity) + phát hiện mẫu bất thường (abnormal pattern).
- Khoảng chờ (cooldown) giữa các lần nhận thưởng (claim).
- Chấm điểm (score) theo tín hiệu: `account_age`, `share_claim_velocity`, `device_reuse_ratio`, `IP_cluster_score`, `network_diversity_score`.
- Hành động theo quy tắc (rule action) theo mức rủi ro:
 - rủi ro thấp (low risk) -> tự động duyệt (auto approve).
 - rủi ro trung bình (medium risk) -> chặn mềm (soft-block) (chờ rà soát).
 - rủi ro cao (high risk) -> chặn cứng (hard-block) + hàng đợi rà soát thủ công (manual review queue).
- Lưu nhật ký sự kiện (event log) tối thiểu: `user/device/IP/time/network/result/risk_score/reason_code`.

Tiêu chí chấp nhận (Acceptance Criteria):
- Link share có TTL và chữ ký chống sửa payload.
- Referral/deep-link theo dõi (tracking) không làm lộ thông tin nhạy cảm của reading.

### 4.7.2 Chuỗi Tarot bạn bè (Friend Tarot Chain)
- Mời bạn cùng rút bài (invite friend draw together) -> cả hai được thưởng theo hạn mức ngày (quota/day limits).

### 4.7.3 Giới thiệu (Referral)
- Thưởng cho người mời và người được mời (inviter + invited rewards) theo cột mốc (milestone) (mốc nạp đầu tiên xử lý chống lặp (idempotent)).

---

## 4.8 Trò chơi hóa và bảng xếp hạng (Gamification & leaderboard)

### 4.8.1 Nhiệm vụ (Quests)
- Nhiệm vụ theo ngày/tuần/tháng/mùa (daily/weekly/monthly/seasonal quests).
- Định nghĩa (definition) tách riêng, bản ghi tiến độ (progress records) theo người dùng (user) + kỳ (period).

### 4.8.2 Thành tựu và danh hiệu (Achievements & titles)
- Mở thành tựu (achievement unlock) một lần cho mỗi người dùng (one-time/user).
- Quyền sở hữu danh hiệu (title ownership) + chọn danh hiệu đang dùng (active title selection).

### 4.8.3 Bảng xếp hạng (Leaderboards)
- 3 hệ thống điểm (score systems) chạy song song:
 - `daily_rank_score` (reset hạng theo ngày)
 - `monthly_rank_score` (reset hạng theo tháng)
 - `lifetime_score` (không reset)

Tiêu chí chấp nhận (Acceptance Criteria):
- Ảnh chụp dữ liệu (snapshot) theo khóa kỳ (period key) là duy nhất (unique).
- Tác vụ tính lại điểm (score recalculation jobs) ổn định và kiểm toán được (auditable).

### 4.8.4 Tăng chuỗi rút bài hằng ngày và Đóng băng chuỗi (Daily draw streak boost + Streak Freeze) (Giai đoạn (Phase) 5.1)
- Mỗi ngày có ít nhất 1 lần rút bài hợp lệ -> streak +1.
- Streak multiplier: mỗi ngày streak cho thêm `+1% EXP` khi rút bài.
- Công thức: `EXP cuối = EXP gốc x (1 + streak_days / 100)`.
- EXP nhận cuối cùng được lưu trong bảng thông tin người dùng (user).
- Ví dụ streak 10 ngày -> +10% EXP cho lần rút trong ngày.

Đóng băng chuỗi (Streak Freeze):
- Dùng để nối lại chuỗi khi người dùng (user) lỡ ngày.
- Giá: `ceil(pre_break_streak_days / 10)` Diamond.
- Khi `pre_break_streak_days = 0` (chưa từng có streak), không hiển thị/không cho mua Streak Freeze.
- Ví dụ:
 - streak 7 ngày -> 1 Diamond.
 - streak 11 ngày -> 2 Diamond.
 - streak 10 ngày bị gãy -> giá Freeze = `ceil(10/10) = 1`.
- Khuyến nghị cửa sổ dùng Đóng băng chuỗi (Freeze): trong 24h sau ngày bị ngắt.

Tiêu chí chấp nhận (Acceptance Criteria):
- Tính streak theo UTC business date.
- Mỗi ngày chỉ cộng streak một lần.
- Tính phí Freeze đúng công thức làm tròn lên.
- Mọi lượt mua Freeze đều ghi giao dịch Diamond vào sổ cái (ledger transaction).
- Có kiểm tra chống lạm dụng (anti-abuse checks) cho hành vi chuỗi/thưởng bất thường.

---

## 4.11 Vòng lặp giữ chân (Retention Loops) (Giai đoạn (Phase) 5.1 sau mobile)

### 4.11.1 Vòng lặp giá trị D0 (D0 Value Loop)
Kích hoạt (Trigger): người dùng (user) vừa xác minh email (verify email). 
Luồng (Flow): rút 1 lá miễn phí ngay + phát luồng AI (AI stream) + gợi ý “câu hỏi tiếp theo”. 
Ghi chú (Note): miễn phí về Diamond/quyền lợi (entitlement) không đồng nghĩa miễn hạn mức AI (AI quota); nếu có gọi AI (AI call) thì vẫn trừ hạn mức ngày. 
KPI: giữ chân D1 (D1 retention), tỷ lệ hoàn tất phiên đầu (completion rate).

### 4.11.2 Vòng lặp thói quen hằng ngày (Daily Habit Loop)
Kích hoạt (Trigger): đến giờ người dùng (user) hay mở ứng dụng (app) (UTC). 
Luồng (Flow): nhắc Lá Bài Hằng Ngày (Daily Card) -> nhận EXP + tăng chuỗi (streak). 
KPI: DAU/MAU, số ngày hoạt động (active) mỗi tuần.

### 4.11.3 Vòng lặp cứu chuỗi (Streak Rescue Loop)
Kích hoạt (Trigger): người dùng (user) sắp gãy chuỗi (streak) hoặc vừa gãy. 
Luồng (Flow): đề xuất Đóng băng chuỗi (Streak Freeze) (giá `ceil(pre_break_streak_days/10)`) + đếm ngược ngắn (countdown). 
KPI: tỷ lệ cứu chuỗi (streak-save rate), mức giảm rời bỏ (churn reduction) sau ngày gãy chuỗi.

### 4.11.4 Vòng lặp hoàn tất phiên (Session Completion Loop)
Kích hoạt (Trigger): người dùng (user) xem xong kết quả AI. 
Luồng (Flow): AI gợi ý 1 câu hỏi tiếp (follow-up) cụ thể + CTA “rút tiếp theo mục tiêu”. 
KPI: tỷ lệ gắn câu hỏi tiếp (follow-up attach rate), độ dài phiên (session length).

### 4.11.5 Vòng lặp tiến trình bộ sưu tập (Collection Progress Loop)
Kích hoạt (Trigger): lá bài sắp lên mốc (L3/L5/L6/L11/L16). 
Luồng (Flow): hiển thị “còn X EXP để mở khóa (unlock) hiệu ứng/ý nghĩa mới”. 
KPI: số người dùng quay lại (returning users) vì bộ sưu tập (collection), số lá bài tăng cấp (level) mỗi ngày.

### 4.11.6 Vòng lặp quan hệ reader (Reader Relationship Loop)
Kích hoạt (Trigger): người dùng (user) đã trò chuyện (chat) với Reader thành công >=1 lần. 
Luồng (Flow): “Reader quen thuộc đang trực tuyến (online)” + gợi ý phiên mới theo chủ đề cũ. 
KPI: tỷ lệ mua lặp lại (repeat buyer rate) ở trò chuyện (chat) Reader.

### 4.11.7 Vòng lặp nhịp nhiệm vụ (Quest Cadence Loop)
kích hoạt: đầu ngày/đầu tuần. 
Luồng (Flow): 1 nhiệm vụ ngày (daily quest) ngắn + 1 nhiệm vụ tuần (weekly quest) phần thưởng lớn. 
KPI: quest completion, D7 giữ chân (retention).

### 4.11.8 Vòng lặp kéo người dùng quay lại (Win-back Loop)
Kích hoạt (Trigger): người dùng (user) im lặng 3/7/14 ngày. 
Luồng (Flow): ưu đãi theo nguyên nhân rời ứng dụng (app) (hết Diamond, AI hết thời gian (timeout), không hứng thú). 
KPI: tỷ lệ tái kích hoạt (reactivation rate), giữ chân 7 ngày sau tái kích hoạt (7-day post-reactivation retention).

---

## 4.12 Ca điều phối kiếm tiền (Monetization Orchestration Cases) (Giai đoạn (Phase) 5.2 sau mobile)

### 4.12.1 Điều phối mua đầu tiên (First Purchase Orchestration)
Sau 2-3 phiên có tỷ lệ hoàn tất cao (high completion), mới hiện gói nạp nhỏ đầu tiên.

### 4.12.2 Ưu đãi theo ý định (Intent-based Offer)
Nếu người dùng (user) bấm câu hỏi tiếp (follow-up) nhưng thiếu Diamond -> hiện gói “vừa đủ”, không đẩy gói lớn ngay.

### 4.12.3 Điều phối thuê bao xếp lớp (Stacked Subscription Orchestration)
Người dùng (user) giữ nhiều gói (pass); bộ máy điều phối (engine) chỉ hiển thị gói còn thiếu quyền lợi (entitlement).

### 4.12.4 Kiếm tiền bằng bảo vệ chuỗi (Streak Protection Monetization)
Chỉ đẩy Streak Freeze khi có nguy cơ gãy streak thật.

### 4.12.5 Điều phối gói sự kiện (Event Pack Orchestration)
Full Moon Pack chỉ hiển thị cho nhóm active gần đây và có hành vi chi tiêu.

### 4.12.6 Bán thêm ký quỹ reader (Reader Escrow Upsell)
Chỉ khi Reader trả lời chất lượng cao mới mở CTA “thêm câu hỏi”.

### 4.12.7 Điều phối khôi phục hoàn tiền (Refund Recovery Orchestration)
Nếu AI hết thời gian (timeout)/hoàn tiền (refund), không bán thêm (upsell) ngay; cho 1 phiên phục hồi trước.

### 4.12.8 Điều phối phân khúc giá trị cao (High-value Segment Orchestration)
Nhóm người dùng giá trị cao/ý định cao (whale/high-intent) nhận ưu đãi dài hạn, hạn chế giảm giá ngắn hạn liên tục.

---

## 4.15 Phân tích sản phẩm và cây KPI (Product analytics & KPI tree)

### 4.15.1 Chỉ số phễu cốt lõi (Core funnel metrics)
- Thu hút (Acquisition): khách truy cập (visitor) -> chuyển đổi đăng ký (register conversion).
- Kích hoạt (Activation): đăng ký (register) -> xác minh email (verified email) -> phiên xem bài đầu tiên hoàn tất (first completed reading).
- Doanh thu (Revenue): tỷ lệ chuyển đổi nạp đầu tiên (first deposit conversion), tỷ lệ người trả phí (payer rate), ARPPU.
- giữ chân (retention): D1/D7/D30 giữ chân (retention) theo cohort.
- Giới thiệu (Referral): lời mời giới thiệu (referral invite) -> chuyển đổi đủ điều kiện (qualified conversion).

mục tiêu cơ sở (baseline) (nội bộ, điều chỉnh theo giai đoạn):
- D1 giữ chân (retention) >= 30% ở MVP.
- D7 giữ chân (retention) >= 15% ở MVP.
- Tỷ lệ chuyển đổi nạp đầu tiên (first deposit conversion) >= 8% trên người dùng (user) đã hoàn tất >=1 phiên xem bài (reading).

---

## 4.17 Cơ sở chi phí và lập ngân sách (Cost & budgeting baseline)

### 4.17.1 Mô hình chi phí theo tầng DAU (Cost model by DAU tier) (kế hoạch nội bộ)
- Tách chi phí theo nhóm:
 - Chi phí suy luận AI (AI inference cost).
 - Chi phí hạ tầng (infra cost) (API, DB, Redis, hàng đợi/tác vụ (queue/jobs)).
 - Lưu lượng ra CDN/media (CDN/media egress).
 - Dịch vụ bên thứ ba (3rd-party services) (email/push/quan sát vận hành (observability)).
- Thiết lập 3 mức planning:
 - Tier S: ~10k DAU.
 - Tier M: ~50k DAU.
 - Tier L: ~100k+ DAU.

### 4.17.2 Quy tắc kiểm soát ngân sách (Budget control rules)
- Có trần ngân sách theo tháng (monthly budget ceiling) cho từng nhóm chi phí (cost group).
- Có cảnh báo tốc độ đốt ngân sách theo ngày (daily burn-rate alert) khi mức chi vượt dự báo (forecast).
- Có playbook giảm tải chi phí:
 - giảm hạn mức AI (AI quota) cho phân khúc thấp (low-value segment).
 - ưu tiên gom lô (batching) cho cache/hàng đợi (queue).
 - giới hạn chiến dịch tốn inference.

Tiêu chí chấp nhận (Acceptance Criteria):
- Mỗi tháng có báo cáo kế hoạch so với thực tế (planned vs actual) theo từng tầng (Tier).
- Cập nhật dự báo (forecast update) tối thiểu mỗi quý hoặc khi tăng trưởng vượt 20% so với kế hoạch.

### 4.17.3 Mẫu ước tính chi phí (Cost estimation template)
- Công thức ước tính chi phí AI (estimate AI cost) theo tháng:
 - `monthly_ai_cost = requests_per_day * 30 * ((avg_input_tokens/1000)*price_in + (avg_output_tokens/1000)*price_out)`.
- Công thức ước tính chi phí hạ tầng (estimate infra cost):
 - `monthly_infra_cost = compute + db + cache + queue + storage + egress`.
- Ví dụ minh họa (không phải cam kết giá, phải thay bằng bảng giá provider hiện hành):
 - Tier S: 10k DAU, 0.8 AI yêu cầu (request)/DAU/ngày, 2k in + 800 out tokens/yêu cầu (request).
 - Dùng giá biến cấu hình `price_in`, `price_out` để tính forecast, không hardcode trong codebase.
- Quy trình làm mới nguồn định giá (pricing source refresh process):
 - Nguồn giá chuẩn: trang giá/nhật ký thay đổi (pricing page/changelog) chính thức của từng nhà cung cấp (provider).
 - Kiểm tra định kỳ tối thiểu hàng quý và mỗi khi có thông báo thay đổi giá.
 - Lưu ảnh chụp nguồn giá (snapshot) + ngày hiệu lực trong tài liệu kế hoạch tài chính (finance planning docs).

Tiêu chí chấp nhận (Acceptance Criteria):
- Bảng dự báo (forecast sheet) phải lưu ảnh chụp giá (snapshot) theo ngày hiệu lực.
- Khi nhà cung cấp (provider) đổi giá, cập nhật dự báo (forecast update) trong <= 5 ngày làm việc.

---

### 4.18 Quy trình rà soát hằng năm (Annual review process)
- Mỗi 12 tháng (hoặc sớm hơn nếu có regulatory change lớn), thực hiện rà soát:
 - legal/compliance matrix theo thị trường.
 - định giá assumptions của nhà cung cấp (provider).
 - competitor landscape và positioning mục 1.4.
- Mọi đề xuất thay đổi sau rà soát (review) phải theo quản trị tối thiểu (governance):
 - có `change_id`, `owner`, `reviewers`, `effective_date`.
 - phân loại ảnh hưởng: sản phẩm/kỹ thuật/pháp lý/tài chính (product/engineering/legal/finance).
 - phê duyệt tối thiểu (approval): Chủ sản phẩm (Product owner) + Trưởng kỹ thuật (Tech lead) + Người rà soát pháp lý (Legal reviewer) (khi chạm tuân thủ/địa lý/thanh toán).

Tiêu chí chấp nhận (Acceptance Criteria):
- Rà soát hằng năm (annual review) phải tạo báo cáo (report) + nhật ký quyết định (decision log) theo quy trình quản trị (governance process) của blueprint.
- Các thay đổi phát sinh phải được gắn thành đề xuất thay đổi (change proposal) với người phụ trách (owner) và hạn chót (deadline).
- Mọi thay đổi từ rà soát hằng năm (annual review) phải đi qua ma trận phê duyệt (approval matrix) đã định nghĩa trước khi có hiệu lực.

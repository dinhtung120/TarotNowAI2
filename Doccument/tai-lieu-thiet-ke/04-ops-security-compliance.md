# TarotWeb - Vận hành, bảo mật và tuân thủ (Ops, Security & Compliance) v1.5

Nguồn: tách từ `FEATURE_REQUIREMENTS_BLUEPRINT_v1.5.md` (2026-03-06).
Mục tiêu: tập trung vào pháp lý/tuân thủ (legal/compliance), yêu cầu phi chức năng vận hành (NFR), ứng phó sự cố (incident response) và bảo trì (maintenance).

Tiền tố ID tài liệu (Doc ID prefix): `OPS-*`

---

## 4.10 Bảng điều khiển quản trị (Admin dashboard) (thực hiện vào Giai đoạn (Phase) 1 - MVP)
- Chỉ số doanh thu (revenue metrics) và phễu thanh toán (payment funnels).
- Quản lý nạp/rút tiền (deposit/withdraw management).
- Khóa/mở khóa người dùng (user lock/unlock) và duyệt reader (reader approval).
- xử lý tranh chấp/báo cáo (dispute/report resolution).
- Quản lý cấu hình (config management) (khuyến mãi/nhiệm vụ/thuê bao).
- Nhật ký AI provider (AI provider logs) và nhật ký hệ thống (system logs).

Tiêu chí chấp nhận (Acceptance Criteria):
- RBAC chặt cho admin routes.
- Vết kiểm toán đầy đủ (full audit trail) cho hành động quản trị (admin actions).

---

## 4.13 Cơ sở pháp lý, tuân thủ và niềm tin (Legal, compliance & trust baseline)

### 4.13.1 Cổng tuổi và trang pháp lý (Age gate + legal pages) (thực hiện vào Giai đoạn (Phase) MVP)
- Bắt buộc cổng tuổi (age gate): người dùng (user) xác nhận đủ 18+ (hoặc theo luật địa phương nếu cao hơn).
- Trước khi dùng đầy đủ tính năng: bắt buộc đồng ý Điều khoản sử dụng và Chính sách riêng tư (TOS + Privacy Policy).
- Bắt buộc tuyên bố miễn trừ AI (AI disclaimer): nội dung Tarot/AI chỉ mang tính tham khảo, không thay thế tư vấn y tế/pháp lý/tài chính.

Tiêu chí chấp nhận (Acceptance Criteria):
- Không chấp nhận hoàn tất tạo tài khoản nếu chưa chọn (tick) đồng ý pháp lý bắt buộc (legal consent).
- Trang pháp lý công khai (legal pages public), có phiên bản hóa (versioning) và lưu lịch sử đồng ý (consent history) theo người dùng (user).

### 4.13.2 KYC reader và tuân thủ chi trả (Reader KYC + payout compliance) (thực hiện vào Giai đoạn (Phase) 2.1)
- Phase 1: xác minh thủ công bởi admin (xem [01-business-rules.md](./01-business-rules.md) mục 4.3.3).
- Phase 2.1+: Reader phải hoàn thành KYC trước khi mở tính năng rút tiền.
- KYC nâng cao (enhanced KYC) theo ngưỡng rút cộng dồn (configurable threshold).
- Lưu hồ sơ tuân thủ (compliance record): trạng thái KYC, loại giấy tờ, thời điểm xác minh, người duyệt.
- Tài liệu KYC/PII phải mã hóa khi lưu trữ (at-rest) (khóa do KMS quản lý), truy cập theo RBAC tối thiểu và có vết kiểm toán truy cập (access audit trail).
- PII từ KYC không được ghi dạng thô (raw) vào nhật ký ứng dụng (application logs); nhật ký chỉ lưu tham chiếu/mã định danh đã che (masked).

Tiêu chí chấp nhận (Acceptance Criteria):
- Không tạo được withdrawal yêu cầu (request) khi KYC chưa đạt yêu cầu.
- Có nhật ký kiểm toán (audit log) đầy đủ cho mọi thao tác liên quan tuân thủ chi trả (payout compliance).
- Có chính sách lưu giữ riêng (retention policy) cho bằng chứng KYC (KYC evidence) và quy trình xóa (purge) khi hết hạn pháp lý (legal).

### 4.13.3 Ranh giới tuân thủ thanh toán (Payment compliance boundary) (PCI/financial data) (thực hiện Giai đoạn (Phase) MVP)
- Nền tảng (platform) không lưu PAN/CVV thô (raw) hoặc dữ liệu thẻ đầy đủ.
- Thanh toán thẻ/nguồn tiền nhạy cảm do nhà cung cấp (provider) xử lý; nền tảng (platform) chỉ lưu token/tham chiếu/trạng thái giao dịch cần thiết.
- Chứng từ giao dịch tài chính phân chia trách nhiệm:
 - nhà cung cấp (provider) giữ chứng từ thanh toán gốc.
 - Platform giữ sổ cái (ledger)/audit/reference để đối soát và xử lý tranh chấp (dispute).
- Truy cập dữ liệu chi trả/bằng chứng tài chính (payout/financial evidence) theo nguyên tắc đặc quyền tối thiểu (least privilege).

Tiêu chí chấp nhận (Acceptance Criteria):
- Kiểm thử xác nhận không có dữ liệu thẻ thô (raw card data) trong DB/nhật ký/sao lưu (backup).
- Có tài liệu ranh giới trách nhiệm (boundary) giữa nhà cung cấp (provider) và nền tảng (platform) cho kiểm toán pháp lý.

### 4.13.4 Lưu giữ bằng chứng sự cố và tranh chấp (Incident & dispute evidence retention) (thực hiện Giai đoạn (Phase) MVP)
- Với giao dịch có tranh chấp (dispute): giữ nhật ký/bằng chứng (log/evidence) theo chính sách lưu giữ riêng (retention policy) (dài hơn log thông thường).
- Bằng chứng (evidence) gồm: chuyển trạng thái tài chính (finance transitions), siêu dữ liệu tin nhắn (message metadata), hành động quản trị (admin actions), mã lý do (reason code).
- giữ chân (retention) windows chuẩn:
 - `ai_prompt_redacted_store`: 90 ngày.
 - `ai_stream_metadata`: 180 ngày.
 - `dispute_or_chargeback_evidence`: 24 tháng (hoặc theo legal policy cao hơn).
- Quyền truy cập bằng chứng nhạy cảm (evidence) chỉ cho vai trò (role) được ủy quyền; mở rộng quyền cần phê duyệt + kiểm toán.

Tiêu chí chấp nhận (Acceptance Criteria):
- Truy xuất được gói evidence đầy đủ cho mỗi tranh chấp (dispute) id trong thời gian giữ chân (retention).
- Có quy trình xuất bằng chứng (export evidence) cho rà soát pháp lý/nội bộ (legal/internal review).

### 4.13.5 Tuân thủ địa lý và khóa tính năng bị quản lý (Geo compliance & regulated-feature gating) (thực hiện Giai đoạn (Phase) 2.1)
- Duy trì ma trận pháp lý (legal matrix) theo quốc gia/khu vực (country/region) cho các tính năng nhạy cảm: gacha RNG kiếm tiền (monetization), vòng lặp thưởng có yếu tố ngẫu nhiên (reward loops with chance), luồng chi trả (payout flows).
- Chốt chặn lúc chạy (runtime gating) theo nhiều tín hiệu địa lý (multi-signal geo):
 - `account_jurisdiction` (ưu tiên cao nhất nếu đã KYC).
 - `kyc_country` / `payment_country`.
 - `ip_geolocation` (tín hiệu phụ trợ).
- Bộ máy quyết định địa lý (geo decision engine) dùng cờ pháp lý có phiên bản (versioned legal flag).
- Nếu khu vực (region) bị hạn chế, UI phải ẩn tính năng (feature) và API trả mã lý do (reason code) thống nhất.
- Bắt buộc thực thi ở lớp API phía máy chủ (server-side API layer) trước mọi xử lý thanh toán/mua/chi trả nhạy cảm (payment/purchase/payout).
- Với vòng lặp thưởng (reward loops) có yếu tố ngẫu nhiên (chance), endpoint `claim` cũng phải thực thi chốt chặn địa lý (geo gate) ở phía máy chủ (server-side).
- Nếu phát hiện tín hiệu mâu thuẫn hoặc rủi ro VPN/proxy cao: chuyển trạng thái `restricted_review`.
- `restricted_review` SLA:
 - xử lý tự động tối đa 24h khi có tín hiệu mới đủ tin cậy.
 - quá 24h phải đẩy manual rà soát hàng đợi + thông báo trạng thái cho user.
- Ngưỡng auto-resolve mẫu (configurable):
 - `kyc_verified = true`
 - `payment_country = account_jurisdiction`
 - `ip_geo_consistency_score >= 0.95`
 - không có cờ `vpn_proxy_risk`.

Tiêu chí chấp nhận (Acceptance Criteria):
- Có nhật ký kiểm toán cho mọi lần từ chối (deny) theo chốt chặn pháp lý (legal gate) (`LEGAL_REGION_BLOCKED`).
- Có audit nhật ký riêng cho case `GEO_SIGNAL_MISMATCH` và `VPN_PROXY_RISK`.
- Có chỉ số (metric) theo dõi `restricted_review_age` và cảnh báo (alert) khi tồn đọng (backlog) vượt ngưỡng.
- Có quy trình cập nhật legal matrix có phiên bản + hiệu lực thời gian.

### 4.13.6 Cơ sở thuế và hóa đơn (Tax & invoicing baseline) (thực hiện ở Giai đoạn (Phase) MVP)
- Với giao dịch nạp có nghĩa vụ thuế/VAT theo khu vực, hệ thống phải lưu các thành phần thuế (tax components) tách biệt trong bản ghi thanh toán (payment record).
- Báo cáo hóa đơn/thuế (invoicing/tax report) xuất được theo kỳ cho bộ phận tài chính/pháp lý.

Tiêu chí chấp nhận (Acceptance Criteria):
- Bản ghi thanh toán (payment record) có đủ trường để truy xuất số tiền trước thuế (pre-tax amount), tiền thuế (tax amount), tổng tiền (total amount) theo tiền tệ (currency).
- Có chính sách (policy) cấu hình thuế theo khu vực pháp lý/phiên bản (jurisdiction/version).

### 4.13.7 Vận hành quyền dữ liệu (Data rights operations) (GDPR/CCPA-like) (thực hiện ở Giai đoạn (Phase) MVP)
- Hỗ trợ yêu cầu (request) loại:
 - truy cập/xuất dữ liệu (data access/export).
 - chỉnh sửa (correction).
 - xóa dữ liệu (deletion) (trừ dữ liệu bắt buộc lưu giữ pháp lý/tài chính).
- Mỗi yêu cầu (request) có SLA xử lý nội bộ và vết kiểm toán đầu-cuối (end-to-end audit trail).
- Template vận hành tối thiểu:
 - `DATA_EXPORT_REQUEST_TEMPLATE`.
 - `DATA_DELETION_DECISION_TEMPLATE`.
 - `LEGAL_RETENTION_EXCEPTION_TEMPLATE`.

Tiêu chí chấp nhận (Acceptance Criteria):
- Có mẫu xuất dữ liệu chuẩn (export template) (đọc được bằng máy + tóm tắt cho người đọc).
- Deletion workflow phải trả về danh mục dữ liệu đã xóa và dữ liệu bị giữ lại kèm lý do pháp lý.

---

## 4.19 Cơ sở công cụ vận hành (Operational tooling baseline) (thực hiện ở Giai đoạn (Phase) 1.7 - trước mobile)

### 4.19.1 Công cụ sự cố và cảnh báo (Incident and alert tooling)
- Kênh cảnh báo chuẩn (alert channel):
 - cảnh báo thời gian thực (realtime alerts) -> kênh sự cố Slack/Teams.
 - gọi trực sự cố mức nghiêm trọng (critical paging) -> công cụ tương đương PagerDuty/Opsgenie.
- Ticket theo dõi (tracking) chuẩn:
 - sự cố/công việc (incidents/tasks) -> công cụ tương đương Jira/Linear với nhãn mức độ nghiêm trọng (severity labels) P1-P3.

### 4.19.2 Tích hợp runbook (Runbook integration)
- Mỗi quy tắc cảnh báo nghiêm trọng (critical alert rule) phải ánh xạ tới URL runbook và người phụ trách (owner).
- Incident ticket phải tự gắn `trace_id`/`correlation_id` khi có.

Tiêu chí chấp nhận (Acceptance Criteria):
- Cảnh báo P1 tạo cuộc gọi trực (page) + phiếu xử lý (ticket) tự động trong <= 2 phút.
- Sau sự cố (incident) phải có tài liệu hậu kiểm (postmortem artifact) liên kết về ticket gốc.

---

## 4.20 Nguồn dữ liệu bên ngoài và quy trình làm mới (External data sources & refresh process) (thực hiện ở Giai đoạn (Phase) 2.5)

### 4.20.1 Nguồn dữ liệu định giá (Pricing data sources)
- Dự báo giá (pricing forecast) cho dịch vụ AI/thanh toán phải dựa trên nguồn chính thức của nhà cung cấp (provider).
- Mỗi lần refresh phải ghi:
 - `source_url`.
 - `checked_at`.
 - `effective_date`.
 - `owner`.

### 4.20.2 Nguồn dữ liệu thuế và thẩm quyền (Tax and jurisdiction data sources)
- Thuế/VAT phải cấu hình theo khu vực pháp lý/phiên bản (jurisdiction/version); không mã cứng (hardcode) trong logic nghiệp vụ.
- Nếu mở rộng đa thị trường (multi-market), ưu tiên dạng bộ điều hợp (adapter) cho bộ máy thuế (tax engine) (`TaxJar`/`Avalara`/equivalent) thay vì mã hóa thủ công.
- Thay đổi mức thuế (tax rates) phải có ngày hiệu lực triển khai (rollout effective-date) và chính sách bù dữ liệu (backfill policy) cho báo cáo.

### 4.20.3 Quét đối thủ và thị trường (Competitor and market scan)
- Thực hiện quét đối thủ hằng năm (annual competitor scan) cho nhóm ứng dụng AI-Tarot/tâm linh (spiritual apps) và cập nhật mục `1.4 Competitive positioning` tại [01-business-rules.md](./01-business-rules.md) khi có thay đổi lớn.
- Nếu phát hiện thay đổi pháp lý/thị trường quan trọng, tạo đề xuất thay đổi blueprint (blueprint change proposal) theo quy trình kiểm soát thay đổi (change-control process) ở mục `6.7 Annual review process` của [01-business-rules.md](./01-business-rules.md).

### 4.20.4 Danh sách mở rộng đăng nhập mạng xã hội (Social login expansion checklist) (nếu bật)
- Rà soát quyền riêng tư/ánh xạ dữ liệu (privacy/data mapping review) (ảnh hưởng kiểu GDPR/CCPA).
- Cập nhật mô hình đe dọa gian lận (fraud threat model) (lạm dụng liên kết tài khoản, tài khoản tổng hợp/synthetic).
- Danh sách kiểm tra đồng ý và tối thiểu hóa dữ liệu (consent and data minimization checklist) trước khi production.

Tiêu chí chấp nhận (Acceptance Criteria):
- Mỗi nguồn dữ liệu external có rà soát cadence + người phụ trách cụ thể.
- Có vết kiểm toán (audit trail) cho lần làm mới gần nhất của tập dữ liệu giá/thuế/pháp lý (pricing/tax/legal datasets).
- Có người phụ trách + nhịp thực hiện (owner + cadence) cho quét đối thủ (competitor scan) và nhật ký quyết định (decision log) khi có thay đổi định vị quan trọng.

---

## 5) Yêu cầu phi chức năng (Non-functional requirements)

### Hiệu năng (Performance)
- Giao diện FE phản hồi tốt trên desktop/di động (responsive).
- Mục tiêu độ trễ token AI đầu tiên (first AI token latency) thấp.
- P95 API mục tiêu:
 - Xác thực (Auth)/hồ sơ (profile)/read endpoints: <= 300ms.
 - Payment/write endpoints: <= 500ms (không tính thời gian provider callback ngoài hệ thống).
 - bảng điều khiển quản trị (Admin dashboard) queries: <= 800ms.
- First AI token P95 <= 4s, P99 <= 8s (không tính outage provider).
- Có kiểm thử tải (load test) riêng cho 3 luồng: trò chuyện thời gian thực (chat realtime), phát luồng AI (AI streaming), callback thanh toán (payment callbacks).

### Bảo mật (Security)
- JWT + refresh rotation.
- giới hạn tốc độ (rate limiting) bắt buộc cho auth/trò chuyện (chat)/payment/webhook.
- Encrypt secrets và dữ liệu payout nhạy cảm.
- Input validation + output encoding.
- Kiểm thử xâm nhập định kỳ (pen-test) cho endpoint thanh toán/webhook/quản trị (payment/webhook/admin endpoints).
- Chính sách xoay khóa bí mật (secrets rotation policy) cho khóa AI, thanh toán và ký JWT.
- Không nhật ký raw PII nhạy cảm trong application nhật ký (password, full card data, full payout info).
- CSRF protection bắt buộc với browser endpoints dùng cookie phiên (session).
- Tài liệu KYC và bằng chứng chi trả (payout evidence) phải áp mã hóa khi lưu (encryption-at-rest) + chính sách xoay khóa riêng (key rotation policy).

### Độ tin cậy (Reliability)
- Idempotency cho payment callbacks và settlement jobs.
- Chính sách thử lại (retry policies) cho nhà cung cấp bên ngoài (external providers) (AI/thanh toán).
- tác vụ nền (Background jobs) cho auto settlement/refund/release (tự động quyết toán/hoàn tiền/giải phóng).
- Dead-letter handling cho tác vụ nền (Background jobs).
- Tác vụ đối soát (reconciliation jobs) cho thanh toán/sổ cái chạy định kỳ và có hàng đợi rà soát (review queue) khi sai lệch (mismatch).
- Compensation jobs cho các luồng fail giữa chừng (external side effects).
- TickerQ jobs bắt buộc có:
 - `job_idempotency_key`.
 - `job_state` table (queued/running/succeeded/failed/retried/dead-lettered).
 - Transition guard để không xử lý lại logic release/refund (giải phóng/hoàn tiền) đã hoàn tất.
- Có SLO vận hành cho tồn đọng (backlog) của tác vụ quyết toán/hoàn tiền (settlement/refund jobs); vượt SLO phải tự tạo sự cố (incident) và gọi trực (paging).

### Khả năng quan sát (Observability)
- OpenTelemetry cho traces/chỉ số/nhật ký correlation.
- Structured nhật ký với trace IDs.
- Bắt buộc nhật ký/search được `idempotency_key` + `correlation_id` + `trace_id` cho mọi finance transition và tác vụ nền (Background jobs).
- Chỉ số (metrics): tỷ lệ thanh toán thành công (payment success), độ trễ AI (AI latency), tỷ lệ ký quỹ bị kẹt (escrow stuck ratio).
- Cảnh báo (alerting) cho lỗi nghiêm trọng (critical failures).
- Ngưỡng cảnh báo vận hành nội bộ:
 - `payment_success_rate` < 99.5% (rolling 7 ngày).
 - `escrow_stuck_ratio` > 0.5% (rolling 1 giờ).
 - `ai_timeout_rate` > 1.0% (rolling 1 giờ).
- Mỗi cảnh báo nghiêm trọng (critical) phải có runbook: người chịu trách nhiệm, bước kiểm tra, bước hoàn tác/sửa nóng (rollback/hotfix).
- AI nhật ký chỉ lưu siêu dữ liệu (metadata):
 - `model`, `token_in`, `token_out`, `latency_ms`, `status`, `error_code`, `trace_id`.
- Nếu cần lưu prompt để QA, phải qua che dữ liệu/khử định danh (redaction/de-identification); mặc định không lưu toàn bộ nội dung trò chuyện thô (full raw chat content).
- Các ngưỡng trên là mục tiêu vận hành nội bộ, không phải cam kết dịch vụ công khai.

### Rào chắn năng lực và chi phí (Capacity & cost guardrails)
- Theo dõi chi phí theo miền (domain): AI nhà cung cấp (provider), lưu lượng ra CDN (CDN egress), cơ sở dữ liệu (database), hàng đợi/tác vụ nền (queue/background workers).
- Thiết lập cảnh báo ngân sách (budget alert) theo tháng và theo ngày cho mức sử dụng AI (AI usage).
- Có quy tắc tự động mở rộng (autoscaling rules) theo CPU/bộ nhớ/độ trễ hàng đợi (queue lag) cho worker API và worker tác vụ nền.

Tiêu chí chấp nhận (Acceptance Criteria):
- Vượt ngưỡng budget cảnh báo phải tự động tạo incident ticket nội bộ.
- Có báo cáo phân rã chi phí (cost breakdown) theo nhóm tính năng (feature group) tối thiểu mỗi tuần.

### Chống gian lận (Anti-fraud)
- Đường ống phát hiện gian lận (fraud detection pipeline) cho thưởng chia sẻ (Share reward), giới thiệu (referral), chuỗi (streak), thanh toán (payment).
- Chấm điểm rủi ro (risk scoring) theo người dùng/thiết bị/IP/khoảng thời gian (time-window).
- Bộ máy luật (rule engine) + hàng đợi rà soát (review queue) cho trường hợp nghi vấn (case).
- Dữ liệu đo đạc tối thiểu (telemetry): tốc độ theo IP/thiết bị/người dùng (velocity), tỷ lệ tuổi tài khoản so với mức chi (account-age-to-spend ratio), điểm tái sử dụng thiết bị (device reuse score), mẫu phiên bất thường (abnormal session patterns).
- Có 3 mức phản ứng: allow, soft-block (pending), hard-block (manual review).
- Các trường hợp `restricted_review` từ chốt chặn địa lý (geo gating) phải đi vào cùng khung hàng đợi rà soát (review queue framework) với SLA xử lý nhất quán.

### Quyền riêng tư và lưu giữ (Privacy & retention)
- Phân loại dữ liệu theo nhóm: tài chính, trò chuyện (chat), nhật ký kỹ thuật, audit nhật ký.
- Chính sách giữ chân (retention) theo từng nhóm dữ liệu và theo loại sự kiện (đặc biệt dispute).
- Hỗ trợ luồng "Delete my data" theo chính sách, trừ dữ liệu bắt buộc giữ theo pháp lý/tài chính.
- Ánh xạ quyền dữ liệu cá nhân theo luật áp dụng (PDPA/GDPR-like/CCPA-like): truy cập (access), chỉnh sửa (correction), yêu cầu xóa (deletion request), hạn chế xử lý (processing restriction), yêu cầu xuất dữ liệu (data export request).
- Quy trình de-identification phải có versioning ruleset và kiểm thử định kỳ hiệu quả redaction.

### Sao lưu và khôi phục thảm họa (Backups & disaster recovery)
- Backup định kỳ cho PostgreSQL, MongoDB, Redis theo chính sách môi trường.
- Có runbook khôi phục và drill định kỳ để kiểm tra khả năng restore.
- Định nghĩa RTO/RPO nội bộ:
 - Production: RTO <= 2 giờ, RPO <= 15 phút.
 - Staging: RTO <= 8 giờ, RPO <= 24 giờ.

### Chính sách bộ nhớ đệm CDN (CDN caching policy)
- `card_image` (immutable versioned URL): cache 365 ngày.
- `reader_avatar`: cache 30 ngày.
- `generated_share_image`: cache 7 ngày.
- Khi tài nguyên (asset) đổi phiên bản (version), bắt buộc đổi URL hoặc xóa bộ nhớ đệm (purge cache) theo khóa (key).

---

## 6) Rủi ro và giảm thiểu (Risks & mitigation)

1. Độ phức tạp ký quỹ (escrow complexity) -> triển khai máy trạng thái (state machine) + bất biến (invariants) + kiểm thử tích hợp (integration tests).
2. Độ trễ/chi phí AI (AI latency/cost) -> chính sách hết thời gian (timeout policy) + tinh chỉnh prompt (prompt tuning) + bộ nhớ đệm chọn lọc (selective cache).
3. Hiệu ứng hình ảnh quá tải trên di động (visual FX overload mobile) -> hồ sơ kết xuất thích nghi (adaptive rendering profiles).
4. Phạm vi tăng mất kiểm soát (scope creep) -> cổng giai đoạn chặt (strict phase gates) + cờ tính năng (feature flags).

### 6.2 tồn đọng nâng cao (không chặn go-live)
- Các hạng mục tùy chọn (optional) sau khi go-live được theo dõi trong tồn đọng kỹ thuật/vận hành riêng (tài liệu công bằng RNG, đăng nhập mạng xã hội, thử nghiệm nâng cao).

### 6.4 Hỗ trợ sau phát hành và cơ sở bảo trì (Post-launch support & maintenance baseline)
- Có quy trình phiếu xử lý (ticket workflow) cho sự cố tranh chấp/thanh toán/lạm dụng (dispute/payment/abuse incidents), phân loại mức độ nghiêm trọng (severity) P1-P3.
- Có lịch trực luân phiên (on-call rotation) cho sự cố API/thanh toán (API/payment incidents) và ma trận leo thang (escalation matrix) rõ ràng.
- Mỗi đợt phát hành (release) phải có kế hoạch hoàn tác (rollback plan) + người chịu trách nhiệm (owner).
- Cadence vận hành growth/product:
 - rà soát KPI hằng tuần (weekly KPI review) (D1/D7, tỷ lệ chuyển đổi, tỷ lệ hoàn tiền, tỷ lệ ký quỹ bị kẹt).
 - rà soát kinh doanh + chi phí hằng tháng (monthly business + cost review) (ARPPU, payer rate, burn-rate).
- KPI dashboard tooling cơ sở (baseline):
 - chỉ số sản phẩm (product metrics): bảng điều khiển tương đương PostHog/BI.
 - chỉ số vận hành (operations metrics): bảng điều khiển quan sát (observability dashboard) tương đương OTel/Grafana.
 - chỉ số thử nghiệm (experiment metrics): bảng điều khiển phân tích cho feature flag.
- Vận hành payout:
 - theo dõi `manual_payout_backlog_age` và `manual_payout_queue_depth`.
 - tồn đọng payout > 24h phải vào incident hàng đợi.
- Vận hành TickerQ tồn đọng:
 - triage dead-letter trước, sau đó replay an toàn theo `job_idempotency_key`.
 - có SLA dọn tồn đọng (clear backlog) cho quyết toán/hoàn tiền (settlement/refund) theo chính sách trực (on-call policy).

Tiêu chí chấp nhận (Acceptance Criteria):
- P1 incident có phản hồi ban đầu <= 15 phút.
- Có mẫu hậu kiểm chuẩn (postmortem template) cho sự cố nghiêm trọng (critical incident).
- KPI dashboard phải có người phụ trách và refresh cadence rõ ràng.

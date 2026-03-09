# TarotWeb – MongoDB Schema

**Nguyên tắc chung:**
- `_id`: ObjectId (trừ `cards_catalog._id` dùng Int32 1–78)
- Tham chiếu PostgreSQL: dùng string UUID
- Tham chiếu trong MongoDB: ObjectId
- Soft delete: `is_deleted`, `deleted_at` cho nghiệp vụ
- TTL: áp dụng cho logs (notifications, ai_provider_logs, gacha_logs)
- Không hỗ trợ COMMENT trong MongoDB → toàn bộ giải thích nằm trong file này
- **Sharding (Tier L):** chat_messages shard key `{conversation_id: "hashed"}`. Cần enableSharding trước.

---

## 1. cards_catalog

**Mục đích:** Danh mục 78 lá bài Tarot. Dữ liệu tĩnh, dùng cho rút bài, hiển thị, và tra cứu ý nghĩa. `_id` Int32 1–78 để map trực tiếp với deck.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | Int32 | 1–78, ID cố định của lá bài |
| code | string | Mã ổn định (vd: the_fool, ace_of_wands) |
| name | {vi, en, zh} | Tên đa ngôn ngữ theo locale |
| arcana | string | major \| minor |
| suit | string | wands \| cups \| swords \| pentacles \| null (null cho Major) |
| number | Int32 | 0–22 (Major), 1–14 (Minor) |
| element | string | fire \| water \| air \| earth |
| meanings | object | upright/reversed: {keywords[], description} |
| visual_profiles | object | Cấu hình hiệu ứng: holographic, parallax, particle theo level |
| created_at, updated_at | ISODate | Audit |

**Index:** unique `code`, `name.vi`, `name.en`, `name.zh`

---

## 2. card_stories

**Mục đích:** Câu chuyện Ascension do AI tạo khi lá bài đạt mốc level (6, 7, ..., 20). Lưu nội dung story và tham chiếu reading nguồn.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID user sở hữu |
| card_id | Int32 | Tham chiếu cards_catalog._id |
| level_trigger | Int32 | Mốc level kích hoạt (6, 7, ..., 20) |
| story_content | string | Nội dung story do AI tạo |
| source_reading_refs | [ObjectId] | reading_sessions._id tạo story |
| is_deleted, deleted_at | | Soft delete |
| created_at | ISODate | |

**Index:** unique `(user_id, card_id, level_trigger)`, `(card_id, level_trigger)`

---

## 3. user_collections

**Mục đích:** Túi bài của user: mỗi lá có level, EXP, skin, thống kê rút. Dùng cho progression, cấp lá (free follow-up slots), và hiệu ứng visual.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| card_id | Int32 | cards_catalog._id |
| level | Int32 | Cấp lá (1–20+), quyết định free follow-up slots |
| exp | Int64 | EXP hiện tại, dùng tính level |
| ascension_tier | Int32 | Bậc Ascension (nếu có) |
| customization | object | signature_name, active_skin_id (N11 fix: skins catalog chưa có – Phase 6+ extension. Hiện tại `active_skin_id` luôn NULL) |
| stats | object | times_drawn_upright, times_drawn_reversed |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at, last_drawn_at | ISODate | |

**Index:** unique `(user_id, card_id)`, `(user_id, level desc)` – xếp hạng theo level

---

## 4. reading_sessions

**Mục đích:** Phiên xem bài + AI. Lưu câu hỏi, lá rút, kết quả AI, follow-up. Tham chiếu bởi ai_requests (PostgreSQL) và reading_rng_audits.

**Schema validator (init.js):** spread_type enum [daily_1, spread_3, spread_5, spread_10], drawn_cards maxItems 10. validationAction: warn.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | Tham chiếu: ai_requests.reading_session_ref, reading_rng_audits |
| user_id | string | UUID |
| session_type | string | personal \| friend_chain |
| spread_type | string | daily_1 \| spread_3 \| spread_5 \| spread_10 – dùng check daily limit |
| question | string | Câu hỏi ban đầu |
| drawn_cards | array | {card_id, position, is_reversed, card_level_at_reading} |
| ai_status | string | pending \| streaming \| completed \| timeout \| failed |
| ai_result | object | summary, share_summary, suggested_followup |
| locale_info | object | { requested_locale, returned_locale, fallback_reason } – ARCH-4.4.6 / UX-4.4.6 |
| followups | array | {sequence, question, answer, cost_diamond, cost_gold (default 0, reserved future), cost_currency (default null, reserved future), is_free_by_level, created_at} – H1 fix: thêm sequence (1-5) mapping với ai_requests.followup_sequence; M1 fix: thêm cost_gold và cost_currency để hỗ trợ multi-currency pricing. **Lưu ý:** cost_gold và cost_currency hiện reserved for future use – follow-up hiện chỉ charge Diamond theo UX-4.4.5. **I1 fix:** khi tạo followup, set cost_gold=0, cost_currency=null |
| cost | object | currency, amount – chi phí phiên. **N10 fix:** Đây là snapshot UI-friendly; source of truth là `ai_requests.charge_gold/charge_diamond` (PostgreSQL); ultimate ledger là `wallet_transactions` |
| refund | object | refunded, wallet_tx_ref, refunded_at – khi AI fail |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at | ISODate | |

**Index:** `(user_id, created_at desc)`, `(ai_status, created_at)`, `(user_id, spread_type, created_at desc)` – check daily 1 card limit

---

## 5. reader_profiles

**Mục đích:** Hồ sơ công khai Reader. Liệt kê, tìm kiếm, lọc theo giá/đánh giá/chuyên môn. Trạng thái online/offline cập nhật realtime.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID – 1:1 với users |
| status | string | online \| offline \| accepting_questions |
| pricing | object | diamond_per_question |
| bio | {vi, en, zh} | Mô tả đa ngôn ngữ |
| specialties | array | love, career, general... |
| stats | object | avg_rating, total_reviews |
| badges, title_ref | | title_ref: ObjectId tham chiếu titles |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at | ISODate | |

**Index:** unique `(user_id)`, `(status, updated_at desc)` – listing Reader online

---

## 6. reader_requests

**Mục đích:** Đơn xin trở thành Reader. Luồng: user gửi → admin duyệt → reader_profiles được tạo.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| status | string | pending \| approved \| rejected |
| intro_text | string | Lời giới thiệu |
| proof_documents | array | Tham chiếu tài liệu đính kèm |
| admin_note | string | Ghi chú admin |
| reviewed_by | string | UUID admin duyệt |
| reviewed_at, created_at, updated_at | ISODate | |
| is_deleted, deleted_at | boolean, ISODate | Soft delete (đồng bộ chính sách lưu trữ) |

**Index:** `(user_id, created_at desc)`, `(status, created_at desc)` – hàng đợi duyệt; `(is_deleted)` nếu filter exclude deleted

---

## 7. conversations

**Mục đích:** Header chat 1-1 giữa user và Reader. 1 conversation = 1 chat_finance_session (PostgreSQL). Dùng cho inbox, message stream.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | Tham chiếu: chat_finance_sessions.conversation_ref (string) |
| user_id | string | UUID – user (payer) |
| reader_id | string | UUID – reader (receiver) |
| finance_session_ref | string | UUID chat_finance_sessions.id (PostgreSQL) |
| status | string | pending \| active \| completed \| cancelled \| disputed |
| confirm | object | user_at, reader_at – xác nhận release. **H7 fix:** Đây là convenience UI field; source of truth cho settlement vẫn là `chat_question_items` (PostgreSQL) theo per-item model (ARCH-4.6.4) |
| last_message_at | ISODate | Cập nhật khi có tin mới |
| unread_count | object | {user: N, reader: M} – M9 fix: Denormalized counter. Reset user count khi user mở conversation hoặc gửi read receipt; reset reader count tương tự. Sử dụng `$set` không dùng `$inc` để tránh race condition |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at | ISODate | |

**Index:** `(user_id, status, updated_at desc)`, `(reader_id, status, updated_at desc)`, `(finance_session_ref)` – lookup từ finance session

---

## 8. chat_messages

**Mục đích:** Tin nhắn trong conversation. Hỗ trợ text, system, card_share, payment_offer/accept/reject, system_refund/release.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| conversation_id | ObjectId | conversations._id |
| sender_id | string | UUID – user hoặc reader |
| type | string | text \| system \| card_share \| payment_offer \| payment_accept \| payment_reject \| system_refund \| system_release \| system_dispute – L5 fix: thêm system_dispute cho thông báo tranh chấp |
| content | string | Nội dung tin |
| payment_payload | object | amount_diamond, proposal_id, expires_at – cho payment_offer |
| is_read | boolean | Đã đọc |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at | ISODate | |

**Index:** `(conversation_id, created_at desc)` – timeline, `(sender_id, created_at desc)` – audit/risk

---

## 9. reviews

**Mục đích:** Đánh giá Reader. Gắn với conversation đã hoàn thành.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| author_id | string | UUID user viết review |
| target | object | {type: "reader", id: UUID} – reader được đánh giá |
| conversation_ref | string | ObjectId string của conversations._id |
| rating | 1–5 | Sao |
| comment | string | Nội dung |
| is_deleted, deleted_at, created_at | | |

**Index:** `(target.type, target.id, created_at desc)` – listing review theo Reader

---

## 10. reports

**Mục đích:** Báo cáo vi phạm (tin nhắn, conversation, user). Admin xử lý, ghi result.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| reporter_id | string | UUID |
| target | {type, id} | Đối tượng bị báo cáo |
| conversation_ref | string | ObjectId string – nếu liên quan chat |
| finance_session_ref | string | UUID – nếu liên quan escrow |
| status | string | pending \| processing \| resolved \| rejected |
| result | string | warn \| freeze \| refund \| no_action |
| admin_note | string | |
| resolved_by, resolved_at | | |
| is_deleted, deleted_at | boolean, ISODate | Soft delete (đồng bộ chính sách) |
| created_at, updated_at | ISODate | |

**Index:** `(status, created_at desc)`, `(target.type, target.id, created_at desc)`; `(is_deleted)` nếu filter exclude deleted

---

## 11. referrals

**Mục đích:** Theo dõi mời bạn. Inviter + invited nhận thưởng theo milestone (joined, first_deposit). Chống self-referral qua unique (inviter_id, invited_user_id).

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| inviter_id | string | UUID user mời |
| invited_user_id | string | UUID user được mời |
| invited_email | string | Email invitee (khi chưa join) |
| status | string | invited \| joined \| first_deposit \| reward_paid |
| first_deposit_ref | string | UUID deposit_orders.id – idempotent reward |
| reward_gold, reward_diamond | | Thưởng đã trả |
| reward_paid_at | ISODate | |
| created_at, updated_at | ISODate | |

**Index:** unique `(inviter_id, invited_user_id)`, `(inviter_id, created_at desc)`

---

## 12. quests

**Mục đích:** Định nghĩa quest (daily/weekly/monthly/seasonal). Không lưu progress – dùng quest_progress.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| code | string | unique – vd: daily_draw_1 |
| type | string | daily \| weekly \| monthly \| seasonal |
| name, description | {vi, en, zh} | |
| rules | object | Điều kiện hoàn thành |
| rewards | object | Phần thưởng |
| is_active | boolean | |
| created_at, updated_at | ISODate | |

**Index:** unique `code`, `(type, is_active)`

---

## 13. quest_progress

**Mục đích:** Tiến độ quest theo user + period. period_key: "2026-03-06" (daily), "2026-W10" (weekly), v.v.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| quest_code | string | quests.code |
| period_key | string | 2026-03-06 (daily) \| 2026-W10 (weekly) \| ... |
| current_progress | Int32 | Tiến độ hiện tại |
| target_progress | Int32 | Mục tiêu |
| is_completed | boolean | |
| completed_at, claimed_at | ISODate | |
| created_at, updated_at | ISODate | |

**Index:** unique `(user_id, quest_code, period_key)`, `(quest_code, period_key, is_completed)`

---

## 14. achievements

**Mục đích:** Định nghĩa thành tựu. User mở khóa 1 lần → user_achievements.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| code | string | unique |
| name, description | {vi, en, zh} | |
| criteria | object | Điều kiện mở khóa |
| rewards | object | Phần thưởng |
| created_at, updated_at | ISODate | |

**Index:** unique `code`

---

## 15. user_achievements

**Mục đích:** Thành tựu user đã mở khóa. One-time per user per achievement.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| achievement_code | string | achievements.code |
| earned_at | ISODate | Thời điểm mở khóa |
| reward_claimed_at | ISODate | Đã nhận thưởng |

**Index:** unique `(user_id, achievement_code)`

---

## 16. titles

**Mục đích:** Định nghĩa danh hiệu (style, color, effect). users.active_title_ref tham chiếu _id.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | Tham chiếu: users.active_title_ref (string) |
| code | string | unique |
| name | {vi, en, zh} | |
| style | object | color, effect |

**Index:** unique `code`

---

## 17. user_titles

**Mục đích:** Danh hiệu user sở hữu. User có thể chọn 1 làm active_title_ref.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| title_ref | ObjectId | titles._id |
| earned_at | ISODate | |

**Index:** unique `(user_id, title_ref)`

---

## 18. reading_chains

**Mục đích:** Friend chain – mời bạn rút bài chung. Cả hai nhận thưởng theo quota ngày. L3 fix: Unique (host_user_id, guest_user_id, business_date) – cho phép mời lại vào ngày khác.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| host_user_id | string | UUID host |
| guest_user_id | string | UUID khách |
| business_date | string | L3 fix: YYYY-MM-DD (UTC) – ngày nghiệp vụ, dùng cho unique constraint per day |
| shared_reading_id | ObjectId | reading_sessions._id |
| status | string | pending \| completed |
| is_deleted, deleted_at, created_at | | |

**Index:** `(host_user_id, guest_user_id, business_date)` – L3 fix: cho phép mời lại cùng cặp host-guest vào ngày khác (BR-4.7.2: thưởng theo hạn mức ngày)

---

## 19. events_config

**Mục đích:** Cấu hình sự kiện (Full Moon, seasonal). Cửa sổ thời gian, giá, bonus.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| code | string | unique – vd: full_moon_2026 |
| name | {vi, en, zh} | |
| config | object | price, bonus, exp_multiplier... |
| available_from | ISODate | Cửa sổ bắt đầu |
| available_until | ISODate | Cửa sổ kết thúc |
| is_active | boolean | |
| created_at, updated_at | ISODate | |

**Index:** unique `code`, `(is_active, available_from, available_until)`

---

## 20. notifications

**Mục đích:** Thông báo in-app. TTL 30 ngày. Đa ngôn ngữ title/body.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| title, body | {vi, en, zh} | Nội dung đa ngôn ngữ |
| type | string | quest, system, streak, escrow, ... |
| is_read | boolean | Đã đọc |
| metadata | object | Dữ liệu bổ sung (deep link, ref) |
| created_at | ISODate | Dùng cho TTL 30d |

**Index:** `(user_id, is_read, created_at desc)`, TTL `created_at` 30 ngày

---

## 21. daily_checkins

**Mục đích:** Điểm danh hằng ngày. 1 record/user/ngày. streak_count tại thời điểm check-in.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| business_date | string | YYYY-MM-DD (UTC) |
| streak_count | Int32 | Chuỗi tại thời điểm check-in |
| reward_gold | Int32 | Số Gold thưởng (C3 fix) |
| reward_claimed_at | ISODate | Thời điểm nhận thưởng, NULL nếu chưa nhận – dùng cho idempotency (C3 fix) |
| created_at | ISODate | |

**Index:** unique `(user_id, business_date)`

---

## 22. ai_provider_logs (L4 fix: đổi tên từ grok_logs → ai_provider_logs vì hỗ trợ nhiều provider)

**Mục đích:** Log AI (model, tokens, latency). Chỉ metadata, không lưu prompt/response raw. TTL 90 ngày.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| reading_ref | ObjectId | reading_sessions._id |
| ai_request_ref | string | M6 fix: UUID tham chiếu ai_requests.id (PostgreSQL) – mapping cross-DB trực tiếp |
| model | string | Model gọi |
| tokens | object | in, out |
| latency_ms | Int32 | |
| prompt_version, policy_version | string | |
| status | string | requested \| completed \| failed – trạng thái cuối khi log (C5 fix) |
| error_code | string | Mã lỗi (nếu failed) – dùng cho alert/debug (C5 fix) |
| trace_id | string | Trace ID để correlation với OpenTelemetry (C5 fix) |
| created_at | ISODate | TTL 90d |

**Index:** `(user_id, created_at desc)`, `(status, created_at desc)`, TTL 90 ngày

---

## 23. admin_logs

**Mục đích:** Audit thao tác admin. Mọi hành động quan trọng đều ghi.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| admin_id | string | UUID |
| action | string | BAN_USER, APPROVE_READER, DISPUTE_RESOLVE... |
| target_ref | string | Đối tượng bị tác động |
| changes | object | before, after – snapshot |
| reason | string | Lý do |
| created_at | ISODate | |

**Index:** `(admin_id, created_at desc)`, `(action, created_at desc)`

---

## 24. gacha_logs

**Mục đích:** Log quay Gacha (MongoDB). PostgreSQL gacha_reward_logs là source of truth audit. TTL 180 ngày.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| spent_diamond | Int64 | |
| results | array | {card_id, exp_gained} |
| odds_version | string | Phiên bản tỷ lệ |
| created_at | ISODate | TTL 180d |

**Index:** `(user_id, created_at desc)`, TTL 180 ngày

---

## 25. leaderboard_snapshots

**Mục đích:** Snapshot BXH theo chu kỳ. 3 track: daily_rank_score, monthly_rank_score, lifetime_score. period_key + type unique.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| period_key | string | 2026-W10 (tuần), 2026-03 (tháng), lifetime |
| type | string | achievement_points, diamond_spend, top_readers |
| rankings | array | {user_id, rank, score} |
| created_at | ISODate | |

**Index:** unique `(period_key, type)`. period_key format: YYYY-MM-DD (daily), YYYY-MM (monthly), YYYY-Www (weekly), "lifetime" – khớp BR-4.8.

**C1 fix – Mapping `type` ↔ BR-4.8 score tracks (thống nhất):**

| DB `type` value | BR score track | Mô tả |
|------|------|------|
| `daily_rank_score` | daily track | Điểm xếp hạng ngày |
| `monthly_rank_score` | monthly track | Điểm xếp hạng tháng |
| `lifetime_score` | lifetime track | Điểm xếp hạng vĩnh viễn |
| `achievement_points` | (Phase 5+ extension) | H4 fix: BXH theo điểm thành tựu – chưa có trong BR, implement khi mở rộng |
| `diamond_spend` | (Phase 5+ extension) | H4 fix: BXH theo Diamond chi tiêu – chưa có trong BR |
| `top_readers` | (Phase 5+ extension) | H4 fix: BXH top reader – chưa có trong BR |

---

## 26. community_posts (Phase 4)

**Mục đích:** Bài viết cộng đồng. Forum/feed. Kiểm duyệt qua reports.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| author_id | string | UUID |
| visibility | string | public \| anonymous |
| content | string | |
| attachments | array | |
| stats | object | likes, shares |
| is_deleted, deleted_at | | Soft delete |
| created_at, updated_at | ISODate | |

**Index:** `(created_at desc)`, `(author_id, created_at desc)`, `(visibility, created_at desc)` – filter public posts

---

## 27. community_reactions (Phase 4)

**Mục đích:** Like/share bài viết. Unique (post_id, user_id, type) tránh duplicate.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| post_id | ObjectId | community_posts._id |
| user_id | string | UUID |
| type | string | like \| share |
| created_at | ISODate | |

**Index:** unique `(post_id, user_id, type)`, `(user_id, created_at desc)`

---

## 28. call_sessions (Phase 4)

**Mục đích:** Metadata gọi thoại/video với Reader. Luồng: requested → accepted/rejected → ended.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| conversation_id | ObjectId | conversations._id |
| initiator_id | string | UUID |
| type | string | audio \| video |
| status | string | requested \| accepted \| rejected \| ended |
| started_at, ended_at | ISODate | |
| created_at, updated_at | ISODate | |

**Index:** `(conversation_id, created_at desc)`, `(status, conversation_id)` – filter active calls

---

## 29. share_claims (H5 fix – Anti-abuse pipeline cho share rewards)

**Mục đích:** Lưu claims chia sẻ để xác minh và chống abuse. Pipeline: submitted → verified/rejected. Tích hợp device fingerprint, IP velocity, risk score.

| Field | Type | Mục đích / Ghi chú |
|-------|------|--------------------|
| _id | ObjectId | |
| user_id | string | UUID |
| share_url | string | URL bài chia sẻ |
| platform | string | facebook \| twitter \| tiktok \| zalo \| other |
| device_fingerprint | string | Fingerprint thiết bị |
| ip_address | string | IP khi submit |
| risk_score | Double | Điểm rủi ro từ pipeline (0-1) |
| status | string | submitted \| verified \| rejected |
| result_reason | string | Lý do reject (nếu có) |
| reward_gold | Int32 | Gold thưởng (sau khi verified) |
| wallet_tx_ref | string | UUID wallet_transactions.id (nếu đã credit) |
| created_at, verified_at | ISODate | |

**Index:** `(user_id, created_at desc)`, `(status, created_at desc)`, `(user_id, platform, created_at desc)` – velocity check per user per platform

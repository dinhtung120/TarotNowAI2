-- =============================================================================
-- TarotWeb – Seed: system_configs
-- =============================================================================
-- Mục đích: Chèn các cấu hình runtime (key-value) tối thiểu cần thiết cho
-- hệ thống hoạt động. Đây là dữ liệu seed khởi tạo, admin có thể thay đổi
-- sau qua giao diện quản trị.
--
-- Cách chạy:
--   psql -h localhost -U postgres -d tarotweb -f database/postgresql/seed_system_configs.sql
--
-- Idempotent: Sử dụng ON CONFLICT DO NOTHING – chạy lại không lỗi, không ghi đè
-- giá trị đã được admin chỉnh sửa.
--
-- Nguồn: BR-4.3.2 (tỷ giá), ARCH-4.4.2 (AI quota), UX-4.4.5 (pricing)
-- =============================================================================

-- -------------------------------------------------------------------------
-- 1. Tỷ giá & Tài chính
-- -------------------------------------------------------------------------

-- Tỷ giá VND → Diamond: 1 Diamond = 1000 VND (BR-4.3.2)
-- Đây là tỷ giá cố định, admin có thể thay đổi theo chính sách giá
INSERT INTO system_configs (key, value, description)
VALUES ('diamond_vnd_rate', '1000', 'Tỷ giá: 1 Diamond = ? VND. Mặc định 1000.')
ON CONFLICT (key) DO NOTHING;

-- Phí nền tảng khi Reader rút tiền: 10% (BR-4.3.2)
INSERT INTO system_configs (key, value, description)
VALUES ('platform_fee_percent', '10', 'Phí nền tảng (%) khi Reader rút tiền. Mặc định 10%.')
ON CONFLICT (key) DO NOTHING;

-- Số Diamond tối thiểu Reader cần có để tạo yêu cầu rút tiền
INSERT INTO system_configs (key, value, description)
VALUES ('min_withdrawal_diamond', '50', 'Số Diamond tối thiểu để rút tiền. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 2. Thưởng & Gamification
-- -------------------------------------------------------------------------

-- Thưởng Gold khi điểm danh hàng ngày (Daily Check-in)
INSERT INTO system_configs (key, value, description)
VALUES ('daily_checkin_gold', '5', 'Số Gold thưởng khi điểm danh hàng ngày. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;

-- Thưởng Gold khi đăng ký tài khoản mới (Register Bonus)
INSERT INTO system_configs (key, value, description)
VALUES ('register_bonus_gold', '50', 'Số Gold thưởng khi đăng ký mới. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;

-- Thưởng Gold khi giới thiệu thành công (Referral)
INSERT INTO system_configs (key, value, description)
VALUES ('referral_reward_gold', '20', 'Số Gold thưởng cho người giới thiệu khi invited user đăng ký.')
ON CONFLICT (key) DO NOTHING;

-- Thưởng Gold khi invited user nạp tiền lần đầu
INSERT INTO system_configs (key, value, description)
VALUES ('referral_first_deposit_gold', '50', 'Thưởng Gold khi người được mời nạp tiền lần đầu.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 3. AI Reading – Quota & Pricing
-- -------------------------------------------------------------------------

-- Số lần xem bài AI miễn phí mỗi ngày cho user FREE (không Premium)
INSERT INTO system_configs (key, value, description)
VALUES ('ai_daily_quota_free', '3', 'Quota AI reading/ngày cho user Free. Mặc định 3.')
ON CONFLICT (key) DO NOTHING;

-- Số lần xem bài AI miễn phí mỗi ngày cho user PREMIUM
INSERT INTO system_configs (key, value, description)
VALUES ('ai_daily_quota_premium', '10', 'Quota AI reading/ngày cho user Premium. Mặc định 10.')
ON CONFLICT (key) DO NOTHING;

-- Chi phí Gold cho bài trải 1 lá (daily_1) – miễn phí theo quota
INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_daily_1_gold', '0', 'Chi phí Gold cho bài 1 lá hàng ngày. Mặc định 0 (miễn phí).')
ON CONFLICT (key) DO NOTHING;

-- Chi phí Gold cho bài trải 3 lá
INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_3_gold', '10', 'Chi phí Gold cho bài trải 3 lá. Mặc định 10.')
ON CONFLICT (key) DO NOTHING;

-- Chi phí Gold cho bài trải 5 lá
INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_5_gold', '20', 'Chi phí Gold cho bài trải 5 lá. Mặc định 20.')
ON CONFLICT (key) DO NOTHING;

-- Chi phí Gold cho bài trải 10 lá (Celtic Cross)
INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_10_gold', '50', 'Chi phí Gold cho bài trải 10 lá. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 4. AI Provider – Timeout & Retry
-- -------------------------------------------------------------------------

-- Timeout tối đa khi gọi AI provider (giây)
INSERT INTO system_configs (key, value, description)
VALUES ('ai_timeout_seconds', '30', 'Timeout tối đa khi gọi AI provider (giây). Mặc định 30.')
ON CONFLICT (key) DO NOTHING;

-- Số lần retry khi AI provider fail
INSERT INTO system_configs (key, value, description)
VALUES ('ai_max_retries', '2', 'Số lần retry khi AI provider fail. Mặc định 2.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 5. Chat & Escrow
-- -------------------------------------------------------------------------

-- Giá tối thiểu Diamond cho 1 câu hỏi chat (Reader đặt)
INSERT INTO system_configs (key, value, description)
VALUES ('chat_min_question_diamond', '5', 'Giá tối thiểu Diamond cho 1 câu hỏi chat. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;

-- Thời gian Reader phải trả lời sau khi accept (giờ) – quá hạn → auto refund
INSERT INTO system_configs (key, value, description)
VALUES ('chat_reader_response_hours', '24', 'Giờ Reader phải trả lời sau accept. Quá hạn → auto refund. Mặc định 24.')
ON CONFLICT (key) DO NOTHING;

-- Thời gian auto-release sau khi Reader đã reply (giờ)
INSERT INTO system_configs (key, value, description)
VALUES ('chat_auto_release_hours', '24', 'Giờ auto-release sau khi Reader reply. Mặc định 24.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 6. Friend Chain & Share
-- -------------------------------------------------------------------------

-- Số lần tối đa mời bạn rút bài chung 1 ngày
INSERT INTO system_configs (key, value, description)
VALUES ('friend_chain_daily_limit', '3', 'Giới hạn mời bạn rút bài/ngày. Mặc định 3.')
ON CONFLICT (key) DO NOTHING;

-- Thưởng Gold khi chia sẻ reading lên mạng xã hội (sau khi verified)
INSERT INTO system_configs (key, value, description)
VALUES ('share_reward_gold', '5', 'Thưởng Gold khi chia sẻ reading (đã verified). Mặc định 5.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 7. Gacha
-- -------------------------------------------------------------------------

-- Chi phí Diamond cho 1 lần quay Gacha
INSERT INTO system_configs (key, value, description)
VALUES ('gacha_cost_diamond', '5', 'Chi phí Diamond cho 1 lần quay Gacha. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;

-- -------------------------------------------------------------------------
-- 8. Streak Freeze
-- -------------------------------------------------------------------------

-- Có cho phép mua Streak Freeze không
INSERT INTO system_configs (key, value, description)
VALUES ('streak_freeze_enabled', 'true', 'Cho phép mua Streak Freeze. Mặc định true.')
ON CONFLICT (key) DO NOTHING;

-- Verify: hiển thị tất cả configs đã seed
-- SELECT key, value, description FROM system_configs ORDER BY key;

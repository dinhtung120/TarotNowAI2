





















INSERT INTO system_configs (key, value, description)
VALUES ('diamond_vnd_rate', '1000', 'Tỷ giá: 1 Diamond = ? VND. Mặc định 1000.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('platform_fee_percent', '10', 'Phí nền tảng (%) khi Reader rút tiền. Mặc định 10%.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('min_withdrawal_diamond', '50', 'Số Diamond tối thiểu để rút tiền. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('daily_checkin_gold', '5', 'Số Gold thưởng khi điểm danh hàng ngày. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('register_bonus_gold', '50', 'Số Gold thưởng khi đăng ký mới. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('referral_reward_gold', '20', 'Số Gold thưởng cho người giới thiệu khi invited user đăng ký.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('referral_first_deposit_gold', '50', 'Thưởng Gold khi người được mời nạp tiền lần đầu.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('ai_daily_quota_free', '3', 'Quota AI reading/ngày cho user Free. Mặc định 3.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_daily_1_gold', '0', 'Chi phí Gold cho bài 1 lá hàng ngày. Mặc định 0 (miễn phí).')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_3_gold', '10', 'Chi phí Gold cho bài trải 3 lá. Mặc định 10.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_5_gold', '20', 'Chi phí Gold cho bài trải 5 lá. Mặc định 20.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('reading_cost_spread_10_gold', '50', 'Chi phí Gold cho bài trải 10 lá. Mặc định 50.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('ai_timeout_seconds', '30', 'Timeout tối đa khi gọi AI provider (giây). Mặc định 30.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('ai_max_retries', '2', 'Số lần retry khi AI provider fail. Mặc định 2.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('chat_min_question_diamond', '5', 'Giá tối thiểu Diamond cho 1 câu hỏi chat. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('chat_reader_response_hours', '24', 'Giờ Reader phải trả lời sau accept. Quá hạn → auto refund. Mặc định 24.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('chat_auto_release_hours', '24', 'Giờ auto-release sau khi Reader reply. Mặc định 24.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('friend_chain_daily_limit', '3', 'Giới hạn mời bạn rút bài/ngày. Mặc định 3.')
ON CONFLICT (key) DO NOTHING;


INSERT INTO system_configs (key, value, description)
VALUES ('share_reward_gold', '5', 'Thưởng Gold khi chia sẻ reading (đã verified). Mặc định 5.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('gacha_cost_diamond', '5', 'Chi phí Diamond cho 1 lần quay Gacha. Mặc định 5.')
ON CONFLICT (key) DO NOTHING;






INSERT INTO system_configs (key, value, description)
VALUES ('streak_freeze_enabled', 'true', 'Cho phép mua Streak Freeze. Mặc định true.')
ON CONFLICT (key) DO NOTHING;



-- =============================================================================
-- TarotWeb – Seed: user_exp_levels + card_exp_levels
-- =============================================================================
-- Mục đích: Chèn bảng quy đổi EXP → Level cho user và card.
-- Đây là lookup table dùng để xác định level dựa trên EXP tích lũy.
--
-- Cách tính level: user/card có EXP >= min_exp của level N
-- thì đạt level N. Level cao nhất thỏa mãn = level hiện tại.
--
-- Cách chạy:
--   psql -h localhost -U postgres -d tarotweb -f database/postgresql/seed_exp_levels.sql
--
-- Idempotent: ON CONFLICT DO NOTHING – chạy lại không lỗi.
--
-- Công thức EXP: Sử dụng progression curve phổ biến trong game design,
-- EXP tăng dần theo cấp số nhân để tạo cảm giác thử thách tăng dần.
-- =============================================================================

-- =========================================================================
-- USER EXP LEVELS (50 levels)
-- =========================================================================
-- Level 1 bắt đầu từ 0 EXP (mọi user mới đều là level 1)
-- Progression: tăng nhanh dần ở các level cao để kéo dài thời gian chơi
-- Level 1-10: dễ đạt (onboarding, giữ chân user mới)
-- Level 11-30: trung bình (core gameplay loop)
-- Level 31-50: khó (endgame, long-term engagement)

INSERT INTO user_exp_levels (level, min_exp) VALUES
  (1, 0),           -- Khởi đầu: mọi user mới
  (2, 100),         -- ~2-3 ngày sử dụng cơ bản
  (3, 250),
  (4, 450),
  (5, 700),         -- ~1 tuần active
  (6, 1000),
  (7, 1400),
  (8, 1900),
  (9, 2500),
  (10, 3200),       -- ~2-3 tuần, milestone đầu tiên
  (11, 4000),
  (12, 5000),
  (13, 6200),
  (14, 7600),
  (15, 9200),       -- ~1 tháng active
  (16, 11000),
  (17, 13000),
  (18, 15500),
  (19, 18500),
  (20, 22000),      -- ~2 tháng, milestone lớn
  (21, 26000),
  (22, 30500),
  (23, 35500),
  (24, 41000),
  (25, 47000),      -- ~3 tháng
  (26, 54000),
  (27, 62000),
  (28, 71000),
  (29, 81000),
  (30, 92000),      -- ~4-5 tháng, milestone endgame starts
  (31, 105000),
  (32, 120000),
  (33, 137000),
  (34, 156000),
  (35, 177000),     -- ~6 tháng
  (36, 200000),
  (37, 226000),
  (38, 255000),
  (39, 287000),
  (40, 322000),     -- ~8-10 tháng
  (41, 362000),
  (42, 407000),
  (43, 457000),
  (44, 513000),
  (45, 575000),     -- ~1 năm
  (46, 645000),
  (47, 723000),
  (48, 810000),
  (49, 907000),
  (50, 1015000)     -- Endgame: ~1.5 năm dedicated play
ON CONFLICT (level) DO NOTHING;

-- =========================================================================
-- CARD EXP LEVELS (20 levels)
-- =========================================================================
-- Mỗi lá bài có hệ thống level riêng (1-20).
-- Level ảnh hưởng đến:
--   - Free follow-up slots (level 5 → 1 free, level 10 → 2 free, etc.)
--   - Visual effects (holographic, parallax, particles)
--   - Ascension stories (mốc level 6, 7, ..., 20)
--
-- EXP tích lũy khi user rút đúng lá đó trong reading sessions.
-- Progression chậm hơn user level vì có 78 lá bài riêng biệt.

INSERT INTO card_exp_levels (level, min_exp) VALUES
  (1, 0),           -- Khởi đầu: lá bài mới được thu thập
  (2, 50),          -- ~3-5 lần rút lá này
  (3, 120),
  (4, 210),
  (5, 320),         -- Milestone: mở khóa 1 free follow-up slot
  (6, 450),         -- Ascension story #1
  (7, 600),         -- Ascension story #2
  (8, 800),
  (9, 1050),
  (10, 1350),       -- Milestone: mở khóa 2 free follow-up slots
  (11, 1700),
  (12, 2100),
  (13, 2600),
  (14, 3200),
  (15, 3900),       -- Milestone: mở khóa 3 free follow-up slots
  (16, 4800),
  (17, 5900),
  (18, 7200),
  (19, 8800),
  (20, 10800)       -- Max level: full visual effects, max free follow-ups
ON CONFLICT (level) DO NOTHING;

-- Verify: hiển thị dữ liệu đã seed
-- SELECT level, min_exp FROM user_exp_levels ORDER BY level;
-- SELECT level, min_exp FROM card_exp_levels ORDER BY level;

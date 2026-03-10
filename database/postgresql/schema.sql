-- =============================================================================
-- TarotWeb – PostgreSQL Schema
-- Version: v3.6 (Blueprint) | Nguồn: tai-lieu-thiet-ke + v1/v2
-- =============================================================================
-- Mục đích: Nguồn sự thật (source of truth) cho dữ liệu tài chính, ví, escrow,
-- subscription, auth. Mọi thao tác tiền bắt buộc nằm trong transaction.
--
-- QUAN TRỌNG:
-- - Mọi finance command (freeze, release, refund, deposit, withdrawal) PHẢI gọi
--   stored procedure (proc_wallet_*) – không cho app code thực hiện rời rạc.
-- - SERIALIZABLE hoặc SELECT ... FOR UPDATE cho finance transactions (ARCH-4.6.4).
-- - Reconciliation job: so sánh v_user_ledger_balance với users.<balance>; alert nếu mismatch.
-- - ON DELETE: Không dùng CASCADE cho users (soft delete). RESTRICT mặc định.
-- - Double-entry: Chuyển khoản (payer→receiver) insert 2 rows, cùng reference_id, amount trái dấu.
-- - Partitioning: wallet_transactions cân nhắc RANGE (created_at) theo tháng khi DAU Tier M/L.
-- =============================================================================

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- =============================================================================
-- ENUM TYPES
-- =============================================================================

-- Vai trò: user (thường), tarot_reader (reader chợ), admin (quản trị)
-- M10 fix: thêm 'system' cho tài khoản hệ thống (platform, escrow)


-- Ngôn ngữ UI: vi, en, zh_hans (map tương ứng zh-Hans trong tài liệu)


-- Cung hoàng đạo 12 cung


-- Loại tiền tệ: gold (miễn phí), diamond (trả phí, 1 Diamond = 1000 VND)


-- Trạng thái đơn nạp: pending→success/failed/refunded/disputed


-- Phương thức thanh toán: bank_transfer, vietqr, paypal


-- Trạng thái rút tiền Reader: pending→approved/rejected→paid


-- Trạng thái duyệt (reader, v.v.): pending→approved/rejected


-- Loại gói thuê bao: monthly, yearly


-- Loại OTP: register (xác minh email), reset_password


-- Trạng thái escrow câu hỏi: pending→accepted→released/refunded/disputed
-- C6 fix: thêm 'accepted' để phân biệt "chưa accept" vs "đã accept chờ reply"


-- Loại câu hỏi: main_question (câu đầu), add_question (câu thêm)


-- Trạng thái phiên tài chính chat: pending→active→completed/refunded/disputed/cancelled


-- Trạng thái yêu cầu AI: requested→first_token_received→completed/failed_*


-- Nguồn reference_id: postgres (UUID), mongo (ObjectId), system
-- Removed reference_source enum used for EF Core

-- Removed VARCHAR(20) and transaction_type custom enums to support EF Core string mapping

-- =============================================================================
-- TABLES
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1) users
-- Mục đích: Bảng tài khoản người dùng, chứa thông tin hồ sơ, số dư ví,
-- role, streak, MFA. Invariant: diamond_balance = available, frozen_diamond_balance = đang escrow.
-- Tổng wallet = diamond_balance + frozen_diamond_balance.
-- -----------------------------------------------------------------------------
CREATE TABLE users (
    id                       UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    -- Đăng nhập & định danh
    email                    VARCHAR(255)  NOT NULL UNIQUE,           -- Email đăng nhập, unique
    username                 VARCHAR(50)   NOT NULL UNIQUE,           -- Tên đăng nhập, unique
    password_hash            VARCHAR(255)  NOT NULL,                  -- Argon2id hash, không lưu plain text
    -- Hồ sơ
    display_name             VARCHAR(100)  NOT NULL,                  -- Tên hiển thị
    date_of_birth            DATE          NOT NULL,                  -- Ngày sinh (cổng tuổi 18+)
    avatar_url               TEXT,                                    -- URL ảnh đại diện
    zodiac_sign              VARCHAR(20),                                  -- Auto-tính từ DOB
    numerology_number        SMALLINT      CHECK ((numerology_number BETWEEN 1 AND 9) OR numerology_number IN (11, 22, 33)),  -- Thần số học từ DOB
    active_title_ref         TEXT,                                    -- ObjectId string tham chiếu titles._id (MongoDB)
    -- Vai trò & phê duyệt
    role                     VARCHAR(50)     NOT NULL DEFAULT 'user',   -- user | tarot_reader | admin
    reader_status            VARCHAR(20) DEFAULT 'pending',       -- Duyệt đăng ký Reader
    -- Ví Gold/Diamond (BIGINT, 1 Diamond = 1000 VND)
    gold_balance             BIGINT        NOT NULL DEFAULT 0 CHECK (gold_balance >= 0),      -- Số dư Gold (tiền miễn phí)
    diamond_balance          BIGINT        NOT NULL DEFAULT 0 CHECK (diamond_balance >= 0),   -- Số dư Diamond khả dụng (chưa freeze)
    frozen_diamond_balance   BIGINT        NOT NULL DEFAULT 0 CHECK (frozen_diamond_balance >= 0),  -- Diamond đang escrow (chat)
    total_diamonds_purchased BIGINT        NOT NULL DEFAULT 0,        -- Tổng Diamond đã nạp (lịch sử)
    -- Chặn payout khi chargeback/dispute
    chargeback_hold          BOOLEAN       NOT NULL DEFAULT false,    -- Chặn rút khi chargeback
    dispute_hold             BOOLEAN       NOT NULL DEFAULT false,    -- Chặn rút khi đang tranh chấp
    -- Trò chơi hóa
    exp                      BIGINT        NOT NULL DEFAULT 0,        -- EXP người dùng (→ level)
    level                    INT           NOT NULL DEFAULT 1,        -- Cấp người dùng
    achievement_points       INT           NOT NULL DEFAULT 0,        -- Điểm thành tựu (leaderboard)
    -- Chuỗi rút bài hàng ngày (Daily streak)
    current_streak           INT           NOT NULL DEFAULT 0,        -- Số ngày streak liên tiếp hiện tại
    last_streak_date         DATE,                                    -- Ngày UTC gần nhất có rút bài hợp lệ
    pre_break_streak         INT           NOT NULL DEFAULT 0,        -- Streak trước khi gãy (dùng tính giá Streak Freeze)
    -- Cài đặt
    preferred_language       VARCHAR(20)     NOT NULL DEFAULT 'vi',     -- vi | en | zh_hans
    status                   VARCHAR(20)   NOT NULL DEFAULT 'pending', -- pending | active | suspended | banned
    -- MFA (bắt buộc cho Reader/Admin trước payout/admin)
    mfa_enabled              BOOLEAN       NOT NULL DEFAULT false,    -- Đã bật MFA
    mfa_secret_encrypted     TEXT,                                    -- Secret TOTP đã mã hóa (không lưu plain)
    mfa_verified_at          TIMESTAMPTZ,                             -- Thời điểm xác minh MFA gần nhất
    -- Referral
    referral_code            VARCHAR(20)   UNIQUE,                    -- Mã giới thiệu của user
    referred_by_id           UUID         REFERENCES users(id),       -- User mời (nếu có)
    -- Soft delete
    is_deleted               BOOLEAN       NOT NULL DEFAULT false,
    deleted_at               TIMESTAMPTZ,
    -- Timestamps
    created_at               TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at               TIMESTAMPTZ,
    last_login_at            TIMESTAMPTZ
);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_role_status ON users(role, status) WHERE is_deleted = false;
CREATE INDEX idx_users_referred_by ON users(referred_by_id) WHERE referred_by_id IS NOT NULL;
CREATE INDEX idx_users_created_at ON users(created_at);

COMMENT ON TABLE users IS 'Bảng tài khoản người dùng: auth, ví Gold/Diamond, role, streak, MFA. Nguồn sự thật cho số dư và trạng thái tài khoản.';
COMMENT ON COLUMN users.id IS 'UUID primary key.';
COMMENT ON COLUMN users.email IS 'Email đăng nhập, unique.';
COMMENT ON COLUMN users.username IS 'Tên đăng nhập, unique.';
COMMENT ON COLUMN users.password_hash IS 'Argon2id hash, không lưu plain text.';
COMMENT ON COLUMN users.display_name IS 'Tên hiển thị công khai.';
COMMENT ON COLUMN users.date_of_birth IS 'Ngày sinh, dùng cổng tuổi 18+.';
COMMENT ON COLUMN users.avatar_url IS 'URL ảnh đại diện.';
COMMENT ON COLUMN users.role IS 'user | tarot_reader | admin.';
COMMENT ON COLUMN users.reader_status IS 'Trạng thái duyệt đăng ký Reader: pending | approved | rejected.';
COMMENT ON COLUMN users.zodiac_sign IS 'Cung hoàng đạo, auto-tính từ DOB.';
COMMENT ON COLUMN users.numerology_number IS 'Thần số học từ DOB (1-9, 11, 22, 33).';
COMMENT ON COLUMN users.active_title_ref IS 'ObjectId string tham chiếu titles._id (MongoDB).';
COMMENT ON COLUMN users.gold_balance IS 'Số dư Gold (tiền miễn phí).';
COMMENT ON COLUMN users.diamond_balance IS 'Số dư Diamond khả dụng (available). Tổng = diamond_balance + frozen_diamond_balance.';
COMMENT ON COLUMN users.frozen_diamond_balance IS 'Diamond đang escrow trong chat, chưa release/refund.';
COMMENT ON COLUMN users.total_diamonds_purchased IS 'Tổng Diamond đã nạp, lịch sử.';
COMMENT ON COLUMN users.chargeback_hold IS 'Chặn payout khi có chargeback chưa xử lý xong.';
COMMENT ON COLUMN users.dispute_hold IS 'Chặn payout khi đang tranh chấp.';
COMMENT ON COLUMN users.exp IS 'EXP người dùng, quy đổi sang level.';
COMMENT ON COLUMN users.level IS 'Cấp người dùng.';
COMMENT ON COLUMN users.achievement_points IS 'Điểm thành tựu (leaderboard).';
COMMENT ON COLUMN users.current_streak IS 'Số ngày streak liên tiếp hiện tại.';
COMMENT ON COLUMN users.last_streak_date IS 'Ngày UTC gần nhất có rút bài hợp lệ.';
COMMENT ON COLUMN users.pre_break_streak IS 'Streak trước khi gãy; dùng cho công thức Streak Freeze: ceil(pre_break_streak/10) Diamond.';
COMMENT ON COLUMN users.preferred_language IS 'vi | en | zh_hans.';
COMMENT ON COLUMN users.status IS 'pending | active | suspended | banned.';
COMMENT ON COLUMN users.mfa_enabled IS 'Đã bật MFA (bắt buộc Reader/Admin trước payout).';
COMMENT ON COLUMN users.mfa_secret_encrypted IS 'Secret TOTP đã mã hóa, không lưu plain.';
COMMENT ON COLUMN users.mfa_verified_at IS 'Thời điểm xác minh MFA gần nhất.';
COMMENT ON COLUMN users.referral_code IS 'Mã giới thiệu của user.';
COMMENT ON COLUMN users.referred_by_id IS 'UUID user mời (nếu có).';
COMMENT ON COLUMN users.is_deleted IS 'Soft delete flag.';
COMMENT ON COLUMN users.deleted_at IS 'Thời điểm soft delete.';
COMMENT ON COLUMN users.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN users.updated_at IS 'Thời điểm cập nhật cuối.';
COMMENT ON COLUMN users.last_login_at IS 'Thời điểm đăng nhập cuối.';

-- -----------------------------------------------------------------------------
-- 2) user_consents
-- Mục đích: Lịch sử đồng ý pháp lý (TOS, Privacy Policy, AI disclaimer).
-- Bắt buộc trước khi tạo tài khoản xong. Có versioning.
-- -----------------------------------------------------------------------------
CREATE TABLE user_consents (
    id           UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id      UUID        NOT NULL REFERENCES users(id) ON DELETE RESTRICT,  -- L1 fix: giữ consent history cho compliance (OPS-4.13.1), không CASCADE
    consent_type VARCHAR(50) NOT NULL,    -- tos | privacy_policy | ai_disclaimer
    version      VARCHAR(20) NOT NULL,    -- Phiên bản tài liệu (vd: v1.0)
    accepted_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ip_address   INET,                    -- IP khi đồng ý (audit)
    user_agent   TEXT                     -- User agent (audit)
);
CREATE INDEX idx_user_consents_user ON user_consents(user_id, consent_type);

COMMENT ON TABLE user_consents IS 'Lịch sử đồng ý pháp lý: TOS, Privacy, AI disclaimer. Cần trước khi hoàn tất đăng ký.';
COMMENT ON COLUMN user_consents.id IS 'UUID primary key.';
COMMENT ON COLUMN user_consents.user_id IS 'FK users.';
COMMENT ON COLUMN user_consents.consent_type IS 'tos | privacy_policy | ai_disclaimer.';
COMMENT ON COLUMN user_consents.version IS 'Phiên bản tài liệu (vd: v1.0).';
COMMENT ON COLUMN user_consents.accepted_at IS 'Thời điểm đồng ý.';
COMMENT ON COLUMN user_consents.ip_address IS 'IP khi đồng ý (audit).';
COMMENT ON COLUMN user_consents.user_agent IS 'User agent (audit).';

-- -----------------------------------------------------------------------------
-- 3) email_otps
-- Mục đích: OTP xác minh email (đăng ký) và reset mật khẩu. Có TTL.
-- -----------------------------------------------------------------------------
CREATE TABLE email_otps (
    id         UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id    UUID        NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    otp_code   VARCHAR(6)  NOT NULL,      -- Mã 6 ký tự
    type       VARCHAR(20)    NOT NULL,      -- register | reset_password
    is_used    BOOLEAN     NOT NULL DEFAULT false,
    expires_at TIMESTAMPTZ NOT NULL,      -- Hết hạn (vd: 30 phút)
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_email_otps_user ON email_otps(user_id);
CREATE INDEX idx_email_otps_expires ON email_otps(expires_at) WHERE is_used = false;

COMMENT ON TABLE email_otps IS 'OTP xác minh email / reset mật khẩu. Hết hạn sau 30 phút, dùng một lần.';
COMMENT ON COLUMN email_otps.id IS 'UUID primary key.';
COMMENT ON COLUMN email_otps.user_id IS 'FK users.';
COMMENT ON COLUMN email_otps.otp_code IS 'Mã 6 ký tự.';
COMMENT ON COLUMN email_otps.type IS 'register | reset_password.';
COMMENT ON COLUMN email_otps.is_used IS 'Đã sử dụng.';
COMMENT ON COLUMN email_otps.expires_at IS 'Hết hạn (vd: 30 phút).';
COMMENT ON COLUMN email_otps.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 4) password_reset_tokens
-- Mục đích: Token reset mật khẩu, dùng 1 lần, có TTL.
-- -----------------------------------------------------------------------------
CREATE TABLE password_reset_tokens (
    id         UUID         PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id    UUID         NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token      VARCHAR(255) NOT NULL UNIQUE,
    is_used    BOOLEAN      NOT NULL DEFAULT false,
    expires_at TIMESTAMPTZ  NOT NULL,     -- Thường 30 phút
    created_at TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

COMMENT ON TABLE password_reset_tokens IS 'Token đặt lại mật khẩu, một lần sử dụng, TTL 30 phút.';
COMMENT ON COLUMN password_reset_tokens.id IS 'UUID primary key.';
COMMENT ON COLUMN password_reset_tokens.user_id IS 'FK users.';
COMMENT ON COLUMN password_reset_tokens.token IS 'Token unique, dùng 1 lần.';
COMMENT ON COLUMN password_reset_tokens.is_used IS 'Đã sử dụng.';
COMMENT ON COLUMN password_reset_tokens.expires_at IS 'Thường 30 phút.';
COMMENT ON COLUMN password_reset_tokens.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 4b) refresh_tokens (N2 fix: cần cho JWT refresh rotation + reuse detection)
-- Mục đích: Lưu refresh token chains. Detect reuse → revoke toàn family.
-- ARCH-4.1.5: refresh rotation, CODING_PLAN P1-AUTH-BE-1.4: reuse detection.
-- -----------------------------------------------------------------------------
CREATE TABLE refresh_tokens (
    id              UUID         PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID         NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token           VARCHAR(500) NOT NULL UNIQUE,
    expires_at      TIMESTAMPTZ  NOT NULL,
    created_at      TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    created_by_ip   VARCHAR(45),
    revoked_at      TIMESTAMPTZ
);
CREATE INDEX idx_refresh_tokens_user ON refresh_tokens(user_id, created_at DESC);
CREATE INDEX idx_refresh_tokens_token ON refresh_tokens(token);

COMMENT ON TABLE refresh_tokens IS 'Refresh token cho Session. Đã đơn giản hóa matching với Backend.';
COMMENT ON COLUMN refresh_tokens.id IS 'UUID primary key.';
COMMENT ON COLUMN refresh_tokens.user_id IS 'FK users.';
COMMENT ON COLUMN refresh_tokens.token IS 'JWT Refresh Token.';
COMMENT ON COLUMN refresh_tokens.expires_at IS 'Thời điểm hết hạn.';
COMMENT ON COLUMN refresh_tokens.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN refresh_tokens.created_by_ip IS 'IP tạo Token.';
COMMENT ON COLUMN refresh_tokens.revoked_at IS 'Thời gian Revoke.';

-- -----------------------------------------------------------------------------
-- 5) deposit_promotions
-- Mục đích: Khuyến mãi nạp tiền (theo mốc VND, cửa sổ sự kiện).
-- -----------------------------------------------------------------------------
CREATE TABLE deposit_promotions (
    id                    UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    code                  VARCHAR(50)   UNIQUE,      -- Mã khuyến mãi (nullable nếu auto-apply)
    name                  VARCHAR(100)  NOT NULL,
    min_amount_vnd        BIGINT        NOT NULL CHECK (min_amount_vnd >= 0),   -- Mốc tối thiểu VND
    bonus_percentage      SMALLINT      NOT NULL DEFAULT 0,  -- % thưởng
    bonus_fixed_diamond   BIGINT        NOT NULL DEFAULT 0,  -- Số Diamond thưởng cố định
    exp_multiplier        SMALLINT      NOT NULL DEFAULT 1 CHECK (exp_multiplier >= 1),  -- Hệ số EXP (event pack), tối thiểu 1
    exp_duration_days     SMALLINT      NOT NULL DEFAULT 0,  -- Số ngày hiệu lực exp_multiplier
    -- M5 fix: nếu exp_multiplier > 1, bắt buộc exp_duration_days > 0 (tránh boost vô hạn)
    CONSTRAINT chk_exp_multiplier_duration CHECK (exp_multiplier = 1 OR exp_duration_days > 0),
    available_from        TIMESTAMPTZ,              -- Cửa sổ sự kiện bắt đầu
    available_until       TIMESTAMPTZ,              -- Cửa sổ sự kiện kết thúc
    is_active             BOOLEAN       NOT NULL DEFAULT true,
    is_deleted            BOOLEAN       NOT NULL DEFAULT false,
    deleted_at            TIMESTAMPTZ,
    created_at            TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at            TIMESTAMPTZ
);

COMMENT ON TABLE deposit_promotions IS 'Khuyến mãi nạp tiền: mốc VND, bonus %, Diamond cố định, cửa sổ sự kiện.';
COMMENT ON COLUMN deposit_promotions.id IS 'UUID primary key.';
COMMENT ON COLUMN deposit_promotions.code IS 'Mã khuyến mãi, nullable nếu auto-apply.';
COMMENT ON COLUMN deposit_promotions.name IS 'Tên hiển thị.';
COMMENT ON COLUMN deposit_promotions.min_amount_vnd IS 'Mốc tối thiểu VND để áp dụng.';
COMMENT ON COLUMN deposit_promotions.bonus_percentage IS 'Phần trăm thưởng.';
COMMENT ON COLUMN deposit_promotions.bonus_fixed_diamond IS 'Số Diamond thưởng cố định.';
COMMENT ON COLUMN deposit_promotions.exp_multiplier IS 'Hệ số EXP (event pack).';
COMMENT ON COLUMN deposit_promotions.exp_duration_days IS 'Số ngày hiệu lực exp_multiplier.';
COMMENT ON COLUMN deposit_promotions.available_from IS 'Cửa sổ sự kiện bắt đầu.';
COMMENT ON COLUMN deposit_promotions.available_until IS 'Cửa sổ sự kiện kết thúc.';
COMMENT ON COLUMN deposit_promotions.is_active IS 'Còn hoạt động.';
COMMENT ON COLUMN deposit_promotions.is_deleted IS 'Soft delete.';
COMMENT ON COLUMN deposit_promotions.deleted_at IS 'Thời điểm soft delete.';
COMMENT ON COLUMN deposit_promotions.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN deposit_promotions.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 6) deposit_orders
-- Mục đích: Đơn nạp Diamond. Lưu fx_rate_snapshot khi provider không VND.
-- Hỗ trợ hold→capture/settle. Có trường thuế cho compliance.
-- -----------------------------------------------------------------------------
CREATE TABLE deposit_orders (
    id                     UUID           PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id                UUID           NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    amount_vnd             BIGINT         NOT NULL CHECK (amount_vnd > 0),      -- Tổng VND (đã quy đổi nếu cần)
    diamond_amount         BIGINT         NOT NULL CHECK (diamond_amount >= 1), -- L6 fix: Số Diamond credit >= 1 (deposit 0 vô nghĩa)
    bonus_diamond          BIGINT         NOT NULL DEFAULT 0,                   -- Bonus từ promotion
    payment_method         VARCHAR(20)    NOT NULL,
    -- M9 fix: Refund tracking fields (hỗ trợ partial refund theo BR-4.3.2)
    refunded_diamond       BIGINT         NOT NULL DEFAULT 0 CHECK (refunded_diamond >= 0),  -- Diamond đã refund
    refunded_amount_vnd    BIGINT         NOT NULL DEFAULT 0 CHECK (refunded_amount_vnd >= 0),  -- VND đã refund
    refund_reason          TEXT,                                                -- Lý do refund (nếu có)
    -- M3 fix: Phân biệt partial/full refund + idempotency riêng cho refund
    refund_type            VARCHAR(20),                                         -- full | partial (NULL = chưa refund)
    refund_idempotency_key VARCHAR(100),                                        -- Idempotency key riêng cho refund operation
    provider_amount        DECIMAL(18,4),              -- Số tiền gốc từ provider (minor units)
    provider_currency      VARCHAR(3),                 -- VND, USD, ...
    fx_rate_snapshot       DECIMAL(18,8),              -- Tỷ giá tại thời điểm capture (1 unit provider = ? VND)
    captured_at            TIMESTAMPTZ,                -- Thời điểm capture (hold→capture)
    settled_at             TIMESTAMPTZ,                -- Thời điểm settle (nếu provider dùng 2 bước)
    gateway_order_id       VARCHAR(100),               -- ID đơn từ gateway (partial unique index bên dưới)
    gateway_transaction_id VARCHAR(100),               -- ID giao dịch từ gateway
    idempotency_key        VARCHAR(100),                -- Chống double-credit (partial unique index bên dưới)
    status                 VARCHAR(20) NOT NULL DEFAULT 'pending',
    promotion_id           UUID          REFERENCES deposit_promotions(id),
    paid_at                TIMESTAMPTZ,                -- Thời điểm hoàn tất thanh toán (legacy/simple flow)
    -- Thuế (compliance: pre_tax, tax, total)
    pre_tax_amount_vnd     BIGINT,                     -- Số tiền trước thuế (VND)
    tax_amount_vnd         BIGINT,                     -- Số thuế (VND)
    total_amount_vnd       BIGINT,                     -- Tổng = pre_tax + tax
    -- C6 fix: Chặn refund vượt tổng Diamond đã credit (diamond_amount + bonus_diamond)
    CONSTRAINT chk_refund_not_exceed CHECK (refunded_diamond <= diamond_amount + bonus_diamond),
    created_at             TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at             TIMESTAMPTZ
);
CREATE INDEX idx_deposit_orders_user ON deposit_orders(user_id, created_at DESC);
CREATE UNIQUE INDEX idx_deposit_orders_gateway_order ON deposit_orders(gateway_order_id) WHERE gateway_order_id IS NOT NULL;
CREATE UNIQUE INDEX idx_deposit_orders_gateway_txn ON deposit_orders(gateway_transaction_id) WHERE status = 'success';
CREATE UNIQUE INDEX idx_deposit_orders_idempotency ON deposit_orders(idempotency_key) WHERE idempotency_key IS NOT NULL;
-- N9 fix: Enforce uniqueness cho refund idempotency key (tránh double-refund)
CREATE UNIQUE INDEX idx_deposit_orders_refund_idempotency ON deposit_orders(refund_idempotency_key) WHERE refund_idempotency_key IS NOT NULL;
-- V1 fix: Index cho admin/reconciliation queries theo status
CREATE INDEX idx_deposit_orders_status ON deposit_orders(status, created_at DESC);

COMMENT ON TABLE deposit_orders IS 'Đơn nạp Diamond. Có fx_rate_snapshot khi provider không VND. Hỗ trợ hold→capture/settle.';
COMMENT ON COLUMN deposit_orders.id IS 'UUID primary key.';
COMMENT ON COLUMN deposit_orders.user_id IS 'FK users.';
COMMENT ON COLUMN deposit_orders.amount_vnd IS 'Tổng VND (đã quy đổi nếu cần).';
COMMENT ON COLUMN deposit_orders.diamond_amount IS 'Số Diamond credit.';
COMMENT ON COLUMN deposit_orders.bonus_diamond IS 'Bonus từ promotion.';
COMMENT ON COLUMN deposit_orders.payment_method IS 'bank_transfer | vietqr | paypal.';
COMMENT ON COLUMN deposit_orders.provider_amount IS 'Số tiền gốc từ provider (minor units).';
COMMENT ON COLUMN deposit_orders.provider_currency IS 'VND, USD, ...';
COMMENT ON COLUMN deposit_orders.fx_rate_snapshot IS 'Tỷ giá cố định tại capture để quy đổi Diamond. Dùng cho đối soát, không dùng tỷ giá realtime.';
COMMENT ON COLUMN deposit_orders.captured_at IS 'Thời điểm capture (hold→capture).';
COMMENT ON COLUMN deposit_orders.settled_at IS 'Thời điểm settle (nếu provider dùng 2 bước).';
COMMENT ON COLUMN deposit_orders.gateway_order_id IS 'ID đơn từ gateway.';
COMMENT ON COLUMN deposit_orders.gateway_transaction_id IS 'ID giao dịch từ gateway.';
COMMENT ON COLUMN deposit_orders.idempotency_key IS 'Chống double-credit.';
COMMENT ON COLUMN deposit_orders.status IS 'pending | success | failed | refunded | disputed.';
COMMENT ON COLUMN deposit_orders.promotion_id IS 'FK deposit_promotions.';
COMMENT ON COLUMN deposit_orders.paid_at IS 'Thời điểm hoàn tất (legacy/simple flow).';
COMMENT ON COLUMN deposit_orders.pre_tax_amount_vnd IS 'Số tiền trước thuế theo khu vực pháp lý. Dùng cho báo cáo thuế.';
COMMENT ON COLUMN deposit_orders.tax_amount_vnd IS 'Số thuế VND.';
COMMENT ON COLUMN deposit_orders.total_amount_vnd IS 'Tổng = pre_tax + tax.';
COMMENT ON COLUMN deposit_orders.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN deposit_orders.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 7) wallet_transactions (Ledger - double-entry)
-- Mục đích: Sổ cái giao dịch ví. Mọi biến động Gold/Diamond phải có dòng.
-- Invariant: balance_after = balance_before + amount (credit dương, debit âm).
-- -----------------------------------------------------------------------------
CREATE TABLE wallet_transactions (
    id              UUID             PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID             NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    currency        VARCHAR(20)      NOT NULL,         -- gold | diamond
    type            VARCHAR(50)      NOT NULL,         -- Loại giao dịch
    amount          BIGINT           NOT NULL,         -- Số tiền (+ credit, - debit)
    balance_before  BIGINT           NOT NULL CHECK (balance_before >= 0),
    balance_after   BIGINT           NOT NULL CHECK (balance_after >= 0),
    reference_source VARCHAR(50),                 -- postgres | mongo | system
    reference_id    TEXT,                             -- UUID hoặc ObjectId string
    description     TEXT,
    metadata_json   JSONB,
    idempotency_key VARCHAR(100),                      -- Chống double-write (partial unique index)
    created_at      TIMESTAMPTZ      NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_wallet_tx_user_date ON wallet_transactions(user_id, created_at DESC);
CREATE INDEX idx_wallet_tx_reference ON wallet_transactions(reference_source, reference_id) WHERE reference_id IS NOT NULL;
CREATE INDEX idx_wallet_tx_ref_id ON wallet_transactions(reference_id, reference_source) WHERE reference_id IS NOT NULL;  -- Dispute/escrow query
CREATE UNIQUE INDEX idx_wallet_tx_idempotency ON wallet_transactions(idempotency_key) WHERE idempotency_key IS NOT NULL;

-- Ràng buộc: balance_after = balance_before + amount; amount != 0 (tránh ledger no-op)
ALTER TABLE wallet_transactions ADD CONSTRAINT chk_wallet_balance_consistency
    CHECK (balance_after = balance_before + amount);
ALTER TABLE wallet_transactions ADD CONSTRAINT chk_wallet_amount_nonzero
    CHECK (amount != 0);

COMMENT ON TABLE wallet_transactions IS 'Sổ cái double-entry: mọi biến động ví phải có dòng. balance_after = balance_before + amount.';
COMMENT ON COLUMN wallet_transactions.id IS 'UUID primary key.';
COMMENT ON COLUMN wallet_transactions.user_id IS 'FK users.';
COMMENT ON COLUMN wallet_transactions.currency IS 'gold | diamond.';
COMMENT ON COLUMN wallet_transactions.type IS 'Loại giao dịch (transaction_type).';
COMMENT ON COLUMN wallet_transactions.amount IS 'Số tiền (+ credit, - debit).';
COMMENT ON COLUMN wallet_transactions.balance_before IS 'Số dư trước giao dịch.';
COMMENT ON COLUMN wallet_transactions.balance_after IS 'Phải = balance_before + amount.';
COMMENT ON COLUMN wallet_transactions.reference_source IS 'postgres | mongo | system.';
COMMENT ON COLUMN wallet_transactions.reference_id IS 'UUID hoặc ObjectId string.';
COMMENT ON COLUMN wallet_transactions.description IS 'Mô tả tùy chọn.';
COMMENT ON COLUMN wallet_transactions.metadata_json IS 'Dữ liệu bổ sung JSON.';
COMMENT ON COLUMN wallet_transactions.idempotency_key IS 'Chống double-write.';
COMMENT ON COLUMN wallet_transactions.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 8) chat_finance_sessions
-- Mục đích: Phiên tài chính 1-1, 1 conversation = 1 session. Gom escrow từng câu hỏi.
-- Tham chiếu: conversation_ref → conversations._id (MongoDB ObjectId string).
-- -----------------------------------------------------------------------------
CREATE TABLE chat_finance_sessions (
    id                  UUID                    PRIMARY KEY DEFAULT gen_random_uuid(),
    conversation_ref    TEXT                    NOT NULL UNIQUE,  -- conversations._id (MongoDB ObjectId 24 hex)
    user_id             UUID                    NOT NULL REFERENCES users(id) ON DELETE RESTRICT,   -- Payer (user)
    reader_id           UUID                    NOT NULL REFERENCES users(id) ON DELETE RESTRICT,   -- Receiver (reader)
    status              VARCHAR(50)        NOT NULL DEFAULT 'pending',
    total_frozen        BIGINT                  NOT NULL DEFAULT 0 CHECK (total_frozen >= 0),  -- Tổng Diamond đang freeze
    created_at          TIMESTAMPTZ             NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ
);
CREATE INDEX idx_chat_finance_user_reader ON chat_finance_sessions(user_id, reader_id, created_at DESC);
ALTER TABLE chat_finance_sessions ADD CONSTRAINT chk_cfs_conversation_ref_format
    CHECK (conversation_ref ~ '^[0-9a-f]{24}$');  -- MongoDB ObjectId hex

COMMENT ON TABLE chat_finance_sessions IS 'Phiên tài chính chat: 1 conversation = 1 session. conversation_ref → conversations._id (MongoDB).';
COMMENT ON COLUMN chat_finance_sessions.id IS 'UUID primary key.';
COMMENT ON COLUMN chat_finance_sessions.conversation_ref IS 'conversations._id (MongoDB) string.';
COMMENT ON COLUMN chat_finance_sessions.user_id IS 'Payer (user).';
COMMENT ON COLUMN chat_finance_sessions.reader_id IS 'Receiver (reader).';
COMMENT ON COLUMN chat_finance_sessions.status IS 'pending | active | completed | refunded | disputed | cancelled.';
COMMENT ON COLUMN chat_finance_sessions.total_frozen IS 'Tổng Diamond đang freeze.';
COMMENT ON COLUMN chat_finance_sessions.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN chat_finance_sessions.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 9) chat_question_items
-- Mục đích: Escrow từng câu hỏi (main + add-question). Mỗi item có timer riêng.
-- auto_refund_at = accepted_at + 24h (reader phải reply trước). reader_response_due_at = cùng mốc, giữ cho UI/audit.
-- auto_release_at = replied_at + 24h. dispute_window: 24h từ release/refund.
-- -----------------------------------------------------------------------------
CREATE TABLE chat_question_items (
    id                    UUID            PRIMARY KEY DEFAULT gen_random_uuid(),
    finance_session_id    UUID            NOT NULL REFERENCES chat_finance_sessions(id),
    conversation_ref      TEXT            NOT NULL,                -- conversations._id
    payer_id              UUID            NOT NULL REFERENCES users(id),
    receiver_id           UUID            NOT NULL REFERENCES users(id),
    type                  VARCHAR(50)       NOT NULL,             -- main_question | add_question
    amount_diamond        BIGINT          NOT NULL CHECK (amount_diamond > 0),
    status                VARCHAR(20)   NOT NULL DEFAULT 'pending',
    proposal_message_ref  TEXT,                                    -- chat_messages._id (proposal)
    offer_expires_at      TIMESTAMPTZ,                             -- Hết hạn nếu reader không accept
    accepted_at           TIMESTAMPTZ,                             -- accepted_at + 24h → auto_refund_at
    reader_response_due_at TIMESTAMPTZ,                            -- = auto_refund_at (cùng mốc 24h), dùng cho UI
    replied_at            TIMESTAMPTZ,                             -- replied_at + 24h → auto_release_at
    auto_release_at       TIMESTAMPTZ,                             -- Tự release nếu không confirm/dispute
    auto_refund_at        TIMESTAMPTZ,                             -- Tự refund nếu reader không reply
    released_at           TIMESTAMPTZ,
    -- M8 fix: Thời điểm user confirm release (NULL = auto-release hoặc chưa confirm)
    confirmed_at          TIMESTAMPTZ,
    refunded_at           TIMESTAMPTZ,
    dispute_window_start  TIMESTAMPTZ,                             -- Bắt đầu cửa sổ tranh chấp (24h)
    dispute_window_end    TIMESTAMPTZ,                             -- Kết thúc cửa sổ tranh chấp
    idempotency_key       VARCHAR(100),                            -- Chống double-freeze/release/refund (partial unique)
    created_at            TIMESTAMPTZ    NOT NULL DEFAULT NOW(),
    -- M6 fix: Thêm updated_at để audit state transitions trong escrow flow
    updated_at            TIMESTAMPTZ
);
CREATE UNIQUE INDEX idx_chat_question_idempotency ON chat_question_items(idempotency_key) WHERE idempotency_key IS NOT NULL;
ALTER TABLE chat_question_items ADD CONSTRAINT chk_cqi_conversation_ref_format
    CHECK (conversation_ref ~ '^[0-9a-f]{24}$');  -- MongoDB ObjectId hex
ALTER TABLE chat_question_items ADD CONSTRAINT chk_cqi_proposal_message_ref_format
    CHECK (proposal_message_ref IS NULL OR proposal_message_ref ~ '^[0-9a-f]{24}$');  -- chat_messages._id
CREATE INDEX idx_chat_question_finance ON chat_question_items(finance_session_id, created_at);
CREATE INDEX idx_chat_question_offers ON chat_question_items(status, offer_expires_at) WHERE status = 'pending' AND offer_expires_at IS NOT NULL;
-- C6 fix: dùng status = 'accepted' cho timer indexes (items đã accept chờ reply/release)
CREATE INDEX idx_chat_question_timers ON chat_question_items(status, auto_refund_at) WHERE status = 'accepted' AND auto_refund_at IS NOT NULL;
CREATE INDEX idx_chat_question_release ON chat_question_items(status, auto_release_at) WHERE status = 'accepted' AND auto_release_at IS NOT NULL;

COMMENT ON TABLE chat_question_items IS 'Escrow từng câu hỏi chat. Timer: offer_expires_at, reader_response_due_at, auto_release_at, auto_refund_at. dispute_window 24h.';
COMMENT ON COLUMN chat_question_items.id IS 'UUID primary key.';
COMMENT ON COLUMN chat_question_items.finance_session_id IS 'FK chat_finance_sessions.';
COMMENT ON COLUMN chat_question_items.conversation_ref IS 'conversations._id.';
COMMENT ON COLUMN chat_question_items.payer_id IS 'FK users (user trả tiền).';
COMMENT ON COLUMN chat_question_items.receiver_id IS 'FK users (reader nhận).';
COMMENT ON COLUMN chat_question_items.type IS 'main_question | add_question.';
COMMENT ON COLUMN chat_question_items.amount_diamond IS 'Số Diamond escrow.';
COMMENT ON COLUMN chat_question_items.status IS 'pending | accepted | released | refunded | disputed.';  -- C2 fix: thêm accepted
COMMENT ON COLUMN chat_question_items.proposal_message_ref IS 'chat_messages._id (proposal).';
COMMENT ON COLUMN chat_question_items.accepted_at IS 'reader_response_due_at = accepted_at + 24h.';
COMMENT ON COLUMN chat_question_items.reader_response_due_at IS 'Quá hạn → auto refund.';
COMMENT ON COLUMN chat_question_items.replied_at IS 'auto_release_at = replied_at + 24h.';
COMMENT ON COLUMN chat_question_items.auto_release_at IS 'Tự release nếu không confirm/dispute.';
COMMENT ON COLUMN chat_question_items.auto_refund_at IS 'Tự refund nếu reader không reply.';
COMMENT ON COLUMN chat_question_items.released_at IS 'Thời điểm release.';
COMMENT ON COLUMN chat_question_items.refunded_at IS 'Thời điểm refund.';
COMMENT ON COLUMN chat_question_items.dispute_window_start IS 'Bắt đầu cửa sổ tranh chấp 24h.';
COMMENT ON COLUMN chat_question_items.dispute_window_end IS 'Kết thúc cửa sổ tranh chấp.';
COMMENT ON COLUMN chat_question_items.idempotency_key IS 'Chống double-freeze/release/refund.';
COMMENT ON COLUMN chat_question_items.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 10) withdrawal_requests
-- Mục đích: Yêu cầu rút tiền Reader. Min 50 Diamond, max 1 request/user/ngày (business_date_utc).
-- Platform fee 10%. Cần KYC đạt trước khi tạo request.
-- -----------------------------------------------------------------------------
CREATE TABLE withdrawal_requests (
    id                  UUID              PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID              NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    business_date_utc   DATE              NOT NULL,    -- Ngày nghiệp vụ UTC (giới hạn 1/ngày)
    amount_diamond      BIGINT            NOT NULL CHECK (amount_diamond >= 50),
    amount_vnd          BIGINT            NOT NULL CHECK (amount_vnd > 0),      -- Gross VND = amount_diamond * 1000
    fee_vnd             BIGINT            NOT NULL CHECK (fee_vnd >= 0),        -- Phí 10% quy đổi ra VND
    net_amount_vnd      BIGINT            NOT NULL CHECK (net_amount_vnd = amount_vnd - fee_vnd), 
    bank_name           VARCHAR(100)      NOT NULL,
    bank_account_name   VARCHAR(200)      NOT NULL,
    bank_account_number VARCHAR(50)       NOT NULL,
    status              VARCHAR(50)       NOT NULL DEFAULT 'pending',
    admin_id            UUID             REFERENCES users(id),      -- Admin xử lý
    admin_note          TEXT,
    processed_at        TIMESTAMPTZ,
    created_at          TIMESTAMPTZ       NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ
);
CREATE INDEX idx_withdrawal_user ON withdrawal_requests(user_id, created_at DESC);
-- M7 fix: loại trừ cả 'paid' để user có thể tạo request mới sau khi request cũ đã paid
CREATE UNIQUE INDEX idx_withdrawal_one_per_day ON withdrawal_requests(user_id, business_date_utc) WHERE status NOT IN ('rejected', 'paid');

COMMENT ON TABLE withdrawal_requests IS 'Yêu cầu rút tiền Reader. Min 50 Diamond, 1 request/user/ngày (business_date_utc). Phí 10%.';
COMMENT ON COLUMN withdrawal_requests.id IS 'UUID primary key.';
COMMENT ON COLUMN withdrawal_requests.user_id IS 'FK users (reader).';
COMMENT ON COLUMN withdrawal_requests.business_date_utc IS 'Ngày nghiệp vụ UTC, giới hạn 1/ngày.';
COMMENT ON COLUMN withdrawal_requests.amount_diamond IS 'Min 50 Diamond.';
COMMENT ON COLUMN withdrawal_requests.amount_vnd IS 'Gross VND = amount_diamond * 1000 (trước trừ phí).';
COMMENT ON COLUMN withdrawal_requests.fee_vnd IS 'Phí 10% tính bằng VND để không bị mất precision.';
COMMENT ON COLUMN withdrawal_requests.net_amount_vnd IS 'Số tiền thực nhận = amount_vnd - fee_vnd.';
COMMENT ON COLUMN withdrawal_requests.bank_name IS 'Tên ngân hàng.';
COMMENT ON COLUMN withdrawal_requests.bank_account_name IS 'Tên chủ tài khoản.';
COMMENT ON COLUMN withdrawal_requests.bank_account_number IS 'Số tài khoản.';
COMMENT ON COLUMN withdrawal_requests.status IS 'pending | approved | rejected | paid.';
COMMENT ON COLUMN withdrawal_requests.admin_id IS 'FK users (admin xử lý).';
COMMENT ON COLUMN withdrawal_requests.admin_note IS 'Ghi chú admin.';
COMMENT ON COLUMN withdrawal_requests.processed_at IS 'Thời điểm xử lý.';
COMMENT ON COLUMN withdrawal_requests.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN withdrawal_requests.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 11) reader_payout_profiles
-- Mục đích: Thông tin ngân hàng Reader, KYC. Cần đạt trước khi tạo withdrawal.
-- -----------------------------------------------------------------------------
CREATE TABLE reader_payout_profiles (
    user_id              UUID        PRIMARY KEY REFERENCES users(id),
    bank_name            VARCHAR(100),
    bank_account_name    VARCHAR(200),
    bank_account_number  VARCHAR(50),
    is_verified          BOOLEAN     NOT NULL DEFAULT false,   -- Legacy: đã xác minh
    verified_at          TIMESTAMPTZ,
    -- KYC chi tiết (Phase 2.1+)
    -- M6 fix: thêm CHECK constraint thay vì VARCHAR tự do
    kyc_status           VARCHAR(30) DEFAULT 'pending' CHECK (kyc_status IN ('pending', 'verified', 'rejected', 'enhanced_required')),
    kyc_verified_at      TIMESTAMPTZ,
    kyc_document_type    VARCHAR(50),                          -- id_card | passport | ...
    created_at           TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ
);

COMMENT ON TABLE reader_payout_profiles IS 'Thông tin ngân hàng + KYC Reader. KYC đạt mới tạo withdrawal.';
COMMENT ON COLUMN reader_payout_profiles.user_id IS 'FK users, primary key.';
COMMENT ON COLUMN reader_payout_profiles.bank_name IS 'Tên ngân hàng.';
COMMENT ON COLUMN reader_payout_profiles.bank_account_name IS 'Tên chủ tài khoản.';
COMMENT ON COLUMN reader_payout_profiles.bank_account_number IS 'Số tài khoản.';
COMMENT ON COLUMN reader_payout_profiles.is_verified IS 'Legacy: đã xác minh.';
COMMENT ON COLUMN reader_payout_profiles.verified_at IS 'Thời điểm xác minh.';
COMMENT ON COLUMN reader_payout_profiles.kyc_status IS 'pending | verified | rejected | enhanced_required.';
COMMENT ON COLUMN reader_payout_profiles.kyc_verified_at IS 'Thời điểm KYC verified.';
COMMENT ON COLUMN reader_payout_profiles.kyc_document_type IS 'id_card | passport | ...';
COMMENT ON COLUMN reader_payout_profiles.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN reader_payout_profiles.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 12) subscription_plans
-- Mục đích: Định nghĩa gói thuê bao (monthly/yearly), entitlement keys.
-- -----------------------------------------------------------------------------
CREATE TABLE subscription_plans (
    id                  UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    code                VARCHAR(50) UNIQUE NOT NULL,
    name                VARCHAR(100) NOT NULL,
    type                VARCHAR(20)   NOT NULL,                   -- monthly | yearly
    price_vnd           BIGINT      NOT NULL CHECK (price_vnd > 0),
    daily_free_draws    SMALLINT    NOT NULL DEFAULT 0,         -- Legacy
    equivalent_diamond  BIGINT      NOT NULL DEFAULT 0,
    entitlement_keys    JSONB,                                 -- {"free_spread_3_daily": 1, ...}
    is_active           BOOLEAN     NOT NULL DEFAULT true,
    is_deleted          BOOLEAN     NOT NULL DEFAULT false,
    deleted_at          TIMESTAMPTZ,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ
);

COMMENT ON TABLE subscription_plans IS 'Định nghĩa gói thuê bao. entitlement_keys: key→quantity (vd: free_spread_3_daily: 1).';
COMMENT ON COLUMN subscription_plans.id IS 'UUID primary key.';
COMMENT ON COLUMN subscription_plans.code IS 'Mã unique (vd: premium_monthly).';
COMMENT ON COLUMN subscription_plans.name IS 'Tên hiển thị.';
COMMENT ON COLUMN subscription_plans.type IS 'monthly | yearly.';
COMMENT ON COLUMN subscription_plans.price_vnd IS 'Giá VND.';
COMMENT ON COLUMN subscription_plans.daily_free_draws IS 'Legacy.';
COMMENT ON COLUMN subscription_plans.equivalent_diamond IS 'Diamond tương đương.';
COMMENT ON COLUMN subscription_plans.entitlement_keys IS 'JSON {"free_spread_3_daily": 1, ...}.';
COMMENT ON COLUMN subscription_plans.is_active IS 'Còn bán.';
COMMENT ON COLUMN subscription_plans.is_deleted IS 'Soft delete.';
COMMENT ON COLUMN subscription_plans.deleted_at IS 'Thời điểm soft delete.';
COMMENT ON COLUMN subscription_plans.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN subscription_plans.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 13) user_subscriptions
-- Mục đích: Đăng ký gói. 1 user có thể nhiều gói active cùng lúc.
-- -----------------------------------------------------------------------------
CREATE TABLE user_subscriptions (
    id               UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id          UUID        NOT NULL REFERENCES users(id),
    plan_id          UUID        NOT NULL REFERENCES subscription_plans(id),
    status           VARCHAR(20) NOT NULL DEFAULT 'active' CHECK (status IN ('active','expired','cancelled')),
    started_at       TIMESTAMPTZ NOT NULL,
    expires_at       TIMESTAMPTZ NOT NULL,   -- Hết hạn độc lập
    auto_renew       BOOLEAN     NOT NULL DEFAULT false,
    deposit_order_id UUID       REFERENCES deposit_orders(id),
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMPTZ
);
CREATE INDEX idx_user_subs_user ON user_subscriptions(user_id, expires_at DESC);
CREATE INDEX idx_user_subs_active ON user_subscriptions(user_id, status) WHERE status = 'active';

COMMENT ON TABLE user_subscriptions IS 'Đăng ký thuê bao. Nhiều gói active cùng lúc. Tiêu thụ entitlement: earliest-expiry-first.';
COMMENT ON COLUMN user_subscriptions.id IS 'UUID primary key.';
COMMENT ON COLUMN user_subscriptions.user_id IS 'FK users.';
COMMENT ON COLUMN user_subscriptions.plan_id IS 'FK subscription_plans.';
COMMENT ON COLUMN user_subscriptions.status IS 'active | expired | cancelled.';
COMMENT ON COLUMN user_subscriptions.started_at IS 'Ngày bắt đầu.';
COMMENT ON COLUMN user_subscriptions.expires_at IS 'Ngày hết hạn độc lập.';
COMMENT ON COLUMN user_subscriptions.auto_renew IS 'Tự gia hạn.';
COMMENT ON COLUMN user_subscriptions.deposit_order_id IS 'FK deposit_orders (đơn mua gói).';
COMMENT ON COLUMN user_subscriptions.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN user_subscriptions.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 14) subscription_entitlement_buckets
-- Mục đích: Quota entitlement theo ngày nghiệp vụ. Reset theo business_date UTC.
-- Tiêu thụ: earliest-expiry-first; tie-break subscription_id.
-- -----------------------------------------------------------------------------
CREATE TABLE subscription_entitlement_buckets (
    id               UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id          UUID        NOT NULL REFERENCES users(id),
    subscription_id  UUID        NOT NULL REFERENCES user_subscriptions(id),
    entitlement_key  VARCHAR(80) NOT NULL,   -- vd: free_spread_3_daily
    quantity         INT         NOT NULL CHECK (quantity >= 0),
    business_date    DATE        NOT NULL,   -- Ngày nghiệp vụ UTC
    expires_at       TIMESTAMPTZ NOT NULL,   -- Để áp earliest-expiry-first
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMPTZ
);
CREATE UNIQUE INDEX idx_entitlement_bucket ON subscription_entitlement_buckets(user_id, subscription_id, entitlement_key, business_date);
CREATE INDEX idx_entitlement_user_date ON subscription_entitlement_buckets(user_id, business_date, expires_at);

COMMENT ON TABLE subscription_entitlement_buckets IS 'Quota entitlement theo ngày. Tiêu thụ: earliest-expiry-first. Reset theo business_date UTC.';
COMMENT ON COLUMN subscription_entitlement_buckets.id IS 'UUID primary key.';
COMMENT ON COLUMN subscription_entitlement_buckets.user_id IS 'FK users.';
COMMENT ON COLUMN subscription_entitlement_buckets.subscription_id IS 'FK user_subscriptions.';
COMMENT ON COLUMN subscription_entitlement_buckets.entitlement_key IS 'vd: free_spread_3_daily.';
COMMENT ON COLUMN subscription_entitlement_buckets.quantity IS 'Quota còn lại.';
COMMENT ON COLUMN subscription_entitlement_buckets.business_date IS 'Ngày nghiệp vụ UTC.';
COMMENT ON COLUMN subscription_entitlement_buckets.expires_at IS 'Để áp earliest-expiry-first.';
COMMENT ON COLUMN subscription_entitlement_buckets.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN subscription_entitlement_buckets.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 15) entitlement_consumes
-- Mục đích: Nhật ký tiêu thụ entitlement. Chống double-count, hỗ trợ dispute.
-- business_date để đối soát theo ngày.
-- -----------------------------------------------------------------------------
CREATE TABLE entitlement_consumes (
    id                  UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id             UUID        NOT NULL REFERENCES users(id),
    entitlement_key     VARCHAR(80) NOT NULL,
    subscription_id     UUID        REFERENCES user_subscriptions(id),
    -- C4 fix: thêm mapping_rule_id theo BR-4.3.4 (ghi usage nhật ký có mapping_rule_id)
    -- C1 fix: FK được thêm qua ALTER TABLE sau khi entitlement_mapping_rules được tạo (xem cuối bảng #23)
    mapping_rule_id     UUID,       -- FK → entitlement_mapping_rules(id), thêm bằng ALTER TABLE bên dưới
    quantity_consumed   INT         NOT NULL CHECK (quantity_consumed > 0),
    business_date       DATE        NOT NULL,    -- Ngày nghiệp vụ (đối soát)
    reference_source    VARCHAR(50),
    reference_id        TEXT,
    idempotency_key     VARCHAR(100),                -- Partial unique index bên dưới
    created_at          TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_entitlement_consume_user ON entitlement_consumes(user_id, created_at DESC);
CREATE INDEX idx_entitlement_consume_user_date ON entitlement_consumes(user_id, business_date);
CREATE UNIQUE INDEX idx_entitlement_consume_idempotency ON entitlement_consumes(idempotency_key) WHERE idempotency_key IS NOT NULL;

COMMENT ON TABLE entitlement_consumes IS 'Nhật ký tiêu thụ entitlement. business_date để đối soát, chống double-count.';
COMMENT ON COLUMN entitlement_consumes.id IS 'UUID primary key.';
COMMENT ON COLUMN entitlement_consumes.user_id IS 'FK users.';
COMMENT ON COLUMN entitlement_consumes.entitlement_key IS 'Key đã tiêu thụ.';
COMMENT ON COLUMN entitlement_consumes.subscription_id IS 'FK user_subscriptions.';
COMMENT ON COLUMN entitlement_consumes.quantity_consumed IS 'Số lượng đã tiêu.';
COMMENT ON COLUMN entitlement_consumes.business_date IS 'Ngày nghiệp vụ (đối soát).';
COMMENT ON COLUMN entitlement_consumes.reference_source IS 'postgres | mongo | system.';
COMMENT ON COLUMN entitlement_consumes.reference_id IS 'ID tham chiếu.';
COMMENT ON COLUMN entitlement_consumes.idempotency_key IS 'Chống double-count.';
COMMENT ON COLUMN entitlement_consumes.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 16) ai_requests
-- Mục đích: Trạng thái AI streaming (refund, idempotency). Tham chiếu reading_sessions._id.
-- State: requested → first_token_received → completed | failed_before_first_token | failed_after_first_token
-- -----------------------------------------------------------------------------
CREATE TABLE ai_requests (
    id                   UUID              PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id              UUID              NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    reading_session_ref  TEXT              NOT NULL,   -- reading_sessions._id (MongoDB ObjectId 24 hex)
    -- C2 fix: chi phí follow-up phụ thuộc highest_card_level + sequence, KHÔNG phải map cứng sequence→giá
    followup_sequence    SMALLINT          CHECK (followup_sequence IS NULL OR (followup_sequence >= 1 AND followup_sequence <= 5)),  -- NULL=initial reading, 1-5=follow-up #1 đến #5. Chi phí tính dynamic theo card level (xem UX-4.4.5)
    status               VARCHAR(50)       NOT NULL DEFAULT 'requested',
    first_token_at       TIMESTAMPTZ,                  -- Nhận token đầu
    completion_marker_at TIMESTAMPTZ,                  -- Hoàn tất
    finish_reason        VARCHAR(50),                  -- Lý do fail (nếu có)
    retry_count          SMALLINT          NOT NULL DEFAULT 0,
    prompt_version       VARCHAR(20),
    policy_version       VARCHAR(20),
    correlation_id       UUID,
    trace_id             VARCHAR(64),
    charge_gold          BIGINT            NOT NULL DEFAULT 0,
    charge_diamond       BIGINT            NOT NULL DEFAULT 0,
    -- Locale control fields (ARCH-4.4.6 / UX-4.4.6)
    requested_locale     VARCHAR(10),                  -- Locale UI gửi (vi/en/zh_hans)
    returned_locale      VARCHAR(10),                  -- Locale thực tế model trả về
    fallback_reason      TEXT,                         -- Lý do fallback (nếu locale khác requested)
    idempotency_key      VARCHAR(100),               -- Refund idempotent (partial unique)
    created_at           TIMESTAMPTZ       NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ
);
CREATE UNIQUE INDEX idx_ai_requests_idempotency ON ai_requests(idempotency_key) WHERE idempotency_key IS NOT NULL;
ALTER TABLE ai_requests ADD CONSTRAINT chk_air_reading_session_ref_format
    CHECK (reading_session_ref ~ '^[0-9a-f]{24}$');  -- MongoDB ObjectId hex
CREATE INDEX idx_ai_requests_reading ON ai_requests(reading_session_ref);
CREATE INDEX idx_ai_requests_status ON ai_requests(status, created_at);

COMMENT ON TABLE ai_requests IS 'Trạng thái AI streaming. Refund idempotent theo id. reading_session_ref → reading_sessions._id.';
COMMENT ON COLUMN ai_requests.id IS 'UUID primary key.';
COMMENT ON COLUMN ai_requests.user_id IS 'FK users.';
COMMENT ON COLUMN ai_requests.reading_session_ref IS 'reading_sessions._id (MongoDB).';
COMMENT ON COLUMN ai_requests.followup_sequence IS 'NULL=initial reading, 1-5=follow-up #1 đến #5; chi phí tính dynamic theo highest card level (UX-4.4.5), KHÔNG map cứng sequence→giá.';  -- H8 fix
COMMENT ON COLUMN ai_requests.status IS 'requested | first_token_received | completed | failed_before/after_first_token.';
COMMENT ON COLUMN ai_requests.first_token_at IS 'Thời điểm nhận token đầu.';
COMMENT ON COLUMN ai_requests.completion_marker_at IS 'Thời điểm hoàn tất.';
COMMENT ON COLUMN ai_requests.finish_reason IS 'Lý do fail (nếu có).';
COMMENT ON COLUMN ai_requests.retry_count IS 'Số lần retry.';
COMMENT ON COLUMN ai_requests.prompt_version IS 'Phiên bản prompt.';
COMMENT ON COLUMN ai_requests.policy_version IS 'Phiên bản policy.';
COMMENT ON COLUMN ai_requests.correlation_id IS 'Correlation ID.';
COMMENT ON COLUMN ai_requests.trace_id IS 'Trace ID.';
COMMENT ON COLUMN ai_requests.charge_gold IS 'Số Gold đã charge.';
COMMENT ON COLUMN ai_requests.charge_diamond IS 'Số Diamond đã charge.';
COMMENT ON COLUMN ai_requests.idempotency_key IS 'Refund idempotent theo ai_request_id.';
COMMENT ON COLUMN ai_requests.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN ai_requests.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 17) reading_rng_audits
-- Mục đích: Audit RNG để replay tranh chấp. Giữ secret cũ ≥ 24 tháng.
-- -----------------------------------------------------------------------------
CREATE TABLE reading_rng_audits (
    id                 UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    reading_session_ref TEXT       NOT NULL,   -- reading_sessions._id (MongoDB ObjectId 24 hex)
    user_id            UUID        NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    algorithm_version  VARCHAR(20) NOT NULL,
    secret_version     VARCHAR(20) NOT NULL,   -- Không lưu raw secret
    session_nonce      VARCHAR(64) NOT NULL,
    seed_digest        VARCHAR(128) NOT NULL,
    deck_order_hash    VARCHAR(128) NOT NULL,
    -- m7 fix: thêm CHECK constraint cho draw_type
    draw_type          VARCHAR(30) NOT NULL CHECK (draw_type IN ('daily_1', 'spread_3', 'spread_5', 'spread_10')),   -- daily_1 | spread_3 | spread_5 | spread_10
    timestamp_utc_ms   BIGINT,                 -- MS dùng trong HMAC (replay chính xác, tránh lệch created_at)
    created_at         TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
ALTER TABLE reading_rng_audits ADD CONSTRAINT chk_rra_reading_session_ref_format
    CHECK (reading_session_ref ~ '^[0-9a-f]{24}$');  -- MongoDB ObjectId hex
CREATE INDEX idx_rng_audit_reading ON reading_rng_audits(reading_session_ref);
CREATE INDEX idx_rng_audit_user ON reading_rng_audits(user_id, created_at DESC);

COMMENT ON TABLE reading_rng_audits IS 'Audit RNG để replay tranh chấp. Không lưu raw secret. Retention ≥ 24 tháng.';
COMMENT ON COLUMN reading_rng_audits.id IS 'UUID primary key.';
COMMENT ON COLUMN reading_rng_audits.reading_session_ref IS 'reading_sessions._id (MongoDB).';
COMMENT ON COLUMN reading_rng_audits.user_id IS 'FK users.';
COMMENT ON COLUMN reading_rng_audits.algorithm_version IS 'Phiên bản thuật toán.';
COMMENT ON COLUMN reading_rng_audits.secret_version IS 'Phiên bản secret (không lưu raw).';
COMMENT ON COLUMN reading_rng_audits.session_nonce IS 'Nonce phiên.';
COMMENT ON COLUMN reading_rng_audits.seed_digest IS 'Digest seed.';
COMMENT ON COLUMN reading_rng_audits.deck_order_hash IS 'Hash thứ tự bài.';
COMMENT ON COLUMN reading_rng_audits.draw_type IS 'daily_1 | spread_3 | spread_5 | spread_10.';
COMMENT ON COLUMN reading_rng_audits.timestamp_utc_ms IS 'Unix ms dùng trong HMAC seed_digest; replay khớp với giá trị gốc.';
COMMENT ON COLUMN reading_rng_audits.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 18) gacha_odds_versions
-- Mục đích: Phiên bản tỷ lệ Gacha (public contract). Có effective_from/to.
-- -----------------------------------------------------------------------------
CREATE TABLE gacha_odds_versions (
    id              UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    odds_version    VARCHAR(30) NOT NULL UNIQUE,
    rarity_pool     JSONB       NOT NULL,     -- Định nghĩa pool
    probabilities   JSONB       NOT NULL,     -- Xác suất
    effective_from  TIMESTAMPTZ NOT NULL,
    effective_to    TIMESTAMPTZ,              -- NULL = vô hạn
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

COMMENT ON TABLE gacha_odds_versions IS 'Phiên bản tỷ lệ Gacha. Public contract, có effective_from/to.';
COMMENT ON COLUMN gacha_odds_versions.id IS 'UUID primary key.';
COMMENT ON COLUMN gacha_odds_versions.odds_version IS 'Phiên bản unique.';
COMMENT ON COLUMN gacha_odds_versions.rarity_pool IS 'JSON định nghĩa pool.';
COMMENT ON COLUMN gacha_odds_versions.probabilities IS 'JSON xác suất.';
COMMENT ON COLUMN gacha_odds_versions.effective_from IS 'Bắt đầu hiệu lực.';
COMMENT ON COLUMN gacha_odds_versions.effective_to IS 'Kết thúc hiệu lực (NULL = vô hạn).';
COMMENT ON COLUMN gacha_odds_versions.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 19) gacha_reward_logs
-- Mục đích: Log phần thưởng Gacha. Mapping odds_version, rng_audit_ref.
-- -----------------------------------------------------------------------------
CREATE TABLE gacha_reward_logs (
    id              UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID        NOT NULL REFERENCES users(id),
    odds_version    VARCHAR(30) NOT NULL,
    rng_audit_ref   TEXT,                     -- Tham chiếu audit RNG
    spent_diamond   BIGINT      NOT NULL,
    results_json    JSONB       NOT NULL,
    idempotency_key VARCHAR(100),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE UNIQUE INDEX idx_gacha_reward_id_key ON gacha_reward_logs(idempotency_key) WHERE idempotency_key IS NOT NULL;
CREATE INDEX idx_gacha_log_user ON gacha_reward_logs(user_id, created_at DESC);
-- V1 fix: FK referential integrity cho odds_version
ALTER TABLE gacha_reward_logs ADD CONSTRAINT fk_gacha_odds_version
    FOREIGN KEY (odds_version) REFERENCES gacha_odds_versions(odds_version);

COMMENT ON TABLE gacha_reward_logs IS 'Log thưởng Gacha. Có mapping odds_version → rng_audit_ref.';
COMMENT ON COLUMN gacha_reward_logs.id IS 'UUID primary key.';
COMMENT ON COLUMN gacha_reward_logs.user_id IS 'FK users.';
COMMENT ON COLUMN gacha_reward_logs.odds_version IS 'Phiên bản tỷ lệ dùng.';
COMMENT ON COLUMN gacha_reward_logs.rng_audit_ref IS 'Tham chiếu audit RNG.';
COMMENT ON COLUMN gacha_reward_logs.spent_diamond IS 'Số Diamond đã dùng.';
COMMENT ON COLUMN gacha_reward_logs.results_json IS 'JSON kết quả.';
COMMENT ON COLUMN gacha_reward_logs.idempotency_key IS 'Chống double-reward.';
COMMENT ON COLUMN gacha_reward_logs.created_at IS 'Thời điểm tạo.';

-- -----------------------------------------------------------------------------
-- 20) user_exp_levels, card_exp_levels
-- Mục đích: Bảng quy đổi EXP → level (user, card).
-- -----------------------------------------------------------------------------
CREATE TABLE user_exp_levels (
    level   SMALLINT PRIMARY KEY,
    min_exp INT      NOT NULL
);
CREATE TABLE card_exp_levels (
    level   SMALLINT PRIMARY KEY,
    min_exp INT      NOT NULL
);

COMMENT ON TABLE user_exp_levels IS 'Quy đổi user EXP → level.';
COMMENT ON COLUMN user_exp_levels.level IS 'Cấp (primary key).';
COMMENT ON COLUMN user_exp_levels.min_exp IS 'EXP tối thiểu để đạt cấp.';

COMMENT ON TABLE card_exp_levels IS 'Quy đổi card EXP → level (1-20+).';
COMMENT ON COLUMN card_exp_levels.level IS 'Cấp (primary key).';
COMMENT ON COLUMN card_exp_levels.min_exp IS 'EXP tối thiểu để đạt cấp.';

-- -----------------------------------------------------------------------------
-- 21) user_geo_signals
-- Mục đích: Tín hiệu địa lý cho geo compliance gating. Multi-signal.
-- -----------------------------------------------------------------------------
CREATE TABLE user_geo_signals (
    user_id                 UUID        PRIMARY KEY REFERENCES users(id),
    account_jurisdiction    VARCHAR(5),             -- Mã quốc gia (ưu tiên cao nếu KYC)
    payment_country         VARCHAR(5),
    kyc_country             VARCHAR(5),
    ip_geo_consistency_score DECIMAL(3,2),         -- 0-1
    vpn_proxy_risk          BOOLEAN     DEFAULT false,
    restricted_review       BOOLEAN     DEFAULT false,  -- Tín hiệu mâu thuẫn / VPN
    restricted_review_at    TIMESTAMPTZ,                  -- M2 fix: Thời điểm chuyển restricted_review=true (để SLA 24h)
    updated_at              TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_geo_restricted ON user_geo_signals(restricted_review) WHERE restricted_review = true;  -- Manual review queue

COMMENT ON TABLE user_geo_signals IS 'Tín hiệu địa lý: account_jurisdiction, payment_country, IP. Dùng cho geo gating RNG/payout.';
COMMENT ON COLUMN user_geo_signals.user_id IS 'FK users, primary key.';
COMMENT ON COLUMN user_geo_signals.account_jurisdiction IS 'Mã quốc gia (ưu tiên cao nếu KYC).';
COMMENT ON COLUMN user_geo_signals.payment_country IS 'Quốc gia thanh toán.';
COMMENT ON COLUMN user_geo_signals.kyc_country IS 'Quốc gia KYC.';
COMMENT ON COLUMN user_geo_signals.ip_geo_consistency_score IS 'Điểm nhất quán IP 0-1.';
COMMENT ON COLUMN user_geo_signals.vpn_proxy_risk IS 'Phát hiện VPN/proxy.';
COMMENT ON COLUMN user_geo_signals.restricted_review IS 'Tín hiệu mâu thuẫn, cần rà soát.';
COMMENT ON COLUMN user_geo_signals.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 22) system_configs
-- Mục đích: Cấu hình runtime (key-value).
-- -----------------------------------------------------------------------------
CREATE TABLE system_configs (
    key         VARCHAR(100) PRIMARY KEY,
    value       TEXT         NOT NULL,
    description TEXT,
    updated_by  UUID        REFERENCES users(id),
    updated_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

COMMENT ON TABLE system_configs IS 'Cấu hình runtime: key-value.';
COMMENT ON COLUMN system_configs.key IS 'Key cấu hình (primary key).';
COMMENT ON COLUMN system_configs.value IS 'Giá trị (text).';
COMMENT ON COLUMN system_configs.description IS 'Mô tả cấu hình.';
COMMENT ON COLUMN system_configs.updated_by IS 'FK users (người cập nhật).';
COMMENT ON COLUMN system_configs.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 23) entitlement_mapping_rules (BR-4.3.4)
-- Mục đích: Ánh xạ quyền lợi (vd: free_spread_5_daily → free_spread_3_daily khi bật).
-- -----------------------------------------------------------------------------
CREATE TABLE entitlement_mapping_rules (
    id               UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    source_key       VARCHAR(80) NOT NULL,   -- Key nguồn (vd: free_spread_5_daily)
    target_key       VARCHAR(80) NOT NULL,   -- Key đích (vd: free_spread_3_daily)
    conversion_ratio NUMERIC(10,4) NOT NULL DEFAULT 1,  -- Tỷ lệ 1:1 hoặc tùy chỉnh
    is_enabled       BOOLEAN     NOT NULL DEFAULT false,
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMPTZ
);
CREATE UNIQUE INDEX idx_entitlement_mapping_source_target ON entitlement_mapping_rules(source_key, target_key);

-- C1 fix: Thêm FK cho entitlement_consumes.mapping_rule_id SAU KHI entitlement_mapping_rules đã tạo
ALTER TABLE entitlement_consumes ADD CONSTRAINT fk_entitlement_consumes_mapping_rule
    FOREIGN KEY (mapping_rule_id) REFERENCES entitlement_mapping_rules(id);

COMMENT ON TABLE entitlement_mapping_rules IS 'Ánh xạ entitlement (vd: 5 lá dùng cho 3 lá khi ON). BR-4.3.4.';
COMMENT ON COLUMN entitlement_mapping_rules.source_key IS 'Key nguồn (vd: free_spread_5_daily).';
COMMENT ON COLUMN entitlement_mapping_rules.target_key IS 'Key đích (vd: free_spread_3_daily).';
COMMENT ON COLUMN entitlement_mapping_rules.conversion_ratio IS 'Tỷ lệ chuyển đổi (mặc định 1).';
COMMENT ON COLUMN entitlement_mapping_rules.is_enabled IS 'Bật/tắt rule.';
COMMENT ON COLUMN entitlement_mapping_rules.created_at IS 'Thời điểm tạo.';
COMMENT ON COLUMN entitlement_mapping_rules.updated_at IS 'Thời điểm cập nhật.';

-- -----------------------------------------------------------------------------
-- 24) data_rights_requests (OPS-4.13.7 – GDPR/CCPA-like)
-- Yêu cầu truy cập/xuất, chỉnh sửa, xóa dữ liệu. Mỗi request có SLA và audit trail.
-- -----------------------------------------------------------------------------
CREATE TABLE data_rights_requests (
    id              UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID        NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    request_type    VARCHAR(30) NOT NULL,   -- access_export | correction | deletion
    status          VARCHAR(20) NOT NULL DEFAULT 'pending',  -- pending | processing | completed | rejected
    requested_at    TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    sla_deadline_at TIMESTAMPTZ,            -- SLA xử lý nội bộ
    completed_at    TIMESTAMPTZ,
    completed_by    UUID        REFERENCES users(id),
    result_summary  TEXT,                   -- Tóm tắt kết quả (đã xóa, đã xuất, ...)
    rejection_reason TEXT,                  -- Lý do từ chối (nếu có)
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ
);
CREATE INDEX idx_data_rights_user ON data_rights_requests(user_id, requested_at DESC);
CREATE INDEX idx_data_rights_status ON data_rights_requests(status, requested_at);

COMMENT ON TABLE data_rights_requests IS 'Yêu cầu quyền dữ liệu: access/export, correction, deletion. OPS-4.13.7.';
COMMENT ON COLUMN data_rights_requests.request_type IS 'access_export | correction | deletion.';
COMMENT ON COLUMN data_rights_requests.status IS 'pending | processing | completed | rejected.';
COMMENT ON COLUMN data_rights_requests.sla_deadline_at IS 'Hạn SLA xử lý nội bộ.';

-- -----------------------------------------------------------------------------
-- 25) admin_actions (audit hành động admin – PostgreSQL)
-- Chi tiết log trong MongoDB admin_logs; bảng này cho query/alert nhanh.
-- -----------------------------------------------------------------------------
CREATE TABLE admin_actions (
    id          UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    admin_id    UUID        NOT NULL REFERENCES users(id) ON DELETE RESTRICT,
    action      VARCHAR(100) NOT NULL,   -- BAN_USER, APPROVE_READER, DISPUTE_RESOLVE, ...
    target_type VARCHAR(50),             -- user | reader_request | withdrawal | ...
    target_id   TEXT,                    -- UUID hoặc ObjectId string
    created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_admin_actions_admin ON admin_actions(admin_id, created_at DESC);
CREATE INDEX idx_admin_actions_action ON admin_actions(action, created_at DESC);

COMMENT ON TABLE admin_actions IS 'Audit hành động admin. Chi tiết trong MongoDB admin_logs.';
COMMENT ON COLUMN admin_actions.admin_id IS 'FK users (admin thực hiện).';
COMMENT ON COLUMN admin_actions.action IS 'Mã hành động (BAN_USER, APPROVE_READER, ...).';
COMMENT ON COLUMN admin_actions.target_type IS 'Loại đối tượng (user, reader_request, withdrawal, ...).';
COMMENT ON COLUMN admin_actions.target_id IS 'ID đối tượng (UUID hoặc ObjectId).';
COMMENT ON COLUMN admin_actions.created_at IS 'Thời điểm thực hiện.';

-- =============================================================================
-- VIEW: Reconciliation – so sánh users balance với ledger
-- Chạy reconciliation job: SELECT * FROM v_user_ledger_balance v
-- JOIN users u ON u.id = v.user_id
-- WHERE (v.currency = 'gold' AND u.gold_balance != v.ledger_balance)
--    OR (v.currency = 'diamond' AND u.diamond_balance != v.ledger_balance);
-- =============================================================================
CREATE OR REPLACE VIEW v_user_ledger_balance AS
SELECT user_id, currency,
       balance_after AS ledger_balance,
       created_at AS last_tx_at,
       id AS last_tx_id
FROM (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY user_id, currency ORDER BY created_at DESC) AS rn
    FROM wallet_transactions
) t
WHERE rn = 1;

COMMENT ON VIEW v_user_ledger_balance IS 'Balance cuối từ ledger (wallet_transactions). Dùng reconciliation job so với users.gold_balance/diamond_balance. Khi DAU cao có thể chuyển sang MATERIALIZED VIEW + REFRESH.';

-- =============================================================================
-- VIEW: Reconciliation frozen balance – so sánh users.frozen_diamond_balance với ledger
-- Logic: frozen balance = tổng đã freeze (payer rows, type=escrow_freeze) 
--        - tổng đã release (receiver rows, type=escrow_release, tracked by reference_id match)
--        - tổng đã refund (payer rows, type=escrow_refund)
-- Vì release không ghi payer row (diamond_balance payer không đổi, chỉ frozen giảm),
-- ta dùng chat_question_items.payer_id + status = 'released' để tính tổng released của payer.
-- Đơn giản hơn: SUM(freeze) - SUM(refund) - SUM(released question items)
-- Chạy: SELECT * FROM v_user_frozen_ledger_balance v
-- JOIN users u ON u.id = v.user_id
-- WHERE u.frozen_diamond_balance != v.ledger_frozen_balance;
-- =============================================================================
CREATE OR REPLACE VIEW v_user_frozen_ledger_balance AS
SELECT 
    u.user_id,
    COALESCE(u.total_frozen, 0) - COALESCE(r.total_refunded, 0) - COALESCE(rel.total_released, 0) AS ledger_frozen_balance
FROM (
    -- Tổng freeze của mỗi user (từ wallet_transactions)
    SELECT user_id, SUM(ABS(amount)) AS total_frozen
    FROM wallet_transactions
    WHERE currency = 'diamond' AND type = 'escrow_freeze'
    GROUP BY user_id
) u
LEFT JOIN (
    -- Tổng refund của mỗi user
    SELECT user_id, SUM(amount) AS total_refunded
    FROM wallet_transactions
    WHERE currency = 'diamond' AND type = 'escrow_refund'
    GROUP BY user_id
) r ON u.user_id = r.user_id
LEFT JOIN (
    -- Tổng released của mỗi payer (từ chat_question_items)
    SELECT payer_id AS user_id, SUM(amount_diamond) AS total_released
    FROM chat_question_items
    WHERE status = 'released'
    GROUP BY payer_id
) rel ON u.user_id = rel.user_id;

COMMENT ON VIEW v_user_frozen_ledger_balance IS 'Frozen balance từ ledger+escrow items: SUM(freeze) - SUM(refund) - SUM(released items). So với users.frozen_diamond_balance. Alert nếu mismatch.';

-- =============================================================================
-- Chú thích: Các STORED PROCEDURES (proc_wallet_credit, proc_wallet_debit, 
-- proc_wallet_freeze, proc_wallet_release, proc_wallet_refund)
-- ĐÃ ĐƯỢC CHUYỂN LOGIC SANG BACKEND (.NET Core Entity Framework).
-- Không còn sử dụng các procedure ở mức Database nữa để dễ cài đặt và maintain code theo chuẩn C# Clean Architecture.
-- Các logic này giờ sẽ dùng `SELECT * FROM users WHERE id = {id} FOR UPDATE` transaction.
-- =============================================================================

-- =============================================================================
-- TRIGGERS (updated_at)
-- =============================================================================

CREATE OR REPLACE FUNCTION update_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_users_updated_at BEFORE UPDATE ON users FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_chat_finance_sessions_updated_at BEFORE UPDATE ON chat_finance_sessions FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_withdrawal_requests_updated_at BEFORE UPDATE ON withdrawal_requests FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_deposit_orders_updated_at BEFORE UPDATE ON deposit_orders FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_subscription_plans_updated_at BEFORE UPDATE ON subscription_plans FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_user_subscriptions_updated_at BEFORE UPDATE ON user_subscriptions FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_subscription_entitlement_buckets_updated_at BEFORE UPDATE ON subscription_entitlement_buckets FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_reader_payout_profiles_updated_at BEFORE UPDATE ON reader_payout_profiles FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_entitlement_mapping_rules_updated_at BEFORE UPDATE ON entitlement_mapping_rules FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_deposit_promotions_updated_at BEFORE UPDATE ON deposit_promotions FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_ai_requests_updated_at BEFORE UPDATE ON ai_requests FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_user_geo_signals_updated_at BEFORE UPDATE ON user_geo_signals FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_system_configs_updated_at BEFORE UPDATE ON system_configs FOR EACH ROW EXECUTE FUNCTION update_updated_at();
CREATE TRIGGER trg_data_rights_requests_updated_at BEFORE UPDATE ON data_rights_requests FOR EACH ROW EXECUTE FUNCTION update_updated_at();
-- M6 fix: Trigger cho chat_question_items.updated_at (escrow state machine audit)
CREATE TRIGGER trg_chat_question_items_updated_at BEFORE UPDATE ON chat_question_items FOR EACH ROW EXECUTE FUNCTION update_updated_at();

-- =============================================================================
-- SEED: system_configs (định giá động – OPS-4.20.1)
-- =============================================================================

INSERT INTO system_configs (key, value, description) VALUES
    ('diamond_vnd_rate',         '1000',  '1 Diamond = 1000 VND (tỷ giá chuẩn)'),
    ('daily_checkin_gold',       '1',    'Vàng trao mỗi ngày điểm danh'),
    ('register_bonus_gold',      '5',    'Vàng tặng khi xác minh email'),
    ('platform_fee_percent',     '10',   'Phí nền tảng (%) khi Reader rút tiền'),
    ('min_withdrawal_diamond',   '50',   'Diamond tối thiểu để rút'),
    ('ai_error_timeout_seconds', '30',   'Timeout AI (giây)'),
    ('ai_daily_quota_free',      '3',    'Số request AI/ngày (Free tier)'),
    ('ai_daily_quota_premium',   '30',   'Số request AI/ngày (Premium)'),
    ('share_reward_gold',        '2',    'Vàng thưởng mỗi lần share MXH'),
    -- L2 fix: thêm configs thiếu từ ARCH
    ('ai_max_retry_per_request', '1',    'Số lần retry tối đa mỗi AI request (BR-14)'),
    ('ai_timeout_before_token_seconds', '30', 'Timeout chờ token đầu tiên (ARCH-4.4.3)'),
    ('ai_in_flight_cap',         '2',    'Số AI request đồng thời tối đa per user (ARCH-4.4.3)'),
    ('streak_freeze_window_hours', '24', 'Cửa sổ mua streak freeze (giờ) – BR-7'),
    -- N8 fix: thêm configs thiếu cho friend chain, gacha, offer timeout
    ('friend_chain_reward_gold', '3', 'Gold thưởng mỗi friend chain reading – BR-4.7.2'),
    ('friend_chain_daily_cap', '3', 'Số lần nhận thưởng friend chain tối đa/ngày – BR-4.7.2'),
    ('gacha_cost_diamond', '5', 'Diamond phí mỗi lần quay Gacha – BR-5.2'),
    ('offer_timeout_hours', '12', 'Thời gian chờ reader accept offer trước khi hết hạn (giờ) – N3 fix')
ON CONFLICT (key) DO NOTHING;

-- System accounts (double-entry: platform, escrow) – dùng khi cần 2 dòng ledger cho transfer
-- INSERT chỉ chạy 1 lần; id cố định để code tham chiếu.
-- M10 fix: dùng role 'system' cho tài khoản hệ thống
INSERT INTO users (id, email, username, password_hash, display_name, date_of_birth, role)
VALUES
    ('00000000-0000-0000-0000-000000000001', 'platform@system.local', 'system_platform', '', 'System Platform', '2000-01-01', 'system'),
    ('00000000-0000-0000-0000-000000000002', 'escrow@system.local', 'system_escrow', '', 'System Escrow', '2000-01-01', 'system')
ON CONFLICT (id) DO NOTHING;

-- =============================================================================
-- TABLES MOVED FROM MONGO TO POSTGRES DUE TO EF CORE IMPLEMENTATION
-- =============================================================================

CREATE TABLE reading_sessions (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    spread_type VARCHAR(50) NOT NULL,
    cards_drawn JSONB,
    is_completed BOOLEAN NOT NULL DEFAULT false,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMPTZ
);
CREATE INDEX idx_reading_sessions_user_id ON reading_sessions(user_id);
CREATE INDEX idx_reading_sessions_user_created ON reading_sessions(user_id, created_at);

CREATE TABLE user_collections (
    user_id UUID NOT NULL REFERENCES users(id),
    card_id INT NOT NULL,
    level INT NOT NULL DEFAULT 1,
    copies INT NOT NULL DEFAULT 1,
    exp_gained BIGINT NOT NULL DEFAULT 0,
    last_drawn_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, card_id)
);
CREATE INDEX idx_user_collections_user_id ON user_collections(user_id);

# 📋 HƯỚNG DẪN CODE LẠI DỰ ÁN TAROTNOWAI2 TỪ ĐẦU

> **Mục đích:** Hướng dẫn từng bước chi tiết để **code lại toàn bộ dự án** từ con số 0.  
> Viết cho người **không biết gì về code** cũng có thể hiểu và làm theo được.

---

## MỤC LỤC

1. [Cài Đặt Phần Mềm (macOS)](#1-cài-đặt-phần-mềm-macos)
2. [Cài Đặt Cơ Sở Dữ Liệu (Schema Chi Tiết)](#2-cài-đặt-cơ-sở-dữ-liệu)
3. [Khởi Tạo Dự Án Trống](#3-khởi-tạo-dự-án-trống)
4. [Thứ Tự Code Theo Tính Năng](#4-thứ-tự-code-theo-tính-năng)
5. [Cấu Hình & Chạy Hệ Thống](#5-cấu-hình--chạy-hệ-thống)
6. [Xử Lý Lỗi Thường Gặp](#6-xử-lý-lỗi-thường-gặp)

---

## 1. CÀI ĐẶT PHẦN MỀM (macOS)

### 1.1 Cài Homebrew (trình quản lý gói cho macOS)

```bash
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```

### 1.2 Cài tất cả phần mềm

```bash
# .NET SDK 8 — để chạy Backend
brew install dotnet@8

# Node.js 20 — để chạy Frontend
brew install node@20

# PostgreSQL 16 — database SQL chính
brew install postgresql@16
brew services start postgresql@16

# MongoDB 7 — database document
brew tap mongodb/brew
brew install mongodb-community@7.0
brew services start mongodb-community@7.0

# Redis 7 — cache & rate limiting
brew install redis
brew services start redis

# Git
brew install git

# EF Core tools (tạo migration cho PostgreSQL)
dotnet tool install --global dotnet-ef
```

### 1.3 Kiểm tra tất cả đã cài xong

```bash
dotnet --version    # Cần: 8.0.xxx
node --version      # Cần: v20+
npm --version       # Cần: 10+
psql --version      # Cần: 16+
mongosh --eval "db.version()"   # Cần: 7+
redis-cli ping      # Cần: PONG
git --version
dotnet ef --version
```

> ⚠️ Tất cả lệnh đều phải trả kết quả. Nếu lệnh nào báo lỗi → cài lại phần mềm đó.

---

## 2. CÀI ĐẶT CƠ SỞ DỮ LIỆU

### 2.1 PostgreSQL — Tạo user & database

```bash
psql postgres
```

```sql
CREATE USER tarot_user WITH PASSWORD 'tarot_dev_pass';
CREATE DATABASE tarotweb OWNER tarot_user;
GRANT ALL PRIVILEGES ON DATABASE tarotweb TO tarot_user;
\q
```

### 2.2 PostgreSQL — Chi tiết TOÀN BỘ BẢNG cần tạo

> 💡 **Lưu ý quan trọng:** Bạn **KHÔNG CẦN** chạy SQL để tạo bảng thủ công. Khi code xong Backend, chạy lệnh `dotnet ef database update` → EF Core sẽ tự tạo tất cả bảng. Schema dưới đây chỉ để bạn **hiểu cấu trúc** trước khi code.

#### Bảng 1: `users` — Người dùng (Bảng trung tâm)

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | Mã định danh duy nhất |
| `email` | `varchar` UNIQUE NOT NULL | Email đăng nhập |
| `username` | `varchar` UNIQUE NOT NULL | Tên đăng nhập |
| `password_hash` | `varchar` NOT NULL | Mật khẩu đã hash (Argon2id) |
| `display_name` | `varchar` NOT NULL | Tên hiển thị |
| `avatar_url` | `varchar` NULL | Link ảnh đại diện |
| `date_of_birth` | `date` NOT NULL | Ngày sinh |
| `has_consented` | `boolean` | Đã đồng ý điều khoản |
| `level` | `int` DEFAULT 1 | Cấp độ người dùng |
| `exp` | `bigint` DEFAULT 0 | Điểm kinh nghiệm |
| `status` | `varchar` NOT NULL | Trạng thái: `pending`, `active`, `locked` |
| `role` | `varchar` NOT NULL | Vai trò: `user`, `tarot_reader`, `admin` |
| `reader_status` | `varchar` NOT NULL | Trạng thái đơn Reader: `pending`, `approved`, `rejected` |
| `mfa_enabled` | `boolean` DEFAULT false | Đã bật bảo mật 2 lớp |
| `mfa_secret_encrypted` | `varchar` NULL | Secret MFA mã hóa |
| `mfa_backup_codes_hash_json` | `varchar` NULL | Backup codes đã hash |
| `created_at` | `timestamp` | Ngày tạo |
| `updated_at` | `timestamp` NULL | Ngày cập nhật |
| **Owned: Wallet** | — | **4 cột ví nằm cùng bảng users** |
| `wallet_gold_balance` | `bigint` DEFAULT 0 | Số dư Gold (miễn phí) |
| `wallet_diamond_balance` | `bigint` DEFAULT 0 | Số dư Diamond (nạp tiền) |
| `wallet_frozen_diamond_balance` | `bigint` DEFAULT 0 | Diamond đang bị đóng băng (escrow) |
| `wallet_total_diamonds_purchased` | `bigint` DEFAULT 0 | Tổng Diamond đã nạp |

#### Bảng 2: `refresh_tokens` — Token làm mới phiên đăng nhập

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | Thuộc user nào |
| `token` | `varchar` NOT NULL | Hash SHA256 của refresh token |
| `expires_at` | `timestamp` NOT NULL | Hết hạn (mặc định 7 ngày) |
| `created_at` | `timestamp` | — |
| `created_by_ip` | `varchar` | IP tạo token |
| `revoked_at` | `timestamp` NULL | Thời điểm bị thu hồi |

#### Bảng 3: `email_otps` — Mã OTP xác thực email

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | — |
| `otp_code` | `varchar` NOT NULL | Hash SHA256 của mã 6 số |
| `type` | `varchar` NOT NULL | `email_verification` hoặc `reset_password` |
| `expires_at` | `timestamp` | Hết hạn (15 phút) |
| `is_used` | `boolean` DEFAULT false | Đã sử dụng chưa |
| `created_at` | `timestamp` | — |

#### Bảng 4: `wallet_transactions` — Sổ cái giao dịch (Append-only)

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | — |
| `currency` | `varchar` NOT NULL | `gold` hoặc `diamond` |
| `type` | `varchar` NOT NULL | `deposit`, `spend`, `release`, `refund`, `admin_add` |
| `amount` | `bigint` NOT NULL | Số tiền giao dịch |
| `balance_before` | `bigint` | Số dư trước giao dịch |
| `balance_after` | `bigint` | Số dư sau giao dịch |
| `reference_source` | `varchar` NULL | Nguồn: `deposit_order`, `ai_session`, `escrow` |
| `reference_id` | `varchar` NULL | ID tham chiếu |
| `description` | `varchar` NULL | Mô tả giao dịch |
| `metadata_json` | `text` NULL | Dữ liệu bổ sung JSON |
| `idempotency_key` | `varchar` UNIQUE NULL | Chống ghi đúp |
| `created_at` | `timestamp` | — |

#### Bảng 5: `deposit_orders` — Đơn nạp tiền

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | — |
| `amount_vnd` | `bigint` NOT NULL | Số tiền VNĐ |
| `diamond_amount` | `bigint` NOT NULL | Số Diamond quy đổi |
| `status` | `varchar` NOT NULL | `Pending`, `Success`, `Failed` |
| `transaction_id` | `varchar` NULL | Mã giao dịch ngân hàng |
| `fx_snapshot` | `varchar` NULL | Khuyến mãi áp dụng |
| `created_at` | `timestamp` | — |
| `processed_at` | `timestamp` NULL | — |

#### Bảng 6: `deposit_promotions` — Chương trình khuyến mãi nạp tiền

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `min_amount_vnd` | `bigint` | Nạp tối thiểu VNĐ để hưởng |
| `bonus_diamond` | `bigint` | Số Diamond thưởng thêm |
| `is_active` | `boolean` | Đang hoạt động |
| `created_at` | `timestamp` | — |

#### Bảng 7: `user_consents` — Lịch sử đồng ý pháp lý

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | — |
| `document_type` | `varchar` NOT NULL | `TOS`, `PrivacyPolicy`, `AiDisclaimer` |
| `version` | `varchar` NOT NULL | Phiên bản tài liệu |
| `consented_at` | `timestamp` | Thời điểm đồng ý |
| `ip_address` | `varchar` | IP khi đồng ý |
| `user_agent` | `varchar` | Trình duyệt |

#### Bảng 8: `ai_requests` — Lịch sử gọi AI

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` FK → users | — |
| `reading_session_ref` | `varchar` NOT NULL | Trỏ tới MongoDB ReadingSession |
| `followup_sequence` | `smallint` NULL | NULL=lần đầu, 1-5=follow-up |
| `status` | `varchar` NOT NULL | `requested`, `completed`, `failed` |
| `first_token_at` | `timestamptz` NULL | Thời điểm token AI đầu tiên |
| `completion_marker_at` | `timestamptz` NULL | Thời điểm AI kết thúc |
| `finish_reason` | `varchar` NULL | Lý do AI dừng |
| `retry_count` | `smallint` DEFAULT 0 | Số lần retry |
| `prompt_version` | `varchar` NULL | Phiên bản prompt |
| `policy_version` | `varchar` NULL | Phiên bản policy |
| `correlation_id` | `uuid` NULL | Tracking ID |
| `trace_id` | `varchar` NULL | OpenTelemetry trace |
| `charge_gold` | `bigint` DEFAULT 0 | Số Gold bị trừ |
| `charge_diamond` | `bigint` DEFAULT 0 | Số Diamond bị trừ |
| `requested_locale` | `varchar` NULL | Ngôn ngữ yêu cầu |
| `returned_locale` | `varchar` NULL | Ngôn ngữ trả về |
| `fallback_reason` | `varchar` NULL | Lý do fallback ngôn ngữ |
| `idempotency_key` | `varchar` NULL | Chống ghi đúp |
| `created_at` | `timestamptz` | — |
| `updated_at` | `timestamptz` NULL | — |

#### Bảng 9: `chat_finance_sessions` — Escrow phiên chat

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `conversation_ref` | `varchar` NOT NULL | Trỏ tới MongoDB Conversation |
| `user_id` | `uuid` | Khách hàng |
| `reader_id` | `uuid` | Reader |
| `status` | `varchar` NOT NULL | `pending`, `active`, `completed`, `refunded` |
| `total_frozen` | `bigint` DEFAULT 0 | Tổng diamond đang đóng băng |
| `created_at` | `timestamp` | — |
| `updated_at` | `timestamp` NULL | — |

#### Bảng 10: `chat_question_items` — Từng câu hỏi escrow

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `finance_session_id` | `uuid` FK → chat_finance_sessions | — |
| `conversation_ref` | `varchar` NOT NULL | Trỏ tới MongoDB Conversation |
| `payer_id` | `uuid` | Người trả tiền (User) |
| `receiver_id` | `uuid` | Người nhận tiền (Reader) |
| `type` | `varchar` NOT NULL | `main_question`, `add_question` |
| `amount_diamond` | `bigint` | Diamond bị giữ cho câu này |
| `status` | `varchar` NOT NULL | `pending`, `accepted`, `released`, `refunded`, `disputed` |
| `proposal_message_ref` | `varchar` NULL | Trỏ tới MongoDB message |
| `offer_expires_at` | `timestamp` NULL | Hết hạn chấp nhận |
| `accepted_at` | `timestamp` NULL | Reader chấp nhận lúc |
| `reader_response_due_at` | `timestamp` NULL | Hạn Reader trả lời (24h) |
| `replied_at` | `timestamp` NULL | Reader trả lời lúc |
| `auto_release_at` | `timestamp` NULL | Tự release sau 24h |
| `auto_refund_at` | `timestamp` NULL | Tự refund nếu Reader không trả lời |
| `released_at` / `confirmed_at` / `refunded_at` | `timestamp` NULL | Timestamps trạng thái |
| `dispute_window_start` / `dispute_window_end` | `timestamp` NULL | Cửa sổ khiếu nại |
| `idempotency_key` | `varchar` NULL | Chống ghi đúp |
| `created_at` / `updated_at` | `timestamp` | — |

#### Bảng 11: `withdrawal_requests` — Yêu cầu rút tiền (Reader)

| Cột | Kiểu | Ghi chú |
|-----|------|---------|
| `id` | `uuid` PK | — |
| `user_id` | `uuid` | Reader yêu cầu rút |
| `business_date_utc` | `date` | Ngày rút (1 lần/ngày) |
| `amount_diamond` | `bigint` | Số Diamond rút (tối thiểu 50) |
| `amount_vnd` | `bigint` | Quy đổi VNĐ (×1000) |
| `fee_vnd` | `bigint` | Phí 10% |
| `net_amount_vnd` | `bigint` | Thực nhận = amount_vnd - fee_vnd |
| `bank_name` | `varchar` NOT NULL | Tên ngân hàng |
| `bank_account_name` | `varchar` NOT NULL | Tên chủ TK |
| `bank_account_number` | `varchar` NOT NULL | Số TK |
| `status` | `varchar` NOT NULL | `pending`, `approved`, `paid`, `rejected` |
| `admin_id` | `uuid` NULL | Admin xử lý |
| `admin_note` | `varchar` NULL | Ghi chú admin |
| `processed_at` | `timestamp` NULL | — |
| `created_at` / `updated_at` | `timestamp` | — |

---

### 2.3 MongoDB — Chi tiết TOÀN BỘ COLLECTIONS

> MongoDB tự tạo collection khi insert document đầu tiên. Không cần tạo trước.

#### Collection 1: `reading_sessions` — Phiên rút bài Tarot

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "spreadType": "single | three_card | celtic_cross",
  "question": "Câu hỏi của khách",
  "cards": [{
    "cardId": 1,
    "code": "the_fool",
    "position": 0,
    "isReversed": true/false,
    "name": { "vi": "...", "en": "...", "zh": "..." }
  }],
  "aiInterpretation": "Nội dung AI giải bài (ghi sau khi stream xong)",
  "followupMessages": [{ "role": "user|assistant", "content": "..." }],
  "status": "initialized | revealed | streaming | completed",
  "locale": "vi",
  "createdAt": "DateTime",
  "completedAt": "DateTime"
}
```

#### Collection 2: `card_catalog` — Danh mục 78 lá bài Tarot

```json
{
  "_id": 1,
  "code": "the_fool",
  "name": { "vi": "Kẻ Ngốc", "en": "The Fool", "zh": "愚者" },
  "arcana": "major | minor",
  "suit": null | "wands | cups | swords | pentacles",
  "number": 0,
  "element": "air | water | fire | earth",
  "meanings": {
    "upright": { "vi": "...", "en": "...", "zh": "..." },
    "reversed": { "vi": "...", "en": "...", "zh": "..." }
  }
}
```

#### Collection 3: `user_collections` — Bộ sưu tập bài đã thu thập

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "cardId": 1,
  "code": "the_fool",
  "firstObtainedAt": "DateTime",
  "count": 5
}
```

#### Collection 4: `conversations` — Cuộc hội thoại Chat

```json
{
  "_id": "ObjectId",
  "participants": ["userId1", "userId2"],
  "type": "user_reader",
  "status": "active | closed",
  "lastMessageAt": "DateTime",
  "lastMessagePreview": "Tin nhắn gần nhất...",
  "unreadCounts": { "userId1": 0, "userId2": 2 },
  "createdAt": "DateTime"
}
```

#### Collection 5: `chat_messages` — Tin nhắn chat

```json
{
  "_id": "ObjectId",
  "conversationId": "ObjectId ref",
  "senderId": "Guid string",
  "type": "text | offer | system",
  "content": "Nội dung tin nhắn",
  "metadata": { "offerAmount": 5 },
  "readBy": ["userId1"],
  "createdAt": "DateTime"
}
```

#### Collection 6: `reader_profiles` — Hồ sơ Reader

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "displayName": "Tên hiển thị",
  "avatarUrl": "URL ảnh",
  "status": "online | offline",
  "bio": { "vi": "...", "en": "...", "zh": "..." },
  "specialties": ["tarot", "astrology"],
  "pricing": { "diamondPerQuestion": 5 },
  "stats": { "avgRating": 4.5, "totalReviews": 12 },
  "badges": ["verified"],
  "isDeleted": false,
  "createdAt": "DateTime"
}
```

#### Collection 7: `reader_requests` — Đơn xin làm Reader

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "displayName": "Tên",
  "bio": "Giới thiệu bản thân",
  "specialties": ["tarot"],
  "experience": "3 năm kinh nghiệm...",
  "status": "pending | approved | rejected",
  "adminNote": "Ghi chú admin",
  "createdAt": "DateTime"
}
```

#### Collection 8: `ai_provider_logs` — Log gọi AI provider

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "readingRef": "ObjectId ref → reading_sessions",
  "aiRequestRef": "Guid ref → ai_requests SQL",
  "model": "gpt-4o-mini",
  "tokens": { "in": 1500, "out": 800 },
  "latencyMs": 2300,
  "status": "requested | completed | failed",
  "errorCode": null,
  "traceId": "OpenTelemetry ID",
  "createdAt": "DateTime"
}
```

#### Collection 9: `reports` — Báo cáo vi phạm

```json
{
  "_id": "ObjectId",
  "reporterId": "Guid string",
  "reportedUserId": "Guid string",
  "conversationId": "ObjectId ref",
  "reason": "Lý do báo cáo",
  "status": "pending | reviewed | resolved",
  "createdAt": "DateTime"
}
```

#### Collection 10: `notifications` — Thông báo (chưa hoàn thiện)

```json
{
  "_id": "ObjectId",
  "userId": "Guid string",
  "type": "system | escrow | chat",
  "title": "Tiêu đề",
  "message": "Nội dung",
  "isRead": false,
  "createdAt": "DateTime"
}
```

---

### 2.4 Redis — Các key pattern sử dụng

| Key Pattern | Kiểu | TTL | Mục đích |
|-------------|------|-----|----------|
| `TarotNow:user:{userId}:profile` | Hash | 30 phút | Cache thông tin user |
| `TarotNow:reading:ratelimit:{userId}` | String | 30 giây | Rate limit rút bài |
| `TarotNow:ai:inflight:{userId}` | String | 5 phút | Đếm AI request đang chạy |
| `TarotNow:cache:backend_state` | String | — | Trạng thái cache backend |

> Redis tự tạo key khi ghi, không cần tạo trước.

---

## 3. KHỞI TẠO DỰ ÁN TRỐNG

### 3.1 Tạo Backend (.NET 8 Clean Architecture)

```bash
# Tạo thư mục gốc
mkdir ~/Desktop/TenDuAnMoi && cd ~/Desktop/TenDuAnMoi

# Tạo solution
mkdir Backend && cd Backend
dotnet new sln -n TarotNow

# Tạo 4 project theo Clean Architecture
dotnet new classlib -n TarotNow.Domain -o src/TarotNow.Domain
dotnet new classlib -n TarotNow.Application -o src/TarotNow.Application
dotnet new classlib -n TarotNow.Infrastructure -o src/TarotNow.Infrastructure
dotnet new webapi -n TarotNow.Api -o src/TarotNow.Api

# Thêm project vào solution
dotnet sln add src/TarotNow.Domain
dotnet sln add src/TarotNow.Application
dotnet sln add src/TarotNow.Infrastructure
dotnet sln add src/TarotNow.Api

# Thiết lập dependency references (Clean Architecture)
# Application tham chiếu Domain
dotnet add src/TarotNow.Application reference src/TarotNow.Domain
# Infrastructure tham chiếu Application
dotnet add src/TarotNow.Infrastructure reference src/TarotNow.Application
# Api tham chiếu Application + Infrastructure
dotnet add src/TarotNow.Api reference src/TarotNow.Application
dotnet add src/TarotNow.Api reference src/TarotNow.Infrastructure
```

### 3.2 Cài NuGet packages cho Backend

```bash
# Domain — KHÔNG cài gì (pure C#)

# Application
cd src/TarotNow.Application
dotnet add package MediatR
dotnet add package FluentValidation
dotnet add package AutoMapper
cd ../..

# Infrastructure
cd src/TarotNow.Infrastructure
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package MongoDB.Driver
dotnet add package StackExchange.Redis
dotnet add package Konscious.Security.Cryptography.Argon2
dotnet add package Microsoft.AspNetCore.SignalR.Common
dotnet add package System.IdentityModel.Tokens.Jwt
cd ../..

# Api
cd src/TarotNow.Api
dotnet add package Serilog.AspNetCore
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.Design
cd ../..
```

### 3.3 Tạo Frontend (Next.js 16)

```bash
cd ~/Desktop/TenDuAnMoi
npx -y create-next-app@latest Frontend --ts --app --tailwind --eslint --src-dir --no-import-alias

cd Frontend
npm install next-intl zustand react-hook-form @hookform/resolvers zod react-hot-toast react-markdown lucide-react @microsoft/signalr next-qrcode
```

---

## 4. THỨ TỰ CODE THEO TÍNH NĂNG

> **Quy tắc vàng:** Code theo thứ tự dưới đây. Mỗi tính năng code xong **cả BE lẫn FE** thì chạy thử test ngay trước khi qua tính năng tiếp theo.

---

### 🔴 TÍNH NĂNG 0: HẠ TẦNG CƠ SỞ (Code trước tiên, tất cả tính năng phụ thuộc vào đây)

> **Mục tiêu:** Sau khi code xong phần này, chạy `dotnet run` và `npm run dev` phải thành công, dù chưa có tính năng nào.

#### Bước 0.1 — Backend Domain Layer (nền tảng)

| Thứ tự | File cần tạo | Mục đích |
|--------|-------------|----------|
| 1 | `Domain/Entities/User.cs` | Entity User — trung tâm mọi tính năng |
| 2 | `Domain/Entities/UserWallet.cs` | Owned Entity quản lý ví (nằm trong User) |
| 3 | `Domain/Enums/UserRole.cs` | Hằng số vai trò: `user`, `tarot_reader`, `admin` |
| 4 | `Domain/Enums/UserStatus.cs` | Hằng số trạng thái: `pending`, `active`, `locked` |
| 5 | `Domain/Enums/CurrencyType.cs` | Hằng số tiền: `gold`, `diamond` |
| 6 | `Domain/Enums/TransactionType.cs` | Hằng số giao dịch: `deposit`, `spend`, `release`... |
| 7 | `Domain/Constants/EconomyConstants.cs` | Hằng số kinh tế: giá AI, tỉ giá VNĐ→Diamond |
| 8 | `Application/Exceptions/BusinessRuleException.cs` | Exception base cho lỗi nghiệp vụ Application (thay cho DomainException) |

#### Bước 0.2 — Backend Infrastructure Layer (kết nối DB)

| Thứ tự | File cần tạo | Mục đích |
|--------|-------------|----------|
| 9 | `Infrastructure/Persistence/ApplicationDbContext.cs` | EF Core DbContext — khai báo tất cả `DbSet<>` cho PostgreSQL |
| 10 | `Infrastructure/Persistence/MongoDbContext.cs` | MongoDB context — khai báo tên collection |
| 11 | `Infrastructure/Persistence/Configurations/UserConfiguration.cs` | Cấu hình bảng `users` + Owned UserWallet |
| 12 | `Infrastructure/DependencyInjection.cs` | Đăng ký DI cho Infrastructure (DbContext, MongoDB, Redis) |
| 13 | `Infrastructure/Services/RedisCacheService.cs` | Service cache Redis |

#### Bước 0.3 — Backend Application Layer (pipeline chung)

| Thứ tự | File cần tạo | Mục đích |
|--------|-------------|----------|
| 14 | `Application/Interfaces/ICacheService.cs` | Interface cache |
| 15 | `Application/Interfaces/ITransactionCoordinator.cs` | Interface Unit of Work |
| 16 | `Application/Common/Models/PaginatedList.cs` | Model phân trang dùng chung |
| 17 | `Application/Behaviors/ValidationBehavior.cs` | MediatR pipeline tự validate |
| 18 | `Application/DependencyInjection.cs` | Đăng ký DI cho Application (MediatR, AutoMapper) |

#### Bước 0.4 — Backend API Layer (khung chạy)

| Thứ tự | File cần tạo | Mục đích |
|--------|-------------|----------|
| 19 | `Api/Program.cs` | Điểm khởi động: DI, middleware, CORS, Swagger |
| 20 | `Api/Middlewares/GlobalExceptionHandler.cs` | Bắt lỗi → ProblemDetails JSON |
| 21 | `Api/appsettings.json` | Config: Connection strings, JWT, CORS |
| 22 | `Api/appsettings.Development.json` | Config dev local |
| 23 | `Api/Controllers/HealthController.cs` | `GET /health` — kiểm tra server sống |

#### Bước 0.5 — Frontend (khung chạy)

| Thứ tự | File cần tạo | Mục đích |
|--------|-------------|----------|
| 24 | `src/lib/api.ts` | HTTP client wrapper — tất cả API call đi qua file này |
| 25 | `src/i18n/request.ts` + `src/i18n/routing.ts` | Cấu hình đa ngôn ngữ |
| 26 | `messages/vi.json` + `messages/en.json` | File bản dịch |
| 27 | `src/app/[locale]/layout.tsx` | Root layout |
| 28 | `src/app/[locale]/page.tsx` | Trang chủ |
| 29 | `src/components/ui/Button.tsx`, `Input.tsx`, `Modal.tsx`... | UI primitives |
| 30 | `src/components/common/Navbar.tsx` | Thanh điều hướng |
| 31 | `.env.local` | `NEXT_PUBLIC_API_URL=http://localhost:5037/api/v1` |

**✅ Test:** Chạy `dotnet run` → mở `http://localhost:5037/api/v1/health`. Chạy `npm run dev` → mở `http://localhost:3000`.

---

### 🟢 TÍNH NĂNG 1: ĐĂNG KÝ & ĐĂNG NHẬP (Authentication)

> **Mục tiêu:** Sau khi code xong, người dùng có thể đăng ký tài khoản, đăng nhập, và duy trì phiên.

#### BE — Domain

| File | Mục đích |
|------|----------|
| `Domain/Entities/RefreshToken.cs` | Entity lưu refresh token (hash SHA256) |
| `Domain/Entities/EmailOtp.cs` | Entity lưu mã OTP 6 số (hash SHA256) |
| `Domain/Enums/OtpType.cs` | Enum: `email_verification`, `reset_password` |

#### BE — Application

| File | Mục đích |
|------|----------|
| `Interfaces/IUserRepository.cs` | Interface CRUD User |
| `Interfaces/IRefreshTokenRepository.cs` | Interface CRUD RefreshToken |
| `Interfaces/IEmailOtpRepository.cs` | Interface CRUD EmailOtp |
| `Interfaces/IPasswordHasher.cs` | Interface hash mật khẩu |
| `Interfaces/ITokenService.cs` | Interface tạo JWT |
| `Interfaces/IEmailSender.cs` | Interface gửi email |
| `Features/Auth/Commands/Register/RegisterCommand.cs` | Command đăng ký |
| `Features/Auth/Commands/Register/RegisterCommandHandler.cs` | Handler xử lý đăng ký |
| `Features/Auth/Commands/Register/RegisterCommandValidator.cs` | Validate input đăng ký |
| `Features/Auth/Commands/Login/LoginCommand.cs` | Command đăng nhập |
| `Features/Auth/Commands/Login/LoginCommandHandler.cs` | Handler xử lý đăng nhập |
| `Features/Auth/Commands/Login/AuthResponse.cs` | DTO trả về accessToken + refreshToken |
| `Features/Auth/Commands/VerifyEmail/*` | Command + Handler xác thực email |
| `Features/Auth/Commands/ForgotPassword/*` | Command + Handler quên mật khẩu |
| `Features/Auth/Commands/ResetPassword/*` | Command + Handler đặt lại mật khẩu |
| `Features/Auth/Commands/RevokeToken/*` | Command + Handler đăng xuất |

#### BE — Infrastructure

| File | Mục đích |
|------|----------|
| `Persistence/Repositories/UserRepository.cs` | Implement IUserRepository (PostgreSQL) |
| `Persistence/Repositories/RefreshTokenRepository.cs` | Implement IRefreshTokenRepository |
| `Persistence/Repositories/EmailOtpRepository.cs` | Implement IEmailOtpRepository |
| `Persistence/Configurations/RefreshTokenConfiguration.cs` | EF cấu hình bảng refresh_tokens |
| `Persistence/Configurations/EmailOtpConfiguration.cs` | EF cấu hình bảng email_otps |
| `Security/Argon2idPasswordHasher.cs` | Hash mật khẩu Argon2id |
| `Security/JwtTokenService.cs` | Tạo JWT access token |
| `Services/MockEmailSender.cs` | Gửi email giả lập (dev) |

#### BE — API

| File | Mục đích |
|------|----------|
| `Controllers/AuthController.cs` | 8 endpoints: register, login, logout, verify-email, forgot-password, reset-password, refresh, send-verification-email |

#### FE — Frontend

| File | Mục đích |
|------|----------|
| `types/auth.ts` | TypeScript types cho auth |
| `actions/authActions.ts` | 6 hàm gọi API auth |
| `store/authStore.ts` | Zustand store lưu token + user info |
| `components/auth/AuthSessionManager.tsx` | Component quản lý session client |
| `components/layout/AuthLayout.tsx` | Layout cho trang auth |
| `app/[locale]/(auth)/login/page.tsx` | Trang đăng nhập |
| `app/[locale]/(auth)/register/page.tsx` | Trang đăng ký |
| `app/[locale]/(auth)/verify-email/page.tsx` | Trang xác thực email |
| `app/[locale]/(auth)/forgot-password/page.tsx` | Trang quên mật khẩu |
| `app/[locale]/(auth)/reset-password/page.tsx` | Trang đặt lại mật khẩu |

**✅ Test:** Đăng ký tài khoản → Đăng nhập → Kiểm tra JWT trong browser DevTools → Đăng xuất.

---

### 🟢 TÍNH NĂNG 2: HỒ SƠ CÁ NHÂN (Profile)

#### BE

| File | Mục đích |
|------|----------|
| `Features/Profile/Queries/GetProfile/*` | Query + Handler lấy thông tin profile |
| `Features/Profile/Commands/UpdateProfile/*` | Command + Handler + Validator cập nhật profile |
| `Controllers/ProfileController.cs` | 2 endpoints: GET + PATCH `/profile` |

#### FE

| File | Mục đích |
|------|----------|
| `actions/profileActions.ts` | Hàm gọi API profile |
| `app/[locale]/(user)/profile/page.tsx` | Trang xem/sửa hồ sơ |

**✅ Test:** Đăng nhập → Vào profile → Sửa tên hiển thị → Reload → Kiểm tra đã lưu.

---

### 🟢 TÍNH NĂNG 3: VÍ & SỔ CÁI (Wallet)

#### BE — Domain

| File | Mục đích |
|------|----------|
| `Domain/Entities/WalletTransaction.cs` | Entity sổ cái giao dịch |

#### BE — Application + Infrastructure + API

| File | Mục đích |
|------|----------|
| `Interfaces/IWalletRepository.cs` | Interface ví |
| `Interfaces/ILedgerRepository.cs` | Interface sổ cái |
| `Features/Wallet/Queries/GetWalletBalance/*` | Query lấy số dư |
| `Features/Wallet/Queries/GetLedgerList/*` | Query lịch sử giao dịch |
| `Persistence/Repositories/WalletRepository.cs` | Implement ví (PostgreSQL) |
| `Persistence/Repositories/LedgerRepository.cs` | Implement sổ cái |
| `Persistence/Configurations/WalletTransactionConfiguration.cs` | EF config bảng |
| `Persistence/TransactionCoordinator.cs` | Unit of Work (ACID) |
| `Controllers/WalletController.cs` | 2 endpoints: balance + ledger |

#### FE

| File | Mục đích |
|------|----------|
| `types/wallet.ts` | Types |
| `actions/walletActions.ts` | API calls |
| `store/walletStore.ts` | Zustand state ví |
| `components/common/WalletWidget.tsx` | Widget hiển thị số dư |
| `app/[locale]/(user)/wallet/page.tsx` | Trang ví |

**✅ Test:** Đăng nhập → Xem số dư (mặc định 0 gold, 0 diamond) → Xem lịch sử giao dịch trống.

---

### 🟢 TÍNH NĂNG 4: RÚT BÀI TAROT (Reading)

#### BE — Domain + Application + Infrastructure + API

| File | Mục đích |
|------|----------|
| `Domain/Entities/ReadingSession.cs` | Entity phiên rút bài |
| `Domain/Enums/SpreadType.cs` | Enum kiểu trải: single, three_card, celtic_cross |
| `Interfaces/IReadingSessionRepository.cs` | Interface phiên (MongoDB) |
| `Interfaces/ICardsCatalogRepository.cs` | Interface danh mục bài (MongoDB) |
| `Interfaces/IUserCollectionRepository.cs` | Interface bộ sưu tập (MongoDB) |
| `Interfaces/IRngService.cs` | Interface random |
| `Features/Reading/Commands/InitSession/*` | Command khởi tạo phiên |
| `Features/Reading/Commands/RevealSession/*` | Command lật bài |
| `Features/Reading/Queries/GetCollection/*` | Query bộ sưu tập |
| `Persistence/MongoDocuments/ReadingSessionDocument.cs` | Schema MongoDB |
| `Persistence/MongoDocuments/CardCatalogDocument.cs` | Schema MongoDB |
| `Persistence/MongoDocuments/UserCollectionDocument.cs` | Schema MongoDB |
| `Persistence/Repositories/Mongo...Repository.cs` | 3 repository MongoDB |
| `Services/RngService.cs` | Service random (cryptographic) |
| `Controllers/TarotController.cs` | 3 endpoints |

#### FE

| File | Mục đích |
|------|----------|
| `lib/tarotData.ts` | Dữ liệu 78 lá bài client-side |
| `actions/readingActions.ts` + `actions/collectionActions.ts` | API calls |
| `app/[locale]/(user)/reading/page.tsx` | Trang rút bài |
| `app/[locale]/(user)/collection/page.tsx` | Trang bộ sưu tập |

**✅ Test:** Rút bài → Lật bài → Xem kết quả → Kiểm tra bộ sưu tập.

---

### 🟢 TÍNH NĂNG 5: AI STREAM + LỊCH SỬ

#### BE

| File | Mục đích |
|------|----------|
| `Domain/Entities/AiRequest.cs` | Entity theo dõi gọi AI |
| `Domain/Enums/AiRequestStatus.cs` | Enum trạng thái AI |
| `Domain/Services/FollowupPricingService.cs` | Tính phí follow-up |
| `Interfaces/IAiProvider.cs` | Interface gọi AI |
| `Interfaces/IAiRequestRepository.cs` | Interface AI request |
| `Features/Reading/Commands/StreamReading/*` | Command stream SSE |
| `Features/Reading/Commands/CompleteAiStream/*` | Command hoàn tất stream |
| `Features/History/Queries/*` | 3 Query lịch sử |
| `Services/Ai/OpenAiProvider.cs` | Gọi API OpenAI |
| `Persistence/MongoDocuments/AiProviderLogDocument.cs` | Log AI MongoDB |
| `Controllers/AiController.cs` | SSE endpoint |
| `Controllers/HistoryController.cs` | 3 endpoints lịch sử |

#### FE

| File | Mục đích |
|------|----------|
| `components/AiInterpretationStream.tsx` | Component SSE stream |
| `actions/historyActions.ts` | API calls lịch sử |
| `app/[locale]/reading/session/[id]/page.tsx` | Trang xem AI stream |
| `app/[locale]/(user)/reading/history/page.tsx` | Danh sách lịch sử |
| `app/[locale]/(user)/reading/history/[id]/page.tsx` | Chi tiết phiên |

**✅ Test:** Rút bài → Nhấn "Giải bài AI" → Xem text streaming → Xem lịch sử.

---

### 🟢 TÍNH NĂNG 6: NẠP TIỀN + PHÁP LÝ + KHUYẾN MÃI

#### BE

| File | Mục đích |
|------|----------|
| `Domain/Entities/DepositOrder.cs` | Entity đơn nạp |
| `Domain/Entities/DepositPromotion.cs` | Entity khuyến mãi |
| `Domain/Entities/UserConsent.cs` | Entity đồng ý pháp lý |
| `Interfaces/I...Repository.cs` | 3 Interface mới |
| `Features/Deposit/Commands/*` | 2 Command: CreateOrder + Webhook |
| `Features/Legal/Commands/*` + `Queries/*` | Consent check + record |
| `Features/Promotions/Commands/*` + `Queries/*` | CRUD promotion |
| `Services/HmacPaymentGatewayService.cs` | Verify webhook |
| `Controllers/DepositController.cs` | 2 endpoints |
| `Controllers/LegalController.cs` | 2 endpoints |
| `Controllers/PromotionsController.cs` | 4 endpoints |

#### FE

| File | Mục đích |
|------|----------|
| `actions/depositActions.ts` + `actions/legalActions.ts` + `actions/promotionActions.ts` | API calls |
| `app/[locale]/(user)/wallet/deposit/page.tsx` | Trang nạp tiền |
| `app/[locale]/legal/tos/page.tsx` + `privacy/page.tsx` + `ai-disclaimer/page.tsx` | Trang pháp lý |

**✅ Test:** Nạp tiền → Kiểm tra consent modal → Mở Swagger test webhook → Kiểm tra số dư tăng.

---

### 🟢 TÍNH NĂNG 7: READER + CHAT + ESCROW + RÚT TIỀN

> Đây là tính năng phức tạp nhất, code sau cùng.

#### BE

| File | Mục đích |
|------|----------|
| **Reader:** `Domain/Enums/Reader*.cs`, `Interfaces/IReader*Repository.cs`, `Features/Reader/Commands/*` + `Queries/*`, MongoDB Documents + Repos, `Controllers/ReaderController.cs` | 6 endpoints Reader |
| **Chat:** `Domain/Enums/Chat*.cs`, `Interfaces/IConversation*Repository.cs` + `IChatMessage*Repository.cs`, `Features/Chat/Commands/*` + `Queries/*`, MongoDB Documents + Repos, `Hubs/ChatHub.cs`, `Controllers/ConversationController.cs` | 3 REST + 1 SignalR |
| **Escrow:** `Domain/Entities/ChatFinanceSession.cs` + `ChatQuestionItem.cs`, `Domain/Enums/QuestionItem*.cs`, `Interfaces/IChatFinanceRepository.cs`, `Features/Escrow/Commands/*` + `Queries/*`, `BackgroundJobs/EscrowTimerService.cs`, `Controllers/EscrowController.cs` | 6 endpoints |
| **Withdrawal:** `Domain/Entities/WithdrawalRequest.cs`, `Features/Withdrawal/Commands/*` + `Queries/*`, `Controllers/WithdrawalController.cs` | 2 endpoints |
| **Report:** `Features/Chat/Commands/CreateReport/*`, `Controllers/ReportController.cs` | 1 endpoint |
| **MFA:** `Interfaces/IMfaService.cs`, `Services/TotpMfaService.cs`, `Features/Mfa/Commands/*` + `Queries/*`, `Controllers/MfaController.cs` | 4 endpoints |

#### FE

| File | Mục đích |
|------|----------|
| `types/reader.ts` + `types/chat.ts` + `types/escrow.ts` + `types/withdrawal.ts` + `types/mfa.ts` | Types |
| `actions/readerActions.ts` + `chatActions.ts` + `escrowActions.ts` + `withdrawalActions.ts` + `mfaActions.ts` | API calls |
| `app/[locale]/(user)/readers/page.tsx` + `[id]/page.tsx` | Danh sách + Chi tiết Reader |
| `app/[locale]/(user)/reader/apply/page.tsx` | Đăng ký Reader |
| `app/[locale]/(user)/profile/reader/page.tsx` | Profile Reader |
| `app/[locale]/(user)/chat/page.tsx` + `[id]/page.tsx` | Inbox + Chat |
| `components/chat/EscrowPanel.tsx` + `DisputeButton.tsx` + `ReportModal.tsx` | Components chat |
| `app/[locale]/(user)/wallet/withdraw/page.tsx` | Trang rút tiền |
| `app/[locale]/(user)/profile/mfa/page.tsx` | Trang MFA |
| `components/auth/MfaChallengeModal.tsx` | Modal MFA |

**✅ Test:** Tạo 2 tài khoản → 1 đăng ký Reader → Admin duyệt → Tạo chat → Gửi offer → Escrow → Rút tiền.

---

### 🟢 TÍNH NĂNG 8: ADMIN PANEL

#### BE

| File | Mục đích |
|------|----------|
| `Interfaces/IAdminRepository.cs` | Interface admin |
| `Features/Admin/Commands/*` | 5 Command (AddBalance, ApproveReader, ProcessDeposit, ResolveDispute, ToggleUserLock) |
| `Features/Admin/Queries/*` | 4 Query (LedgerMismatch, ListDeposits, ListReaderRequests, ListUsers) |
| `Persistence/Repositories/AdminRepository.cs` | Implement admin |
| `Controllers/AdminController.cs` | 11 endpoints admin |

#### FE

| File | Mục đích |
|------|----------|
| `actions/adminActions.ts` | API calls admin |
| `app/[locale]/admin/layout.tsx` | Layout admin |
| `app/[locale]/admin/page.tsx` + `users/page.tsx` + `deposits/page.tsx` + `reader-requests/page.tsx` + `readings/page.tsx` + `withdrawals/page.tsx` + `disputes/page.tsx` + `promotions/page.tsx` | 8 trang admin |

**✅ Test:** Đăng nhập bằng admin → Xem dashboard → Duyệt Reader → Duyệt nạp tiền → Đối soát sổ cái.

---

## 5. CẤU HÌNH & CHẠY HỆ THỐNG

### 5.1 Chạy Migration (tạo bảng PostgreSQL tự động)

```bash
cd ~/Desktop/TenDuAnMoi/Backend/src/TarotNow.Api
dotnet ef migrations add InitialCreate --project ../TarotNow.Infrastructure/TarotNow.Infrastructure.csproj
dotnet ef database update --project ../TarotNow.Infrastructure/TarotNow.Infrastructure.csproj
```

### 5.2 Chạy hàng ngày

```bash
# Terminal 1 — Backend
cd ~/Desktop/TenDuAnMoi/Backend/src/TarotNow.Api
dotnet run

# Terminal 2 — Frontend
cd ~/Desktop/TenDuAnMoi/Frontend
npm run dev
```

> 📝 **Luôn chạy Backend TRƯỚC, Frontend SAU.**

---

## 6. XỬ LÝ LỖI THƯỜNG GẶP

| Lỗi | Nguyên nhân | Giải pháp |
|-----|-------------|-----------|
| `dotnet: command not found` | Chưa cài .NET SDK | `brew install dotnet@8` → restart terminal |
| `Cannot connect to PostgreSQL` | PostgreSQL chưa chạy | `brew services start postgresql@16` |
| `MongoDB connection refused` | MongoDB chưa chạy | `brew services start mongodb-community@7.0` |
| `PONG` không hiện | Redis chưa chạy | `brew services start redis` |
| `npm install` lỗi ERESOLVE | Xung đột version | `npm install --legacy-peer-deps` |
| `dotnet ef` lỗi | Chưa cài tool | `dotnet tool install --global dotnet-ef` |
| Migration `relation already exists` | DB có bảng cũ | Xóa DB tạo lại (xem lệnh ở mục 2.1) |
| FE trang trắng | BE chưa chạy hoặc sai URL | Kiểm tra `.env.local` và BE health endpoint |
| AI stream không chạy | Thiếu API key OpenAI | Thêm key vào `appsettings.json` → `AiProvider.ApiKey` |

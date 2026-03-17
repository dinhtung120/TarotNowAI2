# 🎨 Kế Hoạch Thiết Kế Lại Frontend — TarotNowAI2

> **Phiên bản:** 1.1 — Cập nhật: 2026-03-17  
> **Mục tiêu:** Thiết kế lại toàn bộ Frontend theo phong cách **nhất quán**, **chuyên nghiệp**, và **dễ bảo trì**.

---

## 📌 Mục Lục

1. [Phân Tích Hiện Trạng & Vấn Đề](#1-phân-tích-hiện-trạng--vấn-đề)
2. [Triết Lý Thiết Kế](#2-triết-lý-thiết-kế)
3. [Design System](#3-design-system)
4. [Kiến Trúc Layout](#4-kiến-trúc-layout)
5. [Hệ Thống Điều Hướng (Navigation)](#5-hệ-thống-điều-hướng-navigation)
6. [Chi Tiết Từng Module / Trang](#6-chi-tiết-từng-module--trang)
7. [Thư Viện Component Dùng Chung](#7-thư-viện-component-dùng-chung)
8. [Responsive & Mobile Strategy](#8-responsive--mobile-strategy)
9. [Lộ Trình Triển Khai](#9-lộ-trình-triển-khai)

---

## 1. Phân Tích Hiện Trạng & Vấn Đề

### 1.1 Điểm Mạnh Hiện Có ✅

| Điểm mạnh | Chi tiết |
|------------|----------|
| Nền tảng visual mạnh | Trang chủ có Nebula background và micro-animations tốt để phát triển phong cách huyền bí |
| Dark Theme nhất quán | Tất cả trang đều dùng dark mode (`bg-[#020108]`) |
| Tailwind CSS | Framework CSS mạnh mẽ, utilities sẵn dùng |
| i18n sẵn có | Hỗ trợ đa ngôn ngữ qua `next-intl` |
| Font premium | Geist Sans + Playfair Display (serif) |

### 1.2 Vấn Đề Nghiêm Trọng Cần Sửa ❌

#### 🔴 Code lặp lại quá mức (WET — Write Everything Twice)

Mỗi trang đều **copy-paste** cùng một đoạn code:
- **Background Nebula System** (~20 dòng) — lặp ở: Home, Reading, Wallet, Profile, Admin...
- **Spiritual Particles** (~15 dòng) — lặp ở mọi trang
- **CSS Keyframes** (`@keyframes float, drift, pulse-slow`) — lặp bằng `<style dangerouslySetInnerHTML>` ở **mỗi trang**
- **Noise texture overlay** — dòng `bg-[url('https://grainy-gradients.vercel.app/noise.svg')]` lặp khắp nơi

> ⚠️ **Hậu quả:** Nếu muốn đổi màu nebula, phải sửa ở ~15 files.

#### 🔴 Không có Reusable Components

- **Chỉ có 6 component** (Navbar, WalletWidget, MfaChallengeModal, DisputeButton, EscrowPanel, ReportModal)
- Thiếu các component cơ bản: Button, Input, Card, Modal, Badge, Table, Pagination, EmptyState, LoadingSpinner
- Mỗi trang tự viết button, input, card → phong cách **hơi khác nhau** giữa các trang

#### 🔴 Navbar thiếu nhiều mục quan trọng

Hiện tại Navbar chỉ có: Home, Rút Bài, Miếu Bài, Lịch Sử, Profile, Wallet, Logout.

**Thiếu hoàn toàn:**
- Link **Chat / Tin nhắn** (đã có trang `/chat`)
- Link **Readers** (đã có trang `/readers`)
- Link **Admin** (cho user có role admin)
- **Notification badge** (tin nhắn mới, escrow updates)
- **Mobile hamburger menu**

#### 🔴 2 file globals.css xung đột

| File | Nội dung | Vấn đề |
|------|----------|--------|
| `app/globals.css` | `@tailwind base/components/utilities` (v3) | Tailwind v3 syntax |
| `app/[locale]/globals.css` | `@import "tailwindcss"` (v4) | Tailwind v4 syntax |

→ Cần thống nhất về **một file duy nhất**.

#### 🔴 Thiếu Footer component

- Trang chủ có footer đẹp, nhưng **viết inline** trong `page.tsx`
- Các trang khác **không có footer**
- Cần tách thành component `Footer` dùng chung

#### 🔴 Admin Sidebar thiếu nhiều mục

Hiện tại Admin sidebar chỉ có: Tổng Quan, Người Dùng, Giao Dịch, Khuyến Mãi, Xem Bài.

**Thiếu:**
- **Đơn Xin Reader** (`/admin/reader-requests`) — đã có trang + API
- **Rút Tiền** (`/admin/withdrawals`) — đã có trang + API
- **Tranh Chấp** (`/admin/disputes`) — đã có trang + API

#### 🟡 Thiếu User Sidebar / Dashboard

- User phải navigate qua Navbar link nhỏ để đến Profile, Wallet, History
- Không có **Dashboard tổng hợp** cho user (số dư, phiên gần nhất, tin nhắn mới)
- Trải nghiệm "rời rạc" — mỗi trang là một ốc đảo riêng

#### 🟡 Không có Loading States nhất quán

- Mỗi trang tự viết loading spinner khác nhau
- Không có skeleton loader cho UX tốt hơn

---

## 2. Triết Lý Thiết Kế

### 2.1 Phong Cách: **"Occult Nocturne"**

Giữ nguyên DNA hiện tại nhưng **nâng cấp tính nhất quán**:

| Yếu tố | Mô tả |
|---------|--------|
| **Tone màu** | Dark mode sâu (#05030d) + Indigo/Violet + Gold trầm |
| **Chất liệu** | Layered Obsidian Surfaces (không dùng glass, không backdrop-blur) |
| **Typography** | Playfair Display (tiêu đề serif) + Geist Sans (body) |
| **Hiệu ứng** | Nebula tối, spiritual particles mờ, glow tím thấp |
| **Bo góc** | Lớn (2xl → 3xl cho cards, full cho buttons) |
| **Text style** | Uppercase tracking-widest cho labels, Italic bold cho headings |

### 2.2 Nguyên Tắc DRY (Don't Repeat Yourself)

- Mọi hiệu ứng nền, particle, animation → **1 component duy nhất**
- Mọi style pattern → **design tokens** trong `globals.css`
- Mọi UI element → **reusable component**

---

## 3. Design System

### 3.1 Bảng Màu (Color Palette)

```
🎨 NỀN & BỀ MẶT
--bg-void:          #05030d       (nền chính — đêm sâu)
--bg-surface:       #110b1f       (surface card chuẩn)
--bg-elevated:      #1a1230       (surface nổi)
--bg-surface-hover: #231742       (hover state)
--border-subtle:    rgba(107,82,145,0.24)
--border-default:   rgba(129,98,179,0.38)
--border-focus:     rgba(173,130,236,0.70)

🟣 BRAND — Violet/Indigo
--purple-glow:      rgba(122,86,177,0.30)
--purple-accent:    #9a74cf
--purple-muted:     rgba(154,116,207,0.65)

🟡 ACCENT — Gold Trầm
--amber-accent:     #9d7a36
--amber-glow:       rgba(156,120,44,0.28)

🟢 STATUS
--success:          #3ea58a
--error:            #cf6979
--warning:          #b48b3f
--info:             #4d90a7

📝 TEXT
--text-primary:     #e2d8f6
--text-secondary:   #aa99c9
--text-muted:       #71648e
```

### 3.2 Typography Scale

```
HEADINGS (font-family: Playfair Display, serif)
--h1:  text-4xl md:text-6xl  font-black italic tracking-tighter
--h2:  text-3xl md:text-4xl  font-black italic tracking-tighter
--h3:  text-xl md:text-2xl   font-bold tracking-tight

BODY (font-family: Geist Sans)
--body-lg:   text-base  font-medium  leading-relaxed
--body-sm:   text-sm    font-medium  leading-relaxed
--body-xs:   text-xs    font-medium

LABELS
--label:     text-[10px] font-black uppercase tracking-[0.2em]
--label-sm:  text-[9px]  font-black uppercase tracking-[0.3em]
--label-xs:  text-[8px]  font-black uppercase tracking-[0.4em]
```

### 3.3 Spacing System

```
--space-xs:   4px    (gap nhỏ nhất)
--space-sm:   8px
--space-md:   16px
--space-lg:   24px
--space-xl:   32px
--space-2xl:  48px
--space-3xl:  64px
--space-4xl:  96px

SECTION PADDING
- Section gap:     py-20 md:py-32
- Page padding:    px-6 md:px-8
- Card padding:    p-6 md:p-8
- Container:       max-w-7xl mx-auto
```

### 3.4 Border Radius

```
--radius-sm:   12px  (rounded-xl)     → inputs, badges
--radius-md:   16px  (rounded-2xl)    → buttons, small cards
--radius-lg:   24px  (rounded-3xl)    → cards, panels
--radius-xl:   40px  (rounded-[2.5rem]) → hero cards, big sections
```

### 3.5 Shadow & Glow

```
--shadow-card:      0 18px 38px rgba(3,2,9,0.62)
--shadow-glow-sm:   0 0 24px rgba(122,86,177,0.24)
--shadow-glow-md:   0 0 44px rgba(122,86,177,0.34)
--shadow-glow-lg:   0 0 62px rgba(122,86,177,0.44)
--shadow-button:    0 18px 34px rgba(16,10,32,0.5)
```

---

## 4. Kiến Trúc Layout

### 4.1 Tổng Quan Layout Zones

```
┌─────────────────────────────────────────────────────┐
│                   NAVBAR (fixed top)                │
├───────────┬─────────────────────────────────────────┤
│           │                                         │
│  SIDEBAR  │           MAIN CONTENT                  │
│ (optional)│         (scrollable)                    │
│           │                                         │
│           │                                         │
├───────────┴─────────────────────────────────────────┤
│                 FOOTER (nếu có)                     │
└─────────────────────────────────────────────────────┘
```

### 4.2 Phân Loại Layout

| Zone | Layout | Mô tả |
|------|--------|--------|
| **Public** | Navbar + Content + Footer | Home, Readers, Legal pages |
| **Auth** | Centered (Không Navbar/Footer) | Login, Register, Forgot Password... |
| **User Dashboard** | Navbar + Sidebar + Content | Profile, Wallet, History, Chat, Reading |
| **Admin** | Admin Sidebar + Content (Không Navbar chính) | Toàn bộ trang `/admin/*` |

### 4.3 Đề Xuất Thêm User Sidebar

Hiện tại user pages chỉ dùng Navbar top → rời rạc. **Đề xuất thêm sidebar** cho khu vực người dùng:

```
┌──────────────────────────────────────────────────┐
│                  NAVBAR (top)                    │
├────────────┬─────────────────────────────────────┤
│            │                                     │
│  USER      │    MAIN CONTENT                     │
│  SIDEBAR   │                                     │
│            │                                     │
│  ┌──────┐  │                                     │
│  │ 🏠   │  │                                     │
│  │Dashboard│                                     │
│  │ 🃏   │  │                                     │
│  │Rút Bài │                                     │
│  │ 📚   │  │                                     │
│  │Miếu Bài│                                     │
│  │ 📜   │  │                                     │
│  │Lịch Sử │                                     │
│  │ 💬   │  │                                     │
│  │Chat    │                                     │
│  │ 💎   │  │                                     │
│  │Ví     │                                      │
│  │ 👤   │  │                                     │
│  │Hồ Sơ  │                                     │
│  └──────┘  │                                     │
├────────────┴─────────────────────────────────────┤
```

**Sidebar thu gọn trên mobile** → chuyển thành **Bottom Tab Bar** (giống app mobile).

---

## 5. Hệ Thống Điều Hướng (Navigation)

### 5.1 Navbar Redesign

```
┌─────────────────────────────────────────────────────────────────────┐
│  🌙 TarotNow AI    │  Home  Rút Bài  Readers            │ 🔔 💎 👤 │
│                     │                                     │          │
└─────────────────────────────────────────────────────────────────────┘
```

| Vị trí | Item | Mô tả | Luôn hiển thị? |
|--------|------|--------|---------------|
| **Trái** | Logo "TarotNow AI" | Link về Home | ✅ Luôn hiện |
| **Giữa** | Home | Trang chủ | ✅ Luôn hiện |
| | Rút Bài | Trang rút bài chính | ✅ Luôn hiện |
| | Readers | Danh sách reader | ✅ Luôn hiện |
| **Phải** | 🔔 Notifications | Badge đếm tin nhắn/escrow mới | Khi đăng nhập |
| | 💎 Wallet Widget | Hiển thị số dư nhanh | Khi đăng nhập |
| | 👤 Avatar Menu | Dropdown: Profile, Wallet, History, Chat, Admin (nếu admin), Logout | Khi đăng nhập |
| | Đăng nhập / Đăng ký | Buttons | Khi chưa đăng nhập |

### 5.2 User Sidebar Items

| Icon | Tên | Route | Badge |
|------|------|-------|-------|
| 🏠 | Tổng Quan | `/` (hoặc `/dashboard`) | — |
| 🃏 | Rút Bài Tarot | `/reading` | — |
| 📚 | Miếu Bài (Bộ Sưu Tập) | `/collection` | — |
| 📜 | Lịch Sử | `/reading/history` | — |
| 💬 | Tin Nhắn | `/chat` | 🔴 Số tin nhắn chưa đọc |
| 🔮 | Tìm Reader | `/readers` | — |
| 💎 | Ví | `/wallet` | — |
| 👤 | Hồ Sơ | `/profile` | — |

**Phân cách nhóm:**
- **Nhóm 1 — Tarot:** Rút Bài, Miếu Bài, Lịch Sử
- **Nhóm 2 — Xã Hội:** Tin Nhắn, Tìm Reader
- **Nhóm 3 — Tài Khoản:** Ví, Hồ Sơ

### 5.3 Admin Sidebar Items (Cập Nhật)

| Icon | Tên | Route | Hiện tại |
|------|------|-------|----------|
| 📊 | Tổng Quan | `/admin` | ✅ Có |
| 👥 | Người Dùng | `/admin/users` | ✅ Có |
| 💳 | Nạp Tiền | `/admin/deposits` | ✅ Có |
| 🎁 | Khuyến Mãi | `/admin/promotions` | ✅ Có |
| 🃏 | Xem Bài | `/admin/readings` | ✅ Có |
| 📝 | Đơn Xin Reader | `/admin/reader-requests` | ❌ **Thiếu** |
| 💸 | Rút Tiền | `/admin/withdrawals` | ❌ **Thiếu** |
| ⚖️ | Tranh Chấp | `/admin/disputes` | ❌ **Thiếu** |

### 5.4 Mobile Navigation

**Chiến lược:** Navbar → thu gọn thành hamburger icon. User Sidebar → chuyển thành **Bottom Tab Bar** 5 mục chính:

```
┌───────┬───────┬───────┬───────┬───────┐
│  🏠   │  🃏   │  💬   │  💎   │  👤   │
│ Home  │ Tarot │ Chat  │ Ví   │ Tôi   │
└───────┴───────┴───────┴───────┴───────┘
```

---

## 6. Chi Tiết Từng Module / Trang

### 6.1 Module Auth (Xác Thực)

**Trang:** Login, Register, Verify Email, Forgot Password, Reset Password

**Layout:** Full-screen centered, **không Navbar**, không Footer.

**Thiết kế:**
- Background: Gradient tím `from-indigo-950 via-purple-950 to-slate-950` + Nebula glow nhẹ
- Form card: Obsidian surface (`bg-[var(--bg-surface)] border border-[var(--border-default)] rounded-3xl`)
- Logo lớn ở trên form
- Tất cả dùng chung `AuthLayout` component

**Thêm mới:**
- Nút **"Gửi lại OTP"** ở trang Verify Email (gọi API `POST /auth/send-verification-email` — hiện thiếu)
- Animation chuyển trang mượt mà giữa Login ↔ Register

### 6.2 Module Home (Trang Chủ)

**Layout:** Navbar + Content + Footer

**Sections hiện có (giữ nguyên, cải thiện):**
1. **Hero Section** — "The Mystic Portal" (CTA: Rút Bài + Gặp Readers)
2. **Universe Stats** — Lượt trải bài, Reader, Rating, Support
3. **Reader Showcase** — 4 Reader nổi bật
4. **Core Features** — 3 tính năng chính (AI Insight, Personalized Path, Transparent Protocol)
5. **Final CTA** — "Bắt đầu hành trình Ánh Sáng & Sự Thật"
6. **Footer**

**Cải thiện:**
- Tách **Footer** thành component riêng
- Stats section → lấy dữ liệu thật từ backend (nếu có API)
- Thêm section **"Cách Hoạt Động"** (How It Works) — 3 bước: Chọn Trải Bài → Rút Bài → Nhận AI Insight

### 6.3 Module Reading (Rút Bài Tarot)

**Các trang:**

| Route | Mô tả | Thiết kế |
|-------|--------|----------|
| `/reading` | Chọn loại trải bài + câu hỏi | Grid 2×2 spread cards, textarea câu hỏi, CTA button |
| `/reading/session/[id]` | Phòng rút bài + AI Stream | Animation lật bài 3D, SSE stream typewriter |
| `/reading/history` | Danh sách lịch sử | List cards với filter (loại, ngày), phân trang |
| `/reading/history/[id]` | Chi tiết phiên | Hiển thị bài rút, AI interpretation, follow-up chat |

**Thiết kế chung:**
- Mỗi trang dùng `UserLayout` (Navbar + Sidebar)
- Spread type cards: Icon + Tên + Mô tả + Giá + Selected glow
- History items: Card compact với thumbnail bài, ngày, loại, mô tả ngắn

### 6.4 Module Collection (Miếu Bài)

**Trang:** `/collection`

**Thiết kế:**
- Grid bài Tarot (6 cột Desktop, 3 cột Mobile)
- Mỗi bài: Hình ảnh + Tên + Level/EXP bar
- Filter: Arcana Major/Minor, Rarity, Level
- Card detail modal: Zoom in + thông tin chi tiết

### 6.5 Module Wallet (Ví)

**Các trang:**

| Route | Mô tả |
|-------|--------|
| `/wallet` | Dashboard ví: Balance cards (Diamond + Gold) + Ledger |
| `/wallet/deposit` | Form nạp tiền: Chọn gói/nhập số tiền + QR code |
| `/wallet/withdraw` | Form rút tiền: Nhập số Diamond + thông tin ngân hàng + MFA |

**Thiết kế:**
- **Balance Cards:** 2 thẻ lớn (Diamond tím, Gold vàng trầm) — surface tối, icon glow nhẹ
- **Ledger Table:** Compact, sortable, filterable by currency/type
- **Deposit:** Hiển thị promotions đang active, form nhập số tiền, consent checkbox
- **Withdraw:** Guard checks (min 50D, KYC), form ngân hàng, MFA modal

### 6.6 Module Chat (Tin Nhắn)

**Các trang:**

| Route | Mô tả |
|-------|--------|
| `/chat` | Inbox — danh sách conversations |
| `/chat/[id]` | Phòng chat — tin nhắn realtime (SignalR) |

**Thiết kế:**
- **Inbox:** List conversations, mỗi item hiện: avatar, tên, tin nhắn cuối, unread badge, thời gian
- **Chat Room:** Layout 2 cột trên Desktop (sidebar conversations + main chat area), full screen trên Mobile
- **Escrow Panel:** Tích hợp inline trong chat — hiện trạng thái freeze/release/dispute
- **Report & Dispute:** Buttons trong header phòng chat

### 6.7 Module Readers (Danh Sách Reader)

**Các trang:**

| Route | Mô tả |
|-------|--------|
| `/readers` | Directory listing — grid/list readers |
| `/readers/[id]` | Hồ sơ Reader chi tiết |
| `/reader/apply` | Form đăng ký làm Reader |

**Thiết kế:**
- **Directory:** Grid cards (tương tự Reader Showcase trên Home nhưng full page), filter by specialty/status
- **Reader Profile:** Hero section với bio, specialties, rating, giá, nút Chat
- **Apply Form:** Surface card không blur, textarea intro, upload proof

### 6.8 Module Profile (Hồ Sơ)

**Các trang:**

| Route | Mô tả |
|-------|--------|
| `/profile` | Thông tin cá nhân: tên, avatar, ngày sinh |
| `/profile/mfa` | Quản lý MFA: setup QR, verify, status |
| `/profile/reader` | Hồ sơ Reader (cho user có role reader): bio, pricing, specialties, trạng thái |

**Thiết kế:**
- **Profile Dashboard:** Card cho info cá nhân + Card cho bảo mật + Card cho Reader status
- **MFA Page:** Step-by-step wizard: 1. Quét QR → 2. Nhập code → 3. Xác nhận

### 6.9 Module Admin

**Layout:** Admin Sidebar (dark surface) + Main Content, **KHÔNG dùng Navbar chính**

**Các trang:**

| Route | Mô tả | API |
|-------|--------|-----|
| `/admin` | Dashboard tổng quan: stats, biểu đồ | — |
| `/admin/users` | Quản lý users: search, list, lock/unlock, add balance | Admin Users APIs |
| `/admin/deposits` | Quản lý nạp tiền: list, approve/reject | Admin Deposits APIs |
| `/admin/promotions` | CRUD khuyến mãi | Promotions APIs |
| `/admin/readings` | Xem tất cả lịch sử bốc bài | History Admin API |
| `/admin/reader-requests` | Duyệt đơn xin Reader | Admin Reader Requests APIs |
| `/admin/withdrawals` | Duyệt yêu cầu rút tiền | Admin Withdrawals APIs |
| `/admin/disputes` | Giải quyết tranh chấp escrow | Admin Escrow Resolve API |

### 6.10 Module Legal (Pháp Lý)

**Các trang:** ToS, Privacy Policy, AI Disclaimer

**Layout:** Navbar + Content + Footer (Public)

**Thiết kế:**
- Prose typography: Max width ~prose, line height rộng
- Table of Contents sidebar cố định bên trái (Desktop)
- Surface card tối chứa nội dung văn bản

---

## 7. Thư Viện Component Dùng Chung

### 7.1 Component Mới Cần Tạo

#### 🎨 Layout Components

| Component | Mô tả | Tham số |
|-----------|--------|---------|
| `AstralBackground` | Nebula glow + Noise overlay + Particles | `variant: 'default' \| 'subtle' \| 'intense'` |
| `UserLayout` | Navbar + Sidebar + Content wrapper | `children` |
| `AuthLayout` | Centered card trên gradient background | `children, title, subtitle` |
| `AdminLayout` | *(đã có, cần cập nhật sidebar items)* | `children` |
| `Footer` | Footer chung, tách từ Home page | — |

#### 🧩 UI Primitives

| Component | Mô tả | Variants |
|-----------|--------|----------|
| `Button` | Nút bấm thống nhất | `primary, secondary, ghost, danger, glow` |
| `Input` | Input field + label + error | `text, email, password, textarea` |
| `GlassCard` | Card surface tối (legacy name) | `default, interactive (hover effect)` |
| `Badge` | Status/Label badge | `success, error, warning, info, purple, amber` |
| `Modal` | Dialog overlay | `size: sm, md, lg` |
| `Table` | Bảng dữ liệu dark surface | `columns, data, pagination` |
| `Pagination` | Phân trang nhất quán | `page, totalPages, onChange` |
| `EmptyState` | Trạng thái rỗng (icon + text + CTA) | `icon, message, actionLabel, onAction` |
| `LoadingSpinner` | Spinner thống nhất | `size: sm, md, lg` |
| `SkeletonLoader` | Placeholder loading | `type: card, row, text` |
| `SectionHeader` | Header section với tag + title + subtitle | `tag, title, subtitle` |
| `StatCard` | Thẻ thống kê nhỏ | `icon, value, label, color` |
| `AvatarMenu` | Avatar dropdown trong Navbar | `user, menuItems` |
| `NotificationBell` | Bell icon + badge count | `count` |

#### 📊 Data Components

| Component | Mô tả |
|-----------|--------|
| `TransactionRow` | Dòng giao dịch trong Ledger (icon + type + amount + date) |
| `ReaderCard` | Card reader trong directory |
| `HistoryCard` | Card lịch sử phiên bốc bài |
| `ConversationItem` | Item trong Inbox chat |
| `SpreadSelector` | Grid chọn loại trải bài |

### 7.2 Cấu Trúc Thư Mục Components

```
src/components/
├── layout/
│   ├── AstralBackground.tsx      ← Nebula + Particles + Noise
│   ├── UserLayout.tsx            ← Navbar + User Sidebar wrapper
│   ├── AuthLayout.tsx            ← Centered auth form wrapper
│   ├── Navbar.tsx                ← (refactor từ hiện tại)
│   ├── UserSidebar.tsx           ← Sidebar cho khu vực user
│   ├── Footer.tsx                ← Tách từ Home page
│   └── BottomTabBar.tsx          ← Mobile bottom navigation
│
├── ui/
│   ├── Button.tsx
│   ├── Input.tsx
│   ├── GlassCard.tsx
│   ├── Badge.tsx
│   ├── Modal.tsx
│   ├── Table.tsx
│   ├── Pagination.tsx
│   ├── EmptyState.tsx
│   ├── LoadingSpinner.tsx
│   ├── SkeletonLoader.tsx
│   ├── SectionHeader.tsx
│   ├── StatCard.tsx
│   ├── AvatarMenu.tsx
│   └── NotificationBell.tsx
│
├── data/
│   ├── TransactionRow.tsx
│   ├── ReaderCard.tsx
│   ├── HistoryCard.tsx
│   ├── ConversationItem.tsx
│   └── SpreadSelector.tsx
│
├── auth/
│   └── MfaChallengeModal.tsx     ← (giữ nguyên)
│
├── chat/
│   ├── DisputeButton.tsx         ← (giữ nguyên)
│   ├── EscrowPanel.tsx           ← (giữ nguyên)
│   └── ReportModal.tsx           ← (giữ nguyên)
│
└── reading/
    └── AiInterpretationStream.tsx ← (giữ nguyên, di chuyển vào thư mục)
```

---

## 8. Responsive & Mobile Strategy

### 8.1 Breakpoints

| Breakpoint | Width | Target |
|------------|-------|--------|
| `sm` | ≥ 640px | Mobile landscape |
| `md` | ≥ 768px | Tablet |
| `lg` | ≥ 1024px | Desktop nhỏ |
| `xl` | ≥ 1280px | Desktop |
| `2xl` | ≥ 1536px | Desktop lớn |

### 8.2 Adaptive Behavior

| Element | Mobile (< md) | Desktop (≥ md) |
|---------|---------------|----------------|
| **Navbar** | Logo + Hamburger + Avatar | Full navigation links |
| **User Sidebar** | Ẩn → Bottom Tab Bar | Hiển thị bên trái (w-64) |
| **Admin Sidebar** | Overlay drawer (hamburger) | Cố định bên trái (w-72) |
| **Grid layouts** | 1 cột | 2–4 cột |
| **Tables** | Card view / horizontal scroll | Full table |
| **Balance Cards** | Stack 1 cột | 2 cột ngang |
| **Reader Grid** | 1–2 cột | 4 cột |
| **Chat** | Full screen | 2-panel (inbox + chat) |

---

## 9. Lộ Trình Triển Khai

### Phase 1 — Foundation (Nền Tảng) 🏗️

| # | Task | Ưu tiên | Ảnh hưởng |
|---|------|---------|-----------|
| 1.1 | Thống nhất `globals.css` (1 file duy nhất, design tokens) | 🔴 Cao | Toàn bộ app |
| 1.2 | Tạo `AstralBackground` component (thay thế copy-paste ở mọi trang) | 🔴 Cao | Toàn bộ app |
| 1.3 | Tạo UI Primitives: `Button`, `Input`, `GlassCard`, `Badge`, `Modal` | 🔴 Cao | Toàn bộ app |
| 1.4 | Tạo `LoadingSpinner`, `SkeletonLoader`, `EmptyState` | 🟡 TB | Toàn bộ app |

### Phase 2 — Layout System 📐

| # | Task | Ưu tiên | Ảnh hưởng |
|---|------|---------|-----------|
| 2.1 | Tạo `Footer` component, tích hợp vào `layout.tsx` | 🟡 TB | Public pages |
| 2.2 | Tạo `UserSidebar` + `UserLayout` | 🟡 TB | Toàn bộ user pages |
| 2.3 | Refactor `Navbar`: thêm Notification Bell, Avatar Menu, Readers link | 🔴 Cao | Toàn bộ app |
| 2.4 | Cập nhật Admin Sidebar: thêm 3 mục thiếu | 🔴 Cao | Admin pages |
| 2.5 | Tạo `AuthLayout` component | 🟡 TB | Auth pages |
| 2.6 | Tạo `BottomTabBar` cho mobile | 🟢 Thấp | Mobile UX |

### Phase 3 — Page Refactor (Tái Cấu Trúc Trang) 🔄

| # | Task | Ưu tiên |
|---|------|---------|
| 3.1 | Refactor Home page: dùng `AstralBackground`, `Footer`, components | 🟡 TB |
| 3.2 | Refactor Auth pages: dùng `AuthLayout`, `Button`, `Input` | 🟡 TB |
| 3.3 | Refactor Reading pages: dùng `UserLayout`, `SpreadSelector`, `HistoryCard` | 🟡 TB |
| 3.4 | Refactor Wallet pages: dùng `UserLayout`, `GlassCard`, `Table`, `Pagination` | 🟡 TB |
| 3.5 | Refactor Chat pages: dùng `UserLayout`, `ConversationItem` | 🟡 TB |
| 3.6 | Refactor Profile pages: dùng `UserLayout`, `GlassCard` | 🟡 TB |
| 3.7 | Refactor Readers pages: dùng `ReaderCard`, `SectionHeader` | 🟡 TB |
| 3.8 | Refactor Admin pages: dùng updated `AdminLayout`, `Table`, `Pagination` | 🟡 TB |

### Phase 4 — Polish & Enhancement ✨

| # | Task | Ưu tiên |
|---|------|---------|
| 4.1 | Tích hợp `POST /auth/refresh` (auto-refresh token) | 🔴 Cao |
| 4.2 | Tích hợp `POST /auth/send-verification-email` | 🟡 TB |
| 4.3 | Page transition animations | 🟢 Thấp |
| 4.4 | Notification system (realtime unread count) | 🟡 TB |
| 4.5 | SEO optimization (meta tags, OG images cho mỗi trang) | 🟡 TB |
| 4.6 | Dark/Light mode toggle (nếu cần) | 🟢 Thấp |
| 4.7 | Accessibility audit (ARIA labels, keyboard navigation) | 🟡 TB |

---

## 📌 Tóm Tắt Hành Động Ưu Tiên

> **Cần làm NGAY (theo thứ tự):**
> 
> 1. ✅ Tạo `AstralBackground` component → loại bỏ 15+ đoạn code lặp
> 2. ✅ Thống nhất `globals.css` + design tokens
> 3. ✅ Tạo bộ UI Primitives (Button, Input, GlassCard, Badge, Modal)
> 4. ✅ Refactor Navbar (thêm Readers, Notifications, Avatar Menu)
> 5. ✅ Cập nhật Admin Sidebar (thêm 3 mục thiếu)
> 6. ✅ Áp dụng component mới vào từng trang (refactor dần)

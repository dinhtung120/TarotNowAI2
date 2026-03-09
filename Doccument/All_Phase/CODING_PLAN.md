# TarotWeb – Kế hoạch Chi tiết Phase Coding (Index)

**Phiên bản:** 4.0 (2026-03-08)  
**Nguồn chính:** `All_Phase/tai-lieu-thiet-ke/`, `database/`

---

## Phase Index

| Phase | Sub-Phase | Folder | Mô tả | Tasks |
|---|---|---|---|---:|
| **0** | 0.1 | `Phase_0/Phase_0.1_Repo_CICD/` | Cấu trúc thư mục, Config, Secrets, CI build/test/cache | 7 |
| | 0.2 | `Phase_0/Phase_0.2_Database/` | PostgreSQL schema, MongoDB init, Seed data | 5 |
| | 0.3 | `Phase_0/Phase_0.3_API_Scaffold/` | Base API, Swagger, ProblemDetails, Auth Cookie | 3 |
| | 0.4 | `Phase_0/Phase_0.4_Web_QA/` | Next.js scaffold, i18n, xUnit, Playwright | 4 |
| **1** | 1.1 | `Phase_1/Phase_1.1_Auth/` | Register, Login, JWT, OTP, Password Reset, Consent | 18 |
| | 1.2 | `Phase_1/Phase_1.2_Wallet/` | Balance, Ledger, Guard proc_wallet_*, Reconciliation | 11 |
| | 1.3 | `Phase_1/Phase_1.3_Reading/` | RNG, Daily 1, Spread 3/5/10, Card Collection, EXP | 18 |
| | 1.4 | `Phase_1/Phase_1.4_AI_Streaming/` | AI Provider, SSE, State Machine, Guards, Safety, Refund | 20 |
| | 1.5 | `Phase_1/Phase_1.5_Followup_History/` | Free slots, Paid tiers, Hard cap 5, Reading history | 14 |
| | 1.6 | `Phase_1/Phase_1.6_Legal_Deposit_Admin/` | Legal pages, Consent, Profile, Deposit, Promotions, Admin | 29 |
| **2** | 2.1 | `Phase_2/Phase_2.1_Reader/` | Reader listing, Approval, Directory | 10 |
| | 2.2 | `Phase_2/Phase_2.2_Chat/` | SignalR Hub, Messages, Read state, Report | 11 |
| | 2.3 | `Phase_2/Phase_2.3_Escrow/` | Freeze/Release/Refund, Timers, Dispute, Idempotency | 15 |
| | 2.4 | `Phase_2/Phase_2.4_Withdrawal/` | Withdrawal request, Guard, Fee, Admin payout | 7 |
| | 2.5 | `Phase_2/Phase_2.5_MFA/` | TOTP setup, Verify, Enforcement gates | 7 |
| **3** | 3.1 | `Phase_3/Phase_3.1_Mobile_Auth/` | Expo scaffold, Secure storage, Auth screens | 5 |
| | 3.2 | `Phase_3/Phase_3.2_Wallet_Reading/` | Wallet + Reading + SSE mobile parity | 4 |
| | 3.3 | `Phase_3/Phase_3.3_Chat/` | SignalR mobile, Background/foreground lifecycle | 3 |
| | 3.4 | `Phase_3/Phase_3.4_Push_Deeplink/` | FCM/APNs, Deep-link routing | 3 |
| **4** | 4.1 | `Phase_4/Phase_4.1_Community/` | Posts, Reactions, Moderation | 9 |
| | 4.2 | `Phase_4/Phase_4.2_Call/` | Call metadata, Status transitions, History | 5 |
| **5** | 5.1 | `Phase_5/Phase_5.1_Checkin_Streak/` | Daily check-in, Streak, Freeze | 10 |
| | 5.2 | `Phase_5/Phase_5.2_Subscription/` | Subscription, Entitlement, Cross-key mapping | 9 |
| | 5.3 | `Phase_5/Phase_5.3_Gamification/` | Quests, Achievements, Titles, Leaderboards | 11 |
| | 5.4 | `Phase_5/Phase_5.4_Hardening/` | Rate Limiting, Observability, Background jobs | 8 |
| | 5.5 | `Phase_5/Phase_5.5_Share_Referral/` | Share reward, Referral, Notifications | 11 |
| | 5.6 | `Phase_5/Phase_5.6_Gacha/` | Gacha spin, Odds, Pity timer | 7 |
| | 5.7 | `Phase_5/Phase_5.7_GDPR/` | Data rights: export, correction, deletion | 5 |
| | 5.8 | `Phase_5/Phase_5.8_Geo/` | Geo feature gating, VPN detection | 5 |
| | 5.9 | `Phase_5/Phase_5.9_FriendChain_Events/` | Friend chain, Event packs | 8 |
| | 5.10 | `Phase_5/Phase_5.10_Card_Stories/` | Card ascension stories | 4 |

---

## Mỗi sub-phase folder chứa

| File | Nội dung |
|---|---|
| `CODING_PLAN.md` | Task list chi tiết với checklist `[ ]` |
| `TEST.md` | Verification checklist với lệnh cụ thể |

---

## Tài liệu thiết kế

- `All_Phase/tai-lieu-thiet-ke/` – 5 tài liệu thiết kế gốc
- `Phase_X/thiet-ke-phase-X.md` – Trích phần thiết kế liên quan từng phase

---

## Quy ước chung

- **PD** = person-days (1 người × 1 ngày ≈ 8h focus)
- **Spec mapping:** `BR` = 01-business-rules, `UX` = 02-product-ux-specs, `ARCH` = 03-tech-architecture, `OPS` = 04-ops-security-compliance, `DB` = database/
- **Tuyệt đối không UPDATE balance rời rạc**: mọi credit/debit/freeze/release/refund phải đi qua `proc_wallet_*`
- **AI state machine LOCKED**: không thay đổi trạng thái mà chưa update spec
- **Idempotency**: mọi mutation tài chính bắt buộc idempotent

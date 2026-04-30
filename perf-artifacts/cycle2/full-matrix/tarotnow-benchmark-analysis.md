# TarotNow Benchmark Analysis

- Run time (UTC): 2026-04-29T20:56:04.704Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 27.4 | 2935 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 38.4 | 3275 | 2 | 0 | 0 | 11 | 4 | yes |
| logged-in-reader | desktop | 38 | 37.5 | 3487 | 14 | 0 | 0 | 12 | 6 | yes |
| logged-out | mobile | 9 | 27.4 | 2946 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 37.2 | 3439 | 0 | 0 | 0 | 9 | 5 | yes |
| logged-in-reader | mobile | 38 | 35.9 | 3703 | 0 | 0 | 0 | 8 | 5 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 35.3 | 3059 | 0 |
| logged-in-admin | desktop | auth | 5 | 38.0 | 3852 | 0 |
| logged-in-admin | desktop | chat | 1 | 38.0 | 3269 | 0 |
| logged-in-admin | desktop | collection | 1 | 105.0 | 4953 | 0 |
| logged-in-admin | desktop | community | 1 | 43.0 | 5185 | 0 |
| logged-in-admin | desktop | gacha | 2 | 38.0 | 3214 | 0 |
| logged-in-admin | desktop | gamification | 1 | 35.0 | 3794 | 0 |
| logged-in-admin | desktop | home | 1 | 37.0 | 3733 | 0 |
| logged-in-admin | desktop | inventory | 1 | 40.0 | 3029 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 39.0 | 3934 | 0 |
| logged-in-admin | desktop | legal | 3 | 31.0 | 2900 | 0 |
| logged-in-admin | desktop | notifications | 1 | 39.0 | 2902 | 0 |
| logged-in-admin | desktop | profile | 3 | 45.0 | 2966 | 0 |
| logged-in-admin | desktop | reader | 1 | 34.0 | 2917 | 0 |
| logged-in-admin | desktop | readers | 7 | 33.4 | 3081 | 0 |
| logged-in-admin | desktop | reading | 5 | 36.4 | 3141 | 0 |
| logged-in-admin | desktop | wallet | 4 | 42.0 | 3085 | 2 |
| logged-in-admin | mobile | admin | 10 | 35.4 | 3639 | 0 |
| logged-in-admin | mobile | auth | 5 | 38.0 | 3922 | 0 |
| logged-in-admin | mobile | chat | 1 | 38.0 | 3710 | 0 |
| logged-in-admin | mobile | collection | 1 | 53.0 | 3643 | 0 |
| logged-in-admin | mobile | community | 1 | 43.0 | 4457 | 0 |
| logged-in-admin | mobile | gacha | 2 | 37.5 | 3217 | 0 |
| logged-in-admin | mobile | gamification | 1 | 35.0 | 3169 | 0 |
| logged-in-admin | mobile | home | 1 | 37.0 | 3425 | 0 |
| logged-in-admin | mobile | inventory | 1 | 40.0 | 3160 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 38.0 | 3561 | 0 |
| logged-in-admin | mobile | legal | 3 | 31.0 | 2959 | 0 |
| logged-in-admin | mobile | notifications | 1 | 39.0 | 3067 | 0 |
| logged-in-admin | mobile | profile | 3 | 45.0 | 3061 | 0 |
| logged-in-admin | mobile | reader | 1 | 34.0 | 3036 | 0 |
| logged-in-admin | mobile | readers | 7 | 33.4 | 3621 | 0 |
| logged-in-admin | mobile | reading | 5 | 35.8 | 3096 | 0 |
| logged-in-admin | mobile | wallet | 4 | 41.8 | 3136 | 0 |
| logged-in-reader | desktop | auth | 5 | 38.0 | 4092 | 0 |
| logged-in-reader | desktop | chat | 1 | 38.0 | 3017 | 0 |
| logged-in-reader | desktop | collection | 1 | 105.0 | 4387 | 14 |
| logged-in-reader | desktop | community | 1 | 43.0 | 4271 | 0 |
| logged-in-reader | desktop | gacha | 2 | 38.0 | 3020 | 0 |
| logged-in-reader | desktop | gamification | 1 | 35.0 | 3442 | 0 |
| logged-in-reader | desktop | home | 1 | 37.0 | 5741 | 0 |
| logged-in-reader | desktop | inventory | 1 | 40.0 | 3089 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 37.0 | 3034 | 0 |
| logged-in-reader | desktop | legal | 3 | 31.0 | 2941 | 0 |
| logged-in-reader | desktop | notifications | 1 | 39.0 | 3184 | 0 |
| logged-in-reader | desktop | profile | 3 | 35.3 | 3241 | 0 |
| logged-in-reader | desktop | reader | 1 | 34.0 | 3281 | 0 |
| logged-in-reader | desktop | readers | 7 | 33.6 | 3525 | 0 |
| logged-in-reader | desktop | reading | 5 | 36.6 | 3381 | 0 |
| logged-in-reader | desktop | wallet | 4 | 33.5 | 3110 | 0 |
| logged-in-reader | mobile | auth | 5 | 38.0 | 3731 | 0 |
| logged-in-reader | mobile | chat | 1 | 38.0 | 3174 | 0 |
| logged-in-reader | mobile | collection | 1 | 53.0 | 4079 | 0 |
| logged-in-reader | mobile | community | 1 | 43.0 | 4545 | 0 |
| logged-in-reader | mobile | gacha | 2 | 37.5 | 3260 | 0 |
| logged-in-reader | mobile | gamification | 1 | 35.0 | 4690 | 0 |
| logged-in-reader | mobile | home | 1 | 37.0 | 3810 | 0 |
| logged-in-reader | mobile | inventory | 1 | 39.0 | 3687 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 37.0 | 3465 | 0 |
| logged-in-reader | mobile | legal | 3 | 31.0 | 3103 | 0 |
| logged-in-reader | mobile | notifications | 1 | 40.0 | 3869 | 0 |
| logged-in-reader | mobile | profile | 3 | 35.3 | 3199 | 0 |
| logged-in-reader | mobile | reader | 1 | 34.0 | 3502 | 0 |
| logged-in-reader | mobile | readers | 7 | 33.6 | 3742 | 0 |
| logged-in-reader | mobile | reading | 5 | 35.4 | 3860 | 0 |
| logged-in-reader | mobile | wallet | 4 | 33.5 | 4081 | 0 |
| logged-out | desktop | auth | 5 | 25.2 | 2860 | 0 |
| logged-out | desktop | home | 1 | 34.0 | 3669 | 0 |
| logged-out | desktop | legal | 3 | 29.0 | 2817 | 0 |
| logged-out | mobile | auth | 5 | 25.2 | 2857 | 0 |
| logged-out | mobile | home | 1 | 34.0 | 3310 | 0 |
| logged-out | mobile | legal | 3 | 29.0 | 2973 | 0 |

## Key Findings
1. Critical: 77 page(s) vượt 35 requests.
2. High: 182 page(s) vượt 25 requests.
3. High: 512 request vượt 800ms.
4. Medium: 1336 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.

## Top Slow Pages
| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | /vi | 5741 | 37 | 2032 | 1091.0 | 0.0022 |
| logged-in-reader | desktop | /vi/readers/69dbe86b052d3c8f3f55e231 | 5248 | 33 | 1284 | 18.0 | 0.0026 |
| logged-in-admin | desktop | /vi/community | 5185 | 43 | 2868 | 120.0 | 0.0029 |
| logged-in-admin | desktop | /vi/collection | 4953 | 105 | 2232 | 551.0 | 0.0029 |
| logged-in-admin | mobile | /vi/admin/deposits | 4912 | 35 | 1472 | 26.0 | 0.0000 |
| logged-in-admin | mobile | /vi/reset-password | 4724 | 38 | 1624 | 57.0 | 0.0031 |
| logged-in-admin | mobile | /vi/admin/disputes | 4708 | 36 | 1108 | 4.0 | 0.0000 |
| logged-in-reader | mobile | /vi/gamification | 4690 | 35 | 1448 | 40.0 | 0.0029 |
| logged-in-reader | mobile | /vi/reading/session/26cdad91-d22f-4381-a94d-e8719e43cffe | 4605 | 37 | 4500 | 72.0 | 0.0030 |
| logged-in-reader | mobile | /vi/community | 4545 | 43 | 2124 | 13.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2934 | 171 | https://img.tarotnow.xyz/light-god-50/42_Six_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2884 | 136 | https://img.tarotnow.xyz/light-god-50/41_Five_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2824 | 135 | https://img.tarotnow.xyz/light-god-50/40_Four_of_Pentacles_50_20260325_181413.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2796 | 128 | https://img.tarotnow.xyz/light-god-50/39_Three_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2751 | 130 | https://img.tarotnow.xyz/light-god-50/37_Ace_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2749 | 146 | https://img.tarotnow.xyz/light-god-50/38_Two_of_Pentacles_50_20260325_181411.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2701 | 131 | https://img.tarotnow.xyz/light-god-50/64_King_of_Swords_50_20260325_181428.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2618 | 112 | https://img.tarotnow.xyz/light-god-50/63_Queen_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2572 | 109 | https://img.tarotnow.xyz/light-god-50/62_Knight_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2551 | 113 | https://img.tarotnow.xyz/light-god-50/61_Page_of_Swords_50_20260325_181427.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2536 | 136 | https://img.tarotnow.xyz/light-god-50/59_Nine_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2535 | 151 | https://img.tarotnow.xyz/light-god-50/58_Eight_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2534 | 128 | https://img.tarotnow.xyz/light-god-50/57_Seven_of_Swords_50_20260325_181424.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2534 | 126 | https://img.tarotnow.xyz/light-god-50/60_Ten_of_Swords_50_20260325_181426.avif |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 2507 | 165 | https://img.tarotnow.xyz/light-god-50/56_Six_of_Swords_50_20260325_181424.avif |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | /vi/forgot-password | GET | 200 | 800 | 400 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/forgot-password | GET | 200 | 800 | 306 | https://www.tarotnow.xyz/api/wallet/balance |
| logged-in-reader | desktop | /vi/reading/session/d63a9d0a-2fd1-4b06-8f4b-e1d4b3960d8a | GET | 200 | 800 | 68 | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/wallet/deposit/history | GET | 200 | 800 | 153 | https://www.tarotnow.xyz/_next/static/chunks/08q.cp_si1m5q.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 800 | 77 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/community | GET | 200 | 799 | 150 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 799 | 70 | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 799 | 69 | https://www.tarotnow.xyz/_next/static/chunks/167n5~xbg2bxe.js |
| logged-in-reader | desktop | /vi/wallet | GET | 200 | 798 | 422 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 798 | 317 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 798 | 69 | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | desktop | /vi/wallet/deposit | GET | 200 | 797 | 426 | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | /vi/notifications | GET | 200 | 797 | 64 | https://www.tarotnow.xyz/_next/static/chunks/0nrdqsqvy_hhn.js |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 796 | 73 | https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 796 | 74 | https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0zl0xwyc461q_.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0.npsys3-p5.f.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | desktop | /vi/profile/reader | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
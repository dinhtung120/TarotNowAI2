# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T16:39:11.304Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 3824 | 1 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 30.6 | 3572 | 20 | 0 | 0 | 16 | 0 | yes |
| logged-in-reader | desktop | 38 | 30.4 | 3411 | 1 | 0 | 0 | 17 | 0 | yes |
| logged-out | mobile | 9 | 24.9 | 3356 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 30.2 | 3572 | 8 | 0 | 0 | 17 | 0 | yes |
| logged-in-reader | mobile | 38 | 31.0 | 3398 | 1 | 0 | 0 | 16 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 3586 | 0 |
| logged-in-admin | desktop | auth | 5 | 37.0 | 4031 | 3 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 3058 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6655 | 1 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4786 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 3734 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3228 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3267 | 0 |
| logged-in-admin | desktop | inventory | 1 | 39.0 | 4377 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3168 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.7 | 2985 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 5977 | 0 |
| logged-in-admin | desktop | profile | 3 | 30.0 | 3680 | 0 |
| logged-in-admin | desktop | reader | 1 | 33.0 | 3466 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 3221 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 3070 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.0 | 3023 | 16 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3155 | 0 |
| logged-in-admin | mobile | auth | 5 | 37.0 | 5362 | 6 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-admin | mobile | collection | 1 | 31.0 | 5958 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3454 | 0 |
| logged-in-admin | mobile | gacha | 2 | 35.5 | 3701 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 3052 | 0 |
| logged-in-admin | mobile | home | 1 | 29.0 | 3401 | 0 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 3461 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3413 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 3078 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2926 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 3089 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2885 | 0 |
| logged-in-admin | mobile | readers | 7 | 29.9 | 3790 | 2 |
| logged-in-admin | mobile | reading | 5 | 28.8 | 3268 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.8 | 3229 | 0 |
| logged-in-reader | desktop | auth | 5 | 35.0 | 3511 | 1 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2803 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6501 | 0 |
| logged-in-reader | desktop | community | 1 | 34.0 | 4045 | 0 |
| logged-in-reader | desktop | gacha | 2 | 34.5 | 3553 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2864 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3066 | 0 |
| logged-in-reader | desktop | inventory | 1 | 37.0 | 3624 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 5454 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.7 | 3241 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2851 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 3168 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2828 | 0 |
| logged-in-reader | desktop | readers | 7 | 29.1 | 3533 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.8 | 2958 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.8 | 3046 | 0 |
| logged-in-reader | mobile | auth | 5 | 40.8 | 3680 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2698 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5774 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3611 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 3557 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 3772 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2707 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 3082 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2880 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.7 | 3423 | 0 |
| logged-in-reader | mobile | notifications | 1 | 33.0 | 4542 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.7 | 3144 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2927 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 3169 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 3131 | 1 |
| logged-in-reader | mobile | wallet | 4 | 28.8 | 3517 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 3761 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4168 | 1 |
| logged-out | desktop | legal | 3 | 25.0 | 3813 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 3574 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3248 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3027 | 0 |

## Key Findings
1. Critical: 18 page(s) vượt 35 requests.
2. High: 154 page(s) vượt 25 requests.
3. High: 203 request vượt 800ms.
4. Medium: 448 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | mobile | auth-public | /vi/login | 11637 | 53 | 3164 | 0.0 | 0.0439 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6655 | 29 | 608 | 15.0 | 0.0043 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6501 | 29 | 636 | 0.0 | 0.0040 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 5977 | 33 | 1092 | 0.0 | 0.0042 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5958 | 31 | 1052 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5774 | 29 | 552 | 0.0 | 0.0000 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 5666 | 32 | 1004 | 0.0 | 0.0000 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 5454 | 30 | 1308 | 0.0 | 0.0177 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 5171 | 25 | 736 | 0.0 | 0.0000 |
| logged-out | desktop | auth-public | /vi/register | 5162 | 24 | 632 | 0.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5745 | 4751 | https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5730 | 4708 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5662 | 5209 | https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5602 | 2300 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5531 | 1559 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5374 | 5210 | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5374 | 5214 | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5336 | 5214 | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5323 | 4389 | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5313 | 5214 | https://www.tarotnow.xyz/_next/static/chunks/0_yn3f1mymbbi.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5295 | 4395 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5244 | 4387 | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5242 | 2317 | https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5233 | 5214 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | auth-public | /vi/login | GET | 200 | 5193 | 4361 | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 800 | 338 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 800 | 427 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 799 | 548 | https://www.tarotnow.xyz/_next/static/chunks/0h8lshv9j35pw.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 798 | 312 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 797 | 322 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 796 | 405 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading/session/4d68762d-32b0-4770-9e25-bfc9d9cb7e62 | GET | 200 | 796 | 770 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 795 | 311 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 795 | 527 | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 794 | 187 | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 794 | 323 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 793 | 500 | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 793 | 345 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 792 | 320 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 791 | 745 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0y-5.-cr3v3~z.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | auth-public | /vi/login | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/064xhtgi5eo9n.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | 0 | 0 | 0 | 0 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
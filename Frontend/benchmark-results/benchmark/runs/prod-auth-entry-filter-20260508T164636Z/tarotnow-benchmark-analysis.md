# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T16:58:30.517Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.0 | 3346 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.7 | 3287 | 2 | 0 | 0 | 17 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.3 | 3324 | 3 | 0 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 3262 | 0 | 0 | 0 | 0 | 1 | yes |
| logged-in-admin | mobile | 45 | 30.8 | 3156 | 5 | 0 | 1 | 12 | 0 | yes |
| logged-in-reader | mobile | 35 | 31.0 | 3278 | 1 | 0 | 1 | 19 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.8 | 3414 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2789 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6693 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4085 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 3622 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3764 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3459 | 0 |
| logged-in-admin | desktop | inventory | 1 | 37.0 | 3179 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3213 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2939 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2693 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.3 | 3411 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2716 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.7 | 2937 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 3205 | 0 |
| logged-in-admin | desktop | wallet | 4 | 29.3 | 2931 | 2 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3066 | 0 |
| logged-in-admin | mobile | auth | 2 | 51.5 | 3570 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2733 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5635 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3457 | 0 |
| logged-in-admin | mobile | gacha | 2 | 34.5 | 3210 | 0 |
| logged-in-admin | mobile | gamification | 1 | 58.0 | 4867 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2733 | 0 |
| logged-in-admin | mobile | inventory | 1 | 38.0 | 3046 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2707 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2974 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 3368 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 2888 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2803 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2953 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3213 | 0 |
| logged-in-admin | mobile | wallet | 4 | 27.8 | 3037 | 5 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2762 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6847 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3430 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.5 | 3341 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3105 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2698 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3244 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2792 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 3875 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2945 | 0 |
| logged-in-reader | desktop | profile | 3 | 31.3 | 3353 | 1 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2935 | 0 |
| logged-in-reader | desktop | readers | 7 | 29.1 | 3067 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 3061 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 3445 | 2 |
| logged-in-reader | mobile | auth | 2 | 37.0 | 3332 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2767 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5654 | 0 |
| logged-in-reader | mobile | community | 1 | 37.0 | 3842 | 0 |
| logged-in-reader | mobile | gacha | 2 | 37.0 | 3376 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2921 | 1 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2720 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 3207 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 35.0 | 4361 | 0 |
| logged-in-reader | mobile | legal | 3 | 26.3 | 2970 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2791 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.0 | 2957 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 3007 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 3518 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 2923 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.5 | 3258 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 3400 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3971 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 3047 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 3075 | 0 |
| logged-out | mobile | home | 1 | 31.0 | 3386 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3532 | 0 |

## Key Findings
1. Critical: 13 page(s) vượt 35 requests.
2. High: 147 page(s) vượt 25 requests.
3. High: 113 request vượt 800ms.
4. Medium: 348 request trong dải 400-800ms.
5. High: phát hiện 2 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6847 | 29 | 960 | 0.0 | 0.0040 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 6705 | 34 | 980 | 0.0 | 0.0000 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6693 | 29 | 592 | 0.0 | 0.0043 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5654 | 29 | 564 | 0.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5635 | 29 | 572 | 0.0 | 0.0000 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 4903 | 31 | 1000 | 0.0 | 0.0000 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 4867 | 58 | 1276 | 0.0 | 0.0000 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 4741 | 28 | 940 | 0.0 | 0.0039 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 4612 | 25 | 936 | 0.0 | 0.0019 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 4480 | 31 | 1076 | 0.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 4281 | 3858 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 2519 | 512 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 2367 | 2105 | https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | GET | 200 | 2223 | 2077 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 2092 | 1467 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reading | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | GET | 200 | 2026 | 673 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 2023 | 569 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 2004 | 1041 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1832 | 1816 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1806 | 1256 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1735 | 807 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1700 | 1005 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 1690 | 1633 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 1532 | 209 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1432 | 1400 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 798 | 318 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 797 | 714 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 797 | 323 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 796 | 550 | https://www.tarotnow.xyz/_next/static/chunks/0xy7p7~l6scxr.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 795 | 399 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 794 | 783 | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 792 | 321 | https://www.tarotnow.xyz/vi/gacha |
| logged-out | desktop | auth-public | /vi | GET | 200 | 791 | 261 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | reading | /vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 | GET | 200 | 790 | 329 | https://www.tarotnow.xyz/vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 786 | 658 | https://www.tarotnow.xyz/_next/static/chunks/0jt0d84fr1c7m.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 785 | 767 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 784 | 761 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 783 | 534 | https://www.tarotnow.xyz/_next/static/chunks/0jt0d84fr1c7m.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 783 | 769 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 783 | 324 | https://www.tarotnow.xyz/vi/gacha |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |

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
# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T20:10:48.971Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2877 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.9 | 3032 | 0 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.9 | 3124 | 0 | 0 | 0 | 18 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 2963 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.2 | 3416 | 1 | 0 | 0 | 15 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 3532 | 2 | 0 | 0 | 13 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2892 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2904 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6705 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4070 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2939 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3424 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2894 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2917 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3207 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.7 | 2934 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2914 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.7 | 2967 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2836 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2827 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.4 | 2871 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2961 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 3333 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3384 | 0 |
| logged-in-admin | mobile | collection | 1 | 30.0 | 7437 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 5267 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.0 | 2933 | 0 |
| logged-in-admin | mobile | gamification | 1 | 31.0 | 3181 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2717 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2897 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3991 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2823 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2783 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 3633 | 1 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2834 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.1 | 3221 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.8 | 3993 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.0 | 2829 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2763 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6860 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3575 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.5 | 2921 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2916 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2751 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2924 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 32.0 | 2917 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2792 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2917 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 3176 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2777 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.4 | 3169 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 3068 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2858 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 3824 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 7411 | 0 |
| logged-in-reader | mobile | community | 1 | 34.0 | 4031 | 1 |
| logged-in-reader | mobile | gacha | 2 | 30.5 | 2937 | 0 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 2981 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 3048 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2933 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 32.0 | 4212 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2894 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2926 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 4149 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2857 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 3963 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 3230 | 1 |
| logged-in-reader | mobile | wallet | 4 | 28.3 | 2857 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2745 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3932 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2746 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2896 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3956 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2743 | 0 |

## Key Findings
1. Không có page nào vượt 35 requests.
2. High: 143 page(s) vượt 25 requests.
3. High: 274 request vượt 800ms.
4. Medium: 699 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 9 image request(s) >800ms trên 39 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 7437 | 30 | 876 | 305.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 7411 | 31 | 832 | 17.0 | 0.0000 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6860 | 29 | 552 | 0.0 | 0.0040 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6705 | 29 | 932 | 0.0 | 0.0042 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 5267 | 35 | 2436 | 1.0 | 0.0051 |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | 5190 | 35 | 3796 | 0.0 | 0.0001 |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | 4797 | 33 | 4008 | 0.0 | 0.0001 |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | 4596 | 33 | 3656 | 0.0 | 0.0072 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 4562 | 28 | 1400 | 0.0 | 0.0000 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 4328 | 28 | 1368 | 0.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1568 | 310 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1394 | 64 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1372 | 319 | https://www.tarotnow.xyz/vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1368 | 317 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1365 | 104 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1365 | 74 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1332 | 102 | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1312 | 113 | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1307 | 78 | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1306 | 108 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1283 | 78 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1272 | 105 | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1267 | 78 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1253 | 72 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1240 | 83 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 800 | 75 | https://www.tarotnow.xyz/_next/static/chunks/0b5a588g_0r8q.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 799 | 327 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 798 | 327 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 798 | 336 | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 798 | 319 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 797 | 85 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 796 | 318 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 796 | 323 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 796 | 309 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 795 | 73 | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 793 | 59 | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 793 | 72 | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 792 | 320 | https://www.tarotnow.xyz/vi/gacha |
| logged-out | mobile | auth-public | /vi | GET | 200 | 792 | 728 | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 791 | 299 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 14 | 4 | 5 | 13 | 1 | 0 |
| logged-in-reader | desktop | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 2 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
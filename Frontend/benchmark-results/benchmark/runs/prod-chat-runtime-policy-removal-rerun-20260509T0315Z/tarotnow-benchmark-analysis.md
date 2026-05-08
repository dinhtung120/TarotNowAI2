# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T20:21:39.520Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.6 | 2782 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.7 | 2924 | 0 | 0 | 0 | 15 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.2 | 3132 | 1 | 0 | 0 | 14 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 2941 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.3 | 3139 | 2 | 0 | 0 | 20 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.3 | 3401 | 1 | 0 | 0 | 19 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.0 | 2759 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2757 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6727 | 0 |
| logged-in-admin | desktop | community | 1 | 30.0 | 3585 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2935 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 2889 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2709 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2913 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 2917 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2732 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2741 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2821 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2884 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2819 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.2 | 2873 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2855 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2895 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3023 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 6208 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3750 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.5 | 2910 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2905 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2781 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2907 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 31.0 | 2912 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 3403 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 3754 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2970 | 0 |
| logged-in-admin | mobile | reader | 1 | 29.0 | 3569 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.4 | 3034 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.6 | 3112 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.8 | 3121 | 2 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2835 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6663 | 0 |
| logged-in-reader | desktop | community | 1 | 38.0 | 4460 | 1 |
| logged-in-reader | desktop | gacha | 2 | 33.0 | 3309 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2965 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3176 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2883 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 31.0 | 3211 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2787 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 3291 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2915 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2798 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2877 | 0 |
| logged-in-reader | desktop | reading | 5 | 31.0 | 2958 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 3109 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2925 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 6369 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3722 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.5 | 2953 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2962 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 4550 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2918 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 3047 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 3576 | 0 |
| logged-in-reader | mobile | notifications | 1 | 29.0 | 4261 | 0 |
| logged-in-reader | mobile | profile | 3 | 30.3 | 3419 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 3529 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.4 | 2922 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 3315 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 3507 | 1 |
| logged-out | desktop | auth | 5 | 24.0 | 2758 | 0 |
| logged-out | desktop | home | 1 | 26.0 | 3071 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2725 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2721 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3395 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3156 | 0 |

## Key Findings
1. Critical: 2 page(s) vượt 35 requests.
2. High: 143 page(s) vượt 25 requests.
3. High: 131 request vượt 800ms.
4. Medium: 502 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 10 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6727 | 29 | 504 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6663 | 29 | 944 | 3.0 | 0.0040 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 6369 | 29 | 912 | 102.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 6208 | 29 | 832 | 40.0 | 0.0000 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 4701 | 29 | 1352 | 0.0 | 0.0401 |
| logged-in-reader | mobile | auth-public | /vi | 4550 | 34 | 1528 | 10.0 | 0.0032 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 4474 | 33 | 2376 | 0.0 | 0.0892 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 4460 | 38 | 1768 | 0.0 | 0.0039 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 4261 | 29 | 996 | 0.0 | 0.0000 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 4054 | 25 | 1112 | 0.0 | 0.0032 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 2138 | 315 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1547 | 323 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1348 | 126 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1316 | 126 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1312 | 130 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1298 | 129 | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1282 | 78 | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1281 | 130 | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1272 | 83 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1268 | 70 | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1266 | 130 | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1265 | 130 | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1259 | 63 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 1244 | 68 | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1238 | 317 | https://www.tarotnow.xyz/vi/notifications |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 800 | 333 | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 799 | 325 | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 798 | 311 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 796 | 310 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 796 | 81 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 794 | 311 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 791 | 371 | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 790 | 327 | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 788 | 92 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 787 | 312 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | auth-public | /vi | GET | 200 | 784 | 324 | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 784 | 331 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 784 | 316 | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 783 | 311 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | reading | /vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 | GET | 200 | 780 | 307 | https://www.tarotnow.xyz/vi/reading/session/d622091d-18e5-46d0-83d9-750a4e40ed88 |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 4 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 4 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
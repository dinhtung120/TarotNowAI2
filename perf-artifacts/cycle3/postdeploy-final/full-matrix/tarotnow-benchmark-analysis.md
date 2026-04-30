# TarotNow Benchmark Analysis

- Run time (UTC): 2026-04-30T07:24:43.284Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 26.3 | 2887 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 48 | 32.2 | 3095 | 0 | 0 | 0 | 10 | 1 | yes |
| logged-in-reader | desktop | 38 | 32.3 | 3172 | 0 | 0 | 0 | 6 | 1 | yes |
| logged-out | mobile | 9 | 25.9 | 2956 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 48 | 32.4 | 3277 | 0 | 0 | 0 | 6 | 2 | yes |
| logged-in-reader | mobile | 38 | 31.3 | 2908 | 0 | 0 | 0 | 5 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 33.5 | 2903 | 0 |
| logged-in-admin | desktop | auth | 5 | 31.6 | 3259 | 0 |
| logged-in-admin | desktop | chat | 1 | 31.0 | 2723 | 0 |
| logged-in-admin | desktop | collection | 1 | 32.0 | 6934 | 0 |
| logged-in-admin | desktop | community | 1 | 32.0 | 3563 | 0 |
| logged-in-admin | desktop | gacha | 2 | 34.0 | 3177 | 0 |
| logged-in-admin | desktop | gamification | 1 | 33.0 | 3263 | 0 |
| logged-in-admin | desktop | home | 1 | 33.0 | 3020 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2812 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 34.0 | 3493 | 0 |
| logged-in-admin | desktop | legal | 3 | 28.3 | 2799 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 3326 | 0 |
| logged-in-admin | desktop | profile | 3 | 32.0 | 2925 | 0 |
| logged-in-admin | desktop | reader | 1 | 31.0 | 2798 | 0 |
| logged-in-admin | desktop | readers | 7 | 31.3 | 2938 | 0 |
| logged-in-admin | desktop | reading | 5 | 33.2 | 2976 | 0 |
| logged-in-admin | desktop | wallet | 4 | 31.3 | 3085 | 0 |
| logged-in-admin | mobile | admin | 10 | 33.5 | 3362 | 0 |
| logged-in-admin | mobile | auth | 5 | 33.0 | 3468 | 0 |
| logged-in-admin | mobile | chat | 1 | 31.0 | 2981 | 0 |
| logged-in-admin | mobile | collection | 1 | 32.0 | 6912 | 0 |
| logged-in-admin | mobile | community | 1 | 40.0 | 4172 | 0 |
| logged-in-admin | mobile | gacha | 2 | 37.0 | 3360 | 0 |
| logged-in-admin | mobile | gamification | 1 | 34.0 | 3323 | 0 |
| logged-in-admin | mobile | home | 1 | 29.0 | 3007 | 0 |
| logged-in-admin | mobile | inventory | 1 | 33.0 | 3223 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 33.0 | 3247 | 0 |
| logged-in-admin | mobile | legal | 3 | 28.7 | 3056 | 0 |
| logged-in-admin | mobile | notifications | 1 | 32.0 | 2874 | 0 |
| logged-in-admin | mobile | profile | 3 | 32.3 | 3124 | 0 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 2958 | 0 |
| logged-in-admin | mobile | readers | 7 | 31.4 | 2953 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.2 | 2894 | 0 |
| logged-in-admin | mobile | wallet | 4 | 31.3 | 3308 | 0 |
| logged-in-reader | desktop | auth | 5 | 30.8 | 3181 | 0 |
| logged-in-reader | desktop | chat | 1 | 31.0 | 2745 | 0 |
| logged-in-reader | desktop | collection | 1 | 32.0 | 7250 | 0 |
| logged-in-reader | desktop | community | 1 | 32.0 | 3539 | 0 |
| logged-in-reader | desktop | gacha | 2 | 36.5 | 2949 | 0 |
| logged-in-reader | desktop | gamification | 1 | 34.0 | 3507 | 0 |
| logged-in-reader | desktop | home | 1 | 33.0 | 3155 | 0 |
| logged-in-reader | desktop | inventory | 1 | 33.0 | 2746 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 34.0 | 2876 | 0 |
| logged-in-reader | desktop | legal | 3 | 29.3 | 3004 | 0 |
| logged-in-reader | desktop | notifications | 1 | 32.0 | 2744 | 0 |
| logged-in-reader | desktop | profile | 3 | 32.3 | 3023 | 0 |
| logged-in-reader | desktop | reader | 1 | 31.0 | 2706 | 0 |
| logged-in-reader | desktop | readers | 7 | 31.3 | 3007 | 0 |
| logged-in-reader | desktop | reading | 5 | 35.2 | 3269 | 0 |
| logged-in-reader | desktop | wallet | 4 | 32.0 | 3000 | 0 |
| logged-in-reader | mobile | auth | 5 | 30.0 | 3047 | 0 |
| logged-in-reader | mobile | chat | 1 | 31.0 | 2726 | 0 |
| logged-in-reader | mobile | collection | 1 | 32.0 | 5580 | 0 |
| logged-in-reader | mobile | community | 1 | 32.0 | 3480 | 0 |
| logged-in-reader | mobile | gacha | 2 | 34.0 | 2722 | 0 |
| logged-in-reader | mobile | gamification | 1 | 34.0 | 2837 | 0 |
| logged-in-reader | mobile | home | 1 | 29.0 | 2712 | 0 |
| logged-in-reader | mobile | inventory | 1 | 33.0 | 2797 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 33.0 | 2695 | 0 |
| logged-in-reader | mobile | legal | 3 | 28.0 | 2737 | 0 |
| logged-in-reader | mobile | notifications | 1 | 32.0 | 2859 | 0 |
| logged-in-reader | mobile | profile | 3 | 32.7 | 2857 | 0 |
| logged-in-reader | mobile | reader | 1 | 32.0 | 2965 | 0 |
| logged-in-reader | mobile | readers | 7 | 31.0 | 2762 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.4 | 2760 | 0 |
| logged-in-reader | mobile | wallet | 4 | 31.5 | 2810 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2872 | 0 |
| logged-out | desktop | home | 1 | 33.0 | 3322 | 0 |
| logged-out | desktop | legal | 3 | 28.0 | 2767 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2899 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3514 | 0 |
| logged-out | mobile | legal | 3 | 28.0 | 2864 | 0 |

## Key Findings
1. Critical: 11 page(s) vượt 35 requests.
2. High: 180 page(s) vượt 25 requests.
3. High: 79 request vượt 800ms.
4. Medium: 605 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 0 image request(s) >800ms trên 70 image request(s).

## Top Slow Pages
| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | /vi/collection | 7250 | 32 | 996 | 13.0 | 0.0026 |
| logged-in-admin | desktop | /vi/collection | 6934 | 32 | 964 | 51.0 | 0.0031 |
| logged-in-admin | mobile | /vi/collection | 6912 | 32 | 964 | 85.0 | 0.0000 |
| logged-in-reader | mobile | /vi/collection | 5580 | 32 | 464 | 0.0 | 0.0000 |
| logged-in-admin | mobile | /vi/community | 4172 | 40 | 1884 | 0.0 | 0.0051 |
| logged-in-admin | mobile | /vi/admin/reader-requests | 4024 | 33 | 756 | 0.0 | 0.0000 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | 3757 | 37 | 1144 | 0.0 | 0.0026 |
| logged-in-admin | desktop | /vi/wallet | 3696 | 32 | 1904 | 0.0 | 0.0029 |
| logged-in-admin | mobile | /vi/verify-email | 3640 | 34 | 1068 | 6.0 | 0.0024 |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | 3634 | 37 | 908 | 0.0 | 0.0026 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | /vi/wallet | GET | 200 | 1561 | 1081 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1552 | 321 | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-out | desktop | /vi/login | GET | 200 | 1429 | 1061 | https://www.tarotnow.xyz/vi/login |
| logged-in-admin | desktop | /vi/gacha | GET | 200 | 1294 | 325 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1292 | 315 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 1224 | 343 | https://www.tarotnow.xyz/vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1137 | 101 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1136 | 63 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1116 | 84 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1114 | 87 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1113 | 81 | https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1108 | 64 | https://www.tarotnow.xyz/_next/static/chunks/0ia1rui7sf6...css |
| logged-in-admin | mobile | /vi/admin/readings | GET | 200 | 1089 | 327 | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | /vi/admin/reader-requests | GET | 200 | 1082 | 62 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | /vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 | GET | 200 | 1079 | 411 | https://www.tarotnow.xyz/vi/reading/session/9c624020-f4ae-4422-9c39-a7c852858fd9 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | /vi/gacha/history | GET | 200 | 799 | 453 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | /vi/collection | GET | 200 | 798 | 352 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 795 | 67 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/admin/gamification | GET | 200 | 794 | 336 | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | /vi/wallet | GET | 200 | 788 | 314 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | /vi/wallet/withdraw | GET | 200 | 787 | 317 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | /vi/admin/system-configs | GET | 200 | 785 | 370 | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 781 | 72 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | /vi/wallet/deposit/history | GET | 200 | 777 | 315 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | /vi/admin/disputes | GET | 200 | 777 | 385 | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | mobile | /vi/reading | GET | 200 | 777 | 309 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | /vi/gamification | GET | 200 | 777 | 321 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | /vi/legal/privacy | GET | 200 | 776 | 322 | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | /vi/gacha | GET | 200 | 774 | 316 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | /vi/reading/session/0f201915-be10-483e-86d4-9e5e4381072f | GET | 200 | 773 | 87 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 23 | 8 | 0 | 22 | 1 | 0 |
| logged-in-reader | desktop | 12 | 2 | 0 | 12 | 0 | 0 |
| logged-in-admin | mobile | 23 | 13 | 0 | 22 | 1 | 0 |
| logged-in-reader | mobile | 12 | 3 | 0 | 12 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
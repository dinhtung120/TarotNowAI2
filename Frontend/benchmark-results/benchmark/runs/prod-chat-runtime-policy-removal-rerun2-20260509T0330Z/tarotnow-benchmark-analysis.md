# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T20:32:14.963Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2833 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.7 | 2950 | 0 | 0 | 0 | 11 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3077 | 0 | 0 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 24.9 | 2828 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.2 | 3070 | 0 | 0 | 0 | 17 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 3318 | 0 | 0 | 0 | 14 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2860 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2731 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6606 | 0 |
| logged-in-admin | desktop | community | 1 | 30.0 | 3668 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2914 | 0 |
| logged-in-admin | desktop | gamification | 1 | 29.0 | 2928 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2743 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2968 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 30.0 | 2872 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2786 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2811 | 0 |
| logged-in-admin | desktop | reader | 1 | 29.0 | 2961 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2830 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.4 | 2868 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2871 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2936 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2817 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 6267 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3734 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2850 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 2912 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2752 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2853 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 31.0 | 2887 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 3556 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2755 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2969 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 3401 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.6 | 2907 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3076 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.3 | 2819 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2726 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6845 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3782 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2915 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2922 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2901 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2907 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 3050 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2899 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.0 | 2886 | 0 |
| logged-in-reader | desktop | reader | 1 | 29.0 | 2991 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2831 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.6 | 3070 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.8 | 3000 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2938 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 6291 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3934 | 0 |
| logged-in-reader | mobile | gacha | 2 | 30.5 | 2940 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2970 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 3427 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2997 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 3042 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 3489 | 0 |
| logged-in-reader | mobile | notifications | 1 | 29.0 | 3829 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.0 | 2944 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 4095 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2855 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.0 | 3348 | 0 |
| logged-in-reader | mobile | wallet | 4 | 29.0 | 3515 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2754 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3584 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2714 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2756 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3364 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2769 | 0 |

## Key Findings
1. Không có page nào vượt 35 requests.
2. High: 143 page(s) vượt 25 requests.
3. High: 121 request vượt 800ms.
4. Medium: 438 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 10 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6845 | 29 | 544 | 12.0 | 0.0040 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6606 | 29 | 532 | 0.0 | 0.0042 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 6291 | 29 | 840 | 7.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 6267 | 29 | 896 | 47.0 | 0.0000 |
| logged-in-reader | mobile | reading | /vi/reading/history | 4516 | 29 | 900 | 0.0 | 0.0071 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 4095 | 28 | 876 | 0.0 | 0.0071 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 4079 | 28 | 832 | 0.0 | 0.0071 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 3937 | 31 | 1240 | 0.0 | 0.0330 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3934 | 30 | 2016 | 0.0 | 0.0051 |
| logged-in-reader | desktop | reading | /vi/reading/history | 3898 | 28 | 1204 | 0.0 | 0.0039 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1552 | 329 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1383 | 67 | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1380 | 85 | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1360 | 85 | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1324 | 103 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1243 | 356 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 1233 | 421 | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1226 | 323 | https://www.tarotnow.xyz/vi/reading/history |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1193 | 582 | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1188 | 308 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1152 | 68 | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1148 | 102 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1147 | 103 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1146 | 360 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 1130 | 193 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 800 | 334 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 798 | 320 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 798 | 325 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 798 | 128 | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 798 | 130 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 314 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 794 | 329 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 794 | 324 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 792 | 312 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 790 | 354 | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 790 | 88 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 788 | 325 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 788 | 307 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 787 | 321 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 787 | 104 | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 2 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
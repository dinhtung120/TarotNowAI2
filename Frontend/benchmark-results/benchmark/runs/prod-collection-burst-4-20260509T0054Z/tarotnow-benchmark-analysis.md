# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-09T01:03:31.379Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2952 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.0 | 3050 | 0 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3011 | 0 | 0 | 0 | 12 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2797 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 28.7 | 2949 | 0 | 0 | 0 | 10 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.2 | 2955 | 0 | 0 | 0 | 23 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.4 | 2926 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2793 | 0 |
| logged-in-admin | desktop | collection | 1 | 30.0 | 8138 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4079 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2898 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3356 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2827 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2900 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3335 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2723 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2856 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 3058 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2717 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2816 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 2918 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2861 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2788 | 0 |
| logged-in-admin | mobile | chat | 1 | 29.0 | 2949 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5726 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3703 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2853 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2878 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2786 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2858 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2957 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2787 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2790 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2942 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2857 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.1 | 2828 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 3098 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.3 | 2811 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2811 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 7074 | 0 |
| logged-in-reader | desktop | community | 1 | 35.0 | 3707 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.5 | 2891 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2895 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2849 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2910 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2844 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2744 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2920 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2876 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2696 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2873 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.8 | 2890 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.3 | 2862 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2798 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5570 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3638 | 0 |
| logged-in-reader | mobile | gacha | 2 | 32.0 | 2896 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2906 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2724 | 0 |
| logged-in-reader | mobile | inventory | 1 | 32.0 | 2881 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2891 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2737 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2939 | 0 |
| logged-in-reader | mobile | profile | 3 | 28.7 | 2881 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2734 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.4 | 2813 | 0 |
| logged-in-reader | mobile | reading | 5 | 31.2 | 2928 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.8 | 2860 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2834 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4164 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2746 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2735 | 0 |
| logged-out | mobile | home | 1 | 29.0 | 3240 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2754 | 0 |

## Key Findings
1. Không có page nào vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 35 request vượt 800ms.
4. Medium: 176 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 40 image request(s) >800ms trên 96 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 8138 | 30 | 1048 | 56.0 | 0.0574 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 7074 | 29 | 604 | 0.0 | 0.0569 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5726 | 29 | 468 | 27.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5570 | 29 | 464 | 0.0 | 0.0000 |
| logged-in-admin | mobile | reading | /vi/reading | 4169 | 29 | 2376 | 0.0 | 0.0071 |
| logged-out | desktop | auth-public | /vi | 4164 | 29 | 1760 | 58.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4079 | 35 | 1184 | 0.0 | 0.0041 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 3707 | 35 | 1172 | 0.0 | 0.0037 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3703 | 35 | 1020 | 0.0 | 0.0000 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3638 | 35 | 1000 | 0.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 2095 | 314 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1264 | 351 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1160 | 550 | https://www.tarotnow.xyz/_next/static/chunks/0gg0893b49orw.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1008 | 635 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1007 | 690 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 973 | 373 | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 939 | 935 | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 892 | 335 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 874 | 331 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 874 | 395 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 872 | 342 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 861 | 357 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 847 | 335 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 841 | 390 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 839 | 323 | https://www.tarotnow.xyz/vi/notifications |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 799 | 329 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 798 | 314 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 794 | 312 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 794 | 313 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 793 | 547 | https://www.tarotnow.xyz/_next/static/chunks/0b~4~6uczd_zv.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 793 | 326 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 792 | 373 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 792 | 322 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 791 | 314 | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 791 | 313 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 789 | 305 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 789 | 389 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 787 | 533 | https://www.tarotnow.xyz/_next/static/chunks/0h8ot364y8uxz.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 785 | 535 | https://www.tarotnow.xyz/_next/static/chunks/0em89gm5~b0v..js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 782 | 321 | https://www.tarotnow.xyz/vi/gacha/history |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 38 | 5 | 28 | 37 | 1 | 0 |
| logged-in-reader | desktop | 18 | 7 | 0 | 18 | 0 | 0 |
| logged-in-admin | mobile | 26 | 9 | 12 | 25 | 1 | 0 |
| logged-in-reader | mobile | 14 | 3 | 0 | 14 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T19:27:48.928Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.0 | 2855 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.3 | 2968 | 0 | 0 | 0 | 16 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.4 | 2930 | 0 | 0 | 0 | 18 | 0 | yes |
| logged-out | mobile | 9 | 24.7 | 2750 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 28.9 | 2860 | 0 | 0 | 0 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 28.8 | 2926 | 0 | 0 | 0 | 10 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2876 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6101 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4040 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2910 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3323 | 0 |
| logged-in-admin | desktop | home | 1 | 29.0 | 2883 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2853 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3259 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2694 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2768 | 0 |
| logged-in-admin | desktop | profile | 3 | 30.0 | 2910 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2803 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2808 | 0 |
| logged-in-admin | desktop | reading | 5 | 31.0 | 2880 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2815 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2798 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2772 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5492 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3412 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.5 | 2850 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 2948 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2694 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2859 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 2843 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2682 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2654 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2928 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2638 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2738 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.8 | 2795 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2753 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2759 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6091 | 0 |
| logged-in-reader | desktop | community | 1 | 36.0 | 3703 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2884 | 0 |
| logged-in-reader | desktop | gamification | 1 | 33.0 | 2891 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2705 | 0 |
| logged-in-reader | desktop | inventory | 1 | 33.0 | 2902 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 2847 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2692 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2712 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 2888 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2666 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2774 | 0 |
| logged-in-reader | desktop | reading | 5 | 31.0 | 2857 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.8 | 2805 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2732 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 5802 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3715 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.0 | 2879 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2883 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2725 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2935 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2852 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2680 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2850 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2967 | 0 |
| logged-in-reader | mobile | reader | 1 | 30.0 | 2905 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.3 | 2740 | 0 |
| logged-in-reader | mobile | reading | 5 | 28.8 | 2781 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 2846 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 2736 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3926 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2696 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2679 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3196 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2721 | 0 |

## Key Findings
1. Critical: 3 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 27 request vượt 800ms.
4. Medium: 190 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 11 image request(s) >800ms trên 36 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6101 | 29 | 880 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6091 | 29 | 500 | 0.0 | 0.0040 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5802 | 31 | 972 | 4.0 | 0.0071 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5492 | 29 | 768 | 0.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4040 | 36 | 1864 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 3926 | 29 | 1356 | 245.0 | 0.0000 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3715 | 36 | 1688 | 0.0 | 0.0051 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 3703 | 36 | 1760 | 0.0 | 0.0039 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3412 | 30 | 1820 | 0.0 | 0.0051 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 3323 | 31 | 1036 | 0.0 | 0.0279 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1197 | 597 | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1071 | 707 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1032 | 535 | https://www.tarotnow.xyz/_next/static/chunks/0~vbsxkq2i_qa.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 975 | 511 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 919 | 323 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 877 | 463 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 875 | 388 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 840 | 360 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 834 | 425 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 832 | 341 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 828 | 338 | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 827 | 400 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 827 | 339 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 815 | 315 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 815 | 418 | https://www.tarotnow.xyz/vi/wallet/withdraw |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 800 | 322 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 799 | 412 | https://www.tarotnow.xyz/vi/gamification |
| logged-out | desktop | auth-public | /vi | GET | 200 | 796 | 556 | https://www.tarotnow.xyz/_next/static/chunks/04qofk648hs17.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 334 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 796 | 313 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 795 | 316 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 792 | 318 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 791 | 310 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 790 | 545 | https://www.tarotnow.xyz/_next/static/chunks/0aa.eah0s47oq.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 790 | 332 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 789 | 339 | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 789 | 323 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 788 | 308 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 783 | 329 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 783 | 314 | https://www.tarotnow.xyz/vi/gacha |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 12 | 3 | 7 | 11 | 1 | 0 |
| logged-in-reader | desktop | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 3 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | 5 | 4 | 0 | 5 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
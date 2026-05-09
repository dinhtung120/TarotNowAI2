# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-09T01:19:17.648Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2893 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.9 | 3027 | 0 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3003 | 0 | 0 | 0 | 8 | 0 | yes |
| logged-out | mobile | 9 | 24.6 | 2782 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.1 | 2949 | 0 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 3023 | 0 | 0 | 0 | 10 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2900 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2866 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 7327 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4086 | 0 |
| logged-in-admin | desktop | gacha | 2 | 30.5 | 2930 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3280 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2776 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2971 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3384 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2724 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2971 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.7 | 2919 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 3059 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2822 | 0 |
| logged-in-admin | desktop | reading | 5 | 30.2 | 2863 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2894 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 2903 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2864 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5724 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3667 | 0 |
| logged-in-admin | mobile | gacha | 2 | 31.0 | 2921 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2859 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2777 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2911 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2822 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2748 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2741 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2871 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2857 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.4 | 2856 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.2 | 2922 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2812 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6761 | 0 |
| logged-in-reader | desktop | community | 1 | 35.0 | 3694 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.0 | 2946 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2882 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 2905 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2871 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2837 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2888 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2880 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2781 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.1 | 2858 | 0 |
| logged-in-reader | desktop | reading | 5 | 29.4 | 2860 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 2833 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2835 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5913 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3831 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 2957 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2877 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2759 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 2942 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2868 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2761 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2825 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 3120 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2881 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.1 | 2899 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 2921 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 2894 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2762 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4002 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2742 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2725 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3214 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2732 | 0 |

## Key Findings
1. Không có page nào vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 35 request vượt 800ms.
4. Medium: 262 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 11 image request(s) >800ms trên 38 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 7327 | 29 | 612 | 14.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6761 | 29 | 908 | 5.0 | 0.0037 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5913 | 29 | 788 | 8.0 | 0.0000 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5724 | 29 | 460 | 5.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4086 | 35 | 1176 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 4002 | 29 | 1412 | 761.0 | 0.0000 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3831 | 35 | 1208 | 1.0 | 0.0051 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 3773 | 33 | 1436 | 0.0 | 0.0889 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 3694 | 35 | 1100 | 0.0 | 0.0037 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3667 | 35 | 996 | 0.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1377 | 318 | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1306 | 564 | https://www.tarotnow.xyz/_next/static/chunks/16683zycjf2di.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1304 | 556 | https://www.tarotnow.xyz/_next/static/chunks/0jpe042aa74zf.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1173 | 395 | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1055 | 676 | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 1021 | 333 | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 933 | 329 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 909 | 333 | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | GET | 200 | 887 | 548 | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 886 | 356 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 883 | 327 | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 879 | 415 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 871 | 337 | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 870 | 368 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 864 | 335 | https://www.tarotnow.xyz/vi/wallet |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 800 | 311 | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 800 | 320 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 798 | 332 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 798 | 322 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 798 | 327 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 796 | 321 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 796 | 324 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 796 | 318 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 793 | 322 | https://www.tarotnow.xyz/vi/inventory |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 790 | 325 | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 790 | 316 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 790 | 335 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 789 | 543 | https://www.tarotnow.xyz/_next/static/chunks/0w.yuzykfwjex.js |
| logged-in-admin | mobile | reading | /vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 | GET | 200 | 789 | 336 | https://www.tarotnow.xyz/vi/reading/session/f481bbdc-f180-4075-b725-dfd8bd1ec4c3 |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 789 | 310 | https://www.tarotnow.xyz/vi/reading |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| - | - | - | - | - |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 3 | 4 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 13 | 4 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | 6 | 2 | 2 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.
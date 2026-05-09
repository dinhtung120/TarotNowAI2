# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-09T00:45:40.487Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2874 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 28.8 | 3024 | 0 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | desktop | 33 | 28.7 | 3034 | 0 | 0 | 0 | 7 | 0 | yes |
| logged-out | mobile | 9 | 25.0 | 2766 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 28.8 | 2944 | 0 | 0 | 0 | 8 | 0 | yes |
| logged-in-reader | mobile | 33 | 28.7 | 2954 | 0 | 0 | 0 | 12 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.5 | 2935 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2866 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 7284 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4128 | 0 |
| logged-in-admin | desktop | gacha | 2 | 32.0 | 2964 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3263 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2719 | 0 |
| logged-in-admin | desktop | inventory | 1 | 30.0 | 2852 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3251 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2803 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2840 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2865 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 3196 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2812 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.0 | 2889 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.0 | 2808 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2842 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2962 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5910 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3751 | 0 |
| logged-in-admin | mobile | gacha | 2 | 33.0 | 2872 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2864 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2771 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2886 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2804 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2736 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2724 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2926 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2930 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2859 | 0 |
| logged-in-admin | mobile | reading | 5 | 28.6 | 2855 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.3 | 2892 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2722 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6969 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3649 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.0 | 2864 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2922 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2772 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3024 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2904 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2751 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2871 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 3051 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 3029 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2814 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.0 | 2944 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2929 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2823 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5688 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3783 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.0 | 2884 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2865 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2776 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2876 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2688 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2726 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2891 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 2946 | 0 |
| logged-in-reader | mobile | reader | 1 | 29.0 | 2940 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.1 | 2829 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.4 | 2841 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.3 | 2843 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2776 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3803 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2729 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2713 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3037 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2764 | 0 |

## Key Findings
1. Không có page nào vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 33 request vượt 800ms.
4. Medium: 277 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 11 image request(s) >800ms trên 37 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 7284 | 29 | 588 | 169.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6969 | 29 | 992 | 0.0 | 0.0037 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5910 | 29 | 452 | 16.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5688 | 29 | 476 | 0.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 4128 | 35 | 1380 | 0.0 | 0.0041 |
| logged-out | desktop | auth-public | /vi | 3803 | 29 | 1324 | 651.0 | 0.0000 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3783 | 35 | 1336 | 0.0 | 0.0051 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3751 | 35 | 1036 | 0.0 | 0.0000 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 3649 | 30 | 1344 | 0.0 | 0.0037 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 3291 | 33 | 1112 | 0.0 | 0.0022 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 1095 | 343 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1046 | 538 | https://www.tarotnow.xyz/_next/static/chunks/0b8hzxtf1-_dk.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1043 | 568 | https://www.tarotnow.xyz/_next/static/chunks/12oo753jmxg~2.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1030 | 357 | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 955 | 392 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | reading | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | GET | 200 | 954 | 323 | https://www.tarotnow.xyz/vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 950 | 331 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 932 | 681 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 913 | 322 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 909 | 325 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 893 | 318 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 882 | 542 | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | reading | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | GET | 200 | 866 | 319 | https://www.tarotnow.xyz/vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 851 | 317 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 843 | 354 | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 799 | 335 | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 798 | 323 | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 798 | 330 | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 797 | 317 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 794 | 315 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 793 | 321 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 793 | 318 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 792 | 540 | https://www.tarotnow.xyz/_next/static/chunks/0mp2dyp5p7fbx.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 792 | 315 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 791 | 338 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 791 | 304 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 790 | 320 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 789 | 315 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 789 | 316 | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 789 | 379 | https://www.tarotnow.xyz/vi/notifications |

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
| logged-in-reader | mobile | 5 | 3 | 1 | 5 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.